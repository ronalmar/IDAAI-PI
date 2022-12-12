ALTER PROC sp_modulo
		@i_accion				CHAR(2)				=	NULL,
		@i_nombre				VARCHAR(50)			=	NULL,
		@i_descripcion			VARCHAR(100)		=	NULL,
		@i_id					INT					=	NULL
AS
BEGIN
	DECLARE 
	@nombre				VARCHAR(50),
	@descripcion		VARCHAR(100),
	@descripcionTrim	VARCHAR(100),
	@idInsertado		INT

	SET @nombre				=	TRIM(UPPER(@i_nombre))
	SET @descripcionTrim	=	TRIM(@i_descripcion)
	SET @descripcion			=	CASE
		WHEN @descripcionTrim != ''	THEN	 UPPER(LEFT(@descripcionTrim, 1))	+ LOWER(RIGHT(@descripcionTrim,LEN(@descripcionTrim)-1))
		WHEN @descripcionTrim = ''	THEN @descripcionTrim
		END

	IF(@i_accion = 'IN')
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM Modulos WHERE Nombre=@nombre)
		BEGIN
			INSERT INTO Modulos VALUES (@nombre, @descripcion, 1)

			SET @idInsertado=@@IDENTITY
			SELECT Id, Nombre, Descripcion FROM Modulos WHERE Id=@idInsertado AND Estado=1

			RETURN 0;
		END
		SELECT Id, Nombre, Descripcion FROM Modulos WHERE Id=0
	END
	IF(@i_accion = 'UP')
	BEGIN
		IF EXISTS (SELECT 1 FROM Modulos WHERE Id=@i_id AND Estado=1)
		BEGIN
			UPDATE Modulos SET
			Nombre			=	CASE	WHEN EXISTS(SELECT 1 FROM Modulos WHERE Nombre=@nombre)	THEN Nombre
										WHEN @nombre!=''		THEN	@nombre
								ELSE	Nombre				END,
			Descripcion		=	CASE	WHEN @descripcion!=''	THEN	 @descripcion
								ELSE	Descripcion		END
			WHERE Id=@i_id
			SELECT Id, Nombre, Descripcion FROM Modulos WHERE Id=@i_id AND Estado=1

			RETURN 0;
		END
		SELECT Id, Nombre, Descripcion FROM Modulos WHERE Id=0
	END
	IF(@i_accion = 'CN')
	BEGIN
		SELECT Id, Nombre, Descripcion FROM Modulos 
		WHERE Nombre LIKE '%' + @nombre + '%'
		AND Estado=1
	END
	IF(@i_accion = 'CT')
	BEGIN
		SELECT Id, Nombre, Descripcion FROM Modulos
		Where Estado=1
	END
END
GO