ALTER PROCEDURE sp_usuario
	@i_accion			CHAR(2)				=NULL,
	@i_usuario			VARCHAR(25)			=NULL,
	@i_password			VARCHAR(300)		=NULL,
	@i_passwordAnterior	VARCHAR(300)		=NULL,
	@i_email			VARCHAR(50)			=NULL,
	@i_id				INT					=NULL
AS
BEGIN
	DECLARE 
		@usuario		VARCHAR(25),
		@email			VARCHAR(50),
		@usuarioTrim	VARCHAR(25),
		@emailTrim		VARCHAR(50)
	
	SET @usuarioTrim		=	TRIM(@i_usuario)
	SET @emailTrim			=	TRIM(@i_email)
	SET @usuario			=	LOWER(@usuarioTrim)
	SET @email				=	LOWER(@emailTrim)

	DECLARE @TablaUsuario TABLE(
				Id			INT,
				Usuario		VARCHAR(25),
				Email		VARCHAR(50)
			)

	IF(@i_accion = 'LG')
	BEGIN
		SELECT Id, Usuario, Email FROM Usuarios
		WHERE Usuario=@usuario AND Password=@i_password AND Estado=1
	END
	IF(@i_accion = 'RG')
	BEGIN
		IF EXISTS (SELECT 1 FROM Usuarios WHERE Usuario=@usuario AND Estado=1)
		BEGIN
			INSERT INTO @TablaUsuario (Id) VALUES (0)

			SELECT Id, Usuario, Email FROM @TablaUsuario
			RETURN 0;
		END
		IF EXISTS (SELECT 1 FROM Usuarios WHERE Usuario=@usuario AND Estado=0)
		BEGIN
			UPDATE Usuarios SET
			Estado		= 1,
			Usuario		= @usuario,
			Password	= @i_password,
			Email		= @email
			WHERE Id=(SELECT 1 FROM Usuarios WHERE Usuario=@usuario AND Estado=0)

			SELECT Id, Usuario, Email FROM Usuarios
			WHERE Id=(SELECT Id FROM Usuarios WHERE Usuario=@usuario AND Estado=1)
			AND Estado=1

			RETURN 0;
		END

		INSERT INTO Usuarios (Usuario, Password, Email, Estado) VALUES (@usuario, @i_password, 
		CASE	WHEN @email=''	THEN NULL
				WHEN @email!=''	THEN @email
		END, 1)

		DECLARE @id INT = @@IDENTITY
		SELECT Id, Usuario, Email FROM Usuarios
		WHERE Id=@id
		AND Estado=1
		RETURN 0;
	END
	IF(@i_accion = 'UP')
	BEGIN
		IF EXISTS (SELECT 1 FROM Usuarios WHERE Id=@i_id AND Estado=1)
		BEGIN
			IF EXISTS(SELECT 1 FROM Usuarios WHERE Usuario=@usuario AND Id!=@i_id AND Estado=1)
			BEGIN
				INSERT INTO @TablaUsuario (Id) VALUES (-1)

				SELECT Id, Usuario, Email FROM @TablaUsuario
				RETURN 0;
			END
			IF ISNULL(@i_passwordAnterior,'')!='' AND NOT EXISTS(SELECT 1 FROM Usuarios WHERE Id=@i_id AND Password=@i_passwordAnterior 
				AND Estado=1)
			BEGIN
				INSERT INTO @TablaUsuario (Id) VALUES (-2)

				SELECT Id, Usuario, Email FROM @TablaUsuario
				RETURN 0;
			END
			UPDATE Usuarios SET
			Usuario			=	CASE WHEN ISNULL(@usuario, '')='' THEN Usuario ELSE @usuario END,
			Password		=	CASE WHEN ISNULL(@i_password, '')='' THEN Password ELSE @i_password END,
			Email			=	CASE WHEN ISNULL(@email, '')='' THEN @email ELSE @email END
			WHERE Id=@i_id
			SELECT Id, Usuario, Email FROM Usuarios WHERE Id=@i_id AND Estado=1

			RETURN 0;
		END
		INSERT INTO @TablaUsuario (Id) VALUES (0)

		SELECT Id, Usuario, Email FROM @TablaUsuario
		RETURN 0;
	END
	IF(@i_accion = 'DE')
	BEGIN
		IF EXISTS(SELECT 1 FROM Usuarios WHERE Id=@i_id AND Estado=1)
		BEGIN
			UPDATE Usuarios SET
			Estado = 0
			WHERE Id=@i_id AND Estado=1

			SELECT Id, Usuario, Email FROM Usuarios WHERE Id=@i_id

			RETURN 0;
		END
		SELECT Id, Usuario, Email FROM @TablaUsuario
		RETURN 0;
	END
	IF(@i_accion = 'OU')
	BEGIN
		SELECT TOP 1 Id, Usuario, Email FROM Usuarios WHERE Usuario = @i_usuario AND Estado = 1
	END
END
GO