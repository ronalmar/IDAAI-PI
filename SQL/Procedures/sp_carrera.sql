ALTER PROCEDURE sp_carrera
	@i_accion			CHAR(2)			=	NULL,
	@i_nombre			VARCHAR(100)	=	NULL,
	@i_id				INT				=	NULL,
	@i_modulo			VARCHAR(50)		=	NULL

AS
BEGIN
	DECLARE 
	@nombre		VARCHAR(100),
	@modulo		VARCHAR(50),
	@idModulo	INT,
	@id			INT

	SET @nombre=TRIM(@i_nombre)
	SET @modulo=TRIM(@i_modulo)

	DECLARE @Carreras TABLE(
			Id			INT,
			Nombre		VARCHAR(100),
			Modulo		VARCHAR(50)
	)

	IF(@i_accion='IN')
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM Carreras c
						INNER JOIN Modulos m ON m.Id=c.ModuloId
						WHERE c.Nombre=@nombre 
						AND c.Estado=1
						AND m.Estado=1)
		BEGIN
			SELECT @idModulo=Id	FROM Modulos WHERE Nombre = @modulo		AND Estado=1

			IF(@idModulo IS NULL)
			BEGIN
				INSERT INTO @Carreras (Id) VALUES(-1)

				SELECT * FROM @Carreras
				RETURN 0;
			END
			IF EXISTS(SELECT 1 FROM Carreras c
						INNER JOIN Modulos m ON m.Id=c.ModuloId
						WHERE c.Nombre=@nombre 
						AND c.Estado=0
						AND m.Estado=1)
			BEGIN
				UPDATE Carreras SET
				Nombre				= @nombre,
				ModuloId			= @idModulo,
				Estado = 1
				WHERE Id=(SELECT Id FROM Carreras WHERE Nombre=@nombre AND Estado=0)

				SELECT Id=c.id, Nombre=c.Nombre, Modulo=m.nombre 
				FROM Carreras c
				INNER JOIN Modulos m ON m.Id=c.ModuloId
				WHERE c.Id=(SELECT Id FROM Carreras WHERE Nombre=@nombre AND Estado=1)
				AND c.Estado=1
				AND m.Estado=1

				RETURN 0;
			END			

			INSERT INTO Carreras VALUES(@nombre, @idModulo, 1)

			SET @id=@@IDENTITY

			SELECT Id=c.id, Nombre=c.Nombre, Modulo=m.nombre 
			FROM Carreras c
			INNER JOIN Modulos m ON m.Id=c.ModuloId
			WHERE c.Id=@id
			AND c.Estado=1
			AND m.Estado=1

			RETURN 0;
		END
		INSERT INTO @Carreras (Id) VALUES(0)

		SELECT * FROM @Carreras
		RETURN 0;
	END
	IF(@i_accion='UP')
	BEGIN
		IF EXISTS(SELECT 1 FROM Carreras c
						INNER JOIN Modulos m ON m.Id=c.ModuloId
						WHERE c.Id=@i_id 
						AND c.Estado=1
						AND m.Estado=1)
		BEGIN
			SELECT @idModulo=Id	FROM Modulos WHERE Nombre = @modulo		AND Estado=1

			IF(@idModulo IS NULL AND ISNULL(@modulo, '')!='')
			BEGIN
				--El módulo de la carrera no es válido
				INSERT INTO @Carreras (Id) VALUES(-1)

				SELECT * FROM @Carreras
				RETURN 0;
			END

			UPDATE c SET 
			c.Nombre	=	CASE WHEN ISNULL(@nombre,'')='' THEN c.Nombre ELSE @nombre END,
			c.ModuloId	=	ISNULL(@idModulo, m.Id)
			FROM Carreras c
			INNER JOIN Modulos m ON m.Id=c.ModuloId
			WHERE c.Id=@i_id
			AND c.Estado=1
			AND m.Estado=1

			SELECT Id=c.Id, Nombre=c.Nombre, Modulo=m.nombre 
			FROM Carreras c
			INNER JOIN Modulos m ON m.Id=c.ModuloId
			WHERE c.Id=@i_id
			AND c.Estado=1
			AND m.Estado=1

			RETURN 0;
		END
		INSERT INTO @Carreras (Id) VALUES(0)

		SELECT * FROM @Carreras
		RETURN 0;
	END
	IF(@i_accion = 'DE')
	BEGIN
		IF EXISTS(SELECT 1 FROM Carreras WHERE id=@i_id AND Estado=1)
		BEGIN
			UPDATE Carreras SET	Estado=0
			WHERE	Id=@i_id AND Estado=1
			
			SELECT Id=c.Id, Nombre=c.Nombre, Modulo=m.nombre 
			FROM Carreras c
			INNER JOIN Modulos m ON m.Id=c.ModuloId
			WHERE c.Id=@i_id
			AND m.Estado=1

			RETURN 0;
		END
		SELECT * FROM @Carreras
		RETURN 0;
	END
	IF(@i_accion='CL')
	BEGIN
		SELECT Id=c.Id, Nombre=c.Nombre, Modulo=m.Nombre 
		FROM Carreras c
		INNER JOIN Modulos m ON m.Id=c.ModuloId
		WHERE m.Nombre=@modulo
		AND c.Estado=1
		AND m.Estado=1

		RETURN 0;
	END
	IF(@i_accion='CN')
	BEGIN
		--IF(ISNULL(@nombre,'')='')
		--BEGIN
		--	SELECT Id=c.Id, Nombre=c.Nombre, Modulo=m.Nombre 
		--	FROM Carreras c
		--	INNER JOIN Modulos m ON m.Id=c.ModuloId
		--	WHERE c.Nombre = @nombre
		--	AND m.Nombre=@modulo
		--	AND c.Estado=1
		--	AND m.Estado=1
		--	RETURN 0;
		--END
		SELECT Id=c.Id, Nombre=c.Nombre, Modulo=m.Nombre 
		FROM Carreras c
		INNER JOIN Modulos m ON m.Id=c.ModuloId
		WHERE c.Nombre LIKE '%' + @nombre + '%'
		AND m.Nombre=@modulo
		AND c.Estado=1
		AND m.Estado=1

		RETURN 0;
	END
	IF(@i_accion='CT')
	BEGIN
		SELECT Id=c.Id, Nombre=c.Nombre, Modulo=m.Nombre 
		FROM Carreras c
		INNER JOIN Modulos m ON m.Id=c.ModuloId
		WHERE c.Estado=1
		AND m.Estado=1

		RETURN 0;
	END
	
	RETURN 0;
END
GO