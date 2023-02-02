ALTER PROCEDURE sp_estudiante
	@i_accion			CHAR(2)				=NULL,
	@i_id				INT					=NULL,
	@i_nombres			VARCHAR(100)		=NULL,
	@i_apellidos		VARCHAR(100)		=NULL,
	@i_matricula		VARCHAR(10)			=NULL,
	@i_email			VARCHAR(50)			=NULL,
	@i_direccion		VARCHAR(100)		=NULL,
	@i_carrera			VARCHAR(100)		=NULL,
	@i_modulo			VARCHAR(50)			=NULL,
	@i_xmlEstudiantes	XML					=NULL,
	@i_usuario					VARCHAR(25)		=	NULL
AS
BEGIN
	DECLARE 
		@idCarrera				INT,
		@idModulo				INT,
		@nombres				VARCHAR(100),
		@apellidos				VARCHAR(100),
		@matricula				VARCHAR(10),
		@email					VARCHAR(50),
		@direccion				VARCHAR(100),
		@carrera				VARCHAR(100),
		@modulo					VARCHAR(50),
		@nombresTrim			VARCHAR(100),
		@apellidosTrim			VARCHAR(100),
		@emailTrim				VARCHAR(50),
		@direccionTrim			VARCHAR(100),
		@carreraIdAnterior		INT,
		@xmlPuntero				INT,
		@nombresInsertar		VARCHAR(100),
		@apellidosInsertar		VARCHAR(100),
		@matriculaInsertar		VARCHAR(10),
		@emailInsertar			VARCHAR(50),
		@direccionInsertar		VARCHAR(100),
		@carreraIdInsertar		INT,
		@moduloInsertar			VARCHAR(50),
		@moduloIdInsertar		INT,
		@idInsertado			INT,
		@codigoIdError			INT,
		@usuarioId				INT
	
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
	SELECT @usuarioId=Id FROM Usuarios WHERE Usuario=@i_usuario AND Estado=1

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

	DECLARE @EstudiantesXML TABLE(
				Nombres		VARCHAR(100), 
				Apellidos	VARCHAR(100),
				Matricula	VARCHAR(10),
				Email		VARCHAR(50),
				Modulo		VARCHAR(50),
				Carrera		VARCHAR(100),
				Direccion	VARCHAR(10)
			)

	IF(@i_accion = 'IG')
	BEGIN
		EXEC SP_XML_PREPAREDOCUMENT	@xmlPuntero OUTPUT, @i_xmlEstudiantes	

		SELECT * INTO #ESTUDIANTESXML FROM OPENXML(@xmlPuntero, 'Estudiantes/Estudiante', 2)
		WITH(
			Nombres			VARCHAR(100)		'Nombres',
			Apellidos		VARCHAR(100)		'Apellidos',
			Matricula		VARCHAR(10)			'Matricula',
			Email			VARCHAR(50)			'Email',
			Modulo			VARCHAR(50)			'Modulo',
			Carrera			VARCHAR(100)		'Carrera',
			Direccion		VARCHAR(100)		'Direccion'
		)

		INSERT INTO @EstudiantesXML
		SELECT * FROM #ESTUDIANTESXML
		
		IF(@usuarioId IS NULL)
		BEGIN
			-- VALIDAR QUE EXISTA EL USUARIO INGRESADO
			INSERT INTO @Estudiantes (Id) VALUES (0)
			SELECT * FROM @Estudiantes
			RETURN 0;
		END
		--
			--INSERT INTO @Estudiantes (Id, Nombres) VALUES (-1, 'Usuario')
			--SELECT * FROM @Estudiantes
			--RETURN 0;
		--
		-- VALIDAR QUE EL MODULO EXISTA (ESTADO=1) Y NO SEA LABORATORIO ABIERTO (ID=1)
		IF NOT EXISTS(SELECT 1 FROM Modulos WHERE Nombre=@modulo AND UsuarioId=@usuarioId AND Estado=1)
		BEGIN
			INSERT INTO @Estudiantes (Id, Nombres) VALUES (-1, 'Usuario')
			SELECT * FROM @Estudiantes
			RETURN 0;
		END

		SELECT @moduloInsertar=Nombre FROM Modulos 
			WHERE Nombre=@modulo AND UsuarioId=@usuarioId AND Estado=1
		
		SELECT @moduloIdInsertar=Id	FROM Modulos
			WHERE Nombre=@modulo AND UsuarioId=@usuarioId AND Estado=1
		
		SET @codigoIdError=0
		WHILE(1=1)
		BEGIN			
			IF(SELECT COUNT(*) FROM @EstudiantesXML)=0
				BREAK;

			SELECT TOP(1) @nombresInsertar=Nombres FROM @EstudiantesXML
			SELECT TOP(1) @apellidosInsertar=Apellidos FROM @EstudiantesXML
			SELECT TOP(1) @matriculaInsertar=Matricula FROM @EstudiantesXML
			SELECT TOP(1) @emailInsertar=Email FROM @EstudiantesXML
			SELECT TOP(1) @direccionInsertar=Direccion FROM @EstudiantesXML
			SELECT @carreraIdInsertar=
				(SELECT Id FROM Carreras WHERE Nombre=(SELECT TOP(1)Carrera FROM @EstudiantesXML)
											AND Estado=1)

			-- VALIDAR QUE NO EXISTA UN ESTUDIANTE CON LA MATRICULA ITERADA
			IF EXISTS(SELECT 1 FROM Estudiantes e INNER JOIN Modulos m ON m.Id=e.ModuloId
				WHERE Matricula=(SELECT TOP(1) Matricula FROM @EstudiantesXML)
				AND e.ModuloId=@moduloIdInsertar AND e.Estado=1 AND m.Estado=1)
			BEGIN
				SET @codigoIdError=@codigoIdError-1;

				INSERT INTO @Estudiantes (Id, Nombres, Apellidos, Matricula, Email) 
				SELECT @codigoIdError, @nombresInsertar, @apellidosInsertar, @matriculaInsertar, @emailInsertar

				DELETE TOP(1) FROM @EstudiantesXML
				CONTINUE;
			END

			-- VALIDAR INGRESO LOGICO SI YA EXISTIA ANTES EN DB (UPDATE CUANDO ESTADO=0)
			IF EXISTS(SELECT 1 FROM Estudiantes
				WHERE Matricula=(SELECT TOP(1) Matricula FROM @EstudiantesXML) AND Estado=0)
			BEGIN
				UPDATE Estudiantes SET
				Estado=1,
				Nombres=UPPER(@nombresInsertar),
				Apellidos=UPPER(@apellidosInsertar),
				Matricula=@matriculaInsertar,
				Email=LOWER(@emailInsertar),
				Direccion=CASE ISNULL(@direccionInsertar,'')
					WHEN '' THEN '' ELSE UPPER(@direccionInsertar) END,
				ModuloId=(SELECT Id FROM Modulos WHERE Nombre=@modulo AND UsuarioId=@usuarioId AND Estado=1),
				CarreraId=CASE ISNULL(@carreraIdInsertar,'')
					WHEN '' THEN 0 ELSE @carreraIdInsertar END,
				UsuarioId=@usuarioId
				WHERE Matricula=@matriculaInsertar
				AND Estado=0

				SELECT TOP(1) @idInsertado=Id FROM Estudiantes 
					WHERE Matricula=@matriculaInsertar AND UsuarioId=@usuarioId AND Estado=1

				INSERT INTO @Estudiantes (Id, Nombres, Apellidos, Matricula, 
					Email, Direccion, Carrera, Modulo) 
				SELECT 
					ISNULL(@idInsertado, -99), @nombresInsertar, @apellidosInsertar, @matriculaInsertar, 
					@emailInsertar, ISNULL(@direccionInsertar,''), ISNULL(@carreraIdInsertar,0), 
					@moduloInsertar

				DELETE TOP(1) FROM @EstudiantesXML
				CONTINUE;
			END

			--INSERCION DE NUEVO ESTUDIANTE

			INSERT INTO Estudiantes (Nombres, Apellidos, Matricula, Email, 
				Direccion, CarreraId, ModuloId, UsuarioId, Estado)
			SELECT UPPER(@nombresInsertar), UPPER(@apellidosInsertar), @matriculaInsertar, 
				LOWER(@emailInsertar), UPPER(ISNULL(@direccionInsertar,'')), ISNULL(@idCarrera, 0), 
				@moduloIdInsertar, @usuarioId, 1

			SET @idInsertado=@@IDENTITY

			INSERT INTO @Estudiantes (Id, Nombres, Apellidos, Matricula, 
					Email, Direccion, Carrera, Modulo) 
			SELECT 
				ISNULL(@idInsertado, -99), @nombresInsertar, @apellidosInsertar, @matriculaInsertar, 
				@emailInsertar, ISNULL(@direccionInsertar,''), ISNULL(@carreraIdInsertar,0), 
				@moduloInsertar

			DELETE TOP(1) FROM @EstudiantesXML
		END

		SELECT * FROM @Estudiantes
		RETURN 0;
	END

	IF(@i_accion = 'IN')
	BEGIN
		IF(@usuarioId IS NULL)
		BEGIN
			-- VALIDAR QUE EXISTA EL USUARIO INGRESADO
			INSERT INTO @Estudiantes (Id) VALUES (-3)

			SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
			FROM @Estudiantes
			RETURN 0;
		END
		IF EXISTS (SELECT 1 FROM Estudiantes e
						INNER JOIN Modulos m ON m.Id=e.ModuloId
						WHERE Matricula=@matricula						
						AND  m.Nombre=@modulo
						AND e.UsuarioId=@usuarioId
						AND m.UsuarioId=@usuarioId
						AND e.Estado=1
						AND m.Estado=1)
		BEGIN
			-- VALIDAR QUE NO EXISTA UN ESTUDIANTE CON LA MATRICULA INGRESADA
			INSERT INTO @Estudiantes (Id) VALUES (0)

			SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
			FROM @Estudiantes
			RETURN 0;
		END

		SELECT @idCarrera=Id FROM Carreras WHERE Nombre = @carrera AND UsuarioId=@usuarioId	AND Estado=1
		SELECT @idModulo=Id	FROM Modulos WHERE Nombre = @modulo	AND UsuarioId=@usuarioId	AND Estado=1

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
			ModuloId	=	@idModulo,
			UsuarioId	=	@usuarioId
			WHERE Id=(SELECT TOP(1) e.Id FROM Estudiantes e
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
				WHERE e.Id=(SELECT TOP(1) e.Id FROM Estudiantes e
							INNER JOIN Modulos m ON m.Id=e.ModuloId
							WHERE Matricula=@matricula	
							AND  m.Nombre=@modulo
							AND e.UsuarioId=@usuarioId
							AND e.Estado=1
							AND m.Estado=1)
				RETURN 0;
			END
			SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
			Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
			FROM Estudiantes e
			INNER JOIN Modulos m	ON	m.Id=e.ModuloId
			INNER JOIN Carreras c	ON	c.Id=e.CarreraId
			WHERE e.Id=(SELECT TOP(1) e.Id FROM Estudiantes e
						INNER JOIN Modulos m ON m.Id=e.ModuloId
						WHERE Matricula=@matricula	
						AND  m.Nombre=@modulo
						AND e.UsuarioId=@usuarioId
						AND e.Estado=1
						AND m.Estado=1)

			RETURN 0;
		END

		INSERT INTO Estudiantes (Nombres, Apellidos, Matricula, Email, Direccion, CarreraId, ModuloId, UsuarioId, Estado)
		SELECT @nombres, @apellidos, @matricula, @email, @direccion, @idCarrera, @idModulo, @usuarioId, 1

		DECLARE @id INT = @@IDENTITY

		IF(ISNULL(@idCarrera,'')='')
			BEGIN
				SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
				Email, Direccion, Carrera='', Modulo=m.Nombre
				FROM Estudiantes e
				INNER JOIN Modulos m	ON	m.Id=e.ModuloId
				WHERE e.Id=@id
				RETURN 0;
			END
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
		AND e.UsuarioId=@usuarioId
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
		AND e.UsuarioId=@usuarioId
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
		AND e.UsuarioId=@usuarioId
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
		AND e.UsuarioId=@usuarioId
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
			AND e.UsuarioId=@usuarioId
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
			AND e.UsuarioId=@usuarioId
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
			AND e.UsuarioId=@usuarioId
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
			AND e.UsuarioId=@usuarioId
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
		AND e.UsuarioId=@usuarioId
		AND m.Estado=1

		RETURN 0;
	END
	IF(@i_accion = 'UP')
	BEGIN
		IF (@usuarioId IS NULL)
		BEGIN
			--No existe Usuario con el Id ingresado
			INSERT INTO @Estudiantes (Id) VALUES (-3)

			SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
			FROM @Estudiantes
			RETURN 0;
		END

		IF NOT EXISTS (SELECT 1 FROM Estudiantes WHERE Id=@i_id AND UsuarioId=@usuarioId AND Estado=1)
		BEGIN
			--No existe estudiante con el Id ingresado
			INSERT INTO @Estudiantes (Id) VALUES (0)

			SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
			FROM @Estudiantes
			RETURN 0;
		END		

		IF EXISTS (SELECT 1 FROM Estudiantes e
						INNER JOIN Modulos m ON m.Id=e.ModuloId
						WHERE Matricula=@matricula	
						AND m.Nombre=@modulo
						AND m.UsuarioId=@usuarioId
						AND e.UsuarioId=@usuarioId
						AND e.Id!=@i_id
						AND e.Estado=1
						AND m.Estado=1)
		BEGIN
			-- VALIDAR QUE NO EXISTA UN ESTUDIANTE CON LA MATRICULA INGRESADA
			INSERT INTO @Estudiantes (Id) VALUES (-1)

			SELECT Id, Nombres, Apellidos, Matricula, Email, Direccion, Carrera, Modulo
			FROM @Estudiantes
			RETURN 0;
		END

		SELECT @idCarrera=Id FROM Carreras WHERE Nombre = @carrera AND UsuarioId=@usuarioId	AND Estado=1
		SELECT @idModulo=Id	FROM Modulos WHERE Nombre = @modulo AND UsuarioId=@usuarioId AND Estado=1
		
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
		
		SELECT @carreraIdAnterior=e.CarreraId FROM Estudiantes e
		INNER JOIN Carreras c ON c.Id=e.CarreraId
		WHERE e.Id=@i_id

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
		Email		=	CASE	WHEN ISNULL(@email,'-')='-' THEN e.Email
								ELSE @email
								END,
		Direccion	=	CASE	WHEN ISNULL(@direccion,'-')='-' THEN e.Direccion
								ELSE @direccion
								END,
		CarreraId	=	ISNULL((SELECT Id FROM Carreras WHERE Nombre=@carrera),@carreraIdAnterior),
		ModuloId	=	ISNULL(@idModulo, m.Id)
		FROM Modulos m 
		INNER JOIN Estudiantes e ON e.ModuloId=m.Id
		WHERE e.Id = @i_id
		AND e.UsuarioId=@usuarioId
		AND m.Nombre = CASE		WHEN ISNULL(@modulo,'')='' THEN m.Nombre
								ELSE @modulo
								END

		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera = CASE WHEN c.Estado=1 THEN c.Nombre ELSE NULL END, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		LEFT JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE e.Id=@i_id
		AND e.UsuarioId=@usuarioId
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
			AND m.Estado=1

			RETURN 0;
		END
		
		SELECT * FROM @Estudiantes
		RETURN 0;
	END
END
GO