using System;

namespace AoC2019
{
    public class Day01 : DayBase, IDay
    {
        public void Do()
        {
            (var s, var c) = DoCalc();
            Console.WriteLine($"Simple fuel calc: {s}");
            Console.WriteLine($"Complex fuel calc: {c}");
        }

        public (int, int) DoCalc()
            => DoCalc("Day01.txt");

        public (int, int) DoCalc(string filename)
        {
            var simple = 0;
            var complex = 0;
            foreach (var massText in TextFileLines(filename))
            {
                simple += Day1SimpleFuel(int.Parse(massText));
                complex += Day1ComplexFuel(int.Parse(massText));
            }
            return (simple, complex);
        }

        private int Day1ComplexFuel(int mass)
        {
            var simpleFuel = Day1SimpleFuel(mass);
            return simpleFuel > 0 ? simpleFuel + Day1ComplexFuel(simpleFuel) : 0;
        }

        private int Day1SimpleFuel(int mass)
            => mass / 3 - 2;
    }
}
