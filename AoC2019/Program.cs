using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC2019
{
    class Program
    {
        private const string FILE_PATH = @"..\..\..\Inputs\";

        static void Main(string[] args)
        {
            int start = Environment.TickCount;

            //Day8();
            new Day16().Do();

            Console.WriteLine($"Time: {Environment.TickCount - start} ms");
        }

        static void Day24()
        {
            DoPart1("Day24Test1.txt", 2129920);
            DoPart1("Day24.txt", 13500447);

            void DoPart1(string filename, int expected) => Console.WriteLine($"Case {filename}.  Expected {expected}, actual {FindDupRating(filename)}");
        }

        static int FindDupRating(string filename)
        {
            var seen = new HashSet<int>();
            var grid = InitGrid(filename);
            while (true)
            {
                var rating = BiodiversityRating(grid);
                if (seen.Contains(rating))
                    return rating;
                seen.Add(rating);
                grid = RunMinute(grid);
            }
        }

        static IDictionary<(int, int), bool> RunMinute(IDictionary<(int, int), bool> grid)
        {
            var result = new Dictionary<(int, int), bool>();
            for (var y = 0; y < 5; y++)
                for (var x = 0; x < 5; x++)
                {
                    var nc = NeighborCount(grid, x, y);
                    if (grid[(x, y)])
                    {
                        result[(x, y)] = nc == 1;
                    }
                    else
                    {
                        result[(x, y)] = (nc == 1 || nc == 2);
                    }
                }
            return result;
        }

        static int BiodiversityRating(IDictionary<(int, int), bool> grid)
        {
            var result = 0;
            for (var y = 0; y < 5; y++)
                for (var x = 0; x < 5; x++)
                {
                    if (grid[(x, y)])
                        result += 1 << (5 * y + x);
                }
            return result;
        }

        static int NeighborCount(IDictionary<(int, int), bool> grid, int x, int y)
        {
            var result = 0;
            if (Check(x - 1, y)) result++;
            if (Check(x + 1, y)) result++;
            if (Check(x, y - 1)) result++;
            if (Check(x, y + 1)) result++;
            return result;

            bool Check(int xx, int yy) => grid.ContainsKey((xx, yy)) && grid[(xx, yy)];
        }

        static IDictionary<(int, int), bool> InitGrid(string filename)
        {
            var result = new Dictionary<(int, int), bool>();
            var lines = TextFileLines(filename).ToArray();
            for (var y = 0; y < 5; y++)
                for (var x = 0; x < 5; x++)
                    result[(x, y)] = lines[y][x] == '#';
            return result;
        }

        static void Day19()
        {
            Console.WriteLine(Day19Part1());
        }

        static int Day19Part1()
        {
            int rows = 0;
            var result = 0;
            foreach (var row in GetBoundsPart1())
            {
                if (++rows >= 50)
                    return result;
                if (row.Item1 >= 0)
                {
                    result += row.Item2 - row.Item1 + 1;
                }
            }
            return result;
        }

        static IEnumerable<(int, int)> GetBoundsPart1()
        {
            var program = TextFile("Day19.txt");

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
                    var ic = new Intcode(program);
                    ic.AddInputs(x, y);
                    var output = (int)ic.Execute().First();
                    if (!isFoundFirst && output == 1)
                    {
                        isFoundFirst = true;
                        startX = x;
                        first = x;
                    }
                    else if (isFoundFirst && output==0)
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

        static IEnumerable<(int, int)> GetBoundsPart12()
        {
            var program = TextFile("Day19.txt");

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
                    var ic = new Intcode(program);
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

        private static void Day18()
        {
            DoPart1("Day18Test1.txt", 86);
            DoPart1("Day18Test2.txt", 132);
            DoPart1("Day18Test3.txt", 136);
            DoPart1("Day18Test4.txt", 81);
            DoPart1("Day18.txt", 5242);
            void DoPart1(string inputFile, int expected)
            {
                Console.WriteLine($"Case {inputFile}.");
                var result = SolveMaze(inputFile);
                Console.WriteLine($"Expected: {expected}.  Result: {result}");
            }

        }

        static int _theChamp = int.MaxValue;
        static long _cases = 0;

        private static int SolveMaze(string filename)
        {
            var maze = ParseMaze(filename);
            //PrintMaze(maze);
            var pathInfo = GetPathInfo(maze);

            _cases = 0;
            _theChamp = int.MaxValue;
            var min = MinDistance(maze, pathInfo, '@', "", 0);
            Console.WriteLine($"{_cases:N0} variants explored");
            return min;
        }


        private static int MinDistance(
            MazeInfo maze,
            IDictionary<char, IDictionary<char, PathInfo>> pathInfo,
            char start,
            string keysVisited,
            int cummDistance)
        {
            _cases++;
            if (cummDistance >= _theChamp)
                return 987654;
            if (keysVisited.Length == pathInfo.Count - 1)
            {
                if (cummDistance < _theChamp)
                {
                    _theChamp = cummDistance;
                    Console.WriteLine($"New champ: {_theChamp} at {DateTime.Now}.  {_cases:N0} cases explored.");
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

        private static bool IsBlocked(
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

        private static IDictionary<char, IDictionary<char, PathInfo>> GetPathInfo(MazeInfo maze)
        {
            var result = new Dictionary<char, IDictionary<char, PathInfo>>();
            foreach (var key in maze.Keys.Keys)
            {
                result[key] = GetPathInfoForKey(key, maze);
            }
            return result;
        }

        private static IDictionary<char, PathInfo> GetPathInfoForKey(
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

        private static void PrintMaze(MazeInfo maze)
        {
            for (int y=0; true; y++)
            {
                if (!maze.Cells.ContainsKey((0, y)))
                    break;

                for (int x=0; true; x++)
                {
                    if (maze.Cells.ContainsKey((x, y)))
                        Console.Write(maze.Cells[(x, y)]);
                    else
                        break;
                }
                Console.WriteLine();
            }
        }

        private static MazeInfo ParseMaze(string filename)
        {
            var y = 0;
            var result = new MazeInfo();
            foreach (var line in TextFileLines(filename))
            {
                for (var x=0; x<line.Length; x++)
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

        private static void Day17()
        {
            Day17Print();
            Console.WriteLine($"Alignment parameter sum: {Day17Alignment()}");
            Console.WriteLine($"Dust: {Day17Navigate()}");
        }

        private static void Day17Print()
        {
            var intcode = new Intcode(TextFile("Day17.txt"));
            foreach (var output in intcode.Execute())
                Console.Write((char)output);
        }

        private static long Day17Navigate()
        {
            var intcode = new Intcode(TextFile("Day17.txt"));
            intcode.Poke(0, 2);
            Input("A,A,B,C,C,A,B,C,A,B");
            Input("L,12,L,12,R,12");
            Input("L,8,L,8,R,12,L,8,L,8");
            Input("L,10,R,8,R,12");
            Input("n");
            foreach (var output in intcode.Execute())
            {
                if (output < 127)
                    Console.Write((char)output);
                else
                    return output;
            }
            return 0;

            void Input(string text)
            {
                intcode.AddInputs(text.Select(x => (long)x).ToArray());
                intcode.AddInputs(10);
            }
        }
        private static int Day17Alignment()
        {
            var intcode = new Intcode(TextFile("Day17.txt"));

            var x = 0;
            var y = 0;
            var mx = 0;
            var my = 0;
            var map = new Dictionary<(int, int), char>();
            foreach (var output in intcode.Execute())
            {
                if (output == 10)
                {
                    y++;
                    x = 0;
                    my = y;
                }
                else
                {
                    map[(x++, y)] = (char)output;
                    mx = Math.Max(mx, x);
                }
            }
            var result = 0;
            for (x = 1; x < mx - 1; x++)
            {
                for (y = 1; y < my - 1; y++)
                {
                    try
                    {
                        if (map[(x, y)] == '#' &&
                            map[(x - 1, y)] == '#' &&
                            map[(x + 1, y)] == '#' &&
                            map[(x, y + 1)] == '#' &&
                            map[(x, y - 1)] == '#')
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

        private static void Day16()
        {
            Console.WriteLine("PART 1");
            DoPart1("Day16Test0.txt", 4, 1029498);
            DoPart1("Day16Test1.txt", 100, 24176176);
            DoPart1("Day16Test2.txt", 100, 73745418);
            DoPart1("Day16Test3.txt", 100, 52432133);
            DoPart1("Day16.txt", 100, 85362380);
            void DoPart1(string inputFile, int phaseCount, int expected)
                => Console.WriteLine($"Case {inputFile}.  Expected: {expected}.  Result: {CalcFft(phaseCount, TextFile(inputFile))}");

            Console.WriteLine();
            Console.WriteLine("PART 2");
            DoPart2("Day16Test4.txt", 100, 84462026);
            //DoPart2("Day16.txt", 100, 85362380);
            void DoPart2(string inputFile, int phaseCount, int expected)
                => Console.WriteLine($"Case {inputFile}.  Expected: {expected}.  Result: {CalcRepeatedFft(10000, phaseCount, TextFile(inputFile))}");
        }

        private static sbyte[] _basePattern = new sbyte[] { 0, 1, 0, -1 };

        private static int CalcRepeatedFft(
            int repeats,
            int phaseCount,
            string input)
        {
            var sb = new StringBuilder();
            for (var x = 0; x < repeats; x++)
                sb.Append(input);
            return CalcFft(phaseCount, sb.ToString());
        }

        private static int CalcFft(
            int phaseCount,
            string input)
        {
            var digits = input.Select(x => (byte)(x - '0')).ToArray();

            var data = new byte[input.Length];
            var next = new byte[data.Length];

            Array.Copy(digits, 0, data, 0, digits.Length);

            for (int p = 0; p < phaseCount; p++)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    long sum = 0;

                    for (int k = i; k < digits.Length; k++)
                    {
                        var factor = _basePattern[((k + 1) / (i + 1)) % _basePattern.Length];

                        sum += data[k] * factor;
                    }

                    next[i] = (byte)(Math.Abs(sum) % 10);
                }

                var tmp = data;

                data = next;
                next = tmp;
            }

            return int.Parse(string.Join("", data.Skip(0).Take(8)));
        }







        static IEnumerable<string> TextFileLines(string fileName)
            => File.ReadLines(FILE_PATH + fileName);

        static string TextFile(string fileName)
            => File.ReadAllText(FILE_PATH + fileName);

    }
}
