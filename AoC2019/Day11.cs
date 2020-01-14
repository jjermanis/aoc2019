using System;
using System.Collections.Generic;

namespace AoC2019
{
    public class Day11 : DayBase, IDay
    {
        private readonly string _program;

        public Day11(string filename)
        {
            _program = TextFile(filename);
        }

        public Day11() : this("Day11.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"Spaces painted: {GetPaintedSpaceCount(0)}");

            Console.WriteLine("Visualization:");
            PrintPainting(PaintingVisualization(1));
        }

        public int GetPaintedSpaceCount(int input)
            => GetPainting(input).Count;

        enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        private IDictionary<(int, int), long> GetPainting(int startColor)
        {
            var intcode = new Intcode(_program);
            var painting = new Dictionary<(int, int), long>();
            painting[(0, 0)] = startColor;
            intcode.AddInputs(startColor);

            int x = 0;
            int y = 0;
            Direction dir = Direction.Up;
            using (var enumer = intcode.Execute().GetEnumerator())
            {
                while (enumer.MoveNext())
                {
                    var color = enumer.Current;
                    enumer.MoveNext();
                    var turn = enumer.Current;

                    // Paint before moving
                    painting[(x, y)] = color;

                    // Turn
                    var dt = turn == 0 ? -1 : 1;
                    dir = (Direction)(((int)dir + dt + 4) % 4);
                    switch (dir)
                    {
                        case Direction.Up:
                            y++;
                            break;
                        case Direction.Right:
                            x++;
                            break;
                        case Direction.Down:
                            y--;
                            break;
                        case Direction.Left:
                            x--;
                            break;
                    }

                    // Let robot know about location
                    if (painting.ContainsKey((x, y)))
                        intcode.AddInputs(painting[(x, y)]);
                    else
                        intcode.AddInputs(0);
                }
            }
            return painting;
        }

        private void PrintPainting(IEnumerable<string> visualization)
        {
            foreach (var line in visualization)
                Console.WriteLine(line);
        }

        public IEnumerable<string> PaintingVisualization(int startColor)
        {
            var painting = GetPainting(startColor);

            var result = new List<string>();
            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;
            foreach (var key in painting.Keys)
            {
                minX = Math.Min(minX, key.Item1);
                maxX = Math.Max(maxX, key.Item1);
                minY = Math.Min(minY, key.Item2);
                maxY = Math.Max(maxY, key.Item2);
            }
            var width = maxX - minX + 1;
            for (var y = maxY; y >= minY; y--)
            {
                var line = new char[width];
                for (var x = minX; x <= maxX; x++)
                {
                    if (painting.ContainsKey((x, y)) && (painting[(x, y)] == 1))
                    {
                        line[x+minX] = '#';
                    }
                    else
                    {
                        line[x+minX] = ' ';
                    }
                }
                result.Add(new string(line).TrimEnd());
            }
            return result;
        }
    }
}
