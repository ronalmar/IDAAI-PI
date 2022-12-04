ALTER PROCEDURE sp_registroasistencia
	@i_accion					CHAR(2)				=NULL,
	@i_idRegistroAsistencia		INT					=NULL,
	@i_fecha					DATETIME			=NULL,
	@i_nombres					VARCHAR(100)		=NULL,
	@i_apellidos				VARCHAR(100)		=NULL,
	@i_matricula				VARCHAR(10)			=NULL,
	@i_carrera					VARCHAR(100)		=NULL,
	@i_modulo					VARCHAR(50)			=NULL
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
		@apellidosTrim	VARCHAR(100)
	
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
						WHERE Matricula=@matricula	AND  m.Nombre=@modulo)
		BEGIN
			--Matricula no existe
			SELECT * FROM @RegistroAsistencia
			RETURN 0;
		END
		SELECT @estudianteId=e.Id FROM Estudiantes e
		INNER JOIN Modulos m ON m.Id=e.ModuloId 
		WHERE Matricula=@matricula AND  m.Nombre=@modulo

		INSERT INTO RegistroAsistencia (Fecha, EstudianteId, EstadoAsistenciaId)
		SELECT @i_fecha, @estudianteId, 1

		SET @idInsertado = @@IDENTITY
		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres, Apellidos, Matricula, 
		Email, Fecha=r.Fecha,EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m			ON	m.Id=e.ModuloId
		INNER JOIN Carreras c			ON	c.Id=e.CarreraId
		INNER JOIN RegistroAsistencia r	ON	r.EstudianteId=e.Id
		INNER JOIN EstadoAsistencia ea	ON	ea.Id=r.EstadoAsistenciaId
		WHERE r.Id=@idInsertado
	END
	IF(@i_accion='IF')
	BEGIN
	IF NOT EXISTS(SELECT 1 FROM Estudiantes e
						INNER JOIN Modulos m ON m.Id=e.ModuloId
						WHERE Matricula=@matricula	AND  m.Nombre=@modulo)
		BEGIN
			--Matricula no existe
			SELECT * FROM @RegistroAsistencia
			RETURN 0;
		END
		SELECT @estudianteId=e.Id FROM Estudiantes e
		INNER JOIN Modulos m ON m.Id=e.ModuloId 
		WHERE Matricula=@matricula AND  m.Nombre=@modulo

		INSERT INTO RegistroAsistencia (Fecha, EstudianteId, EstadoAsistenciaId)
		SELECT @i_fecha, @estudianteId, 2

		SET @idInsertado = @@IDENTITY
		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres, Apellidos, Matricula, 
		Email, Fecha=r.Fecha,EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m			ON	m.Id=e.ModuloId
		INNER JOIN Carreras c			ON	c.Id=e.CarreraId
		INNER JOIN RegistroAsistencia r	ON	r.EstudianteId=e.Id
		INNER JOIN EstadoAsistencia ea	ON	ea.Id=r.EstadoAsistenciaId
		WHERE r.Id=@idInsertado
	END
	IF(@i_accion='UA')
	BEGIN
		UPDATE RegistroAsistencia SET EstadoAsistenciaId=1
		WHERE Id=@i_idRegistroAsistencia

		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres, Apellidos, Matricula, 
		Email, Fecha=r.Fecha,EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m			ON	m.Id=e.ModuloId
		INNER JOIN Carreras c			ON	c.Id=e.CarreraId
		INNER JOIN RegistroAsistencia r	ON	r.EstudianteId=e.Id
		INNER JOIN EstadoAsistencia ea	ON	ea.Id=r.EstadoAsistenciaId
		WHERE r.Id=@i_idRegistroAsistencia
	END
	IF(@i_accion='UF')
	BEGIN
		UPDATE RegistroAsistencia SET EstadoAsistenciaId=2
		WHERE Id=@i_idRegistroAsistencia

		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres, Apellidos, Matricula, 
		Email, Fecha=r.Fecha,EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m			ON	m.Id=e.ModuloId
		INNER JOIN Carreras c			ON	c.Id=e.CarreraId
		INNER JOIN RegistroAsistencia r	ON	r.EstudianteId=e.Id
		INNER JOIN EstadoAsistencia ea	ON	ea.Id=r.EstadoAsistenciaId
		WHERE r.Id=@i_idRegistroAsistencia
	END
	IF(@i_accion='CC')
	BEGIN
		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
		Matricula=e.Matricula, Email=e.Email,
		Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
		INNER JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE c.Nombre LIKE '%' + @carrera + '%'
		AND m.Nombre=@modulo
	END
	IF(@i_accion='CL')
	BEGIN
		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
		Matricula=e.Matricula, Email=e.Email,
		Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
		INNER JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE m.Nombre=@modulo
	END
	IF(@i_accion='CM')
	BEGIN
		SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
		Matricula=e.Matricula, Email=e.Email,
		Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
		INNER JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE e.Matricula=@matricula
		AND m.Nombre=@modulo
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
			INNER JOIN Carreras c			ON c.Id=e.CarreraId
			INNER JOIN Modulos m			ON m.Id=e.ModuloId
			WHERE	Nombres		= @nombres
			AND		Apellidos	= @apellidos
			AND m.Nombre=@modulo
		END
		IF(@nombres = '' AND @apellidos != '')
		BEGIN
			SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
			Matricula=e.Matricula, Email=e.Email,
			Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
			FROM RegistroAsistencia r
			INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
			INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
			INNER JOIN Carreras c			ON c.Id=e.CarreraId
			INNER JOIN Modulos m			ON m.Id=e.ModuloId
			WHERE Apellidos LIKE '%' + @apellidos + '%'
			AND m.Nombre=@modulo
		END
		ELSE IF(@nombres != '' AND @apellidos = '')
		BEGIN
			SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
			Matricula=e.Matricula, Email=e.Email,
			Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
			FROM RegistroAsistencia r
			INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
			INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
			INNER JOIN Carreras c			ON c.Id=e.CarreraId
			INNER JOIN Modulos m			ON m.Id=e.ModuloId
			WHERE Nombres LIKE '%' + @nombres + '%'
			AND m.Nombre=@modulo
		END
		ELSE
		BEGIN
			SELECT Id=r.Id, IdEstudiante=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, 
			Matricula=e.Matricula, Email=e.Email,
			Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
			FROM RegistroAsistencia r
			INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
			INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
			INNER JOIN Carreras c			ON c.Id=e.CarreraId
			INNER JOIN Modulos m			ON m.Id=e.ModuloId
			WHERE (Nombres LIKE '%' + @nombres + '%'
			AND Apellidos LIKE '%' + @apellidos + '%')
			AND m.Nombre=@modulo
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
		INNER JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
	END
END
GO