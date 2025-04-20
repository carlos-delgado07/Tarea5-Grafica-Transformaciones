using Newtonsoft.Json;
using System.Collections.Generic;

namespace programGraph
{
    public class Escenario
    {
        // La propiedad listaDeObjetos será serializada en el archivo JSON
        [JsonProperty("objetos")]
        public Dictionary<string, Objeto> listaDeObjetos { get; set; } = new Dictionary<string, Objeto>();

        // Constructor por defecto
        public Escenario() 
        {
            listaDeObjetos = new Dictionary<string, Objeto>();
        }

        // Método para agregar un objeto al escenario
        public void AddObjeto(string name, Objeto objeto)
        {
            listaDeObjetos.Add(name, objeto);
        }

        // Método para dibujar el escenario (se ejecuta cuando se renderiza)
        public void DibujarEscenario()
        {
            foreach (var objeto in listaDeObjetos.Values)
            {
                objeto.Dibujar();
            }
        }

        // Devuelve la lista de objetos (para referencia externa si es necesario)
        public Dictionary<string, Objeto> GetObjetos()
        {
            return listaDeObjetos;
        }
    }
}