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
            PrintField(50, 50);
            Console.WriteLine($"Close point count: {LocalPointCount()}");
            Console.WriteLine($"First square: {FindFirstSquare(100)}");
        }

        public int LocalPointCount()
        {
            var maxX = 50;
            var maxY = 50;
            var result = 0;
            foreach (var row in GetBounds(maxX, maxY))
            {
                if (row.Item1 >= 0)
                {
                    result += row.Item2 - row.Item1 + 1;
                }
            }
            return result;
        }

        public void PrintField(int maxX, int maxY)
        {
            Console.Clear();
            foreach (var row in GetBounds(maxX, maxY))
            {
                if (row.Item1 >= 0)
                {
                    Console.Write(new string('.', row.Item1));
                    Console.Write(new string('#', row.Item2 - row.Item1 + 1));
                    Console.Write(new string('.', maxX - row.Item2 - 1));
                }
                else
                {
                    Console.Write(new string('.', maxX));
                }
                Console.WriteLine();
            }
        }

        public int FindFirstSquare(int size=100)
        {
            // Skip the first few lines.  Except for degenerate cases, the square won't be there.
            // And some lines are blank, which causes this algo problems.
            var START = 5;
            var lines = new List<(int, int)>();
            for (int x = 0; x < START; x++)
                lines.Add((-1, -1));

            foreach (var row in GetBounds(startY: START))
            {
                lines.Add(row);
                if (lines.Count > size &&
                    row.Item2 - row.Item1 >= size)
                {
                    var left = row.Item1;
                    var right = row.Item1 + size -1;
                    var top = lines[lines.Count - size];
                    if (top.Item1 <= left && top.Item2 >= right)
                    {
                        return (10000 * left) + (lines.Count - size);
                    }
                }
            }
            throw new Exception("Never gets here");
        }

        private IEnumerable<(int, int)> GetBounds(
            int maxX=int.MaxValue,
            int maxY=int.MaxValue,
            int startY = 0)
        {
            int prevWidth = 0;
            int startX = 0;
            for (int y = startY; y < maxY; y++)
            {
                bool isFoundFirst = false;
                int first = -1;
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
                        startX = x;
                        if (prevWidth > 0)
                            x += prevWidth - 1;
                    }
                    else if (!isFoundFirst && x - startX > 10)
                    {
                        prevWidth = 0;
                        yield return (-1, -1);
                        isReturnedAnswer = true;
                        break;
                    }
                    else if (isFoundFirst && output == 0)
                    {
                        prevWidth = x - first - 1;
                        yield return (first, x - 1);
                        isReturnedAnswer = true;
                        break;
                    }
                }
                if (!isReturnedAnswer)
                {
                    if (first == -1)
                    {
                        prevWidth = 0;
                        yield return (-1, -1);
                    }
                    else
                    {
                        prevWidth = maxX - first - 1;
                        yield return (first, maxX - 1);
                    }
                }
            }
        }

    }
}
