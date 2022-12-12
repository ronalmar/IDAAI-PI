USE [master]
GO
/****** Object:  Database [IDAAI]    Script Date: 30/11/2022 11:12:29 ******/
CREATE DATABASE [IDAAI]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'IDAAI', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\IDAAI.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'IDAAI_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\IDAAI_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
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
/****** Object:  User [idaaiuser]    Script Date: 30/11/2022 11:12:29 ******/
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
/****** Object:  Table [dbo].[Carreras]    Script Date: 30/11/2022 11:12:29 ******/
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
/****** Object:  Table [dbo].[EstadoAsistencia]    Script Date: 30/11/2022 11:12:29 ******/
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
/****** Object:  Table [dbo].[EstadoDevolucion]    Script Date: 30/11/2022 11:12:29 ******/
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
/****** Object:  Table [dbo].[EstadoItem]    Script Date: 30/11/2022 11:12:29 ******/
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
/****** Object:  Table [dbo].[Estudiantes]    Script Date: 30/11/2022 11:12:29 ******/
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
/****** Object:  Table [dbo].[Inventario]    Script Date: 30/11/2022 11:12:29 ******/
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
/****** Object:  Table [dbo].[Items]    Script Date: 30/11/2022 11:12:29 ******/
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
/****** Object:  Table [dbo].[Modulos]    Script Date: 30/11/2022 11:12:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modulos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Descripcion] [varchar](100) NULL,
	[UsuarioModuloId] [int] NULL,
 CONSTRAINT [PK_Modulos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RegistroAsistencia]    Script Date: 30/11/2022 11:12:29 ******/
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
/****** Object:  Table [dbo].[RegistroPrestamoItem]    Script Date: 30/11/2022 11:12:29 ******/
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
/****** Object:  Table [dbo].[UsuarioModulo]    Script Date: 30/11/2022 11:12:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioModulo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdModulo] [int] NOT NULL,
 CONSTRAINT [PK_UsuarioModulo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 30/11/2022 11:12:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Usuario] [varchar](25) NOT NULL,
	[Password] [varchar](200) NOT NULL,
	[UsuarioModuloId] [int] NULL,
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
ALTER TABLE [dbo].[Modulos]  WITH CHECK ADD FOREIGN KEY([UsuarioModuloId])
REFERENCES [dbo].[UsuarioModulo] ([Id])
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
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK__Usuarios__Usuari__70DDC3D8] FOREIGN KEY([UsuarioModuloId])
REFERENCES [dbo].[UsuarioModulo] ([Id])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK__Usuarios__Usuari__70DDC3D8]
GO
/****** Object:  StoredProcedure [dbo].[sp_estudiante]    Script Date: 30/11/2022 11:12:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_estudiante]
	@i_accion			CHAR(2)				=null,
	@i_id				INT					=null,
	@i_nombres			VARCHAR(100)		=null,
	@i_apellidos		VARCHAR(100)		=null,
	@i_matricula		VARCHAR(10)			=null,
	@i_email			VARCHAR(50)			=null,
	@i_direccion		VARCHAR(100)		=null,
	@i_carrera			VARCHAR(100)		=null,
	@i_modulo			VARCHAR(50)			=null
AS
BEGIN
	DECLARE 
		@idCarrera		INT,
		@idModulo		INT

	IF(@i_accion = 'IN')
	BEGIN
		IF EXISTS (SELECT 1 FROM Estudiantes WHERE Matricula=@i_matricula)
		BEGIN
			SELECT ERROR='La matrícula ya existe en el registro'
			RETURN 0;
		END

		SELECT @idCarrera=Id FROM Carreras WHERE Nombre = @i_carrera
		SELECT @idModulo=Id	FROM Modulos WHERE Nombre = @i_modulo

		INSERT INTO Estudiantes (Nombres, Apellidos, Matricula, Email, Direccion, CarreraId, ModuloId)
		SELECT @i_nombres, @i_apellidos, @i_matricula, @i_email, @i_direccion, @idCarrera, @idModulo

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
		WHERE Email = @i_email
		AND m.Nombre=@i_modulo
	END
	IF(@i_accion = 'CL')
	BEGIN
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		INNER JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE m.Nombre = @i_modulo
	END
	IF(@i_accion = 'CM')
	BEGIN
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		INNER JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE Matricula = @i_matricula
		AND m.Nombre=@i_modulo
	END
	IF(@i_accion = 'CN')
	BEGIN
		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		INNER JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE (Nombres LIKE '%' + @i_nombres + '%'
		OR Apellidos LIKE '%' + @i_apellidos + '%')
		AND m.Nombre=@i_modulo
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
		SELECT @idCarrera=Id FROM Carreras WHERE Nombre = @i_carrera
		SELECT @idModulo=Id	FROM Modulos WHERE Nombre = @i_modulo

		UPDATE e SET 
		Nombres		=	ISNULL(@i_nombres, e.Nombres),
		Apellidos	=	ISNULL(@i_apellidos, e.Apellidos),
		Matricula	=	ISNULL(@i_matricula, e.Matricula),
		Email		=	ISNULL(@i_email, e.Email),
		Direccion	=	ISNULL(@i_direccion, e.Direccion),
		CarreraId	=	ISNULL(@idCarrera, c.Id),
		ModuloId	=	ISNULL(@idModulo, m.Id)
		FROM Modulos m 
		INNER JOIN Estudiantes e ON e.ModuloId=m.Id
		INNER JOIN Carreras c ON c.Id=e.CarreraId
		WHERE e.Id = @i_id
		AND c.Nombre = ISNULL(@i_carrera, c.Nombre)
		AND m.Nombre = ISNULL(@i_modulo, m.Nombre)

		SELECT Id=e.Id, Nombres, Apellidos, Matricula, 
		Email, Direccion, Carrera=c.Nombre, Modulo=m.Nombre
		FROM Estudiantes e
		INNER JOIN Modulos m	ON	m.Id=e.ModuloId
		INNER JOIN Carreras c	ON	c.Id=e.CarreraId
		WHERE e.Id=@i_id
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_registroasistencia]    Script Date: 30/11/2022 11:12:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_registroasistencia]
	@i_accion				CHAR(2)				=null,
	@i_fecha				DATETIME			=null,
	@i_nombres				VARCHAR(100)		=null,
	@i_apellidos			VARCHAR(100)		=null,
	@i_matricula			VARCHAR(10)			=null,
	@i_carrera				VARCHAR(100)		=null,
	@i_modulo				VARCHAR(50)			=null
AS 
BEGIN
	DECLARE
		@estudianteId	INT

	IF(@i_accion='IA')
	BEGIN		
		IF NOT EXISTS(SELECT 1 FROM Estudiantes WHERE Matricula=@i_matricula)
		BEGIN
			SELECT ERROR='No se encontró ningún estudiante con la matrícula ingresada'
			RETURN 0;
		END
		SELECT @estudianteId=Id FROM Estudiantes WHERE Matricula=@i_matricula

		INSERT INTO RegistroAsistencia (Fecha, EstudianteId, EstadoAsistenciaId)
		SELECT @i_fecha, @estudianteId, 1
	END
	IF(@i_accion='IF')
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM Estudiantes WHERE Matricula=@i_matricula)
		BEGIN
			SELECT ERROR='No se encontró ningún estudiante con la matrícula ingresada'
			RETURN 0;
		END
		SELECT @estudianteId=Id FROM Estudiantes WHERE Matricula=@i_matricula

		INSERT INTO RegistroAsistencia (Fecha, EstudianteId, EstadoAsistenciaId)
		SELECT @i_fecha, @estudianteId, 2
	END
	IF(@i_accion='UA')
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM Estudiantes WHERE Matricula=@i_matricula)
		BEGIN
			SELECT ERROR='No se encontró ningún estudiante con la matrícula ingresada'
			RETURN 0;
		END
		SELECT @estudianteId=Id FROM Estudiantes WHERE Matricula=@i_matricula

		UPDATE RegistroAsistencia SET EstadoAsistenciaId=1
		WHERE EstudianteId=@estudianteId
	END
	IF(@i_accion='UF')
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM Estudiantes WHERE Matricula=@i_matricula)
		BEGIN
			SELECT ERROR='No se encontró ningún estudiante con la matrícula ingresada'
			RETURN 0;
		END
		SELECT @estudianteId=Id FROM Estudiantes WHERE Matricula=@i_matricula

		UPDATE RegistroAsistencia SET EstadoAsistenciaId=2
		WHERE EstudianteId=@estudianteId
	END
	IF(@i_accion='CC')
	BEGIN
		SELECT Id=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, Matricula=e.Matricula, Email=e.Email,
		Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
		INNER JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE c.Nombre LIKE '%' + @i_carrera + '%'
		AND m.Nombre=@i_modulo
	END
	IF(@i_accion='CL')
	BEGIN
		SELECT Id=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, Matricula=e.Matricula, Email=e.Email,
		Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
		INNER JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE m.Nombre=@i_modulo
	END
	IF(@i_accion='CM')
	BEGIN
		SELECT Id=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, Matricula=e.Matricula, Email=e.Email,
		Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
		INNER JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE e.Matricula=@i_matricula
		AND m.Nombre=@i_modulo
	END
	IF(@i_accion='CN')
	BEGIN
		SELECT Id=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, Matricula=e.Matricula, Email=e.Email,
		Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
		INNER JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
		WHERE (e.Nombres LIKE '%' + @i_nombres + '%'
		OR e.Apellidos LIKE '%' + @i_apellidos + '%')
		AND m.Nombre=@i_modulo
	END
	IF(@i_accion='CT')
	BEGIN
		SELECT Id=e.Id, Nombres=e.Nombres, Apellidos=e.Apellidos, Matricula=e.Matricula, Email=e.Email,
		Fecha=r.Fecha, EstadoAsistencia=ea.Estado, Carrera=c.Nombre, Modulo=m.Nombre
		FROM RegistroAsistencia r
		INNER JOIN Estudiantes e		ON e.Id=r.EstudianteId
		INNER JOIN EstadoAsistencia ea	ON ea.Id=r.EstadoAsistenciaId
		INNER JOIN Carreras c			ON c.Id=e.CarreraId
		INNER JOIN Modulos m			ON m.Id=e.ModuloId
	END
END
GO
USE [master]
GO
ALTER DATABASE [IDAAI] SET  READ_WRITE 
GO
