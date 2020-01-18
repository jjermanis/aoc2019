using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class Day17 : DayBase, IDay
    {
        private const char NEWLINE = (char)10;
        private const char PATH = (char)35;

        private readonly string _program;

        public Day17(string filename)
        {
            _program = TextFile(filename);
        }

        public Day17() : this("Day17.txt")
        {
        }

        public void Do()
        {
            PrintMap();
            Console.WriteLine($"Sum of alignment params: {SumAlignment()}");
            Console.WriteLine($"Total dust: {NavigateMaze()}");
        }

        public int SumAlignment()
        {
            var map = BuildMap();
            var maxX = map.Keys.Max(key => key.Item1);
            var maxY = map.Keys.Max(key => key.Item2);

            // Check for "T" intersctions.
            var result = 0;
            for (var x = 1; x < maxX - 1; x++)
            {
                for (var y = 1; y < maxY - 1; y++)
                {
                    try
                    {
                        if (map[(x, y)] == PATH &&
                            map[(x - 1, y)] == PATH &&
                            map[(x + 1, y)] == PATH &&
                            map[(x, y + 1)] == PATH &&
                            map[(x, y - 1)] == PATH)
                        {
                            result += x * y;
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                    }
                }
            }
            return result;
        }

        public long NavigateMaze(
            bool showAnimation = false,
            bool continuousFeed = false)
        {
            var asciiComp = new AsciiCapableIntcode(_program);
            asciiComp.Poke(0, 2);

            // TODO: calculate the movement functions.  These were determined manually.
            asciiComp.InputLine("A,A,B,C,C,A,B,C,A,B");
            asciiComp.InputLine("L,12,L,12,R,12");
            asciiComp.InputLine("L,8,L,8,R,12,L,8,L,8");
            asciiComp.InputLine("L,10,R,8,R,12");
            asciiComp.InputLine(continuousFeed ? "y" : "n");

            return asciiComp.AnimateOutputNonAscii(showAnimation);
        }

        private void PrintMap()
        {
            Console.Clear();
            var intcode = new Intcode(_program);
            foreach (var output in intcode.Execute())
                Console.Write((char)output);
        }

        private IDictionary<(int, int), char> BuildMap()
        {
            var intcode = new Intcode(_program);

            var x = 0;
            var y = 0;
            var map = new Dictionary<(int, int), char>();
            foreach (var output in intcode.Execute())
            {
                if (output == NEWLINE)
                {
                    y++;
                    x = 0;
                }
                else
                {
                    map[(x++, y)] = (char)output;
                }
            }
            return map;
        }
    }
}
