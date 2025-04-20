using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL;

namespace programGraph
{
    public class Parte : IGraphics
    {
        [JsonProperty("poligonos")]
        public List<Poligono> listaPoligonos { get; set; } = new List<Poligono>();

        [JsonProperty("centroMasa")]
        private Punto centroMasa;

        [JsonProperty("centro")]
        public Punto centro { get; set; }

        public Parte()
        {
            listaPoligonos = new List<Poligono>();
            centroMasa = new Punto(0.0f, 0.0f, 0.0f);
        }

        public Punto calcularCentroMasa()
        {
            if (listaPoligonos.Count == 0)
                return new Punto(0.0f, 0.0f, 0.0f);

            float ejeX = 0.0f, ejeY = 0.0f, ejeZ = 0.0f;

            foreach (var poligono in listaPoligonos)
            {
                Punto centroPoligono = poligono.calcularCentroMasa();
                ejeX += centroPoligono.X;
                ejeY += centroPoligono.Y;
                ejeZ += centroPoligono.Z;
            }

            int numeroPoligonos = listaPoligonos.Count;
            return new Punto(ejeX / numeroPoligonos, ejeY / numeroPoligonos, ejeZ / numeroPoligonos);
        }

        public void Dibujar()
        {
            GL.PushMatrix();
            GL.Translate(centroMasa.X, centroMasa.Y, centroMasa.Z);
            foreach (var poligono in listaPoligonos)
            {
                poligono.Dibujar();
            }
            GL.PopMatrix();
        }

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
            centroMasa = centro;
        }

        public void trasladar(Punto valorTralado)
        {
            throw new NotImplementedException();
        }

        public List<Poligono> GetPoligonos()
        {
            return listaPoligonos;
        }

        public void AddPoligono(Poligono poligono)
        {
            listaPoligonos.Add(poligono);
            this.centro = this.calcularCentroMasa();
        }
    }
}
