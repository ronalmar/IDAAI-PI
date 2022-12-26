ALTER PROC sp_modulo
		@i_accion				CHAR(2)				=	NULL,
		@i_nombre				VARCHAR(50)			=	NULL,
		@i_descripcion			VARCHAR(100)		=	NULL,
		@i_periodoAcademico		VARCHAR(50)			=	NULL,
		@i_id					INT					=	NULL
AS
BEGIN
	DECLARE 
	@nombre						VARCHAR(50),
	@descripcion				VARCHAR(100),
	@descripcionTrim			VARCHAR(100),
	@periodoAcademico			VARCHAR(50),
	@periodoAcademicoTrim	VARCHAR(50),
	@idInsertado				INT

	SET @nombre				=	TRIM(UPPER(@i_nombre))
	SET @descripcionTrim	=	TRIM(@i_descripcion)
	SET @descripcion			=	CASE
		WHEN @descripcionTrim != ''	THEN	 UPPER(LEFT(@descripcionTrim, 1))	+ LOWER(RIGHT(@descripcionTrim,LEN(@descripcionTrim)-1))
		WHEN @descripcionTrim = ''	THEN @descripcionTrim
		END
	SET @periodoAcademicoTrim	=	TRIM(@i_periodoAcademico)
	SET @periodoAcademico		=	UPPER(@periodoAcademicoTrim)

	IF(@i_accion = 'IN')
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM Modulos WHERE Nombre=@nombre AND Estado=1)
		BEGIN
			IF EXISTS(SELECT 1 FROM Modulos WHERE Nombre=@nombre AND Estado=0)
			BEGIN
				UPDATE Modulos SET
				Nombre				= @nombre,
				Descripcion			= @descripcion,
				PeriodoAcademico	= @periodoAcademico,
				Estado = 1
				WHERE Id=(SELECT Id FROM Modulos WHERE Nombre=@nombre AND Estado=0)

				SELECT Id, Nombre, Descripcion, PeriodoAcademico FROM Modulos 
				WHERE Id=(SELECT Id FROM Modulos WHERE Nombre=@nombre AND Estado=1)
				RETURN 0;
			END

			INSERT INTO Modulos VALUES (@nombre, @descripcion, @periodoAcademico, 1)

			SET @idInsertado=@@IDENTITY
			SELECT Id, Nombre, Descripcion, PeriodoAcademico FROM Modulos 
			WHERE Id=@idInsertado
			AND Estado=1

			RETURN 0;
		END
		SELECT Id, Nombre, Descripcion, PeriodoAcademico FROM Modulos WHERE Id=0
		RETURN 0;
	END
	IF(@i_accion = 'UP')
	BEGIN
		IF EXISTS (SELECT 1 FROM Modulos WHERE (Id=@i_id AND Estado=1)AND(@i_id!=1))
		BEGIN
			UPDATE Modulos SET
			Nombre			=	CASE	WHEN EXISTS(SELECT 1 FROM Modulos WHERE Nombre=@nombre)	THEN Nombre
										WHEN @nombre!=''		THEN	@nombre
								ELSE	Nombre				END,
			Descripcion		=	CASE	WHEN @descripcion!=''	THEN	 @descripcion
								ELSE	Descripcion		END,
			PeriodoAcademico=	CASE	WHEN @periodoAcademico!=''	THEN	@periodoAcademico
								ELSE	PeriodoAcademico	END
			WHERE Id=@i_id
			SELECT Id, Nombre, Descripcion, PeriodoAcademico FROM Modulos 
			WHERE Id=@i_id AND Estado=1

			RETURN 0;
		END
		SELECT Id, Nombre, Descripcion FROM Modulos WHERE Id=0
		RETURN 0;
	END
	IF(@i_accion = 'DE')
	BEGIN
		IF EXISTS(SELECT 1 FROM Modulos WHERE (id=@i_id AND Estado=1) AND (@i_id!=1))
		BEGIN
			UPDATE Modulos SET	Estado=0
			WHERE	Id=@i_id AND Estado=1
			SELECT Id, Nombre, Descripcion, PeriodoAcademico FROM Modulos WHERE Id=@i_id
			RETURN 0;
		END
		SELECT Id, Nombre, Descripcion, PeriodoAcademico FROM Modulos WHERE Id=0
		RETURN 0;
	END
	IF(@i_accion = 'CN')
	BEGIN
		SELECT Id, Nombre, Descripcion, PeriodoAcademico FROM Modulos 
		WHERE Nombre LIKE '%' + @nombre + '%'
		AND Estado=1
		RETURN 0;
	END
	IF(@i_accion = 'CP')
	BEGIN
		SELECT Id, Nombre, Descripcion, PeriodoAcademico FROM Modulos 
		WHERE PeriodoAcademico LIKE '%' + @periodoAcademico + '%'
		AND Estado=1
		RETURN 0;
	END
	IF(@i_accion = 'CT')
	BEGIN
		SELECT Id, Nombre, Descripcion, PeriodoAcademico FROM Modulos
		WHERE Estado=1
		RETURN 0;
	END
END
GO