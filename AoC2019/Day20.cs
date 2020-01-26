using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class PlutoMazeInfo
    {
        public PlutoMazeInfo()
        {
            Cells = new Dictionary<(int, int), char>();
            Warps = new Dictionary<(int, int), (int, int)>();
        }
        public IDictionary<(int, int), char> Cells { get; set; }
        public (int, int) Start { get; set; }
        public (int, int) Finish { get; set; }
        public IDictionary<(int, int), (int, int)> Warps { get; set; }
    }

    public class Day20 : DayBase, IDay
    {
        private readonly PlutoMazeInfo _maze;
        public Day20(string filename)
        {
            _maze = ParseMaze(filename);
        }
        public Day20() : this("Day20.txt")
        {
        }
        public void Do()
        {
            PrintMaze();
            Console.WriteLine($"Shortest path to finish: {ShortestPathLength()}");
        }

        public int ShortestPathLength()
        {
            var fastestPathLen = int.MaxValue;
            var distances = new Dictionary<(int, int), int>();
            var cells = new Queue<((int, int), int)>();
            var start = _maze.Start;
            cells.Enqueue((start, 0));
            distances.Add(start, 0);
            while (cells.Count > 0)
            {
                var curr = cells.Dequeue();
                var loc = curr.Item1;
                var x = curr.Item1.Item1;
                var y = curr.Item1.Item2;
                var dist = curr.Item2;

                if (loc == _maze.Finish)
                    // The start and finish locations are each off-by-one.  Adjust.
                    fastestPathLen = dist - 2;

                Try(x - 1, y);
                Try(x + 1, y);
                Try(x, y - 1);
                Try(x, y + 1);

                void Try(int xx, int yy)
                {
                    if (_maze.Cells[(xx, yy)] != '#')
                    {
                        var isUnvisited = !distances.Keys.Contains((xx, yy));
                        var isFaster = !isUnvisited && dist < distances[(xx, yy)];
                        if (isUnvisited || isFaster)
                        {
                            if (isFaster)
                                distances.Remove((xx, yy));

                            if (_maze.Cells[(xx, yy)] == 'P')
                            {
                                loc = _maze.Warps[(xx, yy)];
                                cells.Enqueue((loc, dist));
                                distances.Add(loc, dist);
                            }
                            else
                            {
                                cells.Enqueue(((xx, yy), dist + 1));
                                distances.Add((xx, yy), dist + 1);
                            }
                        }
                    }
                }
            }

            return fastestPathLen;
        }

        private PlutoMazeInfo ParseMaze(string filename)
        {
            var maze = new PlutoMazeInfo();
            var rawMap = TextFileLines(filename).ToList();
            // First pass, just look for "normal" cells
            for (var y = 0; y < rawMap.Count; y++)
            {
                var line = rawMap[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var cell = line[x];
                    if (cell == ' ')
                        cell = '#';
                    if (cell == '#' || cell == '.')
                        maze.Cells[(x, y)] = cell;
                    else
                        maze.Cells[(x, y)] = '?';
                }
            }
            // Second pass, look for letter pairs
            var outWarps = new Dictionary<string, (int, int)>();
            var inWarps = new Dictionary<string, (int, int)>();
            for (var y = 0; y < rawMap.Count-1; y++)
            {
                var line = rawMap[y];
                for (var x = 0; x < line.Length-1; x++)
                {
                    var cell = line[x];
                    if (Char.IsUpper(cell))
                    {
                        string key = null;
                        bool isVert = true;
                        if (Char.IsUpper(line[x + 1]))
                        {
                            key = WarpKey(cell, line[x + 1]);
                            isVert = false;
                        }
                        else if (Char.IsUpper(rawMap[y + 1][x]))
                        {
                            key = WarpKey(cell, rawMap[y + 1][x]);
                            isVert = true;
                        }

                        if (key != null)
                        {
                            bool isOuter = x < 3 || y < 3 || line.Length - x < 3 || rawMap.Count - y < 3;
                            (int, int) portal;
                            (int, int) other;
                            if (IsDeadEnd(maze, x, y))
                            {
                                other = (x, y);
                                if (isVert)
                                    portal = (x, y + 1);
                                else
                                    portal = (x + 1, y);
                            }
                            else
                            {
                                portal = (x, y);
                                if (isVert)
                                    other = (x, y + 1);
                                else
                                    other = (x + 1, y);
                            }
                            maze.Cells[other] = '#';

                            if ("AA".Equals(key))
                            {
                                maze.Cells[portal] = 'S';
                                maze.Start = portal;
                            }
                            else if ("ZZ".Equals(key))
                            {
                                maze.Cells[portal] = 'F';
                                maze.Finish = portal;
                            }
                            else
                            {
                                maze.Cells[portal] = 'P';
                                if (isOuter)
                                    outWarps.Add(key, portal);
                                else
                                    inWarps.Add(key, portal);
                            }
                        }
                    }
                }
            }
            // Third, now that all portals are known, complete the warp table
            foreach (var key in outWarps.Keys)
            {
                var outWarp = outWarps[key];
                var inWarp = inWarps[key];
                maze.Warps.Add(outWarp, inWarp);
                maze.Warps.Add(inWarp, outWarp);
            }
            return maze;
        }

        private void PrintMaze()
            => PrintMaze(_maze);
        private void PrintMaze(PlutoMazeInfo maze)
        {
            for (int y = 0; true; y++)
            {
                if (!maze.Cells.ContainsKey((0, y)))
                    break;

                for (int x = 0; true; x++)
                {
                    if (maze.Cells.ContainsKey((x, y)))
                        Console.Write(maze.Cells[(x, y)]);
                    else
                        break;
                }
                Console.WriteLine();
            }
        }


        private bool IsDeadEnd(PlutoMazeInfo maze, int x, int y)
        {
            var wallCount = 0;
            if (IsWall(maze, x - 1, y)) wallCount++;
            if (IsWall(maze, x + 1, y)) wallCount++;
            if (IsWall(maze, x, y - 1)) wallCount++;
            if (IsWall(maze, x, y + 1)) wallCount++;
            return wallCount == 3;
        }

        private bool IsWall(PlutoMazeInfo maze, int x, int y)
        {
            if (!maze.Cells.ContainsKey((x, y)))
                return true;
            return maze.Cells[(x, y)] == '#';
        }

        private string WarpKey(char a, char b)
            => $"{a}{b}";
    }
}
