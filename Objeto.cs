using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace programGraph
{
    public class Objeto : IGraphics
    {
        [JsonProperty("partes")]
        public List<Parte> listaPartes { get; set; } = new List<Parte>();

        [JsonProperty("centroDeMasa")]
        private Punto centroDeMasa;

        [JsonProperty("centro")]
        public Punto centro { get; set; }

        public Objeto()
        {
            listaPartes = new List<Parte>();
            centroDeMasa = new Punto(0.0f, 0.0f, 0.0f);
        }

        public Punto calcularCentroMasa()
        {
            if (listaPartes.Count == 0)
            {
                return new Punto(0.0f, 0.0f, 0.0f);
            }

            float ejeX = 0, ejeY = 0, ejeZ = 0;

            foreach (var parte in listaPartes)
            {
                Punto centroParte = parte.calcularCentroMasa();
                ejeX += centroParte.X;
                ejeY += centroParte.Y;
                ejeZ += centroParte.Z;
            }

            int numPartes = listaPartes.Count;

            return new Punto(ejeX / numPartes, ejeY / numPartes, ejeZ / numPartes);
        }

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

        public void escalar(float factor)
        {
            foreach (var parte in listaPartes)
            {
                parte.escalar(factor);
            }
        }

        public void rotar(Punto angulo)
        {
            foreach (var parte in listaPartes)
            {
                parte.rotar(angulo);
            }
        }

        public void setCentro(Punto centro)
        {
            centroDeMasa = centro;
        }

        public void trasladar(Punto valorTraslado)
        {
            foreach (var parte in listaPartes)
            {
                parte.trasladar(valorTraslado);
            }

            centroDeMasa.X += valorTraslado.X;
            centroDeMasa.Y += valorTraslado.Y;
            centroDeMasa.Z += valorTraslado.Z;
        }

        public void Addparte(Parte parte)
        {
            listaPartes.Add(parte);
            this.centro = calcularCentroMasa();
        }

        public List<Parte> Getpartes()
        {
            return listaPartes;
        }
    }
}
