using System;

namespace MotorRecomendacion
{
    public class ListaDoble
    {
        // Punteros principales protegidos (Cabeza y Cola)
        private Nodo primerNodo;
        private Nodo ultimoNodo;

        // Puntero auxiliar para controlar la posición actual sin mover la cabeza/cola
        private Nodo punteroActual;
        private int posicionActual;
        private int totalNodos;

        public ListaDoble()
        {
            primerNodo = null;
            ultimoNodo = null;
            punteroActual = null;
            posicionActual = 0;
            totalNodos = 0;
        }

        public bool EstaVacia()
        {
            return primerNodo == null;
        }

        // 1. Inicio(): Sitúa la posición actual directamente en la cabeza (primerNodo)
        public void Inicio()
        {
            punteroActual = primerNodo;
            posicionActual = (primerNodo != null) ? 1 : 0;
        }

        // 2. Actual(): Devuelve la posición actual dentro de la lista
        public int Actual()
        {
            return posicionActual;
        }

        // Devuelve el nodo en el puntero actual para visualización en interfaz
        public Nodo ObtenerNodoActual()
        {
            return punteroActual;
        }

        // 3. Obtener(posición): Devuelve el objeto Recomendacion en cierta posición (1 a N) o NULL
        public Recomendacion Obtener(int posicion)
        {
            if (posicion < 1 || posicion > totalNodos || EstaVacia())
                return null;

            Nodo punteroAuxiliar = primerNodo;
            int contador = 1;

            while (punteroAuxiliar != null && contador < posicion)
            {
                punteroAuxiliar = punteroAuxiliar.Siguiente;
                contador++;
            }

            return (punteroAuxiliar != null) ? punteroAuxiliar.Dato : null;
        }

        // 4. Añadir(elemento): Añade una recomendación estándar al final (en ultimoNodo / Cola)
        public bool Añadir(Recomendacion elemento)
        {
            if (elemento == null) return false;

            Nodo nuevoNodo = new Nodo(elemento);

            if (EstaVacia())
            {
                primerNodo = nuevoNodo;
                ultimoNodo = nuevoNodo;
                punteroActual = primerNodo;
                posicionActual = 1;
            }
            else
            {
                ultimoNodo.Siguiente = nuevoNodo;
                nuevoNodo.Anterior = ultimoNodo;
                ultimoNodo = nuevoNodo;
            }

            totalNodos++;
            return true;
        }

        // 5. Insertar(elemento, posición): Añade una recomendación prioritaria en cualquier posición
        public bool Insertar(Recomendacion elemento, int posicion)
        {
            if (elemento == null || posicion < 1 || posicion > totalNodos + 1)
                return false;

            // Si la lista está vacía o se inserta al final, usamos Añadir
            if (EstaVacia() || posicion == totalNodos + 1)
            {
                return Añadir(elemento);
            }

            Nodo nuevoNodo = new Nodo(elemento);

            // Insertar al inicio (en la Cabeza / posición 1)
            if (posicion == 1)
            {
                nuevoNodo.Siguiente = primerNodo;
                primerNodo.Anterior = nuevoNodo;
                primerNodo = nuevoNodo;
                totalNodos++;
                if (posicionActual >= posicion) posicionActual++;
                return true;
            }

            // Insertar en posición intermedia usando punteroAuxiliar
            Nodo punteroAuxiliar = primerNodo;
            for (int i = 1; i < posicion; i++)
            {
                punteroAuxiliar = punteroAuxiliar.Siguiente;
            }

            Nodo nodoPrevio = punteroAuxiliar.Anterior;

            nuevoNodo.Siguiente = punteroAuxiliar;
            nuevoNodo.Anterior = nodoPrevio;
            nodoPrevio.Siguiente = nuevoNodo;
            punteroAuxiliar.Anterior = nuevoNodo;

            totalNodos++;
            if (posicionActual >= posicion) posicionActual++;
            return true;
        }

