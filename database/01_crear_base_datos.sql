CREATE DATABASE MotorRecomendacionDB
GO

USE MotorRecomendacionDB
GO


CREATE TABLE Usuarios
(
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE()
)
GO


CREATE TABLE Contenidos
(
    IdContenido INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    TipoContenido NVARCHAR(50) NOT NULL,
    Categoria NVARCHAR(100) NOT NULL,
    AutorDirector NVARCHAR(150) NOT NULL,
    Anio INT NOT NULL,
    Sinopsis NVARCHAR(MAX) NULL,
    PuntajeBase INT NOT NULL DEFAULT 0,
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT CK_Contenidos_PuntajeBase
        CHECK (PuntajeBase BETWEEN 0 AND 100)
)
GO


CREATE TABLE PreferenciasUsuario
(
    IdPreferencia INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    TipoContenido NVARCHAR(50) NOT NULL,
    Categoria NVARCHAR(100) NOT NULL,
    Peso INT NOT NULL DEFAULT 100,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_PreferenciasUsuario_Usuarios
        FOREIGN KEY (IdUsuario)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_PreferenciasUsuario_Peso
        CHECK (Peso BETWEEN 0 AND 100),

    CONSTRAINT UQ_PreferenciasUsuario
        UNIQUE (IdUsuario, TipoContenido, Categoria)
)
GO


CREATE TABLE Interacciones
(
    IdInteraccion INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    IdContenido INT NOT NULL,
    TipoInteraccion NVARCHAR(30) NOT NULL,
    Fecha DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Interacciones_Usuarios
        FOREIGN KEY (IdUsuario)
        REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_Interacciones_Contenidos
        FOREIGN KEY (IdContenido)
        REFERENCES Contenidos(IdContenido),

    CONSTRAINT CK_Interacciones_Tipo
        CHECK (
            TipoInteraccion IN (
                'MeGusta',
                'Guardado',
                'Descartado'
            )
        )
)
GO


CREATE INDEX IX_Interacciones_Usuario
ON Interacciones(IdUsuario)
GO

CREATE INDEX IX_Interacciones_Contenido
ON Interacciones(IdContenido)
GO

CREATE INDEX IX_Contenidos_TipoCategoria
ON Contenidos(TipoContenido, Categoria)
GO