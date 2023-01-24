ALTER PROC	sp_item
		@i_accion				CHAR(2)				=	NULL,
		@i_id					INT					=	NULL,
		@i_estadoItemId			INT					=	NULL,
		@i_rfid					VARCHAR(100)		=	NULL,
		@i_inventario			VARCHAR(100)			=	NULL

AS
BEGIN
	DECLARE
	@inventario				VARCHAR(25),
	@inventarioIdInsertar	INT,
	@idInsertado			INT,
	@cantidadItem			INT,
	@cantidadItemDisponible	INT

	SET @inventario = TRIM(@i_inventario)
	
	DECLARE @Item Table(
		Id				INT,
		Rfid			VARCHAR(100),
		EstadoItem		VARCHAR(25),
		Inventario		VARCHAR(100)
	)

	IF(@i_accion='IN')
	BEGIN
		-- VALIDACION DE QUE NO EXISTE EL INVENTARIO INGRESADO
		IF NOT EXISTS(SELECT 1 FROM Inventario WHERE Nombre=@inventario AND Estado=1)
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(0, 'NO EXISTE EL INVENTARIO INGRESADO')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		-- VALIDACION DE QUE YA EXISTE UN ITEM CON EL RFID INGRESADO
		IF EXISTS(SELECT 1 FROM Items WHERE Rfid=@i_rfid AND Estado=1)
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(-1, 'YA EXISTE UN ITEM CON EL RFID INGRESADO')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END
		
		SELECT @inventarioIdInsertar=Id FROM Inventario WHERE Nombre=@inventario AND Estado=1

		-- VALIDACION DE ACTUALIZAR ESTADO=1 CUANDO EL ITEM CON EL RFID INGRESADO FUE ELIMINADO
		-- LOGICAMENTE
		IF EXISTS(SELECT 1 FROM Items WHERE Rfid=@i_rfid AND Estado=0)
		BEGIN
			UPDATE Items SET
			Estado=1,
			Rfid=@i_rfid,
			EstadoItemId=1,
			InventarioId=@inventarioIdInsertar
			WHERE Id=(SELECT Id FROM Items WHERE Rfid=@i_rfid AND Estado=0)

			SELECT @cantidadItem=COUNT(*) FROM Items 
			WHERE InventarioId=@inventarioIdInsertar AND Estado=1

			SELECT @cantidadItemDisponible=COUNT(*) FROM Items 
			WHERE InventarioId=@inventarioIdInsertar AND EstadoItemId=1 AND Estado=1

			UPDATE Inventario SET
			CantidadTotal=@cantidadItem,
			CantidadDisponible=@cantidadItemDisponible
			WHERE Id=@inventarioIdInsertar

			SELECT Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
			INNER JOIN Inventario i		ON i.Id=it.InventarioId
			INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
			WHERE it.Rfid=@i_rfid
			AND it.Estado=1
			AND i.Estado=1

			RETURN 0;
		END

		-- INSERCION DE NUEVO ITEM
		INSERT INTO Items
		(
			Rfid, EstadoItemId, InventarioId, Estado
		)
		SELECT
			@i_rfid, 1, @inventarioIdInsertar, 1

		SET @idInsertado=@@IDENTITY

		SELECT @cantidadItem=COUNT(*) FROM Items 
		WHERE InventarioId=@inventarioIdInsertar AND Estado=1

		SELECT @cantidadItemDisponible=COUNT(*) FROM Items 
		WHERE InventarioId=@inventarioIdInsertar AND EstadoItemId=1 AND Estado=1

		UPDATE Inventario SET
		CantidadTotal=@cantidadItem,
		CantidadDisponible=@cantidadItemDisponible
		WHERE Id=@inventarioIdInsertar

		SELECT Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
		INNER JOIN Inventario i		ON i.Id=it.InventarioId
		INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
		WHERE it.Id=@idInsertado
		AND it.Estado=1
		AND i.Estado=1

		RETURN 0;
	END

	IF(@i_accion='UP')
	BEGIN
		-- VALIDACION DE QUE EXISTA EL ITEM CON EL ID INGRESADO
		IF NOT EXISTS(SELECT 1 FROM Items WHERE Id=@i_id AND Estado=1)
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(0, 'NO EXISTE UN ITEM CON EL ID INGRESADO')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		-- VALIDACION DE QUE NO EXISTA OTRO ITEM CON EL RFID INGRESADO
		IF ((ISNULL(@i_rfid,'')!='')AND(EXISTS(SELECT 1 FROM Items WHERE Rfid=@i_rfid AND Id!=@i_id AND Estado=1)))
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(-1, 'YA EXISTE OTRO ITEM CON EL RFID INGRESADO')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		-- VALIDACION DE QUE EL ESTADOITEM INGRESADO SEA VALIDO (1[Disponible] O 2[No Disponible])
		IF ((ISNULL(@i_estadoItemId,0)!=0)AND(NOT EXISTS(SELECT 1 FROM EstadoItem WHERE Id=@i_estadoItemId)))
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(-2, 'EL ESTADO ITEM INGRESADO NO ES VÁLIDO (DEBE SER 1[Disponible] O 2[No Disponible])')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		SELECT @inventarioIdInsertar=Id FROM Inventario WHERE Nombre=@inventario AND Estado=1

		-- ACTUALIZACION DE ITEM
		UPDATE Items SET
		Rfid=CASE ISNULL(@i_rfid,'') WHEN '' THEN Items.Rfid 
			ELSE @i_rfid END,
		EstadoItemId=CASE ISNULL(@i_estadoItemId,0) WHEN 0 THEN Items.EstadoItemId
			ELSE @i_estadoItemId END,
		InventarioId=CASE ISNULL(@inventarioIdInsertar,0) WHEN 0 THEN Items.InventarioId
			ELSE @inventarioIdInsertar END
		WHERE Id=@i_id

		SELECT @cantidadItemDisponible=COUNT(*) FROM Items 
		WHERE InventarioId=@inventarioIdInsertar AND EstadoItemId=1 AND Estado=1

		UPDATE Inventario SET
		CantidadDisponible=@cantidadItemDisponible
		WHERE Id=@inventarioIdInsertar

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
		INNER JOIN Inventario i		ON i.Id=it.InventarioId
		INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
		WHERE it.Id=@i_id
		AND it.Estado=1
		AND i.Estado=1

		RETURN 0;
	END

	IF(@i_accion='DE')
	BEGIN
		-- VALIDAR QUE EL ELEMENTO EXISTA (ESTADO=1)
		IF NOT EXISTS(SELECT 1 FROM Items WHERE Id=@i_id AND Estado=1)
		BEGIN
			INSERT INTO @Item(Id, Rfid) VALUES(0, 'NO EXISTE UN ITEM CON EL ID INGRESADO')
			SELECT TOP(1) * FROM @Item
			RETURN 0;
		END

		UPDATE Items SET
		Estado=0
		WHERE Id=@i_id
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
		INNER JOIN Inventario i		ON i.Id=it.InventarioId
		INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
		WHERE it.Id=@i_id
		AND it.Estado=0
		AND i.Estado=1

		RETURN 0;
	END

	IF(@i_accion='CN')
	BEGIN
		IF(ISNULL(@i_rfid,'')!='')AND(ISNULL(@inventario,'')='')
		BEGIN
			SELECT Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
			INNER JOIN Inventario i		ON i.Id=it.InventarioId
			INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
			WHERE it.Rfid=@i_rfid
			AND it.Estado=1
			AND i.Estado=1

			RETURN 0;
		END
		IF(ISNULL(@i_rfid,'')='')AND(ISNULL(@inventario,'')!='')
		BEGIN
			SELECT Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
			INNER JOIN Inventario i		ON i.Id=it.InventarioId
			INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
			WHERE i.Nombre LIKE '%' + @inventario + '%'
			AND it.Estado=1
			AND i.Estado=1

			RETURN 0;
		END
		ELSE
		BEGIN
			SELECT Id=it.Id, Rfid=it.Rfid, EstadoItem=ei.Descripcion, Inventario=i.Nombre FROM Items it
			INNER JOIN Inventario i		ON i.Id=it.InventarioId
			INNER JOIN EstadoItem ei	ON ei.Id=it.EstadoItemId
			WHERE it.Rfid LIKE '%' + @i_rfid + '%'
			AND i.Nombre LIKE '%' + @inventario + '%'
			AND it.Estado=1
			AND i.Estado=1

			RETURN 0;
		END
	END
END
GO