USE [master]
GO
/****** Object:  Database [IDAAI]    Script Date: 09/01/2023 21:18:45 ******/
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
/****** Object:  User [idaaiuser]    Script Date: 09/01/2023 21:18:46 ******/
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
/****** Object:  Table [dbo].[Carreras]    Script Date: 09/01/2023 21:18:46 ******/
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
/****** Object:  Table [dbo].[EstadoAsistencia]    Script Date: 09/01/2023 21:18:46 ******/
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
/****** Object:  Table [dbo].[EstadoDevolucion]    Script Date: 09/01/2023 21:18:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoDevolucion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](25) NOT NULL,
 CONSTRAINT [PK_EstadoDevolucion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoItem]    Script Date: 09/01/2023 21:18:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](25) NOT NULL,
 CONSTRAINT [PK_EstadoItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Estudiantes]    Script Date: 09/01/2023 21:18:46 ******/
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
/****** Object:  Table [dbo].[Inventario]    Script Date: 09/01/2023 21:18:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Descripcion] [varchar](300) NULL,
	[CantidadDisponible] [int] NULL,
	[CantidadTotal] [int] NULL,
	[Estado] [tinyint] NULL,
 CONSTRAINT [PK_Inventario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 09/01/2023 21:18:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Rfid] [varchar](100) NOT NULL,
	[EstadoItemId] [int] NULL,
	[InventarioId] [int] NOT NULL,
	[Estado] [tinyint] NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modulos]    Script Date: 09/01/2023 21:18:46 ******/
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
/****** Object:  Table [dbo].[RegistroAsistencia]    Script Date: 09/01/2023 21:18:46 ******/
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
/****** Object:  Table [dbo].[RegistroPrestamoItem]    Script Date: 09/01/2023 21:18:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegistroPrestamoItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FechaPrestado] [datetime] NOT NULL,
	[FechaDevuelto] [datetime] NULL,
	[ItemId] [int] NOT NULL,
	[EstadoDevolucionId] [int] NOT NULL,
	[EstudianteId] [int] NULL,
	[ModuloId] [int] NULL,
	[ModoClase] [bit] NULL,
	[Estado] [tinyint] NULL,
 CONSTRAINT [PK_RegistroPrestamoItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuarioModulo]    Script Date: 09/01/2023 21:18:46 ******/
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
/****** Object:  Table [dbo].[Usuarios]    Script Date: 09/01/2023 21:18:46 ******/
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
ALTER TABLE [dbo].[RegistroPrestamoItem]  WITH CHECK ADD  CONSTRAINT [FK__RegistroP__Estad__59063A47] FOREIGN KEY([EstadoDevolucionId])
REFERENCES [dbo].[EstadoDevolucion] ([Id])
GO
ALTER TABLE [dbo].[RegistroPrestamoItem] CHECK CONSTRAINT [FK__RegistroP__Estad__59063A47]
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
ALTER TABLE [dbo].[RegistroPrestamoItem]  WITH CHECK ADD  CONSTRAINT [FK__RegistroP__Modul__6CD828CA] FOREIGN KEY([ModuloId])
REFERENCES [dbo].[Modulos] ([Id])
GO
ALTER TABLE [dbo].[RegistroPrestamoItem] CHECK CONSTRAINT [FK__RegistroP__Modul__6CD828CA]
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
/****** Object:  StoredProcedure [dbo].[sp_carrera]    Script Date: 09/01/2023 21:18:46 ******/
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
		IF(ISNULL(@nombre,'')='')
		BEGIN
			SELECT Id=c.Id, Nombre=c.Nombre, Modulo=m.Nombre 
			FROM Carreras c
			INNER JOIN Modulos m ON m.Id=c.ModuloId
			WHERE c.Nombre = @nombre
			AND m.Nombre=@modulo
			AND c.Estado=1
			AND m.Estado=1
			RETURN 0;
		END
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
/****** Object:  StoredProcedure [dbo].[sp_estudiante]    Script Date: 09/01/2023 21:18:46 ******/
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
	@i_modulo			VARCHAR(50)			=NULL,
	@i_xmlEstudiantes	XML					=NULL
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
		@codigoIdError			INT
	
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
		
		-- VALIDAR QUE EL MODULO EXISTA (ESTADO=1) Y NO SEA LABORATORIO ABIERTO (ID=1)
		IF NOT EXISTS(SELECT 1 FROM Modulos WHERE Nombre=@modulo AND Id!=1 AND Estado=1)
		BEGIN
			INSERT INTO @Estudiantes (Id) VALUES (0)
			SELECT * FROM @Estudiantes
			RETURN 0;
		END

		SELECT @moduloInsertar=Nombre FROM Modulos 
			WHERE Nombre=@modulo AND Id!=1 AND Estado=1
		
		SELECT @moduloIdInsertar=Id	FROM Modulos
			WHERE Nombre=@modulo AND Id!=1 AND Estado=1
		
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
			IF EXISTS(SELECT 1 FROM Estudiantes 
				WHERE Matricula=(SELECT TOP(1) Matricula FROM @EstudiantesXML) AND Estado=1)
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
				ModuloId=(SELECT Id FROM Modulos WHERE Nombre=@modulo AND Id!=1 AND Estado=1),
				CarreraId=CASE ISNULL(@carreraIdInsertar,'')
					WHEN '' THEN 0 ELSE @carreraIdInsertar END
				WHERE Matricula=@matriculaInsertar
				AND Estado=0

				SELECT @idInsertado=Id FROM Estudiantes 
					WHERE Matricula=@matriculaInsertar AND Estado=1

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
				Direccion, CarreraId, ModuloId, Estado)
			SELECT UPPER(@nombresInsertar), UPPER(@apellidosInsertar), @matriculaInsertar, 
				LOWER(@emailInsertar), UPPER(ISNULL(@direccionInsertar,'')), ISNULL(@idCarrera, 0), 
				@moduloIdInsertar, 1

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
		IF EXISTS (SELECT 1 FROM Estudiantes e
						INNER JOIN Modulos m ON m.Id=e.ModuloId
						WHERE Matricula=@matricula	
						AND  m.Nombre=@modulo
						AND e.Estado=1
						AND m.Estado=1)
		BEGIN
			-- VALIDAR QUE NO EXISTA UN ESTUDIANTE CON LA MATRICULA INGRESADA
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

		IF EXISTS (SELECT 1 FROM Estudiantes e
						INNER JOIN Modulos m ON m.Id=e.ModuloId
						WHERE Matricula=@matricula	
						AND  m.Nombre=@modulo
						AND e.Id !=@i_id
						AND e.Estado=1
						AND m.Estado=1)
		BEGIN
			-- VALIDAR QUE NO EXISTA UN ESTUDIANTE CON LA MATRICULA INGRESADA
			INSERT INTO @Estudiantes (Id) VALUES (-1)

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
		Email		=	CASE	WHEN ISNULL(@email,'')='' THEN e.Email
								ELSE @email
								END,
		Direccion	=	CASE	WHEN ISNULL(@direccion,'')='' THEN e.Direccion
								ELSE @direccion
								END,
		CarreraId	=	ISNULL((SELECT Id FROM Carreras WHERE Nombre=@carrera),@carreraIdAnterior),
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
			AND m.Estado=1

			RETURN 0;
		END
		
		SELECT * FROM @Estudiantes
		RETURN 0;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_inventario]    Script Date: 09/01/2023 21:18:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_inventario]
		@i_accion				CHAR(2)				=	NULL,
		@i_id					INT					=	NULL,
		@i_nombre				VARCHAR(100)			=	NULL,
		@i_descripcion			VARCHAR(300)			=	NULL,
		@i_cantidadTotal		INT					=	NULL,
		@i_cantidadDisponible	INT					=	NULL,
		@i_modulo				VARCHAR(50)			=	NULL,
		@i_accionExt			BIT					=	NULL
