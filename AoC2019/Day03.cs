using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class Day03 : DayBase, IDay
    {
        public void Do()
        {
            var filename = "Day03.txt";
            Console.WriteLine($"Manhattan distance: {NearestManhattanIntersectDist(filename)}");
            Console.WriteLine($"Wire distance:      {NearestWireIntersectDist(filename)}");
        }

        public int NearestManhattanIntersectDist(string filename)
        {
            var data = TextFileLines(filename).ToArray();
            return NearestManhattanIntersectDist(data[0], data[1]);
        }
        public int NearestWireIntersectDist(string filename)
        {
            var data = TextFileLines(filename).ToArray();
            return NearestWireIntersectDist(data[0], data[1]);
        }

        public int NearestManhattanIntersectDist(string w1, string w2)
        {
            var grid = BuildGrid(w1);

            var minDist = int.MaxValue;
            var curr = new GridLoc();
            foreach (var move in Moves(w2))
            {
                for (int i = 0; i < move.Distance; i++)
                {
                    curr = curr.Move(move.Direction);

                    if (grid.ContainsKey(curr))
                    {
                        var dist = Math.Abs(curr.X) + Math.Abs(curr.Y);
                        minDist = Math.Min(minDist, dist);
                    }
                }
            }
            return minDist;
        }

        public int NearestWireIntersectDist(string w1, string w2)
        {
            var grid = BuildGrid(w1);

            var minDist = int.MaxValue;
            var count = 0;
            var curr = new GridLoc();
            foreach (var move in Moves(w2))
            {
                for (int i = 0; i < move.Distance; i++)
                {
                    curr = curr.Move(move.Direction);
                    count++;

                    if (grid.ContainsKey(curr))
                    {
                        var dist = grid[curr] + count;
                        minDist = Math.Min(minDist, dist);
                    }
                }
            }
            return minDist;
        }

        private IDictionary<GridLoc, int> BuildGrid(string steps)
        {
            var curr = new GridLoc();
            var result = new Dictionary<GridLoc, int>();
            int moves = 0;
            result.Add(curr, moves++);
            foreach (var move in Moves(steps))
            {
                for (int i = 0; i < move.Distance; i++)
                {
                    curr = curr.Move(move.Direction);
                    if (!result.ContainsKey(curr))
                        result.Add(curr, moves++);
                    else
                        moves++;
                }
            }
            return result;
        }

        private IEnumerable<Move> Moves(string steps)
        {
            var moves = steps.Split(',');
            foreach (var move in moves)
                yield return new Move
                {
                    Direction = move[0],
                    Distance = int.Parse(move.Substring(1))
                };
        }

        struct GridLoc
        {
            public int X { get; set; }
            public int Y { get; set; }
            public GridLoc Move(char dir)
            {
                switch (dir)
                {
                    case 'L':
                        return new GridLoc { X = this.X - 1, Y = this.Y };
                    case 'R':
                        return new GridLoc { X = this.X + 1, Y = this.Y };
                    case 'U':
                        return new GridLoc { X = this.X, Y = this.Y + 1 };
                    case 'D':
                        return new GridLoc { X = this.X, Y = this.Y - 1 };
                    default:
                        throw new Exception($"Unknown dir: {dir}");
                }
            }
        }

        struct Move
        {
            public char Direction { get; set; }
            public int Distance { get; set; }
        }
    }
}
