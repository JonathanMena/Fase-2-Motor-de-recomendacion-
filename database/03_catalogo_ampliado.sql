USE MotorRecomendacionDB
GO


-- ==========================================
-- PELÍCULAS
-- ==========================================

IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'John Wick'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'John Wick',
        'Películas',
        'Acción',
        'Chad Stahelski',
        2014,
        'Un antiguo asesino regresa al mundo criminal después de una pérdida personal.',
        92
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'The Hangover'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'The Hangover',
        'Películas',
        'Comedia',
        'Todd Phillips',
        2009,
        'Un grupo de amigos intenta reconstruir lo ocurrido durante una noche en Las Vegas.',
        86
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'The Conjuring'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'The Conjuring',
        'Películas',
        'Terror',
        'James Wan',
        2013,
        'Investigadores paranormales ayudan a una familia que enfrenta una presencia inquietante.',
        89
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'The Lord of the Rings'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'The Lord of the Rings',
        'Películas',
        'Fantasía',
        'Peter Jackson',
        2001,
        'Un grupo inicia un viaje para destruir un poderoso anillo.',
        96
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'Titanic'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'Titanic',
        'Películas',
        'Romance',
        'James Cameron',
        1997,
        'Dos jóvenes de diferentes clases sociales se conocen durante un viaje transatlántico.',
        91
    )
END
GO


-- ==========================================
-- MÚSICA
-- ==========================================

IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'Blinding Lights'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'Blinding Lights',
        'Música',
        'Pop',
        'The Weeknd',
        2019,
        'Canción de pop contemporáneo con influencias synthwave.',
        94
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'Bohemian Rhapsody'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'Bohemian Rhapsody',
        'Música',
        'Rock',
        'Queen',
        1975,
        'Composición de rock caracterizada por múltiples secciones musicales.',
        97
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'One More Time'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'One More Time',
        'Música',
        'Electrónica',
        'Daft Punk',
        2000,
        'Tema de música electrónica con elementos house.',
        90
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'Lose Yourself'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'Lose Yourself',
        'Música',
        'Hip-Hop',
        'Eminem',
        2002,
        'Tema de hip-hop centrado en aprovechar oportunidades decisivas.',
        93
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'So What'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'So What',
        'Música',
        'Jazz',
        'Miles Davis',
        1959,
        'Composición instrumental representativa del jazz modal.',
        92
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'Time'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'Time',
        'Música',
        'Soundtrack',
        'Hans Zimmer',
        2010,
        'Composición instrumental creada para una banda sonora cinematográfica.',
        96
    )
END
GO


-- ==========================================
-- LIBROS
-- ==========================================

IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'Dune'
    AND TipoContenido = 'Libros'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'Dune',
        'Libros',
        'Ciencia Ficción',
        'Frank Herbert',
        1965,
        'Novela de ciencia ficción ambientada en el planeta Arrakis.',
        96
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'The Hobbit'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'The Hobbit',
        'Libros',
        'Fantasía',
        'J. R. R. Tolkien',
        1937,
        'Un hobbit emprende un inesperado viaje acompañado por un grupo de enanos.',
        95
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'Dracula'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'Dracula',
        'Libros',
        'Terror',
        'Bram Stoker',
        1897,
        'Novela gótica centrada en la figura del conde Drácula.',
        90
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'Pride and Prejudice'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'Pride and Prejudice',
        'Libros',
        'Romance',
        'Jane Austen',
        1813,
        'Novela sobre relaciones, expectativas sociales y crecimiento personal.',
        92
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'Treasure Island'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'Treasure Island',
        'Libros',
        'Aventura',
        'Robert Louis Stevenson',
        1883,
        'Una aventura marítima en busca de un tesoro escondido.',
        88
    )
END
GO


IF NOT EXISTS
(
    SELECT 1
    FROM Contenidos
    WHERE Titulo = 'Atomic Habits'
)
BEGIN
    INSERT INTO Contenidos
    (
        Titulo,
        TipoContenido,
        Categoria,
        AutorDirector,
        Anio,
        Sinopsis,
        PuntajeBase
    )
    VALUES
    (
        'Atomic Habits',
        'Libros',
        'No Ficción',
        'James Clear',
        2018,
        'Libro sobre la formación y mejora gradual de hábitos.',
        91
    )
END
GO