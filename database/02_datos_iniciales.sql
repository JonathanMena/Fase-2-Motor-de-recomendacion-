USE MotorRecomendacionDB
GO


INSERT INTO Usuarios
(
    Nombre,
    Email
)
VALUES
(
    'Alejandra Torres',
    'alejandra@recomienda.local'
)
GO


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
    'Interstellar',
    'Películas',
    'Ciencia Ficción',
    'Christopher Nolan',
    2014,
    'Un grupo de exploradores emprende la misión más importante de la historia de la humanidad: viajar más allá de nuestra galaxia a través de un agujero de gusano para asegurar el futuro de la especie humana.',
    94
),
(
    'Inception',
    'Películas',
    'Ciencia Ficción',
    'Christopher Nolan',
    2010,
    'Dom Cobb es un ladrón capaz de adentrarse en los sueños de la gente y hacerse con sus secretos más preciados del subconsciente.',
    91
),
(
    'The Matrix',
    'Películas',
    'Ciencia Ficción',
    'Wachowskis',
    1999,
    'Un experto en computadoras descubre que su mundo es una simulación creada por máquinas.',
    88
),
(
    'Dune: Part Two',
    'Películas',
    'Ciencia Ficción',
    'Denis Villeneuve',
    2024,
    'Paul Atreides se une a los Fremen mientras continúa su viaje por Arrakis.',
    86
),
(
    'Arrival',
    'Películas',
    'Ciencia Ficción',
    'Denis Villeneuve',
    2016,
    'Una lingüista intenta comunicarse con visitantes extraterrestres.',
    83
),
(
    'Blade Runner 2049',
    'Películas',
    'Ciencia Ficción',
    'Denis Villeneuve',
    2017,
    'Un blade runner descubre un secreto capaz de cambiar el futuro de la sociedad.',
    89
)
GO