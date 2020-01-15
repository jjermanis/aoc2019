using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class Day19 : DayBase, IDay
    {
        private readonly string _program;

        public Day19(string filename)
        {
            _program = TextFile(filename);
        }

        public Day19() : this("Day19.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"Close point count: {LocalPointCount()}");
        }

        public int LocalPointCount()
        {
            var maxX = 50;
            var maxY = 50;
            int rows = 0;
            var result = 0;
            foreach (var row in GetBounds(maxX, maxY))
            {
                if (++rows >= maxY)
                    return result;
                if (row.Item1 >= 0)
                {
                    result += row.Item2 - row.Item1 + 1;
                }
            }
            return result;
        }

        private IEnumerable<(int, int)> GetBounds(
            int maxX,
            int maxY)
        {
            for (int y = 0; true; y++)
            {
                bool isFoundFirst = false;
                int first = -1;
                int startX = 0;
                bool isReturnedAnswer = false;
                for (int x = startX; x < maxX; x++)
                {
                    var ic = new Intcode(_program);
                    ic.AddInputs(x, y);
                    var output = (int)ic.Execute().First();
                    if (!isFoundFirst && output == 1)
                    {
                        isFoundFirst = true;
                        first = x;
                    }
                    else if (isFoundFirst && output == 0)
                    {
                        yield return (first, x - 1);
                        isReturnedAnswer = true;
                        break;
                    }
                }
                if (!isReturnedAnswer)
                {
                    if (first == -1)
                        yield return (-1, -1);
                    else
                        yield return (first, 49);
                }
            }
        }

        private IEnumerable<(int, int)> GetBoundsPart12()
        {
            var result = 0;
            for (int y = 0; true; y++)
            {
                bool isFoundFirst = false;
                int first = -1;
                int startX = 0;
                int lastLen = 0;
                bool isReturnedAnswer = false;
                for (int x = startX; x < 50; x++)
                {
                    var ic = new Intcode(_program);
                    ic.AddInputs(x, y);
                    var output = (int)ic.Execute().First();
                    if (!isFoundFirst && output == 1)
                    {
                        isFoundFirst = true;
                        startX = x;
                        first = x;
                    }
                    else if (isFoundFirst && output == 0)
                    {
                        yield return (first, x - 1);
                        isReturnedAnswer = true;
                        break;
                    }
                }
                if (!isReturnedAnswer)
                {
                    if (first == -1)
                        yield return (-1, -1);
                    else
                        yield return (first, 49);
                }
            }
        }


    }
}
