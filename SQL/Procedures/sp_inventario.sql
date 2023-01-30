ALTER PROC sp_inventario
		@i_accion				CHAR(2)				=	NULL,
		@i_id					INT					=	NULL,
		@i_nombre				VARCHAR(100)		=	NULL,
		@i_descripcion			VARCHAR(300)		=	NULL,
		@i_cantidadTotal		INT					=	NULL,
		@i_cantidadDisponible	INT					=	NULL,
		@i_modulo				VARCHAR(50)			=	NULL,
		@i_accionExt			BIT					=	NULL,
		@i_usuario				VARCHAR(25)			=	NULL
AS
BEGIN
	DECLARE
	@nombre						VARCHAR(100),
	@descripcion				VARCHAR(300),
	@modulo						VARCHAR(50),
	@moduloIdInsertar			INT,
	@idInsertado				INT,
	@idInventarioSuperior		INT,
	@cantidadItems				INT,
	@cantidadItemsDisponibles	INT,
	@usuarioId					INT

	SET	@nombre			=	TRIM(@i_nombre)
	SET	@descripcion	=	TRIM(@i_descripcion)
	SET @modulo			=	TRIM(@i_modulo)
	SELECT @usuarioId=Id FROM Usuarios WHERE Usuario=@i_usuario AND Estado=1
	
	DECLARE @Inventario		TABLE(
		Id					INT,
		Nombre				VARCHAR(100),
		Descripcion			VARCHAR(300),
		CantidadDisponible	INT,
		CantidadTotal		INT
	)

	DECLARE @InventarioVerificacion	TABLE(
		Id					INT,
		Nombre				VARCHAR(100),
		Descripcion			VARCHAR(300),
		CantidadDisponible	INT,
		CantidadTotal		INT
	)

	IF(@i_accion='CN')
	BEGIN
		SELECT Id=i.Id, Nombre=i.Nombre, Descripcion=i.Descripcion, 
		CantidadDisponible=i.CantidadDisponible, CantidadTotal=i.CantidadTotal
		FROM Inventario i
		WHERE i.Nombre	LIKE	'%' + @nombre + '%'
		AND UsuarioId=@usuarioId
		AND i.Estado=1
		RETURN 0;
	END
	IF(@i_accion='IN')
	BEGIN
		IF(@usuarioId IS NULL)
		BEGIN
			INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(-1, 0, 0)
			SELECT TOP(1) * FROM @Inventario
			RETURN 0;
		END
		-- VALIDACION DE QUE NO EXISTA REGISTRO EN INVENTARIO CON EL MISMO NOMBRE
		IF EXISTS(SELECT 1 FROM Inventario WHERE Nombre=@nombre AND UsuarioId=@usuarioId AND Estado=1)
		BEGIN
			INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(0, 0, 0)
			SELECT TOP(1) * FROM @Inventario
			RETURN 0;
		END

		-- VALIDACION DE REGISTRO LOGICO (ESTADO=1) SI YA EXISTE PERO ESTABA ELIMINADO LOGICAMENTE
		IF EXISTS(SELECT 1 FROM Inventario WHERE Nombre=@nombre AND Estado=0)
		BEGIN
			UPDATE Inventario SET
			Estado=1,
			Nombre=@nombre,
			Descripcion=@descripcion,
			CantidadTotal=ISNULL(@i_cantidadTotal,0),
			CantidadDisponible=ISNULL(@i_cantidadTotal,0),
			UsuarioId=@usuarioId
			WHERE Id=(SELECT TOP(1) Id FROM Inventario WHERE Nombre=@nombre AND Estado=0)

			SELECT TOP(1) @idInsertado=Id FROM Inventario WHERE Nombre=@nombre AND UsuarioId=@usuarioId AND Estado=1

			SELECT Id=i.Id, Nombre=i.Nombre, Descripcion=i.Descripcion, 
			CantidadDisponible=i.CantidadDisponible, CantidadTotal=i.CantidadTotal
			FROM Inventario i
			WHERE i.Id=@idInsertado
			AND i.Estado=1

			RETURN 0;
		END	

		-- INSERCION NUEVO REGISTRO EN INVENTARIO
		INSERT INTO Inventario
		(
			Nombre, Descripcion, CantidadTotal, CantidadDisponible, UsuarioId, Estado
		)
		SELECT	
			@nombre, @descripcion, ISNULL(@i_cantidadTotal,0), ISNULL(@i_cantidadTotal,0), @usuarioId, 1
		
		SET @idInsertado=@@IDENTITY

		SELECT Id=i.Id, Nombre=i.Nombre, Descripcion=i.Descripcion, 
		CantidadDisponible=i.CantidadDisponible, CantidadTotal=i.CantidadTotal
		FROM Inventario i
		WHERE i.Id=@idInsertado
		AND i.Estado=1

		RETURN 0;
	END
	IF(@i_accion='UP')
	BEGIN
		IF(@usuarioId IS NULL)
		BEGIN
			INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(-2, 0, 0)
			SELECT TOP(1) * FROM @Inventario
			RETURN 0;
		END
		-- VALIDACION DE QUE EXISTA UN INVENTARIO CON EL ID INGRESADO
		IF NOT EXISTS(SELECT 1 FROM Inventario WHERE Id=@i_id AND Estado=1)
		BEGIN
			INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(0, 0, 0)
			SELECT TOP(1) * FROM @Inventario
			RETURN 0;
		END

		-- VALIDAR QUE EL ELEMENTO NO SEA EL INVENTARIO GENERAL
		IF EXISTS(SELECT 1 FROM Inventario WHERE Id=@i_id AND Nombre='General' AND Estado=1)
		BEGIN
			INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(-3, 0, 0)
			SELECT TOP(1) * FROM @Inventario
			RETURN 0;
		END

		-- VALIDACION DE QUE NO EXISTA OTRO INVENTARIO CON EL NOMBRE INGRESADO
		IF ((ISNULL(@nombre,'')!='')AND(EXISTS(SELECT 1 FROM Inventario WHERE Nombre=@nombre AND UsuarioId=@usuarioId AND Id!=@i_id AND Estado=1)))
		BEGIN
			INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(-1, 0, 0)
			SELECT TOP(1) * FROM @Inventario
			RETURN 0;
		END		

		-- ACTUALIZACION DE REGISTRO DE INVENTARIO
		UPDATE Inventario SET
		Nombre=CASE ISNULL(@nombre,'') WHEN '' THEN Inventario.Nombre 
			ELSE @nombre END,
		Descripcion=CASE ISNULL(@descripcion,'') WHEN '' THEN Inventario.Descripcion 
			ELSE @descripcion END,
		CantidadDisponible=CASE ISNULL(@i_cantidadDisponible,'') WHEN '' THEN Inventario.CantidadDisponible 
			ELSE @i_cantidadDisponible END,
		CantidadTotal=CASE ISNULL(@i_cantidadTotal,'') WHEN '' THEN Inventario.CantidadTotal 
			ELSE @i_cantidadTotal END
		WHERE Id=@i_id
		AND Estado=1

		SELECT Id=i.Id, Nombre=i.Nombre, Descripcion=i.Descripcion, 
		CantidadDisponible=i.CantidadDisponible, CantidadTotal=i.CantidadTotal
		FROM Inventario i
		WHERE i.Id=@i_id
		AND i.Estado=1

		RETURN 0;
	END

	IF(@i_accion='DE')
	BEGIN
		-- VALIDAR QUE EL ELEMENTO EXISTA (ESTADO=1)
		IF NOT EXISTS(SELECT 1 FROM Inventario WHERE Id=@i_id AND Estado=1)
		BEGIN
			INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(0, 0, 0)
			SELECT TOP(1) * FROM @Inventario
			RETURN 0;
		END

		-- VALIDAR QUE EL ELEMENTO NO SEA EL INVENTARIO GENERAL
		IF EXISTS(SELECT 1 FROM Inventario WHERE Id=@i_id AND Nombre='General' AND Estado=1)
		BEGIN
			INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(-1, 0, 0)
			SELECT TOP(1) * FROM @Inventario
			RETURN 0;
		END

		UPDATE Inventario SET
		Estado=0
		WHERE Id=@i_id
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=i.Id, Nombre=i.Nombre, Descripcion=i.Descripcion, 
		CantidadDisponible=i.CantidadDisponible, CantidadTotal=i.CantidadTotal
		FROM Inventario i
		WHERE i.Id=@i_id
		AND i.Estado=0

		RETURN 0;
	END

	-- VERIFICACION Y ACTUALIZACION DE CANTIDADES TOTALES Y DISPONIBLES EN INVENTARIO
	IF(@i_accion='VE')
	BEGIN
		INSERT INTO @InventarioVerificacion
		SELECT Id, Nombre, Descripcion, CantidadDisponible, CantidadTotal
		FROM Inventario WHERE Estado=1

		WHILE(1=1)
		BEGIN
			IF(SELECT COUNT(*) FROM @InventarioVerificacion)=0
				BREAK;
			
			SELECT TOP(1) @idInventarioSuperior=Id FROM @InventarioVerificacion

			SELECT @cantidadItems=COUNT(*) FROM Items 
			WHERE InventarioId=@idInventarioSuperior AND Estado=1

			SELECT @cantidadItemsDisponibles=COUNT(*) FROM Items 
			WHERE InventarioId=@idInventarioSuperior AND EstadoItemId=1 AND Estado=1

			UPDATE Inventario SET
			CantidadTotal=@cantidadItems,
			CantidadDisponible=@cantidadItemsDisponibles
			WHERE Id=@idInventarioSuperior

			DELETE TOP(1) FROM @InventarioVerificacion
		END

		IF(@i_accionExt=1)
			RETURN 0;

		INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(0, 0, 0)
		SELECT TOP(1) * FROM @Inventario
		
		RETURN 0;
	END
END
GO