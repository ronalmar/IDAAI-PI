USE [master]
GO
/****** Object:  Database [IDAAI]    Script Date: 18/12/2022 19:34:52 ******/
CREATE DATABASE [IDAAI]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'IDAAI', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\IDAAI.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'IDAAI_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\IDAAI_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [IDAAI] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [IDAAI].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [IDAAI] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [IDAAI] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [IDAAI] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [IDAAI] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [IDAAI] SET ARITHABORT OFF 
GO
ALTER DATABASE [IDAAI] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [IDAAI] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [IDAAI] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [IDAAI] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [IDAAI] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [IDAAI] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [IDAAI] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [IDAAI] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [IDAAI] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [IDAAI] SET  ENABLE_BROKER 
GO
ALTER DATABASE [IDAAI] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [IDAAI] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [IDAAI] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [IDAAI] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [IDAAI] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [IDAAI] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [IDAAI] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [IDAAI] SET RECOVERY FULL 
GO
ALTER DATABASE [IDAAI] SET  MULTI_USER 
GO
ALTER DATABASE [IDAAI] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [IDAAI] SET DB_CHAINING OFF 
GO
ALTER DATABASE [IDAAI] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [IDAAI] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [IDAAI] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [IDAAI] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'IDAAI', N'ON'
GO
ALTER DATABASE [IDAAI] SET QUERY_STORE = OFF
GO
USE [IDAAI]
GO
/****** Object:  User [idaaiuser]    Script Date: 18/12/2022 19:34:52 ******/
CREATE USER [idaaiuser] FOR LOGIN [idaaiuser] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [idaaiuser]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [idaaiuser]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [idaaiuser]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [idaaiuser]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [idaaiuser]
GO
ALTER ROLE [db_datareader] ADD MEMBER [idaaiuser]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [idaaiuser]
GO
ALTER ROLE [db_denydatareader] ADD MEMBER [idaaiuser]
GO
ALTER ROLE [db_denydatawriter] ADD MEMBER [idaaiuser]
GO
/****** Object:  Table [dbo].[Carreras]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carreras](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[ModuloId] [int] NOT NULL,
	[Estado] [tinyint] NULL,
 CONSTRAINT [PK_Carreras] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoAsistencia]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoAsistencia](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Estado] [varchar](25) NOT NULL,
 CONSTRAINT [PK_EstadoAsistencia] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoDevolucion]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoDevolucion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Estado] [varchar](25) NOT NULL,
 CONSTRAINT [PK_EstadoDevolucion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoItem]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Estado] [varchar](25) NOT NULL,
 CONSTRAINT [PK_EstadoItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Estudiantes]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Estudiantes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombres] [varchar](100) NOT NULL,
	[Apellidos] [varchar](100) NOT NULL,
	[Matricula] [varchar](10) NOT NULL,
	[Email] [varchar](50) NULL,
	[Direccion] [varchar](100) NULL,
	[CarreraId] [int] NULL,
	[ModuloId] [int] NOT NULL,
	[Estado] [tinyint] NULL,
 CONSTRAINT [PK_Estudiantes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inventario]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](25) NOT NULL,
	[Descripcion] [varchar](50) NULL,
	[CantidadDisponible] [int] NULL,
	[CantidadTotal] [int] NULL,
	[ModuloId] [int] NOT NULL,
	[Estado] [tinyint] NULL,
 CONSTRAINT [PK_Inventario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Rfid] [varchar](100) NOT NULL,
	[EstadoItemId] [int] NULL,
	[InventarioId] [int] NOT NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modulos]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modulos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Descripcion] [varchar](100) NULL,
	[PeriodoAcademico] [varchar](50) NULL,
	[Estado] [tinyint] NULL,
 CONSTRAINT [PK_Modulos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RegistroAsistencia]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegistroAsistencia](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[EstudianteId] [int] NOT NULL,
	[EstadoAsistenciaId] [int] NOT NULL,
	[Estado] [tinyint] NULL,
 CONSTRAINT [PK_RegistroAsistencia] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RegistroPrestamoItem]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegistroPrestamoItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FechaPrestado] [datetime] NOT NULL,
	[FechaDevuelto] [datetime] NULL,
	[EstudianteId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[EstadoDevolucionId] [int] NOT NULL,
	[Estado] [tinyint] NULL,
 CONSTRAINT [PK_RegistroPrestamoItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuarioModulo]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioModulo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [int] NOT NULL,
	[ModuloId] [int] NOT NULL,
 CONSTRAINT [PK_UsuarioModulo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Usuario] [varchar](25) NOT NULL,
	[Password] [varchar](300) NOT NULL,
	[Email] [varchar](50) NULL,
	[Estado] [tinyint] NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Carreras]  WITH CHECK ADD  CONSTRAINT [FK__Carreras__Modulo__3F115E1A] FOREIGN KEY([ModuloId])
REFERENCES [dbo].[Modulos] ([Id])
GO
ALTER TABLE [dbo].[Carreras] CHECK CONSTRAINT [FK__Carreras__Modulo__3F115E1A]
GO
ALTER TABLE [dbo].[Inventario]  WITH CHECK ADD  CONSTRAINT [FK__Inventari__Modul__45F365D3] FOREIGN KEY([ModuloId])
REFERENCES [dbo].[Modulos] ([Id])
GO
ALTER TABLE [dbo].[Inventario] CHECK CONSTRAINT [FK__Inventari__Modul__45F365D3]
GO
ALTER TABLE [dbo].[Items]  WITH CHECK ADD  CONSTRAINT [FK__Items__EstadoIte__4E88ABD4] FOREIGN KEY([EstadoItemId])
REFERENCES [dbo].[EstadoItem] ([Id])
GO
ALTER TABLE [dbo].[Items] CHECK CONSTRAINT [FK__Items__EstadoIte__4E88ABD4]
GO
ALTER TABLE [dbo].[Items]  WITH CHECK ADD  CONSTRAINT [FK__Items__Inventari__4F7CD00D] FOREIGN KEY([InventarioId])
REFERENCES [dbo].[Inventario] ([Id])
GO
ALTER TABLE [dbo].[Items] CHECK CONSTRAINT [FK__Items__Inventari__4F7CD00D]
GO
ALTER TABLE [dbo].[RegistroAsistencia]  WITH CHECK ADD  CONSTRAINT [FK__RegistroA__Estad__412EB0B6] FOREIGN KEY([EstadoAsistenciaId])
REFERENCES [dbo].[EstadoAsistencia] ([Id])
GO
ALTER TABLE [dbo].[RegistroAsistencia] CHECK CONSTRAINT [FK__RegistroA__Estad__412EB0B6]
GO
ALTER TABLE [dbo].[RegistroAsistencia]  WITH CHECK ADD  CONSTRAINT [FK__RegistroA__Estud__3E52440B] FOREIGN KEY([EstudianteId])
REFERENCES [dbo].[Estudiantes] ([Id])
GO
ALTER TABLE [dbo].[RegistroAsistencia] CHECK CONSTRAINT [FK__RegistroA__Estud__3E52440B]
GO
ALTER TABLE [dbo].[RegistroPrestamoItem]  WITH CHECK ADD FOREIGN KEY([EstadoDevolucionId])
REFERENCES [dbo].[EstadoDevolucion] ([Id])
GO
ALTER TABLE [dbo].[RegistroPrestamoItem]  WITH CHECK ADD  CONSTRAINT [FK__RegistroP__Estud__5441852A] FOREIGN KEY([EstudianteId])
REFERENCES [dbo].[Estudiantes] ([Id])
GO
ALTER TABLE [dbo].[RegistroPrestamoItem] CHECK CONSTRAINT [FK__RegistroP__Estud__5441852A]
GO
ALTER TABLE [dbo].[RegistroPrestamoItem]  WITH CHECK ADD  CONSTRAINT [FK__RegistroP__ItemI__5535A963] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Items] ([Id])
GO
ALTER TABLE [dbo].[RegistroPrestamoItem] CHECK CONSTRAINT [FK__RegistroP__ItemI__5535A963]
GO
ALTER TABLE [dbo].[UsuarioModulo]  WITH CHECK ADD  CONSTRAINT [FK__UsuarioMo__Modul__7F2BE32F] FOREIGN KEY([ModuloId])
REFERENCES [dbo].[Modulos] ([Id])
GO
ALTER TABLE [dbo].[UsuarioModulo] CHECK CONSTRAINT [FK__UsuarioMo__Modul__7F2BE32F]
GO
ALTER TABLE [dbo].[UsuarioModulo]  WITH CHECK ADD  CONSTRAINT [FK__UsuarioMo__Usuar__7E37BEF6] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([Id])
GO
ALTER TABLE [dbo].[UsuarioModulo] CHECK CONSTRAINT [FK__UsuarioMo__Usuar__7E37BEF6]
GO
/****** Object:  StoredProcedure [dbo].[sp_carrera]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_carrera]
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

			INSERT INTO Carreras VALUES(@nombre, 1, @idModulo)

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
/****** Object:  StoredProcedure [dbo].[sp_estudiante]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_estudiante]
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
/****** Object:  StoredProcedure [dbo].[sp_modulo]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_modulo]
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
		IF EXISTS (SELECT 1 FROM Modulos WHERE Id=@i_id AND Estado=1)
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
		IF EXISTS(SELECT 1 FROM Modulos WHERE id=@i_id AND Estado=1)
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
/****** Object:  StoredProcedure [dbo].[sp_registroasistencia]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_registroasistencia]
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
						WHERE Matricula=@matricula	
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
		AND e.Estado=1
		AND m.Estado=1

		INSERT INTO RegistroAsistencia (Fecha, EstudianteId, EstadoAsistenciaId, Estado)
		SELECT @i_fecha, @estudianteId, 1, 1

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
		AND e.Estado=1
		AND m.Estado=1

		INSERT INTO RegistroAsistencia (Fecha, EstudianteId, EstadoAsistenciaId, Estado)
		SELECT @i_fecha, @estudianteId, 2, 1

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
		INNER JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE c.Nombre LIKE '%' + @carrera + '%'
		AND m.Nombre=@modulo
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
		INNER JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE m.Nombre=@modulo
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
		INNER JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE e.Matricula=@matricula
		AND m.Nombre=@modulo
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
			INNER JOIN Carreras c			ON c.Id=e.CarreraId
			INNER JOIN Modulos m			ON m.Id=e.ModuloId
			WHERE	Nombres		= @nombres
			AND		Apellidos	= @apellidos
			AND m.Nombre=@modulo
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
			INNER JOIN Carreras c			ON c.Id=e.CarreraId
			INNER JOIN Modulos m			ON m.Id=e.ModuloId
			WHERE Apellidos LIKE '%' + @apellidos + '%'
			AND m.Nombre=@modulo
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
			INNER JOIN Carreras c			ON c.Id=e.CarreraId
			INNER JOIN Modulos m			ON m.Id=e.ModuloId
			WHERE Nombres LIKE '%' + @nombres + '%'
			AND m.Nombre=@modulo
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
			INNER JOIN Carreras c			ON c.Id=e.CarreraId
			INNER JOIN Modulos m			ON m.Id=e.ModuloId
			WHERE (Nombres LIKE '%' + @nombres + '%'
			AND Apellidos LIKE '%' + @apellidos + '%')
			AND m.Nombre=@modulo
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
		INNER JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE r.Estado=1
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
			WHERE r.Estado=0
			AND e.Estado=1
			AND m.Estado=1

			RETURN 0;
		END
		SELECT * FROM @RegistroAsistencia
		RETURN 0;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_usuario]    Script Date: 18/12/2022 19:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_usuario]
	@i_accion			CHAR(2)				=NULL,
	@i_usuario			VARCHAR(25)			=NULL,
	@i_password			VARCHAR(300)		=NULL,
	@i_email			VARCHAR(50)			=NULL,
	@i_id				INT					=NULL
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

	DECLARE @TablaUsuario TABLE(
				Id			INT,
				Usuario		VARCHAR(25),
				Email		VARCHAR(50)
			)

	IF(@i_accion = 'LG')
	BEGIN
		SELECT Id, Usuario, Email FROM Usuarios
		WHERE Usuario=@usuario AND Password=@i_password AND Estado=1
	END
	IF(@i_accion = 'RG')
	BEGIN
		IF EXISTS (SELECT 1 FROM Usuarios WHERE Usuario=@usuario AND Estado=1)
		BEGIN
			INSERT INTO @TablaUsuario (Id) VALUES (0)

			SELECT Id, Usuario, Email FROM @TablaUsuario
			RETURN 0;
		END
		IF EXISTS (SELECT 1 FROM Usuarios WHERE Usuario=@usuario AND Estado=0)
		BEGIN
			UPDATE Usuarios SET
			Estado		= 1,
			Usuario		= @usuario,
			Password	= @i_password,
			Email		= @email
			WHERE Id=(SELECT 1 FROM Usuarios WHERE Usuario=@usuario AND Estado=0)

			SELECT Id, Usuario, Email FROM Usuarios
			WHERE Id=(SELECT Id FROM Usuarios WHERE Usuario=@usuario AND Estado=1)
			AND Estado=1

			RETURN 0;
		END

		INSERT INTO Usuarios (Usuario, Password, Email, Estado) VALUES (@usuario, @i_password, 
		CASE	WHEN @email=''	THEN NULL
				WHEN @email!=''	THEN @email
		END, 1)

		DECLARE @id INT = @@IDENTITY
		SELECT Id, Usuario, Email FROM Usuarios
		WHERE Id=@id
		AND Estado=1
		RETURN 0;
	END
	IF(@i_accion = 'UP')
	BEGIN
		IF EXISTS (SELECT 1 FROM Usuarios WHERE Id=@i_id AND Estado=1)
		BEGIN
			IF EXISTS(SELECT 1 FROM Usuarios WHERE Usuario=@usuario AND Estado=1)
			BEGIN
				INSERT INTO @TablaUsuario (Id) VALUES (-1)

				SELECT Id, Usuario, Email FROM @TablaUsuario
				RETURN 0;
			END
			UPDATE Usuarios SET
			Usuario			=	CASE WHEN ISNULL(@usuario, '')='' THEN Usuario ELSE @usuario END,
			Password		=	CASE WHEN ISNULL(@i_password, '')='' THEN Password ELSE @i_password END,
			Email			=	CASE WHEN ISNULL(@email, '')='' THEN Email ELSE @email END
			WHERE Id=@i_id
			SELECT Id, Usuario, Email FROM Usuarios WHERE Id=@i_id AND Estado=1

			RETURN 0;
		END
		INSERT INTO @TablaUsuario (Id) VALUES (0)

		SELECT Id, Usuario, Email FROM @TablaUsuario
		RETURN 0;
	END
	IF(@i_accion = 'DE')
	BEGIN
		IF EXISTS(SELECT 1 FROM Usuarios WHERE Id=@i_id AND Estado=1)
		BEGIN
			UPDATE Usuarios SET
			Estado = 0
			WHERE Id=@i_id AND Estado=1

			SELECT Id, Usuario, Email FROM Usuarios WHERE Id=@i_id

			RETURN 0;
		END
		SELECT Id, Usuario, Email FROM @TablaUsuario
		RETURN 0;
	END
END
GO
USE [master]
GO
ALTER DATABASE [IDAAI] SET  READ_WRITE 
GO
