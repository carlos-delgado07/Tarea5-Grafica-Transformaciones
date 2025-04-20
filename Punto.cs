
namespace programGraph
{
    public class Punto
    {
        public float X {get;set;}
        public float Y {get;set;}
        public float Z {get;set;}

        public Punto(float x, float y , float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Punto()
        {
            this.X = this.Y = this.Z = 0.0f;
        }

        public Punto(float v){
            this.X = this.Y = this.Z = v;
        }

        public Punto(Punto p){
            this.X = p.X;
            this.Y = p.Y;
            this.Z = p.Z;
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}
