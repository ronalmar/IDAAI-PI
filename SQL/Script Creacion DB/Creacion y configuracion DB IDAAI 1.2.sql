USE [master]
GO
/****** Object:  Database [IDAAI]    Script Date: 04/12/2022 16:54:51 ******/
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
/****** Object:  User [idaaiuser]    Script Date: 04/12/2022 16:54:52 ******/
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
/****** Object:  Table [dbo].[Carreras]    Script Date: 04/12/2022 16:54:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carreras](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Carreras] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoAsistencia]    Script Date: 04/12/2022 16:54:52 ******/
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
/****** Object:  Table [dbo].[EstadoDevolucion]    Script Date: 04/12/2022 16:54:52 ******/
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
/****** Object:  Table [dbo].[EstadoItem]    Script Date: 04/12/2022 16:54:52 ******/
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
/****** Object:  Table [dbo].[Estudiantes]    Script Date: 04/12/2022 16:54:52 ******/
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
	[CarreraId] [int] NOT NULL,
	[ModuloId] [int] NOT NULL,
 CONSTRAINT [PK_Estudiantes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inventario]    Script Date: 04/12/2022 16:54:52 ******/
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
 CONSTRAINT [PK_Inventario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 04/12/2022 16:54:52 ******/
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
/****** Object:  Table [dbo].[Modulos]    Script Date: 04/12/2022 16:54:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modulos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Descripcion] [varchar](100) NULL,
 CONSTRAINT [PK_Modulos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RegistroAsistencia]    Script Date: 04/12/2022 16:54:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegistroAsistencia](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[EstudianteId] [int] NOT NULL,
	[EstadoAsistenciaId] [int] NOT NULL,
 CONSTRAINT [PK_RegistroAsistencia] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RegistroPrestamoItem]    Script Date: 04/12/2022 16:54:52 ******/
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
 CONSTRAINT [PK_RegistroPrestamoItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuarioModulo]    Script Date: 04/12/2022 16:54:52 ******/
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
/****** Object:  Table [dbo].[Usuarios]    Script Date: 04/12/2022 16:54:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Usuario] [varchar](25) NOT NULL,
	[Password] [varchar](300) NOT NULL,
	[Email] [varchar](50) NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Inventario]  WITH CHECK ADD FOREIGN KEY([ModuloId])
REFERENCES [dbo].[Modulos] ([Id])
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
/****** Object:  StoredProcedure [dbo].[sp_estudiante]    Script Date: 04/12/2022 16:54:52 ******/
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
/****** Object:  StoredProcedure [dbo].[sp_modulo]    Script Date: 04/12/2022 16:54:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_modulo]
		@i_accion				CHAR(2)				=	NULL,
		@i_nombre				VARCHAR(50)			=	NULL,
		@i_descripcion			VARCHAR(100)		=	NULL,
		@i_id					INT					=	NULL
AS
BEGIN
	DECLARE 
	@nombre				VARCHAR(50),
	@descripcion		VARCHAR(100),
	@descripcionTrim	VARCHAR(100),
	@idInsertado		INT

	SET @nombre				=	TRIM(UPPER(@i_nombre))
	SET @descripcionTrim	=	TRIM(@i_descripcion)
	SET @descripcion			=	CASE
		WHEN @descripcionTrim != ''	THEN	 UPPER(LEFT(@descripcionTrim, 1))	+ LOWER(RIGHT(@descripcionTrim,LEN(@descripcionTrim)-1))
		WHEN @descripcionTrim = ''	THEN @descripcionTrim
		END

	IF(@i_accion = 'IN')
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM Modulos WHERE Nombre=@nombre)
		BEGIN
			INSERT INTO Modulos VALUES (@nombre, @descripcion)

			SET @idInsertado=@@IDENTITY
			SELECT Id, Nombre, Descripcion FROM Modulos WHERE Id=@idInsertado

			RETURN 0;
		END
		SELECT Id, Nombre, Descripcion FROM Modulos WHERE Id=0
	END
	IF(@i_accion = 'UP')
	BEGIN
		IF EXISTS (SELECT 1 FROM Modulos WHERE Id=@i_id)
		BEGIN
			UPDATE Modulos SET
			Nombre			=	CASE	WHEN EXISTS(SELECT 1 FROM Modulos WHERE Nombre=@nombre)	THEN Nombre
										WHEN @nombre!=''		THEN	@nombre
								ELSE	Nombre				END,
			Descripcion		=	CASE	WHEN @descripcion!=''	THEN	 @descripcion
								ELSE	Descripcion		END
			WHERE Id=@i_id
			SELECT Id, Nombre, Descripcion FROM Modulos WHERE Id=@i_id

			RETURN 0;
		END
		SELECT Id, Nombre, Descripcion FROM Modulos WHERE Id=0
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_registroasistencia]    Script Date: 04/12/2022 16:54:52 ******/
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
/****** Object:  StoredProcedure [dbo].[sp_usuario]    Script Date: 04/12/2022 16:54:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_usuario]
	@i_accion			CHAR(2)				=NULL,
	@i_usuario			VARCHAR(25)			=NULL,
	@i_password			VARCHAR(300)		=NULL,
	@i_email			VARCHAR(50)			=NULL
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


	IF(@i_accion = 'LG')
	BEGIN
		SELECT Id, Usuario, Email FROM Usuarios
		WHERE Usuario=@usuario AND Password=@i_password
	END
	IF(@i_accion = 'RG')
	BEGIN
		IF EXISTS (SELECT 1 FROM Usuarios WHERE Usuario=@usuario)
		BEGIN
			DECLARE @TablaUsuario TABLE(
				Id			INT,
				Usuario		VARCHAR(25), 
				Email		VARCHAR(50)
			)

			INSERT INTO @TablaUsuario (Id) VALUES (0)

			SELECT Id, Usuario, Email FROM @TablaUsuario
			RETURN 0;
		END

		INSERT INTO Usuarios (Usuario, Password, Email) VALUES (@usuario, @i_password, 
		CASE	WHEN @email=''	THEN NULL
				WHEN @email!=''	THEN @email
		END)

		DECLARE @id INT = @@IDENTITY
		SELECT Id, Usuario, Email FROM Usuarios
		WHERE Id=@id
	END
END
GO
USE [master]
GO
ALTER DATABASE [IDAAI] SET  READ_WRITE 
GO
