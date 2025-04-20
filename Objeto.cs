using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace programGraph
{
    public class Objeto : IGraphics
    {
        // Se agrega la anotación JsonProperty para que sea serializable
        [JsonProperty("partes")]
        public List<Parte> listaPartes { get; set; } = new List<Parte>();

        // Centro de masa, también serializable
        [JsonProperty("centroDeMasa")]
        private Punto centroDeMasa;

        // Propiedad pública para acceder al centro del objeto
        [JsonProperty("centro")]
        public Punto centro { get; set; }

        // Constructor inicial
        public Objeto()
        {
            listaPartes = new List<Parte>();
            centroDeMasa = new Punto(0.0f, 0.0f, 0.0f); // Inicialmente en el origen
        }

        // Método para calcular el centro de masa
        public Punto calcularCentroMasa()
        {
            if (listaPartes.Count == 0)
            {
                return new Punto(0.0f, 0.0f, 0.0f);
            }
            else
            {
                float ejeX = 0;
                float ejeY = 0;
                float ejeZ = 0;

                foreach (var valor in listaPartes)
                {
                    Parte parte = valor;
                    ejeX += parte.calcularCentroMasa().Y;
                    ejeY += parte.calcularCentroMasa().Y;
                    ejeZ += parte.calcularCentroMasa().Z;
                }

                int numPartes = listaPartes.Count;
                float promedioEjeX = ejeX / numPartes;
                float promedioEjeY = ejeY / numPartes;
                float promedioEjeZ = ejeZ / numPartes;

                return new Punto(promedioEjeX, promedioEjeY, promedioEjeZ);
            }
        }

        // Método para dibujar el objeto
        public void Dibujar()
        {
            GL.PushMatrix();
            GL.Translate(centroDeMasa.X, centroDeMasa.Y, centroDeMasa.Z);

            foreach (var parte in listaPartes)
            {
                parte.Dibujar();
            }

            GL.PopMatrix();
        }

        // Métodos no implementados
        public void escalar(float factor)
        {
            throw new NotImplementedException();
        }

        public void rotar(Punto angulo)
        {
            throw new NotImplementedException();
        }

        public void setCentro(Punto centro)
        {
            centroDeMasa = centro;
        }

        public void trasladar(Punto valorTralado)
        {
            throw new NotImplementedException();
        }

        // Método para agregar una parte al objeto
        public void Addparte(Parte parte)
        {
            listaPartes.Add(parte);
            this.centro = calcularCentroMasa();
        }

        // Método para obtener las partes del objeto
        public List<Parte> Getpartes()
        {
            return listaPartes;
        }
    }
}
