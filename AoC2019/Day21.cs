using System;

namespace AoC2019
{
    public class Day21 : DayBase, IDay
    {
        private readonly string _program;

        public Day21(string filename)
        {
            _program = TextFile(filename);
        }

        public Day21() : this("Day21.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"Walk damage: {Walk(true)}");
            Console.WriteLine($"Run damage: {Run(true)}");
        }

        public long Walk(bool animate=false)
        {
            var asciiComp = new AsciiCapableIntcode(_program);
            asciiComp.InputLine("NOT C T");
            asciiComp.InputLine("AND D T");
            asciiComp.InputLine("NOT A J");
            asciiComp.InputLine("OR T J");
            asciiComp.InputLine("WALK");
            return asciiComp.AnimateOutputNonAscii(animate);
        }

        public long Run(bool animate=false)
        {
            var asciiComp = new AsciiCapableIntcode(_program);
            asciiComp.InputLine("NOT C J");
            asciiComp.InputLine("AND D J");
            asciiComp.InputLine("NOT A T");
            asciiComp.InputLine("OR T J");
            asciiComp.InputLine("RUN");
            return asciiComp.AnimateOutputNonAscii(animate);
        }
    }
}
