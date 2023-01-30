ALTER PROC	sp_item
		@i_accion					CHAR(2)				=	NULL,
		@i_id						INT					=	NULL,
		@i_estadoItemId				INT					=	NULL,
		@i_rfid						VARCHAR(100)		=	NULL,
		@i_inventario				VARCHAR(100)		=	NULL,
		@i_usuario					VARCHAR(25)			=	NULL,
		@i_grupoItemsDetectados		XML					=	NULL

AS
BEGIN
	DECLARE
	@inventario				VARCHAR(25),
	@inventarioIdInsertar	INT,
	@idInsertado			INT,
	@cantidadItem			INT,
	@cantidadItemDisponible	INT,
	@usuarioId				INT,
	@xmlPuntero				INT

	SET @inventario = TRIM(@i_inventario)
	SELECT @usuarioId=Id FROM Usuarios WHERE Usuario=@i_usuario AND Estado=1
	
	DECLARE @Item Table(
		Id				INT,
		Rfid			VARCHAR(100),
		EstadoItem		VARCHAR(25),
		Inventario		VARCHAR(100)
	)

	DECLARE @ItemGrupoRespuesta Table(
		Id				INT,
		Rfid			VARCHAR(100),
		EstadoItem		VARCHAR(25),
		Inventario		VARCHAR(100)
	)

	DECLARE @ItemsXML Table(
		Rfid			VARCHAR(100)
	)

	IF(@i_accion='IN')
	BEGIN
		-- VALIDACION DE QUE NO EXISTE EL USUARIO INGRESADO
		IF (@usuarioId IS NULL)
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(-2, 'NO EXISTE EL USUARIO INGRESADO')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END
		-- VALIDACION DE QUE NO EXISTE EL INVENTARIO INGRESADO
		IF NOT EXISTS(SELECT 1 FROM Inventario WHERE Nombre=@inventario AND UsuarioId=@usuarioId AND Estado=1)
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(0, 'NO EXISTE EL INVENTARIO INGRESADO')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		-- VALIDACION DE QUE YA EXISTE UN ITEM CON EL RFID INGRESADO
		IF EXISTS(SELECT 1 FROM Items WHERE Rfid=@i_rfid AND UsuarioId=@usuarioId AND Estado=1)
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(-1, 'YA EXISTE UN ITEM CON EL RFID INGRESADO')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END
		
		SELECT @inventarioIdInsertar=Id FROM Inventario WHERE Nombre=@inventario AND UsuarioId=@usuarioId AND Estado=1

		-- VALIDACION DE ACTUALIZAR ESTADO=1 CUANDO EL ITEM CON EL RFID INGRESADO FUE ELIMINADO
		-- LOGICAMENTE
		IF EXISTS(SELECT 1 FROM Items WHERE Rfid=@i_rfid AND Estado=0)
		BEGIN
			UPDATE Items SET
			Estado=1,
			Rfid=@i_rfid,
			EstadoItemId=1,
			InventarioId=@inventarioIdInsertar,
			UsuarioId=@usuarioId
			WHERE Id=(SELECT TOP(1) Id FROM Items WHERE Rfid=@i_rfid AND Estado=0)

			SELECT @cantidadItem=COUNT(*) FROM Items 
			WHERE InventarioId=@inventarioIdInsertar AND UsuarioId=@usuarioId AND Estado=1

			SELECT @cantidadItemDisponible=COUNT(*) FROM Items 
			WHERE InventarioId=@inventarioIdInsertar AND EstadoItemId=1 AND UsuarioId=@usuarioId AND Estado=1

			UPDATE Inventario SET
			CantidadTotal=@cantidadItem,
			CantidadDisponible=@cantidadItemDisponible
			WHERE Id=@inventarioIdInsertar
			AND UsuarioId=@usuarioId

			SELECT TOP(1) Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
			INNER JOIN Inventario i		ON i.Id=it.InventarioId
			INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
			WHERE it.Rfid=@i_rfid
			AND it.UsuarioId=@usuarioId
			AND it.Estado=1
			AND i.Estado=1

			RETURN 0;
		END

		-- INSERCION DE NUEVO ITEM
		INSERT INTO Items
		(
			Rfid, EstadoItemId, InventarioId, UsuarioId, Estado
		)
		SELECT
			@i_rfid, 1, @inventarioIdInsertar, @usuarioId, 1

		SET @idInsertado=@@IDENTITY

		SELECT @cantidadItem=COUNT(*) FROM Items 
		WHERE InventarioId=@inventarioIdInsertar AND UsuarioId=@usuarioId AND Estado=1

		SELECT @cantidadItemDisponible=COUNT(*) FROM Items 
		WHERE InventarioId=@inventarioIdInsertar AND EstadoItemId=1 AND UsuarioId=@usuarioId AND Estado=1

		UPDATE Inventario SET
		CantidadTotal=@cantidadItem,
		CantidadDisponible=@cantidadItemDisponible
		WHERE Id=@inventarioIdInsertar
		AND UsuarioId=@usuarioId

		SELECT Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
		INNER JOIN Inventario i		ON i.Id=it.InventarioId
		INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
		WHERE it.Id=@idInsertado
		AND it.Estado=1
		AND i.Estado=1

		RETURN 0;
	END

	IF(@i_accion='UP')
	BEGIN
		-- VALIDACION DE QUE NO EXISTE EL USUARIO INGRESADO
		IF (@usuarioId IS NULL)
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(-3, 'NO EXISTE EL USUARIO INGRESADO')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END
		-- VALIDACION DE QUE EXISTA EL ITEM CON EL ID INGRESADO
		IF NOT EXISTS(SELECT 1 FROM Items WHERE Id=@i_id AND Estado=1)
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(0, 'NO EXISTE UN ITEM CON EL ID INGRESADO')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		-- VALIDACION DE QUE NO EXISTA OTRO ITEM CON EL RFID INGRESADO
		IF ((ISNULL(@i_rfid,'')!='')AND(EXISTS(SELECT 1 FROM Items WHERE Rfid=@i_rfid AND UsuarioId=@usuarioId AND Id!=@i_id AND Estado=1)))
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(-1, 'YA EXISTE OTRO ITEM CON EL RFID INGRESADO')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		-- VALIDACION DE QUE EL ESTADOITEM INGRESADO SEA VALIDO (1[Disponible] O 2[No Disponible])
		IF ((ISNULL(@i_estadoItemId,0)!=0)AND(NOT EXISTS(SELECT 1 FROM EstadoItem WHERE Id=@i_estadoItemId)))
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(-2, 'EL ESTADO ITEM INGRESADO NO ES VÁLIDO (DEBE SER 1[Disponible] O 2[No Disponible])')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		SELECT @inventarioIdInsertar=Id FROM Inventario WHERE Nombre=@inventario AND UsuarioId=@usuarioId AND Estado=1

		-- ACTUALIZACION DE ITEM
		UPDATE Items SET
		Rfid=CASE ISNULL(@i_rfid,'') WHEN '' THEN Items.Rfid 
			ELSE @i_rfid END,
		EstadoItemId=CASE ISNULL(@i_estadoItemId,0) WHEN 0 THEN Items.EstadoItemId
			ELSE @i_estadoItemId END,
		InventarioId=CASE ISNULL(@inventarioIdInsertar,0) WHEN 0 THEN Items.InventarioId
			ELSE @inventarioIdInsertar END
		WHERE Id=@i_id

		SELECT @cantidadItemDisponible=COUNT(*) FROM Items 
		WHERE InventarioId=@inventarioIdInsertar AND EstadoItemId=1 AND UsuarioId=@usuarioId AND Estado=1

		UPDATE Inventario SET
		CantidadDisponible=@cantidadItemDisponible
		WHERE Id=@inventarioIdInsertar
		AND UsuarioId=@usuarioId
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
		INNER JOIN Inventario i		ON i.Id=it.InventarioId
		INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
		WHERE it.Id=@i_id
		AND it.Estado=1
		AND i.Estado=1

		RETURN 0;
	END

	IF(@i_accion='DE')
	BEGIN
		-- VALIDAR QUE EL ELEMENTO EXISTA (ESTADO=1)
		IF NOT EXISTS(SELECT 1 FROM Items WHERE Id=@i_id AND Estado=1)
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(0, 'NO EXISTE UN ITEM CON EL ID INGRESADO')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		UPDATE Items SET
		Estado=0
		WHERE Id=@i_id
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
		INNER JOIN Inventario i		ON i.Id=it.InventarioId
		INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
		WHERE it.Id=@i_id
		AND it.Estado=0
		AND i.Estado=1

		RETURN 0;
	END

	IF(@i_accion='EI')
	BEGIN
		DECLARE @itemRfidActual					VARCHAR(100)
		DECLARE @ultimaEntradaMasActual			DATETIME
		DECLARE @usuarioActualSeleccionado		VARCHAR(25)
		DECLARE @idInventarioActualGeneral		INT
		DECLARE @idItemDisponible				INT			

		SELECT @ultimaEntradaMasActual=MAX(UltimaEntrada) FROM Usuarios WHERE Estado=1

		IF(@ultimaEntradaMasActual IS NULL)
		BEGIN
			--No existe ninguna ultima fecha de entrada por usuario
			INSERT INTO @Item(Id, Rfid) VALUES(0, 'No existe ninguna última fecha de entrada por usuario')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		SELECT @usuarioActualSeleccionado=Usuario FROM Usuarios 
		WHERE UltimaEntrada=@ultimaEntradaMasActual AND Estado=1		

		IF(ISNULL(@usuarioActualSeleccionado,'')='')
		BEGIN
			--No existe ningun usuario con ultima entrada
			INSERT INTO @Item(Id, Rfid) VALUES(-1, 'No existe ningún usuario con última entrada')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		SELECT @usuarioId=Id FROM Usuarios
		WHERE Usuario=@usuarioActualSeleccionado AND Estado=1

		SELECT TOP(1)@idInventarioActualGeneral=Id FROM Inventario
		WHERE Nombre='General' AND UsuarioId=@usuarioId AND Estado=1

		IF(ISNULL(@idInventarioActualGeneral,0)=0)
		BEGIN
			--No existe ningun id de inventario general para el usuario actual
			INSERT INTO @Item(Id, Rfid) VALUES(-2, 'No existe ningún id de inventario general para el usuario actual')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		UPDATE Items SET
		EstadoItemId=2
		WHERE InventarioId IN (SELECT Id FROM Inventario WHERE Nombre='General' AND Estado=1)
		AND Estado=1

		EXEC SP_XML_PREPAREDOCUMENT	@xmlPuntero OUTPUT, @i_grupoItemsDetectados	

		INSERT INTO @ItemsXML
		SELECT * FROM OPENXML(@xmlPuntero, 'Items/Item', 2)
		WITH(
			Rfid			VARCHAR(100)		'Rfid'
		)

		WHILE(1=1)
		BEGIN
			IF(SELECT COUNT(*) FROM @ItemsXML)=0
				BREAK;

			SELECT TOP(1)@itemRfidActual=Rfid  FROM @ItemsXML

			IF NOT EXISTS(SELECT 1 FROM Items it INNER JOIN Inventario i ON i.Id=it.InventarioId
				WHERE it.Rfid=@itemRfidActual AND it.UsuarioId=@usuarioId AND i.Nombre='General' AND it.Estado=1 and i.Estado=1)
			BEGIN
				INSERT INTO Items VALUES(@itemRfidActual, 1, @idInventarioActualGeneral, @usuarioId, 1)

				SET @idItemDisponible=@@IDENTITY

				INSERT INTO @ItemGrupoRespuesta(Id, Rfid, EstadoItem, Inventario)
				VALUES(@idItemDisponible, @itemRfidActual, 'Disponible', 'General')
			END
			ELSE
			BEGIN
				UPDATE TOP(1)Items SET
				EstadoItemId=1
				WHERE UsuarioId=@usuarioId
				AND Rfid=@itemRfidActual
				AND InventarioId=@idInventarioActualGeneral
				AND Estado=1

				SELECT TOP(1)@idItemDisponible=Id FROM Items
				WHERE UsuarioId=@usuarioId
				AND Rfid=@itemRfidActual
				AND InventarioId=@idInventarioActualGeneral
				AND Estado=1

				INSERT INTO @ItemGrupoRespuesta(Id, Rfid, EstadoItem, Inventario)
				VALUES(@idItemDisponible, @itemRfidActual, 'Disponible', 'General')
			END			

			DELETE TOP(1) FROM @ItemsXML
		END

		IF NOT EXISTS(SELECT 1 FROM @ItemGrupoRespuesta)
		BEGIN
			--No se ingresó ningún item al inventario General
			INSERT INTO @Item(Id, Rfid) VALUES(-3, 'No se ingresó ningún item al inventario General')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT * FROM @ItemGrupoRespuesta

		RETURN 0;
	END

	IF(@i_accion='CN')
	BEGIN
		IF(ISNULL(@i_rfid,'')!='')AND(ISNULL(@inventario,'')='')
		BEGIN
			SELECT Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
			INNER JOIN Inventario i		ON i.Id=it.InventarioId
			INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
			WHERE it.Rfid=@i_rfid
			AND it.UsuarioId=@usuarioId
			AND it.Estado=1
			AND i.Estado=1

			RETURN 0;
		END
		IF(ISNULL(@i_rfid,'')='')AND(ISNULL(@inventario,'')!='')
		BEGIN
			SELECT Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
			INNER JOIN Inventario i		ON i.Id=it.InventarioId
			INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
			WHERE i.Nombre LIKE '%' + @inventario + '%'
			AND it.UsuarioId=@usuarioId
			AND it.Estado=1
			AND i.Estado=1

			RETURN 0;
		END
		ELSE
		BEGIN
			SELECT Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
			INNER JOIN Inventario i		ON i.Id=it.InventarioId
			INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
			WHERE it.Rfid LIKE '%' + @i_rfid + '%'
			AND i.Nombre LIKE '%' + @inventario + '%'
			AND it.UsuarioId=@usuarioId
			AND it.Estado=1
			AND i.Estado=1

			RETURN 0;
		END
	END
END
GO