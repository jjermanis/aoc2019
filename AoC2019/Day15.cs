using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class Day15 : DayBase, IDay
    {
        private readonly char[] CHARS = new char[] { '#', '.', 'O' };
        private readonly List<int> DIRECTIONS = new List<int> { 1, 4, 2, 3 };
        private readonly List<int> DX = new List<int> { 0, 1, 0, -1 };
        private readonly List<int> DY = new List<int> { 1, 0, -1, 0 };

        private readonly string _program;

        public Day15(string filename)
        {
            _program = TextFile(filename);
        }

        public Day15() : this("Day15.txt")
        {
        }

        public void Do()
        {
            (var p1, var p2) = TraverseMaze(drawMaze: true);
            Console.WriteLine($"Distance to oxygen: {p1}");
            Console.WriteLine($"Time to fill with oxygen: {p2}");
        }

        public (int, int) TraverseMaze(bool drawMaze = false)
        {
            var intcode = new Intcode(_program);

            var resultDistanceToOx = -1;
            var resultMaxDistanceFromOx = -1;

            var x = 0;
            var y = 0;
            var dirIndex = 0;
            var map = new Dictionary<(int, int), char>();
            var deadEnds = new HashSet<(int, int)>();
            var distances = new Dictionary<(int, int), int>();
            var currDistance = 0;
            map[(0, 0)] = '.';
            distances[(0, 0)] = 0;
            int count = 0;
            bool lookingForO = true;

            // Traverse the maze using the right-hand rule.  Two high-level steps:
            // 1) Find the Ox.  Track distance from Start to Ox.
            // 2) Traverse the entire maze.  Track max distance from Ox.
            using (var enumer = intcode.Execute().GetEnumerator())
            {
                // TODO: figure out better way to determine when we are done
                while (true)
                {
                    // Option 1: Can the robot turn right?
                    if (!Try(1))
                    {
                        // Option 2: Can the robot move straight?
                        if (!Try(0))
                        {
                            // Option 3: Can you turn left?
                            if (!Try(3))
                            {
                                // This must be a deadend.
                                if (deadEnds.Contains((x, y)))
                                    break;
                                deadEnds.Add((x, y));
                                // Option 4: Backtrack
                                if (!Try(2))
                                {
                                    throw new Exception("Something went horribly wrong");
                                }
                            }
                        }
                    }
                    
                    count++;

                    if (distances.ContainsKey((x, y)))
                    {
                        currDistance = distances[(x, y)];
                    }
                    else
                    {
                        currDistance++;
                        distances[(x, y)] = currDistance;
                    }
                    if (lookingForO)
                    {
                        if (map[(x, y)] == 'O')
                        {
                            resultDistanceToOx = currDistance;
                            lookingForO = false;
                            distances = new Dictionary<(int, int), int>();
                            deadEnds = new HashSet<(int, int)>();
                            currDistance = 0;
                        }
                    }
                    else
                    {
                        resultMaxDistanceFromOx = Math.Max(resultMaxDistanceFromOx, currDistance);
                    }

                }

                // Locally-scoped methods START
                int Move(int di)
                {
                    intcode.AddInputs(Dir(di));
                    enumer.MoveNext();
                    return (int)enumer.Current;
                }
                bool Try(int delta)
                {
                    var output = Move(dirIndex + delta);
                    map[NewLoc(x, y, dirIndex + delta)] = CHARS[output];
                    if (output != 0)
                    {
                        // You can move forward
                        (x, y) = NewLoc(x, y, dirIndex + delta);
                        dirIndex = (dirIndex + delta) % 4;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                // Locally scoped methods end
            }

            if (drawMaze)
                DrawMaze(map);
            return (resultDistanceToOx, resultMaxDistanceFromOx);
        }

        private int Dir(int index) 
            => DIRECTIONS[index % 4];
        private (int, int) NewLoc(int x, int y, int di) 
            => (x + DX[di % 4], y + DY[di % 4]);

        private void DrawMaze(Dictionary<(int, int), char> map)
        {
            var minX = map.Keys.Min(key => key.Item1);
            var maxX = map.Keys.Max(key => key.Item1);
            var minY = map.Keys.Min(key => key.Item2);
            var maxY = map.Keys.Max(key => key.Item2);
            Console.Clear();
            for (int y = maxY; y >= minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (x == 0 && y == 0)
                        Console.Write('S');
                    else if (map.ContainsKey((x, y)))
                        Console.Write(map[(x, y)]);
                    else
                        Console.Write('#');
                }
                Console.WriteLine();
            }
        }
    }
}