AS
BEGIN
	DECLARE
	@nombre						VARCHAR(100),
	@descripcion				VARCHAR(300),
	@modulo						VARCHAR(50),
	@moduloIdInsertar			INT,
	@idInsertado				INT,
	@idInventarioSuperior		INT,
	@cantidadItems				INT,
	@cantidadItemsDisponibles	INT

	SET	@nombre			=	TRIM(@i_nombre)
	SET	@descripcion	=	TRIM(@i_descripcion)
	SET @modulo			=	TRIM(@i_modulo)
	
	DECLARE @Inventario		TABLE(
		Id					INT,
		Nombre				VARCHAR(100),
		Descripcion			VARCHAR(300),
		CantidadDisponible	INT,
		CantidadTotal		INT
	)

	DECLARE @InventarioVerificacion	TABLE(
		Id					INT,
		Nombre				VARCHAR(100),
		Descripcion			VARCHAR(300),
		CantidadDisponible	INT,
		CantidadTotal		INT
	)

	IF(@i_accion='CN')
	BEGIN
		SELECT Id=i.Id, Nombre=i.Nombre, Descripcion=i.Descripcion, 
		CantidadDisponible=i.CantidadDisponible, CantidadTotal=i.CantidadTotal
		FROM Inventario i
		WHERE i.Nombre	LIKE	'%' + @nombre + '%'
		AND i.Estado=1
		RETURN 0;
	END
	IF(@i_accion='IN')
	BEGIN
		-- VALIDACION DE QUE NO EXISTA REGISTRO EN INVENTARIO CON EL MISMO NOMBRE
		IF EXISTS(SELECT 1 FROM Inventario WHERE Nombre=@nombre AND Estado=1)
		BEGIN
			INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(0, 0, 0)
			SELECT TOP(1) * FROM @Inventario
			RETURN 0;
		END

		-- VALIDACION DE REGISTRO LOGICO (ESTADO=1) SI YA EXISTE PERO ESTABA ELIMINADO LOGICAMENTE
		IF EXISTS(SELECT 1 FROM Inventario WHERE Nombre=@nombre AND Estado=0)
		BEGIN
			UPDATE Inventario SET
			Estado=1,
			Nombre=@nombre,
			Descripcion=@descripcion,
			CantidadTotal=ISNULL(@i_cantidadTotal,0),
			CantidadDisponible=ISNULL(@i_cantidadTotal,0)
			WHERE Id=(SELECT Id FROM Inventario WHERE Nombre=@nombre AND Estado=0)

			SELECT @idInsertado=Id FROM Inventario WHERE Nombre=@nombre AND Estado=1

			SELECT Id=i.Id, Nombre=i.Nombre, Descripcion=i.Descripcion, 
			CantidadDisponible=i.CantidadDisponible, CantidadTotal=i.CantidadTotal
			FROM Inventario i
			WHERE i.Id=@idInsertado
			AND i.Estado=1

			RETURN 0;
		END	

		-- INSERCION NUEVO REGISTRO EN INVENTARIO
		INSERT INTO Inventario
		(
			Nombre, Descripcion, CantidadTotal, CantidadDisponible, Estado
		)
		SELECT	
			@nombre, @descripcion, ISNULL(@i_cantidadTotal,0), ISNULL(@i_cantidadTotal,0), 1
		
		SET @idInsertado=@@IDENTITY

		SELECT Id=i.Id, Nombre=i.Nombre, Descripcion=i.Descripcion, 
		CantidadDisponible=i.CantidadDisponible, CantidadTotal=i.CantidadTotal
		FROM Inventario i
		WHERE i.Id=@idInsertado
		AND i.Estado=1

		RETURN 0;
	END
	IF(@i_accion='UP')
	BEGIN
		-- VALIDACION DE QUE EXISTA UN INVENTARIO CON EL ID INGRESADO
		IF NOT EXISTS(SELECT 1 FROM Inventario WHERE Id=@i_id AND Estado=1)
		BEGIN
			INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(0, 0, 0)
			SELECT TOP(1) * FROM @Inventario
			RETURN 0;
		END

		-- VALIDACION DE QUE NO EXISTA OTRO INVENTARIO CON EL NOMBRE INGRESADO
		IF ((ISNULL(@nombre,'')!='')AND(EXISTS(SELECT 1 FROM Inventario WHERE Nombre=@nombre AND Id!=@i_id AND Estado=1)))
		BEGIN
			INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(-1, 0, 0)
			SELECT TOP(1) * FROM @Inventario
			RETURN 0;
		END

		-- ACTUALIZACION DE REGISTRO DE INVENTARIO
		UPDATE Inventario SET
		Nombre=CASE ISNULL(@nombre,'') WHEN '' THEN Inventario.Nombre 
			ELSE @nombre END,
		Descripcion=CASE ISNULL(@descripcion,'') WHEN '' THEN Inventario.Descripcion 
			ELSE @descripcion END,
		CantidadDisponible=CASE ISNULL(@i_cantidadDisponible,'') WHEN '' THEN Inventario.CantidadDisponible 
			ELSE @i_cantidadDisponible END,
		CantidadTotal=CASE ISNULL(@i_cantidadTotal,'') WHEN '' THEN Inventario.CantidadTotal 
			ELSE @i_cantidadTotal END
		WHERE Id=@i_id
		AND Estado=1

		SELECT Id=i.Id, Nombre=i.Nombre, Descripcion=i.Descripcion, 
		CantidadDisponible=i.CantidadDisponible, CantidadTotal=i.CantidadTotal
		FROM Inventario i
		WHERE i.Id=@i_id
		AND i.Estado=1

		RETURN 0;
	END

	IF(@i_accion='DE')
	BEGIN
		-- VALIDAR QUE EL ELEMENTO EXISTA (ESTADO=1)
		IF NOT EXISTS(SELECT 1 FROM Inventario WHERE Id=@i_id AND Estado=1)
		BEGIN
			INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(0, 0, 0)
			SELECT TOP(1) * FROM @Inventario
			RETURN 0;
		END

		UPDATE Inventario SET
		Estado=0
		WHERE Id=@i_id
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=i.Id, Nombre=i.Nombre, Descripcion=i.Descripcion, 
		CantidadDisponible=i.CantidadDisponible, CantidadTotal=i.CantidadTotal
		FROM Inventario i
		WHERE i.Id=@i_id
		AND i.Estado=0

		RETURN 0;
	END

	-- VERIFICACION Y ACTUALIZACION DE CANTIDADES TOTALES Y DISPONIBLES EN INVENTARIO
	IF(@i_accion='VE')
	BEGIN
		INSERT INTO @InventarioVerificacion
		SELECT Id, Nombre, Descripcion, CantidadDisponible, CantidadTotal
		FROM Inventario WHERE Estado=1

		WHILE(1=1)
		BEGIN
			IF(SELECT COUNT(*) FROM @InventarioVerificacion)=0
				BREAK;
			
			SELECT TOP(1) @idInventarioSuperior=Id FROM @InventarioVerificacion

			SELECT @cantidadItems=COUNT(*) FROM Items 
			WHERE InventarioId=@idInventarioSuperior AND Estado=1

			SELECT @cantidadItemsDisponibles=COUNT(*) FROM Items 
			WHERE InventarioId=@idInventarioSuperior AND EstadoItemId=1 AND Estado=1

			UPDATE Inventario SET
			CantidadTotal=@cantidadItems,
			CantidadDisponible=@cantidadItemsDisponibles
			WHERE Id=@idInventarioSuperior

			DELETE TOP(1) FROM @InventarioVerificacion
		END

		IF(@i_accionExt=1)
			RETURN 0;

		INSERT INTO @Inventario(Id, CantidadDisponible, CantidadTotal) 
			VALUES(0, 0, 0)
		SELECT TOP(1) * FROM @Inventario
		
		RETURN 0;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_item]    Script Date: 09/01/2023 21:18:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC	[dbo].[sp_item]
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
			WHERE it.Rfid=@i_rfid
			AND i.Nombre LIKE '%' + @inventario + '%'
			AND it.Estado=1
			AND i.Estado=1

			RETURN 0;
		END
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_modulo]    Script Date: 09/01/2023 21:18:46 ******/
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
		AND Id!=1
		AND Estado=1
		RETURN 0;
	END
	IF(@i_accion = 'CP')
	BEGIN
		SELECT Id, Nombre, Descripcion, PeriodoAcademico FROM Modulos 
		WHERE PeriodoAcademico LIKE '%' + @periodoAcademico + '%'
		AND Id!=1
		AND Estado=1
		RETURN 0;
	END
	IF(@i_accion = 'CT')
	BEGIN
		SELECT Id, Nombre, Descripcion, PeriodoAcademico FROM Modulos
		WHERE Estado=1
		AND Id!=1
		RETURN 0;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_prestamoitem]    Script Date: 09/01/2023 21:18:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_prestamoitem]
		@i_accion			CHAR(2)			=	NULL,
		@i_id				INT				=	NULL,
		@i_matricula		VARCHAR(10)		=	NULL,
		@i_rfid				VARCHAR(100)	=	NULL,
		@i_modoClases		BIT				=	NULL,
		@i_modulo			VARCHAR(50)		=	NULL,
		@i_grupoItems		BIT				=	NULL,
		@i_xmlItems			XML				=	NULL
