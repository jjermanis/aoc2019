using System;
using System.Linq;

namespace AoC2019
{
    public class Day05 : DayBase, IDay
    {
        private readonly string _program;

        public Day05(string filename)
        {
            _program = TextFile(filename);
        }
        public Day05() : this("Day05.txt")
        {
        }

        public void Do()
        {
            HandleInput(1);
            HandleInput(5);

            void HandleInput(int input) => Console.WriteLine($"Diagnostic code for {input}: {DoCalc(input)}");
        }

        public long DoCalc(int input)
        {
            var intcode = new Intcode(_program);
            intcode.AddInputs(input);
            return intcode.Execute().Last();
        }
    }
}
