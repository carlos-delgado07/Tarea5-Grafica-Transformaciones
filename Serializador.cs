using System;
using System.IO;
using Newtonsoft.Json;

namespace programGraph
{
    public static class Serializador
    {
        private static readonly string carpeta = "escenarios";

        static Serializador()
        {
            // Asegura que la carpeta exista
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);
        }

        public static void GuardarEscenario(Escenario escenario, string nombreArchivo)
        {
            string ruta = Path.Combine(carpeta, nombreArchivo);
            var json = JsonConvert.SerializeObject(escenario, Formatting.Indented);
            File.WriteAllText(ruta, json);
            Console.WriteLine($"✅ Escenario guardado en {ruta}");
        }

        public static Escenario CargarEscenario(string nombreArchivo)
        {
            string ruta = Path.Combine(carpeta, nombreArchivo);

            if (!File.Exists(ruta))
            {
                Console.WriteLine("⚠️ Archivo no encontrado.");
                return null;
            }

            var json = File.ReadAllText(ruta);
            var escenario = JsonConvert.DeserializeObject<Escenario>(json);
            Console.WriteLine($"📂 Escenario cargado desde {ruta}");
            return escenario;
        }
    }
}
