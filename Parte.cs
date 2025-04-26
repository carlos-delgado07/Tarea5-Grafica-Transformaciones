using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenTK;
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
            Punto centro = this.calcularCentroMasa();

            foreach (var poligono in listaPoligonos)
            {
                foreach (var punto in poligono.puntos)
                {
                    punto.X = centro.X + (punto.X - centro.X) * factor;
                    punto.Y = centro.Y + (punto.Y - centro.Y) * factor;
                    punto.Z = centro.Z + (punto.Z - centro.Z) * factor;
                }
            }
        }

        public void rotar(Punto angulo)
        {
            Punto centro = this.calcularCentroMasa();

            float radX = MathHelper.DegreesToRadians(angulo.X);
            float radY = MathHelper.DegreesToRadians(angulo.Y);
            float radZ = MathHelper.DegreesToRadians(angulo.Z);

            foreach (var poligono in listaPoligonos)
            {
                foreach (var punto in poligono.puntos)
                {
                    // Trasladar al origen
                    float x = punto.X - centro.X;
                    float y = punto.Y - centro.Y;
                    float z = punto.Z - centro.Z;

                    // Rotar en X
                    float y1 = y * (float)Math.Cos(radX) - z * (float)Math.Sin(radX);
                    float z1 = y * (float)Math.Sin(radX) + z * (float)Math.Cos(radX);
                    y = y1; z = z1;

                    // Rotar en Y
                    float x1 = x * (float)Math.Cos(radY) + z * (float)Math.Sin(radY);
                    z1 = -x * (float)Math.Sin(radY) + z * (float)Math.Cos(radY);
                    x = x1; z = z1;

                    // Rotar en Z
                    x1 = x * (float)Math.Cos(radZ) - y * (float)Math.Sin(radZ);
                    y1 = x * (float)Math.Sin(radZ) + y * (float)Math.Cos(radZ);
                    x = x1; y = y1;

                    // Trasladar de regreso
                    punto.X = centro.X + x;
                    punto.Y = centro.Y + y;
                    punto.Z = centro.Z + z;
                }
            }
        }

        public void setCentro(Punto centro)
        {
            centroMasa = centro;
        }

        public void trasladar(Punto valorTraslado)
        {
            foreach (var poligono in listaPoligonos)
            {
                foreach (var punto in poligono.puntos)
                {
                    punto.X += valorTraslado.X;
                    punto.Y += valorTraslado.Y;
                    punto.Z += valorTraslado.Z;
                }
            }

            centroMasa.X += valorTraslado.X;
            centroMasa.Y += valorTraslado.Y;
            centroMasa.Z += valorTraslado.Z;
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
