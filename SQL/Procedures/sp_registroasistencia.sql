ALTER PROCEDURE sp_registroasistencia
	@i_accion					CHAR(2)				=NULL,
	@i_tipoAccion				CHAR(2)				=NULL,
	@i_idRegistroAsistencia		INT					=NULL,
	@i_fecha					DATETIME			=NULL,
	@i_nombres					VARCHAR(100)		=NULL,
	@i_apellidos				VARCHAR(100)		=NULL,
	@i_matricula				VARCHAR(10)			=NULL,
	@i_carrera					VARCHAR(100)		=NULL,
	@i_modulo					VARCHAR(50)			=NULL,
	@i_usuario					VARCHAR(25)			=NULL
AS 
BEGIN
	DECLARE 
		@idCarrera		INT,
		@idModulo		INT,
		@estudianteId	INT,
		@idInsertado	INT,
		@nombres		VARCHAR(100),
		@apellidos		VARCHAR(100),
		@matricula		VARCHAR(10),
		@carrera		VARCHAR(100),
		@modulo			VARCHAR(50),
		@nombresTrim	VARCHAR(100),
		@apellidosTrim	VARCHAR(100),
		@usuarioId					INT
	
	SET @nombresTrim		=	TRIM(@i_nombres)
	SET @apellidosTrim		=	TRIM(@i_apellidos)
	SET @nombres			=	CASE 
		WHEN @nombresTrim != ''		THEN	UPPER(LEFT(@nombresTrim, 1))	+ LOWER(RIGHT(@nombresTrim,LEN(@nombresTrim)-1))
		WHEN @nombresTrim = ''		THEN	@nombresTrim
		END
	SET @apellidos			=	CASE
		WHEN @apellidosTrim != ''	THEN	UPPER(LEFT(@apellidosTrim, 1))	+ LOWER(RIGHT(@apellidosTrim,LEN(@apellidosTrim)-1))
		WHEN @apellidosTrim = ''	THEN @apellidosTrim
		END
	SET @matricula			=	TRIM(@i_matricula)	
	SET @carrera			=	TRIM(@i_carrera)
	SET @modulo				=	TRIM(@i_modulo)
	SELECT @usuarioId=Id FROM Usuarios WHERE Usuario=@i_usuario AND Estado=1

	DECLARE @RegistroAsistencia TABLE(
				Id					INT, 
				IdEstudiante		INT,
				Nombres				VARCHAR(100), 
				Apellidos			VARCHAR(100),
				Matricula			VARCHAR(10),
				Email				VARCHAR(50),
				Fecha				DATETIME,
				EstadoAsistencia	VARCHAR(25),
				Carrera				VARCHAR(100),
				Modulo				VARCHAR(50)
			)	

	IF(@i_accion='IA')
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM Estudiantes e
						INNER JOIN Modulos m ON m.Id=e.ModuloId
						WHERE Matricula=@matricula
						AND e.UsuarioId=@usuarioId
						AND m.UsuarioId=@usuarioId
						AND m.Nombre=@modulo
						AND e.Estado=1
						AND m.Estado=1)
		BEGIN
			--Matricula no existe
			SELECT * FROM @RegistroAsistencia
			RETURN 0;
		END
		SELECT @estudianteId=e.Id FROM Estudiantes e
		INNER JOIN Modulos m ON m.Id=e.ModuloId 
		WHERE Matricula=@matricula 
		AND m.Nombre=@modulo
		AND e.UsuarioId=@usuarioId
		AND m.UsuarioId=@usuarioId
		AND e.Estado=1
		AND m.Estado=1

		INSERT INTO RegistroAsistencia (Fecha, EstudianteId, EstadoAsistenciaId, UsuarioId, Estado)
		SELECT @i_fecha, @estudianteId, 1, @usuarioId, 1

		SET @idInsertado = @@IDENTITY
		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres, Apellidos, Matricula, 
		Email, Fecha=r.Fecha,EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m			ON	m.Id=e.ModuloId
		LEFT JOIN Carreras c			ON	c.Id=e.CarreraId
		INNER JOIN RegistroAsistencia r	ON	r.EstudianteId=e.Id
		INNER JOIN EstadoAsistencia ea	ON	ea.Id=r.EstadoAsistenciaId
		WHERE r.Id=@idInsertado		
	END
	IF(@i_accion='IF')
	BEGIN
	IF NOT EXISTS(SELECT 1 FROM Estudiantes e
						INNER JOIN Modulos m ON m.Id=e.ModuloId
						WHERE Matricula=@matricula	
						AND m.Nombre=@modulo
						AND e.UsuarioId=@usuarioId
						AND m.UsuarioId=@usuarioId
						AND e.Estado=1
						AND m.Estado=1)
		BEGIN
			--Matricula no existe
			SELECT * FROM @RegistroAsistencia
			RETURN 0;
		END
		SELECT @estudianteId=e.Id FROM Estudiantes e
		INNER JOIN Modulos m ON m.Id=e.ModuloId 
		WHERE Matricula=@matricula 
		AND m.Nombre=@modulo
		AND e.UsuarioId=@usuarioId
		AND m.UsuarioId=@usuarioId
		AND e.Estado=1
		AND m.Estado=1

		INSERT INTO RegistroAsistencia (Fecha, EstudianteId, EstadoAsistenciaId, UsuarioId, Estado)
		SELECT @i_fecha, @estudianteId, 2, @usuarioId, 1

		SET @idInsertado = @@IDENTITY
		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres, Apellidos, Matricula, 
		Email, Fecha=r.Fecha,EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m			ON	m.Id=e.ModuloId
		LEFT JOIN Carreras c			ON	c.Id=e.CarreraId
		INNER JOIN RegistroAsistencia r	ON	r.EstudianteId=e.Id
		INNER JOIN EstadoAsistencia ea	ON	ea.Id=r.EstadoAsistenciaId
		WHERE r.Id=@idInsertado
	END
	IF(@i_accion='UA')
	BEGIN
		UPDATE r SET EstadoAsistenciaId=1
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e ON e.Id=r.EstudianteId
		INNER JOIN Modulos m ON m.Id=e.ModuloId
		WHERE r.Id=@i_idRegistroAsistencia 
		AND r.UsuarioId=@usuarioId
		AND r.Estado=1
		AND e.Estado=1
		AND m.Estado=1

		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres, Apellidos, Matricula, 
		Email, Fecha=r.Fecha,EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m			ON	m.Id=e.ModuloId
		LEFT JOIN Carreras c			ON	c.Id=e.CarreraId
		INNER JOIN RegistroAsistencia r	ON	r.EstudianteId=e.Id
		INNER JOIN EstadoAsistencia ea	ON	ea.Id=r.EstadoAsistenciaId
		WHERE r.Id=@i_idRegistroAsistencia
		AND r.UsuarioId=@usuarioId
		AND r.Estado=1
		AND e.Estado=1
		AND m.Estado=1
	END
	IF(@i_accion='UF')
	BEGIN
		UPDATE r SET EstadoAsistenciaId=2
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e ON e.Id=r.EstudianteId
		INNER JOIN Modulos m ON m.Id=e.ModuloId
		WHERE r.Id=@i_idRegistroAsistencia
		AND r.UsuarioId=@usuarioId
		AND r.Estado=1
		AND e.Estado=1
		AND m.Estado=1

		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres, Apellidos, Matricula, 
		Email, Fecha=r.Fecha,EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m			ON	m.Id=e.ModuloId
		LEFT JOIN Carreras c			ON	c.Id=e.CarreraId
		INNER JOIN RegistroAsistencia r	ON	r.EstudianteId=e.Id
		INNER JOIN EstadoAsistencia ea	ON	ea.Id=r.EstadoAsistenciaId
		WHERE r.Id=@i_idRegistroAsistencia
		AND r.UsuarioId=@usuarioId
		AND r.Estado=1
		AND e.Estado=1
		AND m.Estado=1
	END
	IF(@i_accion='CC')
	BEGIN
		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
		Matricula=e.Matricula, Email=e.Email,
		Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
		LEFT JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE c.Nombre LIKE '%' + @carrera + '%'
		AND m.Nombre=@modulo
		AND r.UsuarioId=@usuarioId
		AND r.Estado=1
		AND e.Estado=1
		AND m.Estado=1
	END
	IF(@i_accion='CL')
	BEGIN
		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
		Matricula=e.Matricula, Email=e.Email,
		Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
		LEFT JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE m.Nombre=@modulo
		AND r.UsuarioId=@usuarioId
		AND r.Estado=1
		AND e.Estado=1
		AND m.Estado=1
	END
	IF(@i_accion='CM')
	BEGIN
		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
		Matricula=e.Matricula, Email=e.Email,
		Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
		LEFT JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE e.Matricula=@matricula
		AND m.Nombre=@modulo
		AND r.UsuarioId=@usuarioId
		AND r.Estado=1
		AND e.Estado=1
		AND m.Estado=1
	END
	IF(@i_accion='CN')
	BEGIN
		IF(@nombres = '' AND @apellidos = '')
		BEGIN
			SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
			Matricula=e.Matricula, Email=e.Email,
			Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
			FROM RegistroAsistencia r
			INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
			INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
			LEFT JOIN Carreras c			ON c.Id=e.CarreraId
			INNER JOIN Modulos m			ON m.Id=e.ModuloId
			WHERE	Nombres		= @nombres
			AND		Apellidos	= @apellidos
			AND m.Nombre=@modulo
			AND r.UsuarioId=@usuarioId
			AND r.Estado=1
			AND e.Estado=1
			AND m.Estado=1
		END
		IF(@nombres = '' AND @apellidos != '')
		BEGIN
			SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
			Matricula=e.Matricula, Email=e.Email,
			Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
			FROM RegistroAsistencia r
			INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
			INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
			LEFT JOIN Carreras c			ON c.Id=e.CarreraId
			INNER JOIN Modulos m			ON m.Id=e.ModuloId
			WHERE Apellidos LIKE '%' + @apellidos + '%'
			AND m.Nombre=@modulo
			AND r.UsuarioId=@usuarioId
			AND r.Estado=1
			AND e.Estado=1
			AND m.Estado=1
		END
		ELSE IF(@nombres != '' AND @apellidos = '')
		BEGIN
			SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
			Matricula=e.Matricula, Email=e.Email,
			Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
			FROM RegistroAsistencia r
			INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
			INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
			LEFT JOIN Carreras c			ON c.Id=e.CarreraId
			INNER JOIN Modulos m			ON m.Id=e.ModuloId
			WHERE Nombres LIKE '%' + @nombres + '%'
			AND m.Nombre=@modulo
			AND r.UsuarioId=@usuarioId
			AND r.Estado=1
			AND e.Estado=1
			AND m.Estado=1
		END
		ELSE
		BEGIN
			SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
			Matricula=e.Matricula, Email=e.Email,
			Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
			FROM RegistroAsistencia r
			INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
			INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
			LEFT JOIN Carreras c			ON c.Id=e.CarreraId
			INNER JOIN Modulos m			ON m.Id=e.ModuloId
			WHERE (Nombres LIKE '%' + @nombres + '%'
			AND Apellidos LIKE '%' + @apellidos + '%')
			AND m.Nombre=@modulo
			AND r.UsuarioId=@usuarioId
			AND r.Estado=1
			AND e.Estado=1
			AND m.Estado=1
		END		
	END
	IF(@i_accion='CT')
	BEGIN
		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
		Matricula=e.Matricula, Email=e.Email,
		Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
		LEFT JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE r.Estado=1
		AND r.UsuarioId=@usuarioId
		AND e.Estado=1
		AND m.Estado=1
	END	
	IF(@i_accion='DE')
	BEGIN
		IF EXISTS(SELECT 1
				FROM RegistroAsistencia r
				INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
				INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
				LEFT JOIN Carreras c			ON c.Id=e.CarreraId
				INNER JOIN Modulos m			ON m.Id=e.ModuloId
				WHERE r.Id=@i_idRegistroAsistencia
				AND r.Estado=1
				AND e.Estado=1
				AND m.Estado=1)
		BEGIN
			UPDATE RegistroAsistencia SET
			Estado=0
			WHERE Id=@i_idRegistroAsistencia

			SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
			Matricula=e.Matricula, Email=e.Email,
			Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
			FROM RegistroAsistencia r
			INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
			INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
			LEFT JOIN Carreras c			ON c.Id=e.CarreraId
			INNER JOIN Modulos m			ON m.Id=e.ModuloId
			WHERE r.Id=@i_idRegistroAsistencia
			AND r.Estado=0
			AND e.Estado=1
			AND m.Estado=1
			RETURN 0;
		END
		SELECT * FROM @RegistroAsistencia
		RETURN 0;
	END
	IF(@i_accion = 'AE')
	BEGIN
		DECLARE @usuarioActualSeleccionado		VARCHAR(25)
		DECLARE @moduloActualSeleccionado		VARCHAR(50)
		DECLARE @ultimaEntradaMasActual			DATETIME
		DECLARE @idModuloActualSeleccionado		INT
		DECLARE @idEstudianteParaAsistencia		INT
		DECLARE @idAsistenciaCreadaEstudiante	INT
		DECLARE @idAsistenciaDeEstudiante		INT
		DECLARE @fechaHoy						DATETIME
		DECLARE @fechaAyer						DATETIME

		SET @fechaHoy		=	GETDATE()
		SET @fechaAyer		=	DATEADD(dd, -1, @fechaHoy)

		IF(@i_tipoAccion = 'AA')
		BEGIN
			SELECT Id=r.Id, IdEstudiante=e.Id, Nombres, Apellidos, Matricula, 
			e.Email, Fecha=r.Fecha,EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
			FROM Estudiantes e
			INNER JOIN Modulos m			ON	m.Id=e.ModuloId
			LEFT JOIN Carreras c			ON	c.Id=e.CarreraId
			INNER JOIN RegistroAsistencia r	ON	r.EstudianteId=e.Id
			INNER JOIN EstadoAsistencia ea	ON	ea.Id=r.EstadoAsistenciaId
			INNER JOIN Usuarios u			ON u.Id=e.UsuarioId
			WHERE e.ModuloId=u.ModuloActualId
			AND u.Usuario=@i_usuario
			AND CAST(r.Fecha AS DATE) > CAST(@fechaAyer AS DATE)
			AND r.Estado=1
			AND e.Estado=1
			AND m.Estado=1
			AND u.Estado=1

			RETURN 0;
		END

		SELECT @ultimaEntradaMasActual=MAX(UltimaEntrada) FROM Usuarios WHERE Estado=1
		--
		--select @fechaHoy as FechaHoy
		--select @fechaAyer as FechaAyer
		--select @ultimaEntradaMasActual as UltimaEntradaMasActual
		--
		IF(@ultimaEntradaMasActual IS NULL)
		BEGIN
			--No existe ninguna ultima fecha de entrada por usuario
			INSERT INTO @RegistroAsistencia(Id, IdEstudiante, Fecha) VALUES(0, 0, GETDATE())
			SELECT * FROM @RegistroAsistencia
			RETURN 0;
		END

		SELECT @usuarioActualSeleccionado=Usuario FROM Usuarios 
		WHERE UltimaEntrada=@ultimaEntradaMasActual AND Estado=1		

		IF(ISNULL(@usuarioActualSeleccionado,'')='')
		BEGIN
			--No existe ningun usuario con ultima entrada
			INSERT INTO @RegistroAsistencia(Id, IdEstudiante, Fecha) VALUES(-1, 0, GETDATE())
			SELECT * FROM @RegistroAsistencia
			RETURN 0;
		END

		SELECT @usuarioId=Id FROM Usuarios
		WHERE Usuario=@usuarioActualSeleccionado AND Estado=1

		SELECT @moduloActualSeleccionado=m.Nombre FROM Usuarios u
		INNER JOIN Modulos m ON m.Id=u.ModuloActualId
		WHERE u.Usuario=@usuarioActualSeleccionado AND m.UsuarioId=@usuarioId AND u.Estado=1 AND m.Estado=1

		IF(ISNULL(@moduloActualSeleccionado,'')='')
		BEGIN
			--No existe ningun módulo con ultima entrada
			INSERT INTO @RegistroAsistencia(Id, IdEstudiante, Fecha) VALUES(-2, 0, GETDATE())
			SELECT * FROM @RegistroAsistencia
			RETURN 0;
		END

		SELECT TOP(1)@idModuloActualSeleccionado=ModuloActualId FROM Usuarios u
		INNER JOIN Modulos m ON m.Id=u.ModuloActualId
		WHERE u.Usuario=@usuarioActualSeleccionado AND m.Nombre=@moduloActualSeleccionado
		AND u.Estado=1 AND m.Estado=1

		IF(ISNULL(@idModuloActualSeleccionado,0)=0)
		BEGIN
			--No existe ningun id de módulo con ultima entrada
			INSERT INTO @RegistroAsistencia(Id, IdEstudiante, Fecha) VALUES(-3, 0, GETDATE())
			SELECT * FROM @RegistroAsistencia
			RETURN 0;
		END
		--
		--
		--

		SELECT TOP(1)@idEstudianteParaAsistencia=Id FROM Estudiantes 
		WHERE ModuloId=@idModuloActualSeleccionado AND Matricula=@i_matricula AND Estado=1

		IF(ISNULL(@idEstudianteParaAsistencia,0)=0)
		BEGIN
			--No existe ningun id de estudiante con la matrícula ingresada para el modulo actual de usuario mas reciente
			INSERT INTO @RegistroAsistencia(Id, IdEstudiante, Fecha) VALUES(-4, 0, GETDATE())
			SELECT * FROM @RegistroAsistencia
			RETURN 0;
		END

		SELECT TOP(1)@idAsistenciaCreadaEstudiante=r.Id FROM RegistroAsistencia r
		INNER JOIN Estudiantes e ON e.Id=r.EstudianteId
		WHERE r.EstudianteId=@idEstudianteParaAsistencia
		AND Fecha > @fechaAyer
		--AND EstadoAsistenciaId=1
		AND r.Estado=1
		AND e.Estado=1
		ORDER BY Fecha DESC

		IF(ISNULL(@idAsistenciaCreadaEstudiante,0)=0)
		BEGIN
			--No existe ningun id de asistencia con falta de estudiante con la matrícula ingresada para el modulo actual de usuario mas reciente
			INSERT INTO @RegistroAsistencia(Id, IdEstudiante, Fecha) VALUES(-5, 0, GETDATE())
			SELECT * FROM @RegistroAsistencia
			RETURN 0;
		END

		SELECT TOP(1)@idAsistenciaDeEstudiante=r.Id FROM RegistroAsistencia r
		INNER JOIN Estudiantes e ON e.Id=r.EstudianteId
		WHERE EstudianteId=@idEstudianteParaAsistencia
		AND r.Id=@idAsistenciaCreadaEstudiante
		AND Fecha > @fechaAyer
		AND EstadoAsistenciaId=2
		AND r.Estado=1
		AND e.Estado=1
		ORDER BY Fecha DESC

		IF(ISNULL(@idAsistenciaDeEstudiante,0)=0)
		BEGIN
			--El estudiante ingresado ya registra asistencia para el día de hoy
			INSERT INTO @RegistroAsistencia(Id, IdEstudiante, Fecha) VALUES(-6, 0, GETDATE())
			SELECT * FROM @RegistroAsistencia
			RETURN 0;
		END
		--
		--select @idAsistenciaCreadaEstudiante as IdÁsistenciaEstudianteDeMatriculaDeModuloActualDeUsuarioMasReciente
		--

		UPDATE RegistroAsistencia SET
		EstadoAsistenciaId=1
		WHERE Id=@idAsistenciaCreadaEstudiante

		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres, Apellidos, Matricula, 
		Email, Fecha=r.Fecha,EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m			ON	m.Id=e.ModuloId
		LEFT JOIN Carreras c			ON	c.Id=e.CarreraId
		INNER JOIN RegistroAsistencia r	ON	r.EstudianteId=e.Id
		INNER JOIN EstadoAsistencia ea	ON	ea.Id=r.EstadoAsistenciaId
		WHERE r.Id=@idAsistenciaCreadaEstudiante
		AND r.Estado=1
		AND e.Estado=1
		AND m.Estado=1

		RETURN 0;
	END

	IF(@i_accion = 'JB')
	BEGIN
		DECLARE @Modulos Table(
				Id					INT,
				Nombre				VARCHAR(50),
				Descripcion			VARCHAR(100),
				PeriodoAcademico	VARCHAR(50),
				DiasClase			VARCHAR(10),
				UsuarioId			INT
		)

		DECLARE @Estudiantes Table(
				Id					INT,
				Matricula			VARCHAR(10),
				UsuarioId			INT
		)

		DECLARE @RespuestaRegistroAsistencia TABLE(
				Id					INT, 
				IdEstudiante		INT,
				Nombres				VARCHAR(100), 
				Apellidos			VARCHAR(100),
				Matricula			VARCHAR(10),
				Email				VARCHAR(50),
				Fecha				DATETIME,
				EstadoAsistencia	VARCHAR(25),
				Carrera				VARCHAR(100),
				Modulo				VARCHAR(50)
			)	

		DECLARE @moduloActual			VARCHAR(50)
		DECLARE @diasModuloActual		VARCHAR(10)
		DECLARE @idModuloActual			INT
		DECLARE @fechaActual			DATETIME
		DECLARE @fechaUltimaAsistencia	DATETIME
		DECLARE @diaActual				VARCHAR(100)
		DECLARE @matriculaActual		VARCHAR(10)
		DECLARE @idEstudianteActual		INT		
		DECLARE @usuarioIdActual		INT
		
		SET @fechaActual = GETDATE()
		SET @diaActual	= DATENAME(WEEKDAY,@fechaActual)

		INSERT INTO @Modulos
		SELECT Id, Nombre, Descripcion, PeriodoAcademico, DiasClase, UsuarioId 
		FROM Modulos WHERE Estado = 1 AND Nombre!='LA'
		
		WHILE(1=1)
		BEGIN
			IF(SELECT COUNT(*) FROM @Modulos)=0
				BREAK;
		
			SELECT TOP(1) @moduloActual=Nombre FROM @Modulos
			SELECT TOP(1) @diasModuloActual=DiasClase FROM @Modulos
			SELECT TOP(1) @idModuloActual=Id FROM @Modulos
		
			INSERT INTO @Estudiantes
			SELECT e.Id, Matricula, e.UsuarioId FROM Estudiantes e
			INNER JOIN Modulos m ON m.Id=e.ModuloId
			WHERE m.Id=@idModuloActual
			AND e.Estado=1 
			AND m.Estado=1
			
			IF((ISNULL(@diasModuloActual,'')!='') AND ((SELECT COUNT(*) FROM @Estudiantes) > 0))
			BEGIN

			WHILE(1=1)
			BEGIN
			IF(SELECT COUNT(*) FROM @Estudiantes)=0
				BREAK;
			
			SET @fechaUltimaAsistencia = DATEADD(yyyy, -1, @fechaActual)

			SELECT TOP(1) @matriculaActual=Matricula FROM @Estudiantes
			SELECT TOP(1) @idEstudianteActual=Id FROM @Estudiantes
			SELECT TOP(1) @usuarioIdActual=UsuarioId FROM @Estudiantes
			--
			--select @usuarioIdActual as UsuarioIdActual
			--select @idEstudianteActual as EstudianteIdActual
			--select @matriculaActual as MatriculaEstudianteActual
			--select (CHARINDEX(LEFT(@diaActual,1),@diasModuloActual)) as PosicionPrefijoDia
			--select @diasModuloActual as DiasModuloActual
			--select (LEFT(@diaActual,1)) as PrefijoDiaActual
			--
			IF(@diaActual = 'Miércoles' OR @diaActual = 'Miercoles')
			BEGIN
				IF(CHARINDEX('X',@diasModuloActual) > 0)
				BEGIN
					SELECT TOP(1) @fechaUltimaAsistencia=Fecha FROM RegistroAsistencia r
					WHERE r.EstudianteId=@idEstudianteActual
					AND r.UsuarioId=@usuarioIdActual
					AND r.Estado=1 ORDER BY Fecha DESC

					IF NOT EXISTS(SELECT Fecha FROM RegistroAsistencia 
					WHERE EstudianteId=@idEstudianteActual AND Estado=1) 
					OR NOT(CAST(@fechaUltimaAsistencia AS DATE) = CAST(@fechaActual AS DATE))
					BEGIN
						--
						--select @idEstudianteActual as EstudianteIdActual
						--select @usuarioIdActual as UsuarioIdActual
						--select (CHARINDEX(LEFT(@diaActual,1),@diasModuloActual)) as PosicionPrefijoDia
						--select @diasModuloActual as DiasModuloActual
						--select (LEFT(@diaActual,1)) as PrefijoDiaActual
						--
						INSERT INTO RegistroAsistencia (Fecha, EstudianteId, EstadoAsistenciaId, UsuarioId, Estado)
						SELECT @fechaActual, @idEstudianteActual, 2, @usuarioIdActual, 1
					END	
				END
			END
			ELSE
			BEGIN
				IF(CHARINDEX(LEFT(@diaActual,1),@diasModuloActual) > 0)
				BEGIN
					SELECT TOP(1) @fechaUltimaAsistencia=Fecha FROM RegistroAsistencia r
					WHERE r.EstudianteId=@idEstudianteActual
					AND r.UsuarioId=@usuarioIdActual
					AND r.Estado=1 ORDER BY Fecha DESC

					IF NOT EXISTS(SELECT Fecha FROM RegistroAsistencia r
					WHERE EstudianteId=@idEstudianteActual AND Estado=1) 
					OR NOT(CAST(@fechaUltimaAsistencia AS DATE) = CAST(@fechaActual AS DATE))
					BEGIN
						--
						--select @idEstudianteActual as EstudianteIdActual
						--select @usuarioIdActual as UsuarioIdActual
						--select (CHARINDEX(LEFT(@diaActual,1),@diasModuloActual)) as PosicionPrefijoDia
						--select @diasModuloActual as DiasModuloActual
						--select (LEFT(@diaActual,1)) as PrefijoDiaActual
						--
						INSERT INTO RegistroAsistencia (Fecha, EstudianteId, EstadoAsistenciaId, UsuarioId, Estado)
						SELECT @fechaActual, @idEstudianteActual, 2, @usuarioIdActual, 1
					END			
				END
			END

			DELETE TOP(1) FROM @Estudiantes
			END
		END

		DELETE FROM @Estudiantes
		DELETE TOP(1) FROM @Modulos
		END

		INSERT INTO @RespuestaRegistroAsistencia(Id, IdEstudiante, Fecha) VALUES(0, 0, GETDATE())
		SELECT * FROM @RespuestaRegistroAsistencia

		RETURN 0;
	END
END
GO