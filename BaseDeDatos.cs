using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace MotorRecomendacion
{
    public static class BaseDeDatos
    {
        private static readonly string rutaArchivo = "bd_recomienda.json";

        // Carga los registros desde el disco duro hacia nuestra Lista Doblemente Enlazada
        public static ListaDoble InicializarYCargarLista()
        {
            ListaDoble lista = new ListaDoble();

            if (!File.Exists(rutaArchivo))
            {
                // Semilla inicial basada exactamente en los Mockups del documento Fase 1
                var datosIniciales = new List<Recomendacion>
                {
                    new Recomendacion(1, "Interstellar", "Películas", "Ciencia Ficción", "Christopher Nolan", 2014,
                        "Un grupo de exploradores emprende la misión más importante de la historia de la humanidad: viajar más allá de nuestra galaxia a través de un agujero de gusano para asegurar el futuro de la especie humana.", 94),
                    new Recomendacion(2, "Inception", "Películas", "Ciencia Ficción", "Christopher Nolan", 2010,
                        "Dom Cobb es un ladrón capaz de adentrarse en los sueños de la gente y hacerse con sus secretos más preciados del subconsciente.", 91),
                    new Recomendacion(3, "The Matrix", "Películas", "Ciencia Ficción", "Wachowskis", 1999,
                        "Un experto en computadoras descubre que su mundo es una simulación total creada con maliciosas intenciones por parte de la ciberinteligencia.", 88),
                    new Recomendacion(4, "Dune: Part Two", "Películas", "Ciencia Ficción", "Denis Villeneuve", 2024,
                        "Paul Atreides se une a la tribu de los Fremen y comienza un viaje espiritual y marcial para convertirse en mesías.", 86),
                    new Recomendacion(5, "Arrival", "Películas", "Ciencia Ficción", "Denis Villeneuve", 2016,
                        "Doce naves extraterrestres comienzan a llegar a nuestro planeta. Los altos mandos militares piden ayuda a una experta lingüista.", 83),
                    new Recomendacion(6, "Blade Runner 2049", "Películas", "Ciencia Ficción", "Denis Villeneuve", 2017,
                        "Un nuevo blade runner, el oficial K del Departamento de Policía de Los Ángeles, desentierra un antiguo secreto.", 89)
                };

                foreach (var item in datosIniciales)
                {
                    lista.Añadir(item); // Inserta creando Nodos con punteros Ant y Sig
                }

                GuardarDesdeLista(lista);
            }
            else
            {
                string json = File.ReadAllText(rutaArchivo);
                var registros = JsonSerializer.Deserialize<List<Recomendacion>>(json);
                if (registros != null)
                {
                    foreach (var item in registros)
                    {
                        lista.Añadir(item);
                    }
                }
            }

            lista.Inicio(); // Sitúa el puntero en primerNodo (Cabeza)
            return lista;
        }

        // Recorre la ListaDoble nodo por nodo con punteroAuxiliar y guarda en disco
        public static void GuardarDesdeLista(ListaDoble lista)
        {
            var registros = new List<Recomendacion>();
            int total = lista.Longitud();

            for (int i = 1; i <= total; i++)
            {
                Recomendacion dato = lista.Obtener(i);
                if (dato != null)
                {
                    registros.Add(dato);
                }
            }

            var opciones = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(rutaArchivo, JsonSerializer.Serialize(registros, opciones));
        }
    }
}