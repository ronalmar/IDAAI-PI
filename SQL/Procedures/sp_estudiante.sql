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
						WHERE Matricula=@matricula	
						AND  m.Nombre=@modulo
						AND e.Estado=1
						AND m.Estado=1)
		BEGIN
			--Ya existe un estudiante con esa matricula
			INSERT INTO @Estudiantes (Id) VALUES (0)

			SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
			FROM @Estudiantes
			RETURN 0;
		END

		SELECT @idCarrera=Id FROM Carreras WHERE Nombre = @carrera	AND Estado=1
		SELECT @idModulo=Id	FROM Modulos WHERE Nombre = @modulo		AND Estado=1

		--IF(@idCarrera IS NULL)
		--BEGIN
		--	INSERT INTO @Estudiantes (Id) VALUES (-1)

		--	SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
		--	FROM @Estudiantes
		--	RETURN 0;
		--END
		IF(@idModulo IS NULL)
		BEGIN
			INSERT INTO @Estudiantes (Id) VALUES (-2)

			SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
			FROM @Estudiantes
			RETURN 0;
		END

		IF EXISTS(SELECT 1 FROM Estudiantes e
						INNER JOIN Modulos m ON m.Id=e.ModuloId
						WHERE Matricula=@matricula	
						AND  m.Nombre=@modulo
						AND e.Estado=0
						AND m.Estado=1)
		BEGIN
			UPDATE Estudiantes SET
			Estado		=	1,
			Nombres		=	@nombres,
			Apellidos	=	@apellidos,
			Matricula	=	@matricula,
			Email		=	@email,
			Direccion	=	@direccion,
			CarreraId	=	@idCarrera,
			ModuloId	=	@idModulo
			WHERE Id=(SELECT e.Id FROM Estudiantes e
						INNER JOIN Modulos m ON m.Id=e.ModuloId
						WHERE Matricula=@matricula	
						AND  m.Nombre=@modulo
						AND e.Estado=0
						AND m.Estado=1)
			IF(ISNULL(@idCarrera,'')='')
			BEGIN
				SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
				Email, Direccion, Carrera='', Modulo=m.Nombre
				FROM Estudiantes e
				INNER JOIN Modulos m	ON	m.Id=e.ModuloId
				WHERE e.Id=(SELECT e.Id FROM Estudiantes e
							INNER JOIN Modulos m ON m.Id=e.ModuloId
							WHERE Matricula=@matricula	
							AND  m.Nombre=@modulo
							AND e.Estado=1
							AND m.Estado=1)
				RETURN 0;
			END
			SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
			Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
			FROM Estudiantes e
			INNER JOIN Modulos m	ON	m.Id=e.ModuloId
			INNER JOIN Carreras c	ON	c.Id=e.CarreraId
			WHERE e.Id=(SELECT e.Id FROM Estudiantes e
						INNER JOIN Modulos m ON m.Id=e.ModuloId
						WHERE Matricula=@matricula	
						AND  m.Nombre=@modulo
						AND e.Estado=1
						AND m.Estado=1)

			RETURN 0;
		END

		INSERT INTO Estudiantes (Nombres, Apellidos, Matricula, Email, Direccion, CarreraId, ModuloId, Estado)
		SELECT @nombres, @apellidos, @matricula, @email, @direccion, @idCarrera, @idModulo, 1

		DECLARE @id INT = @@IDENTITY
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		INNER JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE e.Id=@id

		RETURN 0;
	END
	IF(@i_accion = 'CC')
	BEGIN
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera= CASE WHEN c.Estado=1 THEN c.Nombre ELSE NULL END, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		LEFT JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE c.Nombre LIKE '%' + @i_carrera + '%'
		AND m.Nombre=@i_modulo
		AND e.Estado=1
		AND m.Estado=1

		RETURN 0;
	END
	IF(@i_accion = 'CE')
	BEGIN
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera= CASE WHEN c.Estado=1 THEN c.Nombre ELSE NULL END, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		LEFT JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE Email = @email
		AND m.Nombre=@modulo
		AND e.Estado=1
		AND m.Estado=1

		RETURN 0;
	END
	IF(@i_accion = 'CL')
	BEGIN
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera= CASE WHEN c.Estado=1 THEN c.Nombre ELSE NULL END, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		LEFT JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE m.Nombre = @modulo
		AND e.Estado=1
		AND m.Estado=1

		RETURN 0;
	END
	IF(@i_accion = 'CM')
	BEGIN
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera= CASE WHEN c.Estado=1 THEN c.Nombre ELSE NULL END, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		LEFT JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE Matricula = @matricula
		AND m.Nombre=@modulo
		AND e.Estado=1
		AND m.Estado=1

		RETURN 0;
	END
	IF(@i_accion = 'CN')
	BEGIN
		IF(@nombres = '' AND @apellidos = '')
		BEGIN
			SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
			Email, Direccion, Carrera= CASE WHEN c.Estado=1 THEN c.Nombre ELSE NULL END, Modulo=m.Nombre
			FROM Estudiantes e
			INNER JOIN Modulos m	ON	m.Id=e.ModuloId
			LEFT JOIN Carreras c	ON	c.Id=e.CarreraId
			WHERE	Nombres		= @nombres
			AND		Apellidos	= @apellidos
			AND m.Nombre=@modulo
			AND e.Estado=1
			AND m.Estado=1

			RETURN 0;
		END
		IF(@nombres = '' AND @apellidos != '')
		BEGIN
			SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
			Email, Direccion, Carrera= CASE WHEN c.Estado=1 THEN c.Nombre ELSE NULL END, Modulo=m.Nombre
			FROM Estudiantes e
			INNER JOIN Modulos m	ON	m.Id=e.ModuloId
			LEFT JOIN Carreras c	ON	c.Id=e.CarreraId
			WHERE Apellidos LIKE '%' + @apellidos + '%'
			AND m.Nombre=@modulo
			AND e.Estado=1
			AND m.Estado=1

			RETURN 0;
		END
		ELSE IF(@nombres != '' AND @apellidos = '')
		BEGIN
			SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
			Email, Direccion, Carrera= CASE WHEN c.Estado=1 THEN c.Nombre ELSE NULL END, Modulo=m.Nombre
			FROM Estudiantes e
			INNER JOIN Modulos m	ON	m.Id=e.ModuloId
			LEFT JOIN Carreras c	ON	c.Id=e.CarreraId
			WHERE Nombres LIKE '%' + @nombres + '%'
			AND m.Nombre=@modulo
			AND e.Estado=1
			AND m.Estado=1

			RETURN 0;
		END
		ELSE
		BEGIN
			SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
			Email, Direccion, Carrera= CASE WHEN c.Estado=1 THEN c.Nombre ELSE NULL END, Modulo=m.Nombre
			FROM Estudiantes e
			INNER JOIN Modulos m	ON	m.Id=e.ModuloId
			LEFT JOIN Carreras c	ON	c.Id=e.CarreraId
			WHERE (Nombres LIKE '%' + @nombres + '%'
			AND Apellidos LIKE '%' + @apellidos + '%')
			AND m.Nombre=@modulo
			AND e.Estado=1
			AND m.Estado=1

			RETURN 0;
		END		
	END
	IF(@i_accion = 'CT')
	BEGIN
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera = CASE WHEN c.Estado=1 THEN c.Nombre ELSE NULL END, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		LEFT JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE e.Estado=1
		AND m.Estado=1

		RETURN 0;
	END
	IF(@i_accion = 'UP')
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM Estudiantes WHERE Id=@i_id AND Estado=1)
		BEGIN
			--No existe estudiante con el Id ingresado
			INSERT INTO @Estudiantes (Id) VALUES (0)

			SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
			FROM @Estudiantes
			RETURN 0;
		END

		SELECT @idCarrera=Id FROM Carreras WHERE Nombre = @carrera	AND Estado=1
		SELECT @idModulo=Id	FROM Modulos WHERE Nombre = @modulo		AND Estado=1
		
		--IF(@idCarrera IS NULL AND ISNULL(@carrera, '')!='')
		--BEGIN
		--	--La carrera del estudiante no es válida
		--	INSERT INTO @Estudiantes (Id) VALUES (-1)

		--	SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
		--	FROM @Estudiantes
		--	RETURN 0;
		--END

		IF(@idModulo IS NULL AND ISNULL(@modulo, '')!='')
		BEGIN
			--El módulo del estudiante no es válido
			INSERT INTO @Estudiantes (Id) VALUES (-2)

			SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
			FROM @Estudiantes
			RETURN 0;
		END

		UPDATE e SET 
		Nombres		=	CASE	WHEN ISNULL(@nombres,'')='' THEN e.Nombres
								ELSE @nombres
								END,
		Apellidos	=	CASE	WHEN ISNULL(@apellidos,'')='' THEN e.Apellidos
								ELSE @apellidos
								END,
		Matricula	=	CASE	WHEN ISNULL(@matricula,'')='' THEN e.Matricula
								ELSE @matricula
								END,
		Email		=	CASE	WHEN ISNULL(@email,'')='' THEN e.Email
								ELSE @email
								END,
		Direccion	=	CASE	WHEN ISNULL(@direccion,'')='' THEN e.Direccion
								ELSE @direccion
								END,
		CarreraId	=	ISNULL((SELECT Id FROM Carreras WHERE Nombre=@carrera),''),
		ModuloId	=	ISNULL(@idModulo, m.Id)
		FROM Modulos m 
		INNER JOIN Estudiantes e ON e.ModuloId=m.Id
		WHERE e.Id = @i_id
		AND m.Nombre = CASE		WHEN ISNULL(@modulo,'')='' THEN m.Nombre
								ELSE @modulo
								END

		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera = CASE WHEN c.Estado=1 THEN c.Nombre ELSE NULL END, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		LEFT JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE e.Id=@i_id
		AND e.Estado=1
		AND m.Estado=1

		RETURN 0;
	END
	IF(@i_accion = 'DE')
	BEGIN
		IF EXISTS(SELECT 1 FROM Estudiantes e INNER JOIN Modulos m ON m.Id=e.ModuloId 
		WHERE e.Id=@i_id 
		AND e.Estado=1
		AND m.Estado=1)
		BEGIN
			UPDATE Estudiantes SET
			Estado = 0
			WHERE Id = (SELECT e.Id FROM Estudiantes e INNER JOIN Modulos m ON m.Id=e.ModuloId 
			WHERE e.Id=@i_id 
			AND e.Estado=1
			AND m.Estado=1)			
			
			IF(ISNULL((SELECT CarreraId FROM Estudiantes WHERE Id=@i_id),'')='')
			BEGIN
				SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
				Email, Direccion, Carrera='', Modulo=m.Nombre
				FROM Estudiantes e
				INNER JOIN Modulos m	ON	m.Id=e.ModuloId
				WHERE e.Id=@i_id
				AND e.Estado=0
				AND m.Estado=1
				RETURN 0;
			END

			SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
			Email, Direccion, Carrera = CASE WHEN c.Estado=1 THEN c.Nombre ELSE NULL END, Modulo=m.Nombre
			FROM Estudiantes e
			INNER JOIN Modulos m	ON	m.Id=e.ModuloId
			INNER JOIN Carreras c	ON	c.Id=e.CarreraId
			WHERE e.Id=@i_id
			AND e.Estado=0
			AND m.Estado=0

			RETURN 0;
		END
		
		SELECT * FROM @Estudiantes
		RETURN 0;
	END
END
GO