# Casos de prueba: Lista doblemente enlazada

Proyecto: Motor de recomendación de contenido (Fase 2)

## Cómo se ejecutaron las pruebas

Las pruebas son manuales, sobre la interfaz web en http://localhost:5150. Para volver a los datos originales entre casos:

1. Detener el servidor con Ctrl + C.
2. Ejecutar `git restore bd_recomienda.json; dotnet run`.
3. Recargar la página.

Los valores se leen en el panel "Mecánica de Traversal" (Actual y Longitud), en las tarjetas de nodo anterior, actual y siguiente, y en la vista Historial.

Notación: "Siguiente x3" significa presionar el botón Siguiente tres veces. "Historial > Eliminar(n)" es el botón de eliminar del nodo n en la vista Historial. Las capturas se guardan en docs/capturas/ con el nombre del caso

Datos iniciales (7 nodos, Actual = 1):

| Posición | Título |
|---|---|
| 1 | Nuevo Contenido |
| 2 | Interstellar |
| 3 | Inception |
| 4 | The Matrix |
| 5 | Dune: Part Two |
| 6 | Arrival |
| 7 | Blade Runner 2049 |

## Bloque 1: Recorrido (no modifica datos)

| ID | Caso | Pasos | Resultado esperado | Resultado obtenido | Estado |
|---|---|---|---|---|---|
| CP-01 | Estado inicial | Reiniciar y cargar la página | Longitud = 7, Actual = 1, anterior = NULL, nodo actual = Nuevo Contenido, siguiente = Interstellar | Longitud = 7, Actual= 1, anterior = NULL, Actual = Nuevo contenido; siguiente = Interestellar | Pasó |
| CP-02 | Siguiente y Anterior | Desde Actual = 1: Siguiente; luego Anterior | Tras Siguiente: Actual = 2 (Interstellar), anterior = Nuevo Contenido, siguiente = Inception. Tras Anterior: Actual = 1 |Actual= 2 (interestellar), anterior = Nuevo Contenido, siguiente = inception, tras anterior: Actual = 1 | Pasó |
| CP-03 | Anterior en la cabeza | Con Actual = 1, presionar Anterior | No cambia nada: Actual = 1, anterior = NULL | Anterior, no cambia nada, Actual = 1, anterior = NULL, siguiente = Interestellar | Pasó |
| CP-04 | Siguiente en la cola | Siguiente x6 hasta Actual = 7; presionar Siguiente una vez más | Actual = 7 (Blade Runner 2049), siguiente = NULL | Siguiente 6 veces, actual = 7, siguiente = Null, anterior = Arrival | Pasó |
| CP-05 | Inicio | Desde Actual = 7, presionar "Ir a Cabeza" | Actual = 1 (Nuevo Contenido) | Desde actual = 7, "Ir a cabeza", Actual = Nuevo contenido | Pasó |

## Bloque 2: Inserción (reiniciar antes de cada caso)

Formulario "Insertar Recomendación por Posición": título, director, similitud (95) y posición.

| ID | Caso | Pasos | Resultado esperado | Resultado obtenido | Estado |
|---|---|---|---|---|---|
| CP-06 | Insertar al inicio con el puntero en la cabeza | Con Actual = 1: título "Prueba A", posición 1 | Longitud = 8. Prueba A en #1 y Nuevo Contenido en #2. El nodo actual sigue siendo Nuevo Contenido, por lo que Actual = 2 y su anterior = Prueba A | Longitud = 8, Prueba A en #1, Nuevo contenido en #2, Actual = 1 (esperaba 2), anterior = Prueba A, Sig = Interestellar | Falló |
| CP-07 | Insertar en medio, después del puntero | Con Actual = 1: "Prueba B", posición 4 | Longitud = 8. Prueba B en #4 y The Matrix en #5. Actual = 1 sin cambios | Longitud = 8; Prueba B en #4; The Matrix en #5; Actual = 1; Puntero Actual en #1 (Nuevo Contenido). Caso repetido: la primera ejecución no partió de Actual = 1.| Pasó |
| CP-08 | Insertar en medio, antes del puntero | Siguiente x5 (Actual = 6, Arrival); "Prueba C", posición 3 | Longitud = 8. Prueba C en #3. Arrival pasa a #7 y sigue siendo el nodo actual, por lo que Actual = 7 | Longitud = 8, Prueba C en #3, Arrival en #7, Puntero actual = 6 (esperaba 7) | Falló |
| CP-09 | Insertar al final | "Prueba D", posición 8 | Longitud = 8. Prueba D en #8 (nueva cola) y Blade Runner 2049 en #7. Actual = 1 | Longitud =8, Prueba D en #8 Blade Runner 2049 #7, Puntero Actual = 1 | Pasó |
| CP-10 | Insertar con posición igual a la longitud | "Prueba E", posición 7 | Longitud = 8. Prueba E en #7 y Blade Runner 2049 pasa a #8 (se inserta antes del último, no al final) | Longitud = 8, Prueba E en #7, Blade Runner 2049 en $8 | Pasó |