        // 6. Eliminar(posición): Elimina un contenido almacenado en cierta posición
        public bool Eliminar(int posicion)
        {
            if (EstaVacia() || posicion < 1 || posicion > totalNodos)
                return false;

            Nodo nodoAEliminar;

            // Caso 1: Único nodo en la lista
            if (totalNodos == 1)
            {
                primerNodo = null;
                ultimoNodo = null;
                punteroActual = null;
                posicionActual = 0;
                totalNodos = 0;
                return true;
            }

            // Caso 2: Eliminar la Cabeza (posición 1)
            if (posicion == 1)
            {
                nodoAEliminar = primerNodo;
                primerNodo = primerNodo.Siguiente;
                primerNodo.Anterior = null;

                if (punteroActual == nodoAEliminar)
                    punteroActual = primerNodo;
                else
                    posicionActual--;

                totalNodos--;
                return true;
            }

            // Caso 3: Eliminar la Cola (última posición)
            if (posicion == totalNodos)
            {
                nodoAEliminar = ultimoNodo;
                ultimoNodo = ultimoNodo.Anterior;
                ultimoNodo.Siguiente = null;

                if (punteroActual == nodoAEliminar)
                {
                    punteroActual = ultimoNodo;
                    posicionActual = totalNodos - 1;
                }

                totalNodos--;
                return true;
            }

            // Caso 4: Eliminar en posición intermedia con punteroAuxiliar
            Nodo punteroAuxiliar = primerNodo;
            for (int i = 1; i < posicion; i++)
            {
                punteroAuxiliar = punteroAuxiliar.Siguiente;
            }

            Nodo nodoAnt = punteroAuxiliar.Anterior;
            Nodo nodoSig = punteroAuxiliar.Siguiente;

            nodoAnt.Siguiente = nodoSig;
            nodoSig.Anterior = nodoAnt;

            if (punteroActual == punteroAuxiliar)
                punteroActual = nodoSig;
            else if (posicion < posicionActual)
                posicionActual--;

            totalNodos--;
            return true;
        }

        // 7. Siguiente(): Incrementa la posición actual y devuelve el valor para avanzar nodo por nodo
        public Recomendacion Siguiente()
        {
            if (punteroActual != null && punteroActual.Siguiente != null)
            {
                punteroActual = punteroActual.Siguiente;
                posicionActual++;
                return punteroActual.Dato;
            }
            return null; // Llegó a NULL (final de la lista)
        }

        // Método complementario Anterior() para el botón "<- Anterior" del Mockup
        public Recomendacion Anterior()
        {
            if (punteroActual != null && punteroActual.Anterior != null)
            {
                punteroActual = punteroActual.Anterior;
                posicionActual--;
                return punteroActual.Dato;
            }
            return null; // Llegó a NULL (inicio de la lista)
        }

        // 8. Longitud(): Devuelve la cantidad total de nodos en la lista
        public int Longitud()
        {
            return totalNodos;
        }

        // Muestra gráficamente la traza de punteros como en los Mockups (Null <-> [Nodo] <-> Null)
        public void MostrarTrazaEnMemoria()
        {
            Console.WriteLine("\n--- TRAZA DE LA LISTA DOBLEMENTE ENLAZADA EN MEMORIA ---");
            if (EstaVacia())
            {
                Console.WriteLine("Cabeza -> NULL | Cola -> NULL (Lista Vacía)");
                return;
            }

            Console.Write("NULL <-- ");
            Nodo punteroAuxiliar = primerNodo;
            while (punteroAuxiliar != null)
            {
                if (punteroAuxiliar == punteroActual)
                    Console.ForegroundColor = ConsoleColor.Cyan; // Resalta el nodo "Actual"

                Console.Write($"[{punteroAuxiliar.Dato.Titulo} ({punteroAuxiliar.Dato.PuntajeSimilitud}%)]");
                Console.ResetColor();

                if (punteroAuxiliar.Siguiente != null)
                    Console.Write(" <==> ");

                punteroAuxiliar = punteroAuxiliar.Siguiente;
            }
            Console.WriteLine(" --> NULL");
            Console.WriteLine($"Cabeza (primerNodo): {primerNodo.Dato.Titulo} | Cola (ultimoNodo): {ultimoNodo.Dato.Titulo} | Longitud: {Longitud()}\n");
        }
    }
}