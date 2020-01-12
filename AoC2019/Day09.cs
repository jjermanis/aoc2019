using System;
using System.Linq;

namespace AoC2019
{
    public class Day09 : DayBase, IDay
    {
        private readonly string _program;

        public Day09(string filename)
        {
            _program = TextFile(filename);
        }
        public Day09() : this("Day09.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"BOOST keycode: {Run(1)}");
            Console.WriteLine($"Distress coordinates: {Run(2)}");
        }

        public long Run(int input)
        {
            var intcode = new Intcode(_program);
            intcode.AddInputs(input);
            return intcode.Execute().First();
        }
    }
}
