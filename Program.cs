
using System;
using programGraph;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dibujando una U");

            using (Game game = new(800, 600))
            {
                game.Run();
            }
        }
    }
}

