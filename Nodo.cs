using System;

namespace MotorRecomendacion
{
    public class Nodo
    {
        // 3 componentes fundamentales del nodo doblemente enlazado
        private Recomendacion dato;
        private Nodo anterior;
        private Nodo siguiente;

        public Recomendacion Dato
        {
            get { return dato; }
            set { dato = value; }
        }

        public Nodo Anterior
        {
            get { return anterior; }
            set { anterior = value; }
        }

        public Nodo Siguiente
        {
            get { return siguiente; }
            set { siguiente = value; }
        }

        // Constructor con un parámetro (punteros inician en NULL)
        public Nodo(Recomendacion valorDato)
        {
            dato = valorDato;
            anterior = null;
            siguiente = null;
        }

        // Constructor completo
        public Nodo(Recomendacion valorDato, Nodo nodoAnterior, Nodo nodoSiguiente)
        {
            dato = valorDato;
            anterior = nodoAnterior;
            siguiente = nodoSiguiente;
        }
    }
}