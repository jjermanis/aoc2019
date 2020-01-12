using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class Day01 : DayBase, IDay
    {
        private readonly IEnumerable<int> _moduleMasses;

        public Day01(string filename)
        {
            _moduleMasses = TextFileLines(filename).Select(m => int.Parse(m));
        }

        public Day01() : this("Day01.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"Simple fuel calc: {SimpleFuelTotal()}");
            Console.WriteLine($"Complex fuel calc: {ComplexFuelTotal()}");
        }

        public int SimpleFuelTotal()
            => _moduleMasses.Sum(m => Day1SimpleFuel(m));

        public int ComplexFuelTotal()
            => _moduleMasses.Sum(m => Day1ComplexFuel(m));


        private int Day1ComplexFuel(int mass)
        {
            var simpleFuel = Day1SimpleFuel(mass);
            return simpleFuel > 0 ? simpleFuel + Day1ComplexFuel(simpleFuel) : 0;
        }

        private int Day1SimpleFuel(int mass)
            => mass / 3 - 2;
    }
}
