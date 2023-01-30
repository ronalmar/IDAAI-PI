ALTER PROCEDURE sp_usuario
	@i_accion			CHAR(2)				=NULL,
	@i_usuario			VARCHAR(25)			=NULL,
	@i_password			VARCHAR(300)		=NULL,
	@i_passwordAnterior	VARCHAR(300)		=NULL,
	@i_email			VARCHAR(50)			=NULL,
	@i_id				INT					=NULL,
	@i_modulo			VARCHAR(50)			=NULL
AS
BEGIN
	DECLARE 
		@usuario		VARCHAR(25),
		@email			VARCHAR(50),
		@usuarioTrim	VARCHAR(25),
		@emailTrim		VARCHAR(50),
		@moduloActualId	INT,
		@fechaHoy		DATETIME
	
	SET @usuarioTrim		=	TRIM(@i_usuario)
	SET @emailTrim			=	TRIM(@i_email)
	SET @usuario			=	LOWER(@usuarioTrim)
	SET @email				=	LOWER(@emailTrim)
	SET @fechaHoy			=	GETDATE()

	SELECT TOP(1)@moduloActualId=m.Id FROM Modulos m INNER JOIN Usuarios u ON u.Id=m.UsuarioId
	WHERE m.Nombre=@i_modulo AND u.Usuario=@i_usuario AND u.Estado=1 AND m.Estado=1

	DECLARE @TablaUsuario TABLE(
				Id				INT,
				Usuario			VARCHAR(25),
				Email			VARCHAR(50),
				ModuloActual	VARCHAR(50)
			)

	IF(@i_accion = 'LG')
	BEGIN
		IF EXISTS(SELECT u.Id, Usuario, Email, ModuloActual=m.Nombre
			FROM Usuarios u
			LEFT JOIN Modulos m ON m.Id=u.ModuloActualId
			WHERE u.Usuario=@usuario AND Password=@i_password AND u.Estado=1)
		BEGIN
			UPDATE Usuarios SET
			UltimaEntrada = @fechaHoy
			WHERE Id = (SELECT TOP(1) Id FROM Usuarios 
						WHERE Usuario=@usuario AND Password=@i_password AND Estado=1)
		END

		SELECT u.Id, Usuario, Email, ModuloActual=m.Nombre
		FROM Usuarios u
		LEFT JOIN Modulos m ON m.Id=u.ModuloActualId
		WHERE Usuario=@usuario AND Password=@i_password AND u.Estado=1
	END
	IF(@i_accion = 'RG')
	BEGIN
		IF EXISTS (SELECT 1 FROM Usuarios WHERE Usuario=@usuario AND Estado=1)
		BEGIN
			INSERT INTO @TablaUsuario (Id) VALUES (0)

			SELECT Id, Usuario, Email, ModuloActual FROM @TablaUsuario
			RETURN 0;
		END
		IF EXISTS (SELECT 1 FROM Usuarios WHERE Usuario=@usuario AND Estado=0)
		BEGIN
			UPDATE Usuarios SET
			Estado			= 1,
			Usuario			= @usuario,
			Password		= @i_password,
			Email			= @email,
			UltimaEntrada	= @fechaHoy
			WHERE Id=(SELECT TOP(1)Id FROM Usuarios WHERE Usuario=@usuario AND Estado=0)

			DECLARE @idUsuario INT = (SELECT Id FROM Usuarios WHERE Usuario=@usuario AND Estado=1)

			INSERT INTO Modulos (Nombre, Descripcion, PeriodoAcademico, DiasClase, UsuarioId, Estado)
			VALUES('LA', 'Laboratorio Abierto', '', '', @idUsuario, 1)

			INSERT INTO Inventario (Nombre, Descripcion, CantidadDisponible, CantidadTotal, UsuarioId, Estado)
			VALUES('General', 'Inventario general', 0, 0, @idUsuario, 1)

			SELECT u.Id, Usuario, Email, ModuloActual=m.Nombre
			FROM Usuarios u
			LEFT JOIN Modulos m ON m.Id=u.ModuloActualId
			WHERE u.Id=(SELECT Id FROM Usuarios WHERE Usuario=@usuario AND Estado=1)
			AND u.Estado=1

			RETURN 0;
		END

		INSERT INTO Usuarios (Usuario, Password, Email, UltimaEntrada, Estado) VALUES (@usuario, @i_password, 
		CASE	WHEN @email=''	THEN NULL
				WHEN @email!=''	THEN @email
		END, @fechaHoy, 1)

		DECLARE @id INT = @@IDENTITY

		INSERT INTO Modulos (Nombre, Descripcion, PeriodoAcademico, DiasClase, UsuarioId, Estado)
		VALUES('LA', 'Laboratorio Abierto', '', '', @id, 1)

		INSERT INTO Inventario (Nombre, Descripcion, CantidadDisponible, CantidadTotal, UsuarioId, Estado)
			VALUES('General', 'Inventario general', 0, 0, @id, 1)

		SELECT u.Id, Usuario, Email, ModuloActual=m.Nombre
		FROM Usuarios u
		LEFT JOIN Modulos m ON m.Id=u.ModuloActualId
		WHERE u.Id=@id
		AND u.Estado=1
		RETURN 0;
	END
	IF(@i_accion = 'UP')
	BEGIN
		IF EXISTS (SELECT 1 FROM Usuarios WHERE Id=@i_id AND Estado=1)
		BEGIN
			IF EXISTS(SELECT 1 FROM Usuarios WHERE Usuario=@usuario AND Id!=@i_id AND Estado=1)
			BEGIN
				INSERT INTO @TablaUsuario (Id) VALUES (-1)

				SELECT Id, Usuario, Email, ModuloActual FROM @TablaUsuario
				RETURN 0;
			END
			IF ISNULL(@i_passwordAnterior,'')!='' AND NOT EXISTS(SELECT 1 FROM Usuarios WHERE Id=@i_id AND Password=@i_passwordAnterior 
				AND Estado=1)
			BEGIN
				INSERT INTO @TablaUsuario (Id) VALUES (-2)

				SELECT Id, Usuario, Email, ModuloActual FROM @TablaUsuario
				RETURN 0;
			END

			UPDATE Usuarios SET
			Usuario			=	CASE WHEN ISNULL(@usuario, '')='' THEN Usuario ELSE @usuario END,
			Password		=	CASE WHEN ISNULL(@i_password, '')='' THEN Password ELSE @i_password END,
			Email			=	CASE WHEN ISNULL(@email, '')='' THEN @email ELSE @email END
			WHERE Id=@i_id

			SELECT u.Id, Usuario, Email, ModuloActual=m.Nombre
			FROM Usuarios u
			LEFT JOIN Modulos m ON m.Id=u.ModuloActualId
			WHERE u.Id=@i_id
			AND u.Estado=1

			RETURN 0;
		END
		INSERT INTO @TablaUsuario (Id) VALUES (0)

		SELECT Id, Usuario, Email, ModuloActual FROM @TablaUsuario
		RETURN 0;
	END
	IF(@i_accion = 'FE')
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM Usuarios u
			LEFT JOIN Modulos m ON m.Id=u.ModuloActualId
			WHERE u.Usuario=@i_usuario
			AND u.Estado=1)
		BEGIN
			INSERT INTO @TablaUsuario (Id) VALUES (0)

			SELECT Id, Usuario, Email, ModuloActual FROM @TablaUsuario
			RETURN 0;
		END

		UPDATE TOP(1)Usuarios SET
		UltimaEntrada=@fechaHoy
		WHERE Usuario=@i_usuario
		AND Estado=1

		SELECT TOP(1)u.Id, Usuario, Email, ModuloActual=m.Nombre FROM Usuarios u
		LEFT JOIN Modulos m ON m.Id=u.ModuloActualId
		WHERE u.Usuario=@i_usuario
		AND u.Estado=1
	END
	IF(@i_accion = 'DE')
	BEGIN
		IF EXISTS(SELECT 1 FROM Usuarios WHERE Id=@i_id AND Estado=1)
		BEGIN
			UPDATE Usuarios SET
			Estado = 0
			WHERE Id=@i_id AND Estado=1

			SELECT u.Id, Usuario, Email, ModuloActual=m.Nombre FROM Usuarios u
			LEFT JOIN Modulos m ON m.Id=u.ModuloActualId
			WHERE u.Id=@i_id

			RETURN 0;
		END
		SELECT Id, Usuario, Email, ModuloActual FROM @TablaUsuario
		RETURN 0;
	END
	IF(@i_accion = 'OU')
	BEGIN
		SELECT TOP 1 u.Id, Usuario, Email, ModuloActual=m.Nombre FROM Usuarios u
		LEFT JOIN Modulos m ON m.Id=u.ModuloActualId
		WHERE Usuario = @i_usuario
		AND u.Estado = 1
	END
	IF(@i_accion = 'MA')
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM Usuarios WHERE Usuario=@i_usuario AND Estado=1)
		BEGIN
			INSERT INTO @TablaUsuario (Id) VALUES (0)

			SELECT Id, Usuario, Email, ModuloActual FROM @TablaUsuario
			RETURN 0;
		END
		IF(ISNULL(@moduloActualId,0)=0)
		BEGIN
			INSERT INTO @TablaUsuario (Id) VALUES (-1)

			SELECT Id, Usuario, Email, ModuloActual FROM @TablaUsuario
			RETURN 0;
		END
		UPDATE Usuarios SET
		ModuloActualId = ISNULL(@moduloActualId,ModuloActualId)
		WHERE Usuario=@i_usuario
		--AND Id=@i_id
		AND Estado=1

		SELECT u.Id, Usuario, Email, ModuloActual=m.Nombre FROM Usuarios u
		LEFT JOIN Modulos m ON m.Id=u.ModuloActualId
		WHERE Usuario = @i_usuario
		AND u.Estado=1
	END
END
GO