AS
BEGIN
	DECLARE
	@fechaAhora					DATETIME,
	@estudianteId				INT,
	@itemId						INT,
	@idInsertado				INT,
	@modulo						VARCHAR(50),
	@moduloId					INT,
	@xmlPuntero					INT,
	@rfidSuperior				VARCHAR(10),
	@rfid						VARCHAR(100),
	@FechaPrestadoInsertar		DATETIME,
	@ItemIdInsertar				INT,
	@EstadoDevolucionIdInsertar	INT,
	@ModuloIdInsertar			INT,
	@ModoClaseInsertar			BIT

	SET @fechaAhora = GETDATE()
	SET @modulo=TRIM(@i_modulo)

	DECLARE @PrestamoItemEstudiante TABLE(
		Id					INT,
		FechaPrestado		DATETIME,
		FechaDevuelto		DATETIME,
		Inventario			VARCHAR(25),
		Item				VARCHAR(100),
		Nombres				VARCHAR(100),
		Apellidos			VARCHAR(100),
		Matricula			VARCHAR(10),
		Email				VARCHAR(50),
		EstadoDevolucion	VARCHAR(25)
	)

	DECLARE @PrestamoItemClase TABLE(
		Id					INT,
		FechaPrestado		DATETIME,
		FechaDevuelto		DATETIME,
		Inventario			VARCHAR(25),
		Item				VARCHAR(100),
		Modulo				VARCHAR(50),
		EstadoDevolucion	VARCHAR(25)
	)

	DECLARE @PrestamoItemInsertar TABLE(
		FechaPrestado		DATETIME,
		ItemId				INT,
		EstadoDevolucionId	INT,
		ModuloId			INT,
		ModoClase			BIT
	)

	DECLARE @ItemsIds TABLE(
		Id					INT
	)

	DECLARE @PrestamoItemsIds TABLE(
		Id					INT
	)

	DECLARE @ItemsXML TABLE(
		Rfid				VARCHAR(100)
	)

	IF(@i_accion='IN')
	BEGIN		
		-- SI EL REGISTRO ES POR CLASE (LABORATORIO CERRADO) - REGISTRO DE ITEM UNICO
		IF(@i_modoClases=1)
		BEGIN
			-- VALIDACION PARA QUE EL MODULO INGRESADO EXISTA Y NO SEA ID 1 (LABORATORIO ABIERTO)
			IF NOT EXISTS(SELECT 1 FROM Modulos WHERE Nombre=@modulo AND Id!=1 AND Estado=1)
			BEGIN
				INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
				VALUES(-3, GETDATE(), GETDATE())
				SELECT TOP(1) * FROM @PrestamoItemClase
				RETURN 0;
			END

			-- REGISTRO POR GRUPO DE ITEMS

			-- VALIDACION SI SE ENVIA LISTA XML DE X ITEMS A REGISTRAR
			IF(ISNULL(@i_grupoItems,0)!=0)
			BEGIN
				EXEC SP_XML_PREPAREDOCUMENT	@xmlPuntero OUTPUT, @i_xmlItems	

				SELECT * INTO #ITEMSXML FROM OPENXML(@xmlPuntero, 'Items/Item', 2)
				WITH(
					Rfid			VARCHAR(100)		'Rfid'
				)

				--EXEC sp_xml_removedocument 
				--		@xmlPuntero
				
				INSERT INTO @ItemsXML
				SELECT * FROM #ITEMSXML

				-- VALIDACION DE QUE NO HAYAN ELEMENTOS (RFID) REPETIDOS EN EL XML
				IF((SELECT COUNT(DISTINCT Rfid) FROM @ItemsXML)!=(SELECT COUNT(Rfid) FROM @ItemsXML))
				BEGIN
					INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
					VALUES(-6, GETDATE(), GETDATE())
					SELECT TOP(1) * FROM @PrestamoItemClase
					RETURN 0;
				END

				WHILE(1=1)
				BEGIN
					IF(SELECT COUNT(*) FROM @ItemsXML)=0
						BREAK;

					SELECT TOP(1) @rfidSuperior=Rfid FROM @ItemsXML

					-- VALIDACION DE QUE EXISTA UN ITEM CON EL RFID ITERADO
					IF NOT EXISTS(SELECT 1 FROM Items WHERE Rfid=@rfidSuperior AND Estado=1)
					BEGIN
						INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
						VALUES(-7, GETDATE(), GETDATE())
						SELECT TOP(1) * FROM @PrestamoItemClase
						RETURN 0;
					END

					-- VALIDACION DE QUE EL ITEM ITERADO ESTE DISPONIBLE
					IF((SELECT EstadoItemId FROM Items WHERE Rfid=@rfidSuperior AND Estado=1)=2)
					BEGIN
						INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
						VALUES(-8, GETDATE(), GETDATE())
						SELECT TOP(1) * FROM @PrestamoItemClase
						RETURN 0;
					END

					-- INSERCION DE NUEVO REGISTRO DE PRESTAMO DE ITEM ITERADO A MODULO
					SELECT @moduloId=Id FROM Modulos WHERE Nombre=@modulo AND Id!=1 AND Estado=1
					SELECT @itemId=Id FROM Items WHERE Rfid=@rfidSuperior AND Estado=1

					INSERT INTO @PrestamoItemInsertar
					(
						FechaPrestado, ItemId, EstadoDevolucionId, ModuloId, ModoClase
					)
					SELECT
						@fechaAhora, @itemId, 1, @moduloId, 1									
					
					DELETE TOP(1) FROM @ItemsXML
				END

				WHILE(1=1)
				BEGIN
					IF(SELECT COUNT(*) FROM @PrestamoItemInsertar)=0
						BREAK;

					SELECT TOP(1) @FechaPrestadoInsertar=FechaPrestado FROM @PrestamoItemInsertar
					SELECT TOP(1) @ItemIdInsertar=ItemId FROM @PrestamoItemInsertar
					SELECT TOP(1) @EstadoDevolucionIdInsertar=EstadoDevolucionId FROM @PrestamoItemInsertar
					SELECT TOP(1) @ModuloIdInsertar=ModuloId FROM @PrestamoItemInsertar
					SELECT TOP(1) @ModoClaseInsertar=ModoClase FROM @PrestamoItemInsertar

					INSERT INTO RegistroPrestamoItem
					(
						FechaPrestado, ItemId, EstadoDevolucionId, ModuloId, ModoClase, Estado
					)
					SELECT
						@FechaPrestadoInsertar, 
						@ItemIdInsertar,
						@EstadoDevolucionIdInsertar,
						@ModuloIdInsertar,
						@ModoClaseInsertar,
						1
					
					SET @idInsertado=@@IDENTITY
					INSERT INTO @ItemsIds(Id) SELECT @ItemIdInsertar
					INSERT INTO @PrestamoItemsIds(Id) SELECT @idInsertado

					DELETE TOP(1) FROM @PrestamoItemInsertar
				END

				UPDATE Items SET
				EstadoItemId=2
				WHERE Id IN (SELECT Id FROM @ItemsIds)
				AND Estado=1

				EXEC sp_inventario
				@i_accion		=	'VE',
				@i_accionExt	=	1

				SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
				Inventario=i.Nombre, Item=it.Rfid, Modulo=m.Nombre, EstadoDevolucion=ed.Descripcion
				FROM RegistroPrestamoItem r
				INNER JOIN Modulos m			ON m.Id=r.ModuloId
				INNER JOIN Items it				ON it.Id=r.ItemId
				INNER JOIN Inventario i			ON i.Id=it.InventarioId
				INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
				WHERE r.Id IN (SELECT Id FROM @PrestamoItemsIds)
				AND m.Estado=1
				AND it.Estado=1
				AND i.Estado=1
				AND r.Estado=1

				RETURN 0
			END
			
			-- REGISTRO POR ITEM UNICO EN MODULO

			-- VALIDACION DE QUE EXISTA UN ITEM CON EL RFID INGRESADO
			IF NOT EXISTS(SELECT 1 FROM Items WHERE Rfid=@i_rfid AND Estado=1)
			BEGIN
				INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
				VALUES(-4, GETDATE(), GETDATE())
				SELECT TOP(1) * FROM @PrestamoItemClase
				RETURN 0;
			END

			-- VALIDACION DE QUE EL ITEM INGRESADO ESTE DISPONIBLE
			IF((SELECT EstadoItemId FROM Items WHERE Rfid=@i_rfid AND Estado=1)=2)
			BEGIN
				INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
				VALUES(-5, GETDATE(), GETDATE())
				SELECT TOP(1) * FROM @PrestamoItemClase
				RETURN 0;
			END
			
			-- INSERCION DE NUEVO REGISTRO DE PRESTAMO DE ITEM A MODULO
			SELECT @moduloId=Id FROM Modulos WHERE Nombre=@modulo AND Id!=1 AND Estado=1
			SELECT @itemId=Id FROM Items WHERE Rfid=@i_rfid AND Estado=1

			INSERT INTO RegistroPrestamoItem
			(
				FechaPrestado, ItemId, EstadoDevolucionId, ModuloId, ModoClase, Estado
			)
			SELECT
				@fechaAhora, @itemId, 1, @moduloId, 1, 1
		
			SET @idInsertado=@@IDENTITY

			UPDATE Items SET
			EstadoItemId=2
			WHERE Id=@itemId
			AND Estado=1

			EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

			SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
			Inventario=i.Nombre, Item=it.Rfid, Modulo=m.Nombre, EstadoDevolucion=ed.Descripcion
			FROM RegistroPrestamoItem r
			INNER JOIN Modulos m			ON m.Id=r.ModuloId
			INNER JOIN Items it				ON it.Id=r.ItemId
			INNER JOIN Inventario i			ON i.Id=it.InventarioId
			INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
			WHERE r.Id=@idInsertado
			AND m.Estado=1
			AND it.Estado=1
			AND i.Estado=1
			AND r.Estado=1

			RETURN 0;
		END
		
		-- REGISTRO POR ESTUDIANTE

		-- VALIDACION DE QUE EXISTA UN ESTUDIANTE CON LA MATRICULA INGRESADA
		IF NOT EXISTS(SELECT 1 FROM Estudiantes e INNER JOIN Modulos m ON m.Id=e.ModuloId
					WHERE e.Matricula=@i_matricula AND m.Id=1 AND m.Estado=1 AND e.Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(0, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END		

		-- VALIDACION DE QUE EXISTA UN ITEM CON EL RFID INGRESADO
		IF NOT EXISTS(SELECT 1 FROM Items WHERE Rfid=@i_rfid AND Estado=1)
		BEGIN
		INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-1, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END

		-- VALIDACION DE QUE EL ITEM INGRESADO ESTE DISPONIBLE
		IF((SELECT EstadoItemId FROM Items WHERE Rfid=@i_rfid AND Estado=1)=2)
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-2, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END

		-- INSERCION DE NUEVO REGISTRO DE PRESTAMO DE ITEM A ESTUDIANTE
		SELECT @estudianteId=e.Id FROM Estudiantes e
		INNER JOIN Modulos m ON m.Id=e.ModuloId
		WHERE e.Matricula=@i_matricula AND m.Id=1 AND e.Estado=1 AND m.Estado=1
		SELECT TOP(1) @itemId=Id FROM Items WHERE Rfid=@i_rfid AND Estado=1

		INSERT INTO RegistroPrestamoItem
		(
			FechaPrestado, ItemId, EstadoDevolucionId, EstudianteId, ModoClase, Estado
		)
		SELECT
			@fechaAhora, @itemId, 1, @estudianteId, 0, 1
		
		SET @idInsertado=@@IDENTITY

		UPDATE Items SET
		EstadoItemId=2
		WHERE Id=@itemId
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Nombres=e.Nombres, Apellidos=e.Apellidos,
		Matricula=e.Matricula, Email=e.Email, EstadoDevolucion=ed.Descripcion, Modulo=m.Nombre
		FROM RegistroPrestamoItem r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON ed.Id=r.EstadoDevolucionId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE r.Id=@idInsertado
		AND e.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND m.Estado=1
		AND r.Estado=1

		RETURN 0;
	END

	IF(@i_accion='UE')
	BEGIN
		-- VALIDACION DE QUE DEBE EXISTIR EL REGISTRO DE PRESTAMO DE ITEM CON EL ID INGRESADO
		IF NOT EXISTS(SELECT 1 FROM RegistroPrestamoItem WHERE Id=@i_id AND Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(0, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END

		-- VALIDACION DE QUE EL ITEM DEBE TENER ESTADO PRESTADO PARA DEVOLERSE
		IF EXISTS(SELECT 1 FROM RegistroPrestamoItem r 
			INNER JOIN EstadoDevolucion ed ON ed.Id=r.EstadoDevolucionId
			WHERE r.Id=@i_id AND (ed.Id=2 OR ed.Id=4) AND r.Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-1, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END

		-- VALIDACION DE QUE EL ITEM DEBE HABER SIDO PRESTADO A ESTUDIANTE
		IF ISNULL((SELECT ModuloId FROM RegistroPrestamoItem WHERE Id=@i_id),'')!=''
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-2, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END

		UPDATE RegistroPrestamoItem SET
		FechaDevuelto=@fechaAhora,
		EstadoDevolucionId=2
		WHERE Id=@i_id
		AND Estado=1		

		SELECT @rfid=it.Rfid FROM RegistroPrestamoItem  r
		INNER JOIN Items it ON it.Id=r.ItemId
		WHERE r.Id=@i_id

		UPDATE Items SET
		EstadoItemId=1
		WHERE Id=(SELECT Id FROM Items WHERE Rfid=@rfid)
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Nombres=e.Nombres, Apellidos=e.Apellidos,
		Matricula=e.Matricula, Email=e.Email, EstadoDevolucion=ed.Descripcion
		FROM RegistroPrestamoItem r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
		WHERE r.Id=@i_id
		AND e.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND r.Estado=1

		RETURN 0;
	END

	IF(@i_accion='UM')
	BEGIN
		-- VALIDACION DE QUE DEBE EXISTIR EL REGISTRO DE PRESTAMO DE ITEM CON EL ID INGRESADO
		IF NOT EXISTS(SELECT 1 FROM RegistroPrestamoItem WHERE Id=@i_id AND Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
			VALUES(0, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemClase
			RETURN 0;
		END

		-- VALIDACION DE QUE EL ITEM DEBE TENER ESTADO PRESTADO PARA DEVOLERSE
		IF EXISTS(SELECT 1 FROM RegistroPrestamoItem r 
			INNER JOIN EstadoDevolucion ed ON ed.Id=r.EstadoDevolucionId
			WHERE r.Id=@i_id AND (ed.Id=2 OR ed.Id=4) AND r.Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-1, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemClase
			RETURN 0;
		END

		-- VALIDACION DE QUE EL ITEM DEBE HABER SIDO PRESTADO A MODULO
		IF ISNULL((SELECT EstudianteId FROM RegistroPrestamoItem WHERE Id=@i_id),'')!=''
		BEGIN
			INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
			VALUES(-2, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemClase
			RETURN 0;
		END

		UPDATE RegistroPrestamoItem SET
		FechaDevuelto=@fechaAhora,
		EstadoDevolucionId=2
		WHERE Id=@i_id
		AND Estado=1

		SELECT @rfid=it.Rfid FROM RegistroPrestamoItem  r
		INNER JOIN Items it ON it.Id=r.ItemId
		WHERE r.Id=@i_id

		UPDATE Items SET
		EstadoItemId=1
		WHERE Id=(SELECT Id FROM Items WHERE Rfid=@rfid)
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Modulo=m.Nombre, EstadoDevolucion=ed.Descripcion
		FROM RegistroPrestamoItem r
		INNER JOIN Modulos m			ON m.Id=r.ModuloId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
		WHERE r.Id=@i_id
		AND m.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND r.Estado=1

		RETURN 0;
	END

	IF(@i_accion='DE')
	BEGIN
		-- VALIDAR QUE EL ELEMENTO EXISTA (ESTADO=1)
		IF NOT EXISTS(SELECT 1 FROM RegistroPrestamoItem WHERE Id=@i_id AND ModoClase=0 AND Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemEstudiante(Id, FechaPrestado, FechaDevuelto) 
			VALUES(0, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemEstudiante
			RETURN 0;
		END
		
		UPDATE RegistroPrestamoItem SET
		Estado=0
		WHERE Id=@i_id
		AND Estado=1

		UPDATE Items SET
		EstadoItemId=1
		WHERE Id=(SELECT ItemId FROM RegistroPrestamoItem WHERE Id=@i_id)
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1

		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Nombres=e.Nombres, Apellidos=e.Apellidos,
		Matricula=e.Matricula, Email=e.Email, EstadoDevolucion=ed.Descripcion
		FROM RegistroPrestamoItem r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
		WHERE r.Id=@i_id
		AND e.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND r.Estado=0

		RETURN 0;
	END

	IF(@i_accion='DM')
	BEGIN
		-- VALIDAR QUE EL ELEMENTO EXISTA (ESTADO=1)
		IF NOT EXISTS(SELECT 1 FROM RegistroPrestamoItem WHERE Id=@i_id AND ModoClase=1 AND Estado=1)
		BEGIN
			INSERT INTO @PrestamoItemClase(Id, FechaPrestado, FechaDevuelto) 
			VALUES(0, GETDATE(), GETDATE())
			SELECT TOP(1) * FROM @PrestamoItemClase
			RETURN 0;
		END
		
		UPDATE RegistroPrestamoItem SET
		Estado=0
		WHERE Id=@i_id
		AND Estado=1

		UPDATE Items SET
		EstadoItemId=1
		WHERE Id=(SELECT ItemId FROM RegistroPrestamoItem WHERE Id=@i_id)
		AND Estado=1

		EXEC sp_inventario
			@i_accion		=	'VE',
			@i_accionExt	=	1
		
		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Modulo=m.Nombre, EstadoDevolucion=ed.Descripcion
		FROM RegistroPrestamoItem r
		INNER JOIN Modulos m			ON m.Id=r.ModuloId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
		WHERE r.Id=@i_id
		AND m.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND r.Estado=0

		RETURN 0;
	END

	IF(@i_accion='CE')
	BEGIN
		IF(ISNULL(@i_matricula,'')='')
		BEGIN
			SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
			Inventario=i.Nombre, Item=it.Rfid, Nombres=e.Nombres, Apellidos=e.Apellidos,
			Matricula=e.Matricula, Email=e.Email, EstadoDevolucion=ed.Descripcion
			FROM RegistroPrestamoItem r
			INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
			INNER JOIN Items it				ON it.Id=r.ItemId
			INNER JOIN Inventario i			ON i.Id=it.InventarioId
			INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
			WHERE e.Estado=1
			AND it.Estado=1
			AND i.Estado=1
			AND r.Estado=1

			RETURN 0;
		END

		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Nombres=e.Nombres, Apellidos=e.Apellidos,
		Matricula=e.Matricula, Email=e.Email, EstadoDevolucion=ed.Descripcion
		FROM RegistroPrestamoItem r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
		WHERE e.Matricula=@i_matricula
		AND e.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND r.Estado=1
		AND r.ModoClase=0

		RETURN 0;
	END

	IF(@i_accion='CM')
	BEGIN
		SELECT Id=r.Id, FechaPrestado=r.FechaPrestado, FechaDevuelto=r.FechaDevuelto,
		Inventario=i.Nombre, Item=it.Rfid, Modulo=m.Nombre, EstadoDevolucion=ed.Descripcion
		FROM RegistroPrestamoItem r
		INNER JOIN Modulos m			ON m.Id=r.ModuloId
		INNER JOIN Items it				ON it.Id=r.ItemId
		INNER JOIN Inventario i			ON i.Id=it.InventarioId
		INNER JOIN EstadoDevolucion ed	ON  ed.Id=r.EstadoDevolucionId
		WHERE m.Nombre=@modulo
		AND m.Estado=1
		AND it.Estado=1
		AND i.Estado=1
		AND r.Estado=1
		AND r.ModoClase=1

		RETURN 0;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_registroasistencia]    Script Date: 09/01/2023 21:18:46 ******/
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
			WHERE r.Id=@i_idRegistroAsistencia
			AND r.Estado=0
			AND e.Estado=1
			AND m.Estado=1

			RETURN 0;
		END
		SELECT * FROM @RegistroAsistencia
		RETURN 0;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_usuario]    Script Date: 09/01/2023 21:18:46 ******/
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
