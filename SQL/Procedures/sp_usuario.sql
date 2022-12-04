ALTER PROCEDURE sp_usuario
	@i_accion			CHAR(2)				=NULL,
	@i_usuario			VARCHAR(25)			=NULL,
	@i_password			VARCHAR(300)		=NULL,
	@i_email			VARCHAR(50)			=NULL
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


	IF(@i_accion = 'LG')
	BEGIN
		SELECT Id, Usuario, Email FROM Usuarios
		WHERE Usuario=@usuario AND Password=@i_password
	END
	IF(@i_accion = 'RG')
	BEGIN
		IF EXISTS (SELECT 1 FROM Usuarios WHERE Usuario=@usuario)
		BEGIN
			DECLARE @TablaUsuario TABLE(
				Id			INT,
				Usuario		VARCHAR(25), 
				Email		VARCHAR(50)
			)

			INSERT INTO @TablaUsuario (Id) VALUES (0)

			SELECT Id, Usuario, Email FROM @TablaUsuario
			RETURN 0;
		END

		INSERT INTO Usuarios (Usuario, Password, Email) VALUES (@usuario, @i_password, 
		CASE	WHEN @email=''	THEN NULL
				WHEN @email!=''	THEN @email
		END)

		DECLARE @id INT = @@IDENTITY
		SELECT Id, Usuario, Email FROM Usuarios
		WHERE Id=@id
	END
END
GO