using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class MazeInfo
    {
        public MazeInfo()
        {
            Cells = new Dictionary<(int, int), char>();
            Keys = new Dictionary<char, (int, int)>();
            Doors = new Dictionary<char, (int, int)>();
        }
        public IDictionary<(int, int), char> Cells { get; set; }
        public IDictionary<char, (int, int)> Keys { get; set; }
        public IDictionary<char, (int, int)> Doors { get; set; }
    }

    public class PathInfo
    {
        public PathInfo()
        {
            Distance = 0;
            Blockers = "";
        }
        public int Distance { get; set; }
        public string Blockers { get; set; }
    }

    public class Day18 : DayBase, IDay
    {
        private const int LONG_PATH = 999999;

        private readonly MazeInfo _mazeInfo;
        private readonly IDictionary<char, IDictionary<char, PathInfo>> _pathInfo;

        private int _fastestPathLen = Int32.MaxValue;

        public Day18(string filename)
        {
            _mazeInfo = ParseMaze(filename);
            _pathInfo = GetPathInfo(_mazeInfo);
        }
        public Day18() : this("Day18.txt")
        {
        }

        public void Do()
        {
            throw new NotImplementedException();
        }

        public int SolveMaze()
        {
            var min = MinDistance(_mazeInfo, _pathInfo, '@', "", 0);
            return min;
        }

        private void PrintMaze()
        {
            for (int y = 0; true; y++)
            {
                if (!_mazeInfo.Cells.ContainsKey((0, y)))
                    break;

                for (int x = 0; true; x++)
                {
                    if (_mazeInfo.Cells.ContainsKey((x, y)))
                        Console.Write(_mazeInfo.Cells[(x, y)]);
                    else
                        break;
                }
                Console.WriteLine();
            }
        }

        private int MinDistance(
            MazeInfo maze,
            IDictionary<char, IDictionary<char, PathInfo>> pathInfo,
            char start,
            string keysVisited,
            int cummDistance)
        {
            if (cummDistance >= _fastestPathLen)
                return LONG_PATH;
            if (keysVisited.Length == pathInfo.Count - 1)
            {
                if (cummDistance < _fastestPathLen)
                {
                    _fastestPathLen = cummDistance;
                    //Console.WriteLine($"New champ: {_theChamp} at {DateTime.Now}.  {_cases:N0} cases explored.");
                }
                return cummDistance;
            }
            var candidates = new List<char>();
            foreach (var key in maze.Keys.Keys)
            {
                if (key == '@')
                    continue;
                if (start == key)
                    continue;
                if (keysVisited.Contains(key))
                    continue;
                if (IsBlocked(keysVisited, pathInfo[start][key]))
                    continue;
                candidates.Add(key);
            }
            candidates = candidates.OrderBy(c => pathInfo[start][c].Distance).Take(4).ToList();
            int shortestDistance = int.MaxValue;
            foreach (var key in candidates)
            {
                var attempt = MinDistance(maze, pathInfo, key, keysVisited + key, cummDistance + pathInfo[start][key].Distance);
                shortestDistance = Math.Min(shortestDistance, attempt);
            }

            return shortestDistance;
        }

        private bool IsBlocked(
            string keysVisited,
            PathInfo pathInfo)
        {
            foreach (var blocker in pathInfo.Blockers)
            {
                if (!keysVisited.Contains((char)(blocker + 32)))
                    return true;
            }
            return false;
        }

        private MazeInfo ParseMaze(string filename)
        {
            var y = 0;
            var result = new MazeInfo();
            foreach (var line in TextFileLines(filename))
            {
                for (var x = 0; x < line.Length; x++)
                {
                    var cell = line[x];
                    result.Cells[(x, y)] = cell;
                    if (Char.IsUpper(cell))
                        result.Doors[cell] = (x, y);
                    if (Char.IsLower(cell))
                        result.Keys[cell] = (x, y);
                    if ('@' == cell)
                        result.Keys[cell] = (x, y);
                }
                y++;
            }
            return result;
        }

        private IDictionary<char, IDictionary<char, PathInfo>> GetPathInfo(MazeInfo maze)
        {
            var result = new Dictionary<char, IDictionary<char, PathInfo>>();
            foreach (var key in maze.Keys.Keys)
            {
                result[key] = GetPathInfoForKey(key, maze);
            }
            return result;
        }

        private IDictionary<char, PathInfo> GetPathInfoForKey(
            char key,
            MazeInfo info)
        {
            var result = new Dictionary<char, PathInfo>();

            var visited = new HashSet<(int, int)>();
            var cells = new Queue<(int, int, PathInfo)>();
            var start = info.Keys[key];
            cells.Enqueue((start.Item1, start.Item2, new PathInfo()));
            visited.Add((start.Item1, start.Item2));
            while (cells.Count > 0)
            {
                var curr = cells.Dequeue();
                var x = curr.Item1;
                var y = curr.Item2;
                var dist = curr.Item3.Distance;
                var blockers = curr.Item3.Blockers;
                var val = info.Cells[(x, y)];

                if (Char.IsLower(val) && val != key)
                    result[val] = curr.Item3;
                if (Char.IsUpper(val))
                    blockers += val;

                Try(x - 1, y);
                Try(x + 1, y);
                Try(x, y - 1);
                Try(x, y + 1);

                void Try(int xx, int yy)
                {
                    if (info.Cells[(xx, yy)] != '#')
                        if (!visited.Contains((xx, yy)))
                        {
                            var pi = new PathInfo()
                            {
                                Distance = dist + 1,
                                Blockers = blockers
                            };
                            cells.Enqueue((xx, yy, pi));
                            visited.Add((xx, yy));
                        }
                }
            }

            return result;
        }

    }
}
