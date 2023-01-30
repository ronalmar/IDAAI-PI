ALTER PROC sp_prestamoitem
		@i_accion			CHAR(2)			=	NULL,
		@i_id				INT				=	NULL,
		@i_matricula		VARCHAR(10)		=	NULL,
		@i_rfid				VARCHAR(100)	=	NULL,
		@i_modoClases		BIT				=	NULL,
		@i_modulo			VARCHAR(50)		=	NULL,
		@i_grupoItems		BIT				=	NULL,
		@i_xmlItems			XML				=	NULL,
		@i_usuario			VARCHAR(25)		=	NULL
AS
BEGIN
	DECLARE
	@fechaAhora					DATETIME,
	@estudianteId				INT,
	@itemId						INT,
	@idInsertado				INT,
	@modulo						VARCHAR(50),
	@moduloId					INT,
	@xmlPuntero					INT,
	@rfidSuperior				VARCHAR(10),
	@rfid						VARCHAR(100),
	@FechaPrestadoInsertar		DATETIME,
	@ItemIdInsertar				INT,
	@EstadoDevolucionIdInsertar	INT,
	@ModuloIdInsertar			INT,
	@ModoClaseInsertar			BIT,
	@usuarioId					INT

	SET @fechaAhora = GETDATE()
	SET @modulo=TRIM(@i_modulo)
	SELECT @usuarioId=Id FROM Usuarios WHERE Usuario=@i_usuario AND Estado=1

	DECLARE @PrestamoItemEstudiante TABLE(
		Id					INT,
		FechaPrestado		DATETIME,
		FechaDevuelto		DATETIME,
		Inventario			VARCHAR(25),
		Item				VARCHAR(100),
		Nombres				VARCHAR(100),
		Apellidos			VARCHAR(100),
		Matricula			VARCHAR(10),
		Email				VARCHAR(50),
		EstadoDevolucion	VARCHAR(25)
	)

	DECLARE @PrestamoItemClase TABLE(
		Id					INT,
		FechaPrestado		DATETIME,
		FechaDevuelto		DATETIME,
		Inventario			VARCHAR(25),
		Item				VARCHAR(100),
		Modulo				VARCHAR(50),
		EstadoDevolucion	VARCHAR(25)
	)

	DECLARE @PrestamoItemInsertar TABLE(
		FechaPrestado		DATETIME,
		ItemId				INT,
		EstadoDevolucionId	INT,
		ModuloId			INT,
		ModoClase			BIT,
		UsuarioId			INT
	)

	DECLARE @ItemsIds TABLE(
		Id					INT
	)

	DECLARE @PrestamoItemsIds TABLE(
		Id					INT
	)

	DECLARE @ItemsXML TABLE(
		Rfid				VARCHAR(100)
	)

	IF(@i_accion='IN')
	BEGIN		
		-- SI EL REGISTRO ES POR CLASE (LABORATORIO CERRADO) - REGISTRO DE ITEM UNICO
		IF(@i_modoClases=1)
		BEGIN			
			-- VALIDACION DE QUE DEBE EXISTIR EL USUARIO INGRESADO
			IF (@usuarioId IS NULL)
			BEGIN
				INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
				VALUES(-9, GETDATE(), GETDATE())
				SELECT TOP(1) * FROM @PrestamoItemClase
				RETURN 0;
			END
			-- VALIDACION PARA QUE EL MODULO INGRESADO EXISTA Y NO SEA ID 1 (LABORATORIO ABIERTO)
			IF NOT EXISTS(SELECT 1 FROM Modulos WHERE Nombre=@modulo AND Nombre!='LA' AND UsuarioId=@usuarioId AND Estado=1)
			BEGIN
				INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
				VALUES(-3, GETDATE(), GETDATE())
				SELECT TOP(1) * FROM @PrestamoItemClase
				RETURN 0;
			END

			-- REGISTRO POR GRUPO DE ITEMS

			-- VALIDACION SI SE ENVIA LISTA XML DE X ITEMS A REGISTRAR
			IF(ISNULL(@i_grupoItems,0)!=0)
			BEGIN
				EXEC SP_XML_PREPAREDOCUMENT	@xmlPuntero OUTPUT, @i_xmlItems	

				SELECT * INTO #ITEMSXML FROM OPENXML(@xmlPuntero, 'Items/Item', 2)
				WITH(
					Rfid			VARCHAR(100)		'Rfid'
				)

				--EXEC sp_xml_removedocument 
				--		@xmlPuntero
				
				INSERT INTO @ItemsXML
				SELECT * FROM #ITEMSXML

				-- VALIDACION DE QUE NO HAYAN ELEMENTOS (RFID) REPETIDOS EN EL XML
				IF((SELECT COUNT(DISTINCT Rfid) FROM @ItemsXML)!=(SELECT COUNT(Rfid) FROM @ItemsXML))
				BEGIN
					INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
					VALUES(-6, GETDATE(), GETDATE())
					SELECT TOP(1) * FROM @PrestamoItemClase
					RETURN 0;
				END

				WHILE(1=1)
				BEGIN
					IF(SELECT COUNT(*) FROM @ItemsXML)=0
						BREAK;

					SELECT TOP(1) @rfidSuperior=Rfid FROM @ItemsXML

					-- VALIDACION DE QUE EXISTA UN ITEM CON EL RFID ITERADO
					IF NOT EXISTS(SELECT 1 FROM Items it INNER JOIN Inventario i ON i.Id=it.InventarioId
					WHERE Rfid=@rfidSuperior AND it.UsuarioId=@usuarioId AND i.UsuarioId=@usuarioId AND it.Estado=1 AND i.Estado=1)
					BEGIN
						INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
						VALUES(-7, GETDATE(), GETDATE())
						SELECT TOP(1) * FROM @PrestamoItemClase
						RETURN 0;
					END

					-- VALIDACION DE QUE EL RFID NO CORRESPONDA AL INVENTARIO GENERAL
					IF EXISTS(SELECT 1 FROM Items it INNER JOIN Inventario i ON i.Id=it.InventarioId
						WHERE Rfid=@i_rfid AND i.Nombre='General' AND it.UsuarioId=@usuarioId AND i.UsuarioId=@usuarioId AND it.Estado=1 AND i.Estado=1)
					BEGIN
					INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
							VALUES(-13, GETDATE(), GETDATE())
							SELECT TOP(1) * FROM @PrestamoItemClase
							RETURN 0;
					END

					-- VALIDACION DE QUE EL ITEM ITERADO ESTE DISPONIBLE
					IF((SELECT EstadoItemId FROM Items WHERE Rfid=@rfidSuperior AND UsuarioId=@usuarioId AND Estado=1)=2)
					BEGIN
						INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
						VALUES(-8, GETDATE(), GETDATE())
						SELECT TOP(1) * FROM @PrestamoItemClase
						RETURN 0;
					END

					-- INSERCION DE NUEVO REGISTRO DE PRESTAMO DE ITEM ITERADO A MODULO
					SELECT @moduloId=Id FROM Modulos WHERE Nombre=@modulo AND UsuarioId=@usuarioId AND Id!=1 AND Estado=1
					SELECT @itemId=Id FROM Items WHERE Rfid=@rfidSuperior AND UsuarioId=@usuarioId AND Estado=1

					INSERT INTO @PrestamoItemInsertar
					(
						FechaPrestado, ItemId, EstadoDevolucionId, ModuloId, ModoClase, UsuarioId
					)
					SELECT
						@fechaAhora, @itemId, 1, @moduloId, 1, @usuarioId									
					
					DELETE TOP(1) FROM @ItemsXML
				END

				WHILE(1=1)
				BEGIN
					IF(SELECT COUNT(*) FROM @PrestamoItemInsertar)=0
						BREAK;

					SELECT TOP(1) @FechaPrestadoInsertar=FechaPrestado FROM @PrestamoItemInsertar
					SELECT TOP(1) @ItemIdInsertar=ItemId FROM @PrestamoItemInsertar
					SELECT TOP(1) @EstadoDevolucionIdInsertar=EstadoDevolucionId FROM @PrestamoItemInsertar
					SELECT TOP(1) @ModuloIdInsertar=ModuloId FROM @PrestamoItemInsertar
					SELECT TOP(1) @ModoClaseInsertar=ModoClase FROM @PrestamoItemInsertar

					INSERT INTO RegistroPrestamoItem
					(
						FechaPrestado, ItemId, EstadoDevolucionId, ModuloId, ModoClase, UsuarioId, Estado
					)
					SELECT
						@FechaPrestadoInsertar, 
						@ItemIdInsertar,
						@EstadoDevolucionIdInsertar,
						@ModuloIdInsertar,
						@ModoClaseInsertar,
						@usuarioId,
						1
					
					SET @idInsertado=@@IDENTITY
					INSERT INTO @ItemsIds(Id) SELECT @ItemIdInsertar
					INSERT INTO @PrestamoItemsIds(Id) SELECT @idInsertado

					DELETE TOP(1) FROM @PrestamoItemInsertar
				END

				UPDATE Items SET
				EstadoItemId=2
				WHERE Id IN (SELECT Id FROM @ItemsIds)
				AND UsuarioId=@usuarioId
				AND Estado=1

				EXEC sp_inventario
				@i_accion		=	'VE',
				@i_accionExt	=	1

				SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
				Inventario=i.Nombre, Item=it.Rfid, Modulo=m.Nombre, EstadoDevolucion=ed.Descripcion
				FROM RegistroPrestamoItem r
				INNER JOIN Modulos m			ON m.Id=r.ModuloId
				INNER JOIN Items it				ON it.Id=r.ItemId
				INNER JOIN Inventario i			ON i.Id=it.InventarioId
				INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
				WHERE r.Id IN (SELECT Id FROM @PrestamoItemsIds)
				AND r.UsuarioId=@usuarioId
				AND m.Estado=1
				AND it.Estado=1
				AND i.Estado=1
				AND r.Estado=1

				RETURN 0
			END
			
			-- REGISTRO POR ITEM UNICO EN MODULO

			-- VALIDACION DE QUE DEBE EXISTIR EL MODULO INGRESADO
			IF NOT EXISTS(SELECT 1 FROM Modulos WHERE Nombre=@modulo AND UsuarioId=@usuarioId AND Nombre!='LA' AND Estado=1)
			BEGIN
				INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
				VALUES(-10, GETDATE(), GETDATE())
				SELECT TOP(1) * FROM @PrestamoItemClase
				RETURN 0;
			END

			-- VALIDACION DE QUE EXISTA UN ITEM CON EL RFID INGRESADO
			IF NOT EXISTS(SELECT 1 FROM Items it INNER JOIN Inventario i ON i.Id=it.InventarioId
				WHERE Rfid=@i_rfid AND it.UsuarioId=@usuarioId AND i.UsuarioId=@usuarioId AND it.Estado=1 AND i.Estado=1)
			BEGIN
				INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
				VALUES(-4, GETDATE(), GETDATE())
				SELECT TOP(1) * FROM @PrestamoItemClase
				RETURN 0;
			END

			-- VALIDACION DE QUE EL RFID NO CORRESPONDA AL INVENTARIO GENERAL
			IF EXISTS(SELECT 1 FROM Items it INNER JOIN Inventario i ON i.Id=it.InventarioId
				WHERE Rfid=@i_rfid AND i.Nombre='General' AND it.UsuarioId=@usuarioId AND i.UsuarioId=@usuarioId AND it.Estado=1 AND i.Estado=1)
			BEGIN
			INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
					VALUES(-12, GETDATE(), GETDATE())
					SELECT TOP(1) * FROM @PrestamoItemClase
					RETURN 0;
			END

			-- VALIDACION DE QUE EL ITEM INGRESADO ESTE DISPONIBLE
			IF((SELECT EstadoItemId FROM Items WHERE Rfid=@i_rfid AND UsuarioId=@usuarioId AND Estado=1)=2)
			BEGIN
				INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
				VALUES(-5, GETDATE(), GETDATE())
				SELECT TOP(1) * FROM @PrestamoItemClase
				RETURN 0;
			END
			
			-- INSERCION DE NUEVO REGISTRO DE PRESTAMO DE ITEM A MODULO
			SELECT @moduloId=Id FROM Modulos WHERE Nombre=@modulo AND UsuarioId=@usuarioId AND Nombre!='LA' AND Estado=1
			SELECT @itemId=Id FROM Items WHERE Rfid=@i_rfid AND UsuarioId=@usuarioId AND Estado=1

			INSERT INTO RegistroPrestamoItem
			(
				FechaPrestado, ItemId, EstadoDevolucionId, ModuloId, ModoClase, UsuarioId, Estado
			)
			SELECT
				@fechaAhora, @itemId, 1, @moduloId, 1, @usuarioId, 1
		
			SET @idInsertado=@@IDENTITY

			UPDATE Items SET
			EstadoItemId=2
			WHERE Id=@itemId
			AND UsuarioId=@usuarioId
			AND Estado=1

			EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

			SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
			Inventario=i.Nombre, Item=it.Rfid, Modulo=m.Nombre, EstadoDevolucion=ed.Descripcion
			FROM RegistroPrestamoItem r
			INNER JOIN Modulos m			ON m.Id=r.ModuloId
			INNER JOIN Items it				ON it.Id=r.ItemId
			INNER JOIN Inventario i			ON i.Id=it.InventarioId
			INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
			WHERE r.Id=@idInsertado
			AND r.UsuarioId=@usuarioId
			AND m.Estado=1
			AND it.Estado=1
			AND i.Estado=1
			AND r.Estado=1

			RETURN 0;
		END
		
		-- REGISTRO POR ESTUDIANTE

		-- VALIDACION DE QUE DEBE EXISTIR EL USUARIO INGRESADO
		IF (@usuarioId IS NULL)
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-10, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END

		-- VALIDACION DE QUE EL RFID NO CORRESPONDA AL INVENTARIO GENERAL
		IF EXISTS(SELECT 1 FROM Items it INNER JOIN Inventario i ON i.Id=it.InventarioId
			WHERE Rfid=@i_rfid AND i.Nombre='General' AND it.UsuarioId=@usuarioId AND i.UsuarioId=@usuarioId AND it.Estado=1 AND i.Estado=1)
		BEGIN
		INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-11, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END

		-- VALIDACION DE QUE EXISTA UN ESTUDIANTE CON LA MATRICULA INGRESADA
		IF NOT EXISTS(SELECT 1 FROM Estudiantes e INNER JOIN Modulos m ON m.Id=e.ModuloId
					WHERE e.Matricula=@i_matricula AND m.UsuarioId=@usuarioId AND e.UsuarioId=@usuarioId AND m.Nombre='LA' AND m.Estado=1 AND e.Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(0, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END		

		-- VALIDACION DE QUE EXISTA UN ITEM CON EL RFID INGRESADO
		IF NOT EXISTS(SELECT 1 FROM Items WHERE Rfid=@i_rfid AND UsuarioId=@usuarioId AND Estado=1)
		BEGIN
		INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-1, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END		

		-- VALIDACION DE QUE EL ITEM INGRESADO ESTE DISPONIBLE
		IF((SELECT EstadoItemId FROM Items WHERE Rfid=@i_rfid AND UsuarioId=@usuarioId AND Estado=1)=2)
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-2, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END

		-- INSERCION DE NUEVO REGISTRO DE PRESTAMO DE ITEM A ESTUDIANTE
		SELECT @estudianteId=e.Id FROM Estudiantes e
		INNER JOIN Modulos m ON m.Id=e.ModuloId
		WHERE e.Matricula=@i_matricula AND e.UsuarioId=@usuarioId AND m.UsuarioId=@usuarioId AND m.Nombre='LA' AND e.Estado=1 AND m.Estado=1
		SELECT TOP(1) @itemId=Id FROM Items WHERE Rfid=@i_rfid AND UsuarioId=@usuarioId AND Estado=1

		INSERT INTO RegistroPrestamoItem
		(
			FechaPrestado, ItemId, EstadoDevolucionId, EstudianteId, ModoClase, UsuarioId, Estado
		)
		SELECT
			@fechaAhora, @itemId, 1, @estudianteId, 0, @usuarioId, 1
		
		SET @idInsertado=@@IDENTITY

		UPDATE Items SET
		EstadoItemId=2
		WHERE Id=@itemId
		AND UsuarioId=@usuarioId
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Nombres=e.Nombres, Apellidos=e.Apellidos,
		Matricula=e.Matricula, Email=e.Email, EstadoDevolucion=ed.Descripcion, Modulo=m.Nombre
		FROM RegistroPrestamoItem r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON ed.Id=r.EstadoDevolucionId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE r.Id=@idInsertado
		AND r.UsuarioId=@usuarioId
		AND e.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND m.Estado=1
		AND r.Estado=1

		RETURN 0;
	END

	IF(@i_accion='UE')
	BEGIN
		-- VALIDACION DE QUE DEBE EXISTIR EL USUARIO INGRESADO
		IF (@usuarioId IS NULL)
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-3, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END
		-- VALIDACION DE QUE DEBE EXISTIR EL REGISTRO DE PRESTAMO DE ITEM CON EL ID INGRESADO
		IF NOT EXISTS(SELECT 1 FROM RegistroPrestamoItem WHERE Id=@i_id AND UsuarioId=@usuarioId AND Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(0, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END

		-- VALIDACION DE QUE EL ITEM DEBE TENER ESTADO PRESTADO PARA DEVOLERSE
		IF EXISTS(SELECT 1 FROM RegistroPrestamoItem r 
			INNER JOIN EstadoDevolucion ed ON ed.Id=r.EstadoDevolucionId
			WHERE r.Id=@i_id AND (ed.Id=2 OR ed.Id=4) AND r.Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-1, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END

		-- VALIDACION DE QUE EL ITEM DEBE HABER SIDO PRESTADO A ESTUDIANTE
		IF ISNULL((SELECT ModuloId FROM RegistroPrestamoItem WHERE Id=@i_id),'')!=''
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-2, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END

		UPDATE RegistroPrestamoItem SET
		FechaDevuelto=@fechaAhora,
		EstadoDevolucionId=2
		WHERE Id=@i_id
		AND UsuarioId=@usuarioId
		AND Estado=1		

		SELECT @rfid=it.Rfid FROM RegistroPrestamoItem  r
		INNER JOIN Items it ON it.Id=r.ItemId
		WHERE r.Id=@i_id
		AND r.UsuarioId=@usuarioId
		AND r.Estado=1
		AND it.Estado=1

		UPDATE Items SET
		EstadoItemId=1
		WHERE Id=(SELECT Id FROM Items WHERE Rfid=@rfid AND UsuarioId=@usuarioId)
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Nombres=e.Nombres, Apellidos=e.Apellidos,
		Matricula=e.Matricula, Email=e.Email, EstadoDevolucion=ed.Descripcion
		FROM RegistroPrestamoItem r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
		WHERE r.Id=@i_id
		AND r.UsuarioId=@usuarioId
		AND e.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND r.Estado=1

		RETURN 0;
	END

	IF(@i_accion='UM')
	BEGIN
		-- VALIDACION DE QUE DEBE EXISTIR EL USUARIO INGRESADO
		IF (@usuarioId IS NULL)
		BEGIN
			INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-3, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemClase
			RETURN 0;
		END
		-- VALIDACION DE QUE DEBE EXISTIR EL REGISTRO DE PRESTAMO DE ITEM CON EL ID INGRESADO
		IF NOT EXISTS(SELECT 1 FROM RegistroPrestamoItem WHERE Id=@i_id AND UsuarioId=@usuarioId AND Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
			VALUES(0, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemClase
			RETURN 0;
		END

		-- VALIDACION DE QUE EL ITEM DEBE TENER ESTADO PRESTADO PARA DEVOLERSE
		IF EXISTS(SELECT 1 FROM RegistroPrestamoItem r 
			INNER JOIN EstadoDevolucion ed ON ed.Id=r.EstadoDevolucionId
			WHERE r.Id=@i_id AND (ed.Id=2 OR ed.Id=4) AND r.Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-1, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemClase
			RETURN 0;
		END

		-- VALIDACION DE QUE EL ITEM DEBE HABER SIDO PRESTADO A MODULO
		IF ISNULL((SELECT EstudianteId FROM RegistroPrestamoItem WHERE Id=@i_id),'')!=''
		BEGIN
			INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-2, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemClase
			RETURN 0;
		END

		UPDATE RegistroPrestamoItem SET
		FechaDevuelto=@fechaAhora,
		EstadoDevolucionId=2
		WHERE Id=@i_id
		AND UsuarioId=@usuarioId
		AND Estado=1

		SELECT @rfid=it.Rfid FROM RegistroPrestamoItem  r
		INNER JOIN Items it ON it.Id=r.ItemId
		INNER JOIN Inventario i ON i.Id=it.InventarioId
		WHERE r.Id=@i_id
		AND r.UsuarioId=@usuarioId
		AND r.Estado=1
		AND i.UsuarioId=@usuarioId
		AND it.Estado=1

		UPDATE Items SET
		EstadoItemId=1
		WHERE Id=(SELECT Id FROM Items WHERE Rfid=@rfid AND UsuarioId=@usuarioId)
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Modulo=m.Nombre, EstadoDevolucion=ed.Descripcion
		FROM RegistroPrestamoItem r
		INNER JOIN Modulos m			ON m.Id=r.ModuloId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
		WHERE r.Id=@i_id
		AND r.UsuarioId=@usuarioId
		AND m.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND r.Estado=1

		RETURN 0;
	END

	IF(@i_accion='DE')
	BEGIN
		-- VALIDAR QUE EL ELEMENTO EXISTA (ESTADO=1)
		IF NOT EXISTS(SELECT 1 FROM RegistroPrestamoItem WHERE Id=@i_id AND ModoClase=0 AND Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(0, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END
		
		UPDATE RegistroPrestamoItem SET
		Estado=0
		WHERE Id=@i_id
		AND Estado=1

		UPDATE Items SET
		EstadoItemId=1
		WHERE Id=(SELECT ItemId FROM RegistroPrestamoItem WHERE Id=@i_id)
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Nombres=e.Nombres, Apellidos=e.Apellidos,
		Matricula=e.Matricula, Email=e.Email, EstadoDevolucion=ed.Descripcion
		FROM RegistroPrestamoItem r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
		WHERE r.Id=@i_id
		AND e.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND r.Estado=0

		RETURN 0;
	END

	IF(@i_accion='DM')
	BEGIN
		-- VALIDAR QUE EL ELEMENTO EXISTA (ESTADO=1)
		IF NOT EXISTS(SELECT 1 FROM RegistroPrestamoItem WHERE Id=@i_id AND ModoClase=1 AND Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
			VALUES(0, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemClase
			RETURN 0;
		END
		
		UPDATE RegistroPrestamoItem SET
		Estado=0
		WHERE Id=@i_id
		AND Estado=1

		UPDATE Items SET
		EstadoItemId=1
		WHERE Id=(SELECT ItemId FROM RegistroPrestamoItem WHERE Id=@i_id)
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1
		
		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Modulo=m.Nombre, EstadoDevolucion=ed.Descripcion
		FROM RegistroPrestamoItem r
		INNER JOIN Modulos m			ON m.Id=r.ModuloId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
		WHERE r.Id=@i_id
		AND m.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND r.Estado=0

		RETURN 0;
	END

	IF(@i_accion='CE')
	BEGIN
		IF(ISNULL(@i_matricula,'')='')
		BEGIN
			SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
			Inventario=i.Nombre, Item=it.Rfid, Nombres=e.Nombres, Apellidos=e.Apellidos,
			Matricula=e.Matricula, Email=e.Email, EstadoDevolucion=ed.Descripcion
			FROM RegistroPrestamoItem r
			INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
			INNER JOIN Items it				ON it.Id=r.ItemId
			INNER JOIN Inventario i			ON i.Id=it.InventarioId
			INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
			WHERE e.Estado=1
			AND r.UsuarioId=@usuarioId
			AND it.Estado=1
			AND i.Estado=1
			AND r.Estado=1

			RETURN 0;
		END

		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Nombres=e.Nombres, Apellidos=e.Apellidos,
		Matricula=e.Matricula, Email=e.Email, EstadoDevolucion=ed.Descripcion
		FROM RegistroPrestamoItem r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
		WHERE e.Matricula=@i_matricula
		AND r.UsuarioId=@usuarioId
		AND e.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND r.Estado=1
		AND r.ModoClase=0

		RETURN 0;
	END

	IF(@i_accion='CM')
	BEGIN
		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Modulo=m.Nombre, EstadoDevolucion=ed.Descripcion
		FROM RegistroPrestamoItem r
		INNER JOIN Modulos m			ON m.Id=r.ModuloId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
		WHERE m.Nombre=@modulo
		AND r.UsuarioId=@usuarioId
		AND m.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND r.Estado=1
		AND r.ModoClase=1

		RETURN 0;
	END
END
GO