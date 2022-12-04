ALTER PROCEDURE sp_estudiante
	@i_accion			CHAR(2)				=NULL,
	@i_id				INT					=NULL,
	@i_nombres			VARCHAR(100)		=NULL,
	@i_apellidos		VARCHAR(100)		=NULL,
	@i_matricula		VARCHAR(10)			=NULL,
	@i_email			VARCHAR(50)			=NULL,
	@i_direccion		VARCHAR(100)		=NULL,
	@i_carrera			VARCHAR(100)		=NULL,
	@i_modulo			VARCHAR(50)			=NULL
AS
BEGIN
	DECLARE 
		@idCarrera		INT,
		@idModulo		INT,
		@nombres		VARCHAR(100),
		@apellidos		VARCHAR(100),
		@matricula		VARCHAR(10),
		@email			VARCHAR(50),
		@direccion		VARCHAR(100),
		@carrera		VARCHAR(100),
		@modulo			VARCHAR(50),
		@nombresTrim	VARCHAR(100),
		@apellidosTrim	VARCHAR(100),
		@emailTrim		VARCHAR(50),
		@direccionTrim	VARCHAR(100)
	
	SET @nombresTrim		=	TRIM(@i_nombres)
	SET @apellidosTrim		=	TRIM(@i_apellidos)
	SET @direccionTrim		=	TRIM(@i_direccion)
	SET @emailTrim			=	TRIM(@i_email)
	SET @nombres			=	CASE 
		WHEN @nombresTrim != ''		THEN	UPPER(LEFT(@nombresTrim, 1))	+ LOWER(RIGHT(@nombresTrim,LEN(@nombresTrim)-1))
		WHEN @nombresTrim = ''		THEN	@nombresTrim
		END
	SET @apellidos			=	CASE
		WHEN @apellidosTrim != ''	THEN	UPPER(LEFT(@apellidosTrim, 1))	+ LOWER(RIGHT(@apellidosTrim,LEN(@apellidosTrim)-1))
		WHEN @apellidosTrim = ''	THEN @apellidosTrim
		END
	SET @matricula			=	TRIM(@i_matricula)
	SET @email				=	LOWER(@emailTrim)
	SET @direccion			=	CASE
		WHEN @direccionTrim != ''	THEN	 UPPER(LEFT(@direccionTrim, 1))	+ LOWER(RIGHT(@direccionTrim,LEN(@direccionTrim)-1))
		WHEN @direccionTrim = ''	THEN @direccionTrim
		END
	SET @carrera			=	TRIM(@i_carrera)
	SET @modulo				=	TRIM(@i_modulo)

	DECLARE @Estudiantes TABLE(
				Id			INT, 
				Nombres		VARCHAR(100), 
				Apellidos	VARCHAR(100),
				Matricula	VARCHAR(10),
				Email		VARCHAR(50),
				Direccion	VARCHAR(100),
				Carrera		VARCHAR(100),
				Modulo		VARCHAR(50)
			)

	IF(@i_accion = 'IN')
	BEGIN
		IF EXISTS (SELECT 1 FROM Estudiantes e
						INNER JOIN Modulos m ON m.Id=e.ModuloId
						WHERE Matricula=@matricula	AND  m.Nombre=@modulo)
		BEGIN
			--Ya existe estudiante con esa matricula			
			INSERT INTO @Estudiantes (Id) VALUES (0)

			SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
			FROM @Estudiantes
			RETURN 0;
		END

		SELECT @idCarrera=Id FROM Carreras WHERE Nombre = @carrera
		SELECT @idModulo=Id	FROM Modulos WHERE Nombre = @modulo

		IF(@idCarrera IS NULL)
		BEGIN
			INSERT INTO @Estudiantes (Id) VALUES (-1)

			SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
			FROM @Estudiantes
			RETURN 0;
		END
		IF(@idModulo IS NULL)
		BEGIN
			INSERT INTO @Estudiantes (Id) VALUES (-2)

			SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
			FROM @Estudiantes
			RETURN 0;
		END

		INSERT INTO Estudiantes (Nombres, Apellidos, Matricula, Email, Direccion, CarreraId, ModuloId)
		SELECT @nombres, @apellidos, @matricula, @email, @direccion, @idCarrera, @idModulo

		DECLARE @id INT = @@IDENTITY
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		INNER JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE e.Id=@id
	END
	IF(@i_accion = 'CC')
	BEGIN
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		INNER JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE c.Nombre LIKE '%' + @i_carrera + '%'
		AND m.Nombre=@i_modulo
	END
	IF(@i_accion = 'CE')
	BEGIN
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		INNER JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE Email = @email
		AND m.Nombre=@modulo
	END
	IF(@i_accion = 'CL')
	BEGIN
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		INNER JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE m.Nombre = @modulo
	END
	IF(@i_accion = 'CM')
	BEGIN
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		INNER JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE Matricula = @matricula
		AND m.Nombre=@modulo
	END
	IF(@i_accion = 'CN')
	BEGIN
		IF(@nombres = '' AND @apellidos = '')
		BEGIN
			SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
			Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
			FROM Estudiantes e
			INNER JOIN Modulos m	ON	m.Id=e.ModuloId
			INNER JOIN Carreras c	ON	c.Id=e.CarreraId
			WHERE	Nombres		= @nombres
			AND		Apellidos	= @apellidos
			AND m.Nombre=@modulo
		END
		IF(@nombres = '' AND @apellidos != '')
		BEGIN
			SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
			Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
			FROM Estudiantes e
			INNER JOIN Modulos m	ON	m.Id=e.ModuloId
			INNER JOIN Carreras c	ON	c.Id=e.CarreraId
			WHERE Apellidos LIKE '%' + @apellidos + '%'
			AND m.Nombre=@modulo
		END
		ELSE IF(@nombres != '' AND @apellidos = '')
		BEGIN
			SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
			Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
			FROM Estudiantes e
			INNER JOIN Modulos m	ON	m.Id=e.ModuloId
			INNER JOIN Carreras c	ON	c.Id=e.CarreraId
			WHERE Nombres LIKE '%' + @nombres + '%'
			AND m.Nombre=@modulo
		END
		ELSE
		BEGIN
			SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
			Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
			FROM Estudiantes e
			INNER JOIN Modulos m	ON	m.Id=e.ModuloId
			INNER JOIN Carreras c	ON	c.Id=e.CarreraId
			WHERE (Nombres LIKE '%' + @nombres + '%'
			AND Apellidos LIKE '%' + @apellidos + '%')
			AND m.Nombre=@modulo
		END		
	END
	IF(@i_accion = 'CT')
	BEGIN
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		INNER JOIN Carreras c	ON	c.Id=e.CarreraId
	END
	IF(@i_accion = 'UP')
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM Estudiantes WHERE Id=@i_id)
		BEGIN
			--No existe estudiante con el Id ingresado
			INSERT INTO @Estudiantes (Id) VALUES (0)

			SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
			FROM @Estudiantes
			RETURN 0;
		END

		SELECT @idCarrera=Id FROM Carreras WHERE Nombre = @carrera
		SELECT @idModulo=Id	FROM Modulos WHERE Nombre = @modulo

		IF(@idCarrera IS NULL)
		BEGIN
			SELECT @carrera=c.Nombre 
			FROM Carreras c INNER JOIN Estudiantes e ON e.CarreraId=c.Id
			WHERE e.Id=@i_id
		END
		IF(@idModulo IS NULL)
		BEGIN
			SELECT @modulo=m.Nombre 
			FROM Modulos m INNER JOIN Estudiantes e ON e.ModuloId=m.Id
			WHERE e.Id=@i_id
		END
		UPDATE e SET 
		Nombres		=	CASE	WHEN @nombres='' OR @nombres=NULL THEN e.Nombres 
								ELSE @nombres
								END,
		Apellidos	=	CASE	WHEN @apellidos='' OR @apellidos=NULL THEN e.Apellidos
								ELSE @apellidos
								END,
		Matricula	=	CASE	WHEN @matricula='' OR @matricula=NULL THEN e.Matricula
								ELSE @matricula
								END,
		Email		=	CASE	WHEN @email='' OR @email=NULL THEN e.Email
								ELSE @email
								END,
		Direccion	=	CASE	WHEN @direccion='' OR @direccion=NULL THEN e.Direccion
								ELSE @direccion
								END,
		CarreraId	=	ISNULL(@idCarrera, c.Id),
		ModuloId	=	ISNULL(@idModulo, m.Id)
		FROM Modulos m 
		INNER JOIN Estudiantes e ON e.ModuloId=m.Id
		INNER JOIN Carreras c ON c.Id=e.CarreraId
		WHERE e.Id = @i_id
		AND c.Nombre = CASE		WHEN @carrera='' OR @carrera=NULL THEN c.Nombre
								ELSE @carrera
								END
		AND m.Nombre = CASE		WHEN @modulo='' OR @modulo=NULL THEN m.Nombre
								ELSE @modulo
								END

		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		INNER JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE e.Id=@i_id
	END
END
GO