using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace programGraph
{
    public class Poligono : IGraphics
    {
        [JsonProperty("puntos")]
        public List<Punto> puntos { get; set; } = new List<Punto>();

        [JsonProperty("color")]
        public float[] color { get; set; }

        [JsonProperty("centro")]
        public Punto centro { get; set; } = new Punto();

        [JsonIgnore]
        public PrimitiveType primitiveType { get; set; } = PrimitiveType.LineLoop;

        public Poligono(List<Punto> listaPuntos, float r, float g, float b)
        {
            this.puntos = new List<Punto>(listaPuntos);
            this.centro = new Punto();
            this.color = new float[] { r, g, b };
        }

        public Poligono(Punto centro)
        {
            this.centro = centro;
            this.puntos = new List<Punto>();
            this.primitiveType = PrimitiveType.LineLoop;
        }

        public Poligono()
        {
            this.centro = new Punto();
            this.puntos = new List<Punto>();
            this.primitiveType = PrimitiveType.LineLoop;
            this.color = new float[] { 0.1f, 0.1f, 0.1f };
        }

        public List<Punto> GetPuntos() => puntos;
        public float[] GetColor() => color;

        public Punto calcularCentroMasa()
        {
            if (puntos.Count == 0) return new Punto(0, 0, 0);

            float minX = puntos.Min(p => p.X);
            float maxX = puntos.Max(p => p.X);

            float minY = puntos.Min(p => p.Y);
            float maxY = puntos.Max(p => p.Y);

            float minZ = puntos.Min(p => p.Z);
            float maxZ = puntos.Max(p => p.Z);

            return new Punto((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2);
        }

        public void Dibujar()
        {
            GL.Color4(this.color[0], this.color[1], this.color[2], 1.0f);
            GL.Begin(primitiveType);
            foreach (var punto in puntos)
            {
                GL.Vertex3(punto.X, punto.Y, punto.Z);
            }
            GL.End();
            GL.Flush();
        }

        public void escalar(float factor)
        {
            var centro = this.calcularCentroMasa();
            for (int i = 0; i < puntos.Count; i++)
            {
                puntos[i].X = centro.X + (puntos[i].X - centro.X) * factor;
                puntos[i].Y = centro.Y + (puntos[i].Y - centro.Y) * factor;
                puntos[i].Z = centro.Z + (puntos[i].Z - centro.Z) * factor;
            }
        }

        public void rotar(Punto angulo)
        {
            var centro = this.calcularCentroMasa();

            float radX = MathHelper.DegreesToRadians(angulo.X);
            float radY = MathHelper.DegreesToRadians(angulo.Y);
            float radZ = MathHelper.DegreesToRadians(angulo.Z);

            for (int i = 0; i < puntos.Count; i++)
            {
                float x = puntos[i].X - centro.X;
                float y = puntos[i].Y - centro.Y;
                float z = puntos[i].Z - centro.Z;

                float y1 = y * (float)Math.Cos(radX) - z * (float)Math.Sin(radX);
                float z1 = y * (float)Math.Sin(radX) + z * (float)Math.Cos(radX);
                y = y1; z = z1;

                float x1 = x * (float)Math.Cos(radY) + z * (float)Math.Sin(radY);
                z1 = -x * (float)Math.Sin(radY) + z * (float)Math.Cos(radY);
                x = x1; z = z1;

                x1 = x * (float)Math.Cos(radZ) - y * (float)Math.Sin(radZ);
                y1 = x * (float)Math.Sin(radZ) + y * (float)Math.Cos(radZ);
                x = x1; y = y1;

                puntos[i].X = centro.X + x;
                puntos[i].Y = centro.Y + y;
                puntos[i].Z = centro.Z + z;
            }
        }

        public void setCentro(Punto centro)
        {
            this.centro = centro;
        }

        public void trasladar(Punto valorTraslado)
        {
            foreach (var punto in puntos)
            {
                punto.X += valorTraslado.X;
                punto.Y += valorTraslado.Y;
                punto.Z += valorTraslado.Z;
            }
        }

        public Punto getCentro() => this.calcularCentroMasa();
    }
}