## Bloque 3: Eliminación (reiniciar antes de cada caso, salvo CP-17)

| ID | Caso | Pasos | Resultado esperado | Resultado obtenido | Estado |
|---|---|---|---|---|---|
| CP-11 | Eliminar la cabeza siendo la actual | Con Actual = 1: Descartar | Longitud = 6. Actual = 1, nodo actual = Interestellar, anterior = NULL | Longitud = 6, Puntero actual = 1, Nodo actual = Interestellar, Anterior = NULL | Pasó |
| CP-12 | Eliminar la cabeza con el puntero más adelante | Siguiente x2 (Actual = 3, Inception); Historial > Eliminar(1) | Longitud = 6. Inception pasa a #2 y sigue siendo el nodo actual, por lo que Actual = 2 | Longitud =6, Inception en #2, Actual = 3 (esperaba 2) Puntero Actual aparece en #3 (The Matrix) | Falló |
| CP-13 | Eliminar la cola siendo la actual | Siguiente x6 (Actual = 7); Descartar | Longitud = 6. Actual = 6, nodo actual = Arrival, siguiente = NULL | Descarté Blade Runner 2049, Longitud = 6, Actual = 6, Nodo Actual = Arrival, Sig = NULL | Pasó |
| CP-14 | Eliminar un nodo intermedio siendo el actual | Siguiente x3 (Actual = 4, The Matrix); Descartar | Longitud = 6. Actual = 4, nodo actual = Dune: Part Two, anterior = Inception, siguiente = Arrival | Descarté The Matrix, Longitud = 6, Actual = 4, Nodo Actual = Dune Part Two, Ant. = Inception, Sig. = Arrival | Pasó |
| CP-15 | Eliminar un nodo intermedio antes del puntero | Siguiente x4 (Actual = 5, Dune: Part Two); Historial > Eliminar(2) | Longitud = 6. Dune: Part Two pasa a #4 y sigue siendo el nodo actual, por lo que Actual = 4 | Eliminé Interestellar, Longitud = 6, Dune Part Two es #4, Nodo actual: Dune Part Two, Puntero Actual es #5 (esperaba 4)| Falló |
| CP-16 | Eliminar hasta dejar la lista vacía | Con Actual = 1: Descartar x6, revisar; Descartar una vez más | Tras 6: Longitud = 1 (Blade Runner 2049), Actual = 1, anterior y siguiente = NULL. Tras la 7.ª: Longitud = 0, Actual = 0, pantalla "Lista Vacía" | Tras 6 clicks en descartar: Longitud = 1, Actual = 1, Nodo Actual = Blade Runner 2049, Anterior y Siguiente = NULL; tras el 7mo click en descartar: Longitud = 0, Posición = 0, Nodo Actual: Lista Vacía (NULL) | Pasó |
| CP-17 | Insertar en lista vacía | Continuar de CP-16: "Prueba F", posición 1 | Longitud = 1, Actual = 1, nodo actual = Prueba F, anterior y siguiente = NULL | Al insertar "Prueba F", Actual = Prueba F, Posición = 1, Longitud = 1, Anterior y siguiente = NULL| Pasó |

## Bloque 4: Persistencia

| ID | Caso | Pasos | Resultado esperado | Resultado obtenido | Estado |
|---|---|---|---|---|---|
| CP-18 | Persistencia del estado | Siguiente x3 (The Matrix); presionar "Me gusta"; Ctrl + C; `dotnet run` sin restaurar el JSON; abrir Historial | The Matrix conserva el estado "Me gusta" tras reiniciar y bd_recomienda.json lo refleja | Al reiniciar sin restaurar el JSON, The Matrix muestra el estado: Me gusta, bd_recomienda.json lo reflejaba antes de reiniciar: Me gusta | Pasó |

## Resumen de resultados

| Total de casos | Pasaron | Fallaron |
|---|---|---|
| 18 | 14 | 4 |

## Defectos encontrados

(Por cada caso que falló: qué ocurrió, qué se esperaba y la causa probable. Pasan a la bitácora de dificultades.)