using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class Day03 : DayBase, IDay
    {
        private readonly string _wire1;
        private readonly string _wire2;
        private readonly IDictionary<GridLoc, int> _grid1;

        public Day03(string filename)
        {
            var data = TextFileLines(filename).ToArray();
            _wire1 = data[0];
            _wire2 = data[1];
            _grid1 = BuildGrid(_wire1);
        }
        public Day03() : this("Day03.txt")
        {
        }
        public void Do()
        {
            Console.WriteLine($"Manhattan distance: {NearestManhattanIntersectDist()}");
            Console.WriteLine($"Wire distance:      {NearestWireIntersectDist()}");
        }

        public int NearestManhattanIntersectDist()
        {
            // Manhattan distance is simply the sume of differences between points for their
            // x and y coordinates.
            var minDist = int.MaxValue;
            var curr = new GridLoc();
            foreach (var move in Moves(_wire2))
            {
                for (int i = 0; i < move.Distance; i++)
                {
                    curr = curr.Move(move.Direction);

                    if (_grid1.ContainsKey(curr))
                    {
                        var dist = Math.Abs(curr.X) + Math.Abs(curr.Y);
                        minDist = Math.Min(minDist, dist);
                    }
                }
            }
            return minDist;
        }

        public int NearestWireIntersectDist()
        {
            var minDist = int.MaxValue;
            var count = 0;
            var curr = new GridLoc();
            foreach (var move in Moves(_wire2))
            {
                for (int i = 0; i < move.Distance; i++)
                {
                    curr = curr.Move(move.Direction);
                    count++;

                    if (_grid1.ContainsKey(curr))
                    {
                        var dist = _grid1[curr] + count;
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
