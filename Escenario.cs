using Newtonsoft.Json;
using System.Collections.Generic;

namespace programGraph
{
    public class Escenario
    {
        [JsonProperty("objetos")]
        public Dictionary<string, Objeto> listaDeObjetos { get; set; } = new Dictionary<string, Objeto>();

        public Escenario()
        {
            listaDeObjetos = new Dictionary<string, Objeto>();
        }

        // Método para agregar un objeto al escenario
        public void AddObjeto(string name, Objeto objeto)
        {
            listaDeObjetos.Add(name, objeto);
        }

        // Método para dibujar todos los objetos en el escenario
        public void DibujarEscenario()
        {
            foreach (var objeto in listaDeObjetos.Values)
            {
                objeto.Dibujar();
            }
        }

        // Método para obtener todos los objetos en el escenario
        public Dictionary<string, Objeto> GetObjetos()
        {
            return listaDeObjetos;
        }

        // Transformación de todos los objetos
        public void trasladarTodo(Punto valorTraslado)
        {
            foreach (var objeto in listaDeObjetos.Values)
            {
                objeto.trasladar(valorTraslado);
            }
        }

        public void escalarTodo(float factor)
        {
            foreach (var objeto in listaDeObjetos.Values)
            {
                objeto.escalar(factor);
            }
        }

        public void rotarTodo(Punto angulo)
        {
            foreach (var objeto in listaDeObjetos.Values)
            {
                objeto.rotar(angulo);
            }
        }

        // Transformación de un objeto específico por nombre
        public void trasladarObjeto(string nombre, Punto valorTraslado)
        {
            if (listaDeObjetos.ContainsKey(nombre))
            {
                listaDeObjetos[nombre].trasladar(valorTraslado);
            }
        }

        public void escalarObjeto(string nombre, float factor)
        {
            if (listaDeObjetos.ContainsKey(nombre))
            {
                listaDeObjetos[nombre].escalar(factor);
            }
        }

        public void rotarObjeto(string nombre, Punto angulo)
        {
            if (listaDeObjetos.ContainsKey(nombre))
            {
                listaDeObjetos[nombre].rotar(angulo);
            }
        }
    }
}
