using System;

namespace AoC2019
{
    class Program
    {
        private const string FILE_PATH = @"..\..\..\Inputs\";

        static void Main(string[] args)
        {
            int start = Environment.TickCount;

            new Day25().Do();

            Console.WriteLine($"Time: {Environment.TickCount - start} ms");
        }

    }
}
