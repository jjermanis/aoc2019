using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC2019
{
    class Program
    {
        private const string FILE_PATH = @"C:\Users\john.jermanis\Documents\src\dump\AoC2019\AoC2019\";

        static void Main(string[] args)
        {
            int start = Environment.TickCount;

            Day24();

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
                    if (grid[(x,y)])
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
                    result[(x,y)] = lines[y][x] == '#';
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
                intcode.AddInput(10);
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

        private static byte[] _cache;

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

        public class Compound
        {
            public Compound() { }
            public Compound(string desc)
            {
                var vals = desc.Trim().Split(" ");
                Chemical = vals[1];
                Units = long.Parse(vals[0]);
            }
            public string Chemical { get; set; }
            public long Units { get; set; }
            public override string ToString()
                => $"{Units} {Chemical}";
        }
        public class Reaction
        {
            public Reaction(string desc)
            {
                var inOut = desc.Split("=>");
                var inputs = inOut[0].Split(',');
                Inputs = new List<Compound>();
                foreach (var input in inputs)
                    Inputs.Add(new Compound(input));
                Output = new Compound(inOut[1]);
            }
            public Compound Output { get; set; }
            public List<Compound> Inputs { get; set; }
        }

        static List<int> directions = new List<int> { 1, 4, 2, 3 };
        static List<int> dx         = new List<int> { 0, 1, 0, -1 };
        static List<int> dy         = new List<int> { 1, 0, -1, 0 };

        static void Day15()
        {
            Console.WriteLine($"Steps to oxygen: {TraverseMaze(true)}");
            Console.WriteLine($"Time to oxygenate: {TraverseMaze(false)}");
        }

        static int TraverseMaze(bool justFindO)
        {
            var intcode = new Intcode(TextFile("Day15.txt"));

            var CHARS = new char[] { '#', '.', 'O' };
            var x = 0;
            var y = 0;
            var dirIndex = 0;
            var map = new Dictionary<(int, int), char>();
            var distances = new Dictionary<(int, int), int>();
            var currDistance = 0;
            map[(0, 0)] = '.';
            distances[(0, 0)] = 0;
            int count = 0;
            bool lookingForO = true;
            var maxDistance = 0;
            using (var enumer = intcode.Execute().GetEnumerator())
            {
                while (count < 10000)
                {
                    // Option 1: Can the robot turn right?
                    var output = Move(dirIndex + 1);
                    map[Loc(dirIndex + 1)] = CHARS[output];
                    if (output != 0)
                    {
                        // You can turn right
                        (x, y) = Loc(dirIndex + 1);
                        dirIndex = (dirIndex + 1) % 4;
                    }
                    else
                    {
                        // Option 2: Can the robot move straight?
                        output = Move(dirIndex);
                        map[Loc(dirIndex)] = CHARS[output];
                        if (output != 0)
                        {
                            // You can move forward
                            (x, y) = Loc(dirIndex);
                        }
                        else
                        {
                            // Option 3: Can you turn left?
                            output = Move(dirIndex + 3);
                            map[Loc(dirIndex + 3)] = CHARS[output];
                            if (output != 0)
                            {
                                // You can turn left
                                (x, y) = Loc(dirIndex + 3);
                                dirIndex = (dirIndex + 3) % 4;
                            }
                            else
                            {
                                // Option 4: Backtrack
                                output = Move(dirIndex + 2);
                                map[Loc(dirIndex + 2)] = CHARS[output];
                                if (output != 0)
                                {
                                    // You can turn left
                                    (x, y) = Loc(dirIndex + 2);
                                    dirIndex = (dirIndex + 2) % 4;
                                }
                                else
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
                            if (justFindO)
                            {
                                return currDistance;
                            }
                            else
                            {
                                lookingForO = false;
                                distances = new Dictionary<(int, int), int>();
                                currDistance = 0;
                            }
                        }
                    }
                    else
                    {
                        maxDistance = Math.Max(maxDistance, currDistance);
                    }
                       
                }

                int Dir(int index) => directions[index % 4];
                (int, int) Loc(int di) => (x + dx[di % 4], y + dy[di % 4]);
                int Move(int di)
                {
                    intcode.AddInput(Dir(di));
                    enumer.MoveNext();
                    return (int)enumer.Current;
                }
    
            }

            return maxDistance;
            //Console.Clear();
            //DrawMaze(map);

        }

        static void DrawMaze(Dictionary<(int, int), char> map) 
        {
            for (int yy = 22; yy > -22; yy--)
            {
                for (int xx = -25; xx < 25; xx++)
                {
                    if (xx == 0 && yy == 0)
                        Console.Write('S');
                    else if (map.ContainsKey((xx, yy)))
                        Console.Write(map[(xx, yy)]);
                    else
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        static void Day14()
        {
            Do("Day14Test1.txt", 31);
            Do("Day14Test2.txt", 165);
            Do("Day14Test3.txt", 13312);
            Do("Day14Test4.txt", 180697);
            Do("Day14Test5.txt", 2210736);
            Do("Day14.txt", 1920219);
            void Do(string fileName, int expected)
                => Console.WriteLine($"Case {fileName}. Result={OreNeeded(fileName)}. Expected={expected}");

            var oot = OreNeeded("Day14Test4.txt", 5586022);
            Do2("Day14Test3.txt", 82892753);
            Do2("Day14Test4.txt", 5586022);
            Do2("Day14Test5.txt", 460664);
            Do2("Day14.txt", 1330066);
            void Do2(string fileName, int expected)
                => Console.WriteLine($"Case {fileName}. Result={MaxForTrillion(fileName)}. Expected={expected}");

        }

        static long MaxForTrillion(
            string filename)
        {
            long min = 1;
            long max = 1_000_000_000;

            while (max - min > 1)
            {
                var curr = (min + max) / 2;
                var ore = OreNeeded(filename, curr);
                if (ore > 1_000_000_000_000)
                    max = curr - 1;
                else
                    min = curr;
            }
            return (OreNeeded(filename, max) <= 1_000_000_000_000) ? max : min;
        }

        static long OreNeeded(
            string filename,
            long fuelNeeded = 1)
        {
            var reactions = LoadReactions(filename);
            var demands = new Queue<Compound>();
            demands.Enqueue(new Compound() { Chemical = "FUEL", Units = fuelNeeded });
            var bank = new Dictionary<string, long>();
            var result = 0L;
            while (demands.Count > 0)
            {
                var curr = demands.Dequeue();
                if (curr.Chemical.Equals("ORE"))
                {
                    result += curr.Units;
                }
                else
                {
                    // Check the bank - we might already haev everything that's needed
                    var quantityNeeded = curr.Units - WithdrawlFromBank(bank, curr.Chemical, curr.Units);

                    if (quantityNeeded > 0)
                    {
                        // Find the reaction that produces the needed chemical
                        var reaction = reactions.Where(r => r.Output.Chemical.Equals(curr.Chemical)).First();

                        long reactionsNeeded = (quantityNeeded - 1) / reaction.Output.Units + 1;
                        long unitsProduced = reactionsNeeded * reaction.Output.Units;
                        if (unitsProduced > quantityNeeded)
                            AddToBank(bank, curr.Chemical, unitsProduced - quantityNeeded);

                        // Add demands for all input chemicals in the reaction
                        foreach (var input in reaction.Inputs)
                        {
                            var newDemand = new Compound() { Chemical = input.Chemical, Units = reactionsNeeded * input.Units };
                            demands.Enqueue(newDemand);
                        }
                    }
                }
            }
            return result;
        }

        static void AddToBank(
            Dictionary<string, long> bank, 
            string chemical,
            long count)
        {
            if (bank.ContainsKey(chemical))
                bank[chemical] += count;
            else
                bank[chemical] = count;
        }
        static long WithdrawlFromBank(
            Dictionary<string, long> bank,
            string chemical,
            long amount)
        {
            if (bank.ContainsKey(chemical))
            {
                var result = Math.Min(amount, bank[chemical]);
                bank[chemical] -= result;
                return result;
            }
            else
                return 0;
        }

        static List<Reaction> LoadReactions(string filename)
        {
            var result = new List<Reaction>();
            foreach (var line in TextFileLines(filename))
                result.Add(new Reaction(line));
            return result;
        }

        static void Day13()
        {
            Console.WriteLine($"Block tile count: {Day13Part1()}");
            Console.WriteLine($"Final score: {Day13Part2()}");
        }

        static long Day13Part2()
        {
            var intcode = new Intcode(TextFile("Day13.txt"));
            // Instructions state to put 2 at mem addr 0 for free play
            intcode.Poke(0, 2);
            var screen = new Dictionary<(long, long), long>();

            var ballX = 0L;
            var paddleX = 0L;
            var score = 0L;
            using (var enumer = intcode.Execute().GetEnumerator())
            {
                while (enumer.MoveNext())
                {
                    // Read x, y, and tile from output
                    var x = enumer.Current;
                    enumer.MoveNext();
                    var y = enumer.Current;
                    enumer.MoveNext();
                    var tile = enumer.Current;

                    if (x == -1 && y == 0)
                    {
                        // Special case for score
                        score = tile;
                    }
                    else
                    {
                        // Check if this is paddle or ball
                        screen[(x, y)] = tile;

                        if (tile == 4)
                            ballX = x;
                        if (tile == 3)
                            paddleX = x;
                    }

                    // Update input to match known game state
                    long dx;
                    if (ballX > paddleX)
                        dx = 1;
                    else if (ballX < paddleX)
                        dx = -1;
                    else
                        dx = 0;
                    intcode.UpdateInput(dx);

                }
            }
            return score;
        }

        static int TileCount(Dictionary<(long, long), long> screen, long tile)
        {
            var result = 0;
            foreach (var pixel in screen.Values)
                if (pixel == tile)
                    result++;
            return result;
        }

        static int Day13Part1()
        {
            var intcode = new Intcode(TextFile("Day13.txt"));
            var screen = new Dictionary<(long, long), long>();

            using (var enumer = intcode.Execute().GetEnumerator())
            {
                while (enumer.MoveNext())
                {
                    var x = enumer.Current;
                    enumer.MoveNext();
                    var y = enumer.Current;
                    enumer.MoveNext();
                    var tile = enumer.Current;

                    screen[(x, y)] = tile;
                }
                return TileCount(screen, 2);
            }
        }

        public class Loc12
        {
            public Loc12() : this(0, 0, 0) { }

            public Loc12(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public override string ToString()
                => $"{X}, {Y}, {Z}";
        }

        public static Loc12 AddLoc(Loc12 a, Loc12 b)
            => new Loc12(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        static void Day12()
        {
            Do("Day12Test1.txt", 10, 179);
            Do("Day12Test2.txt", 100, 1940);
            Do("Day12.txt", 1000, 12490);
            void Do(string fileName, int count, int expected)
                => Console.WriteLine($"Case {fileName}. Result={CalcEnergy(fileName, count)}. Expected={expected}");

            Do2("Day12Test1.txt", 2772);
            Do2("Day12Test2.txt", 4686774924);
            Do2("Day12.txt", 392733896255168);
            void Do2(string fileName, long expected)
                => Console.WriteLine($"Case {fileName}. Result={CyclePeriod(fileName)}. Expected={expected}");
        }

        static long CyclePeriod(string fileName)
        {
            var cycles = new List<long>
            {
                CyclePeriod(fileName, o => o.X),
                CyclePeriod(fileName, o => o.Y),
                CyclePeriod(fileName, o => o.Z),
            };

            return Util.LeastCommonMultiple(cycles);
        }
        
        static long CyclePeriod(
            string fileName,
            Func<Loc12, int> Coord)
        {
            var startMoons = LoadMoons(fileName);
            var currMoons = LoadMoons(fileName);
            int count = 0;
            foreach (var step in AsteroidMovement(currMoons))
            {
                count++;
                var isCycle = true;
                var locs = step.Item1;
                var velos = step.Item2;
                foreach (var velo in velos)
                    if (Coord(velo) != 0)
                        isCycle = false;
                for (int x = 0; x < locs.Count; x++)
                    if (Coord(locs[x]) != Coord(startMoons[x]))
                        isCycle = false;
                if (isCycle)
                    return count;
            }
            throw new Exception("Never gets here");
        }

        static List<Loc12> LoadMoons(string filename)
        {
            var moons = new List<Loc12>();
            foreach (var moon in TextFileLines(filename))
            {
                moons.Add(ParseLoc(moon));
            }
            return moons;
        }

        static int CalcEnergy(string filename, int stepCount)
        {
            var moons = LoadMoons(filename);
            var (locs, velos) = AsteroidMovement(moons, stepCount);
            var result = CalcEnergy(locs, velos);
            return result;
        }

        static int CalcEnergy(List<Loc12> moons, List<Loc12> velocities)
        {
            int result = 0;

            for (int i=0; i<moons.Count; i++)
            {
                int pot = AvSum(moons[i]);
                int kin = AvSum(velocities[i]);
                result += pot * kin;
            }
            return result;

            int AvSum(Loc12 a) => Math.Abs(a.X) + Math.Abs(a.Y) + Math.Abs(a.Z);
        }

        static (List<Loc12>, List<Loc12>) AsteroidMovement(
            List<Loc12> locs,
            int stepCount)
        {
            (List<Loc12>, List<Loc12>) result = (null, null);
            int count = 0;
            foreach (var step in AsteroidMovement(locs))
            {
                result = step;
                if (++count == stepCount)
                    break;
            }
            return result;
        }

        static IEnumerable<(List<Loc12>, List<Loc12>)> AsteroidMovement(List<Loc12> locs)
        {
            var velocities = new List<Loc12>();
            for (int x = 0; x < locs.Count; x++)
                velocities.Add(new Loc12());

            while (true)
            {
                velocities = Gravity(locs, velocities);
                for (int i = 0; i < locs.Count; i++)
                {
                    locs[i] = AddLoc(locs[i], velocities[i]);
                }
                yield return (locs, velocities);
            }
        }

        static Loc12 ParseLoc(string raw)
        {
            raw = raw.Substring(1, raw.Length - 2);
            var vals = raw.Split(", ");
            return new Loc12(GetNum(0), GetNum(1), GetNum(2));

            int GetNum(int index) => int.Parse(vals[index].Substring(2));
        }

        static List<Loc12> Gravity(List<Loc12> moons, List<Loc12> velocities)
        {
            for (int a = 0; a < moons.Count - 1; a++)
            {
                var moonA = moons[a];
                for (int b = a + 1; b < moons.Count; b++)
                {
                    var moonB = moons[b];

                    if (moonA.X > moonB.X)
                    {
                        velocities[a].X--;
                        velocities[b].X++;
                    }
                    else if (moonA.X < moonB.X)
                    {
                        velocities[a].X++;
                        velocities[b].X--;
                    }
                    if (moonA.Y > moonB.Y)
                    {
                        velocities[a].Y--;
                        velocities[b].Y++;
                    }
                    else if (moonA.Y < moonB.Y)
                    {
                        velocities[a].Y++;
                        velocities[b].Y--;
                    }
                    if (moonA.Z > moonB.Z)
                    {
                        velocities[a].Z--;
                        velocities[b].Z++;
                    }
                    else if (moonA.Z < moonB.Z)
                    {
                        velocities[a].Z++;
                        velocities[b].Z--;
                    }
                }
            }
            return velocities;
        }

        enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        static void Day11()
        {
            var painting = GetPainting(0);
            Console.WriteLine($"Spaces painted: {painting.Count}");
            painting = GetPainting(1);
            PrintPainting(painting);
        }
        static void PrintPainting(IDictionary<(int, int), long> painting)
        {
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
            for (var y = maxY; y >= minY; y--)
            {
                Console.WriteLine();
                for (var x = minX; x <= maxX; x++)
                {
                    if (painting.ContainsKey((x, y)) && (painting[(x, y)] == 1))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

            }
        }

        static IDictionary<(int, int), long> GetPainting(int startColor)
        {
            var intcode = new Intcode(TextFile("Day11.txt"));
            var painting = new Dictionary<(int, int), long>();
            painting[(0, 0)] = startColor;
            intcode.AddInput(startColor);

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
                        intcode.AddInput(painting[(x, y)]);
                    else
                        intcode.AddInput(0);
                }
            }
            return painting;
        }

        static void Day10()
        {
            var best = MostVisibleAsteroids();
            Console.WriteLine($"Most visible asteroids: {best.Item2.Count}");
            var nth = 200;
            var code = LocationCodeForAsteroid(best.Item1, best.Item2, nth);
            Console.WriteLine($"Location for asteroid #{nth} to be destroyed: {code}");
        }

        static ((int, int), HashSet<(int, int)>) MostVisibleAsteroids()
        {
            var locs = AsteroidLocations("Day10.txt");

            var result = ((-1, -1), new HashSet<(int, int)>());
            foreach (var asteroid in locs)
            {
                var currVis = new HashSet<(int, int)>();
                foreach (var target in locs)
                {
                    if (asteroid == target)
                        continue;

                    var dx = target.Item1 - asteroid.Item1;
                    var dy = target.Item2 - asteroid.Item2;

                    (var stepX, var stepY) = GetDelta(dx, dy);

                    var currLoc = asteroid;
                    var isBlocked = false;
                    while (currLoc != target)
                    {
                        currLoc.Item1 += stepX;
                        currLoc.Item2 += stepY;
                        if (locs.Contains(currLoc) && currLoc != target)
                            isBlocked = true;
                    }
                    if (!isBlocked)
                        currVis.Add(target);
                }
                if (currVis.Count > result.Item2.Count)
                    result = (asteroid, currVis);
            }
            return result;

        }

        class LocRes
        {
            public double Angle { get; set; }
            public int LocCode { get; set; }
        }

        static int LocationCodeForAsteroid(
            (int, int) location,
            HashSet<(int, int)> asteroids,
            int destroyCount
            )
        {
            var roids = new List<LocRes>();

            foreach (var asteroid in asteroids)
            {
                // This looks weird because we don't want "normal" trig angles.  0 degrees is straight up. 
                // Rotation proceeds clockwise.
                var angle = Math.Atan2(
                    asteroid.Item1 - location.Item1,
                    location.Item2 - asteroid.Item2
                    );
                if (angle < 0)
                    angle += Math.PI * 2;
                roids.Add(new LocRes { Angle = angle, LocCode = asteroid.Item1 * 100 + asteroid.Item2 });
            }
            roids = roids.OrderBy(o => o.Angle).ToList();
            return roids[destroyCount - 1].LocCode;
        }

        static (int, int) GetDelta(int x, int y)
        {
            if (x == 0)
                return (0, Sign(y));
            if (y == 0)
                return (Sign(x), 0);

            int signX = Sign(x);
            int signY = Sign(y);
            (var rx, var ry) = Util.ReduceCommonFactors(Math.Abs(x), Math.Abs(y));
            return (rx * signX, ry * signY);
            int Sign(int v) => v / Math.Abs(v);
        }

        static HashSet<(int, int)> AsteroidLocations(string fileName)
        {
            var result = new HashSet<(int, int)>();

            var y = 0;
            foreach (var line in TextFileLines(fileName))
            {
                for (int x = 0; x < line.Length; x++)
                    if (line[x] == '#')
                        result.Add((x, y));
                y++;
            }
            return result;
        }

        static void Day9()
        {
            IntcodeTest();
            Console.WriteLine("Regression tests passed");

            var part1 = (new Intcode()).Execute(TextFile("Day9.txt"), new long[] { 1 }, false).First();
            Console.WriteLine($"Part 1: {part1}");
            var part2 = (new Intcode()).Execute(TextFile("Day9.txt"), new long[] { 2 }, false).First();
            Console.WriteLine($"Part 2: {part2}");
        }

        static void IntcodeTest()
        {
            if (MaxSignal() != 19650)
                throw new Exception();
            if (ExecuteDay5(1) != 13210611)
                throw new Exception();
            if (ExecuteDay5(5) != 584126)
                throw new Exception();

            long? ExecuteDay5(long input) => (new Intcode()).Execute(TextFile("Day5.txt"), new long[] { input }).Last();
        }

        static void Day8()
        {
            var image = TextFile("Day8.txt");
            var width = 25;
            var height = 6;
            var size = width * height;

            var part1 = Day8Part1(image, size);
            Console.WriteLine($"Part 1: {part1}");
            Day8Part2(image, width, height);
        }

        static void Day8Part2(string image, int width, int height)
        {
            var size = width * height;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var layer = 0;
                    while (Curr() == '2')
                        layer++;
                    Console.Write(Curr() == '0' ? ' ' : '#');

                    char Curr() => image[layer * size + width * y + x];
                }
                Console.WriteLine();
            }      
        }

        static int Day8Part1(string image, int size)
        {
            var layers = image.Length / size;
            var min = int.MaxValue;
            IDictionary<int, int> minProfile = null;
            for (int layer=0; layer < layers; layer++)
            {
                var curr = GetLayerProfile(image, layer, size);
                if (curr[0] < min)
                {
                    min = curr[0];
                    minProfile = curr;
                }
            }
            return minProfile[1] * minProfile[2];
        }

        static IDictionary<int, int> GetLayerProfile(
            string image,
            int layer,
            int size)
        {
            var result = new Dictionary<int, int>();
            for (var x = 0; x < 10; x++)
                result[x] = 0;
            for (int x = layer * size; x < (layer + 1) * size; x++)
            {
                result[image[x] - '0']++;
            }
            return result;
        }

        static void Day7()
        {
            Console.WriteLine(MaxSignal());
            Console.WriteLine(MaxSignalPart2());
        }

        static long MaxSignalPart2()
        {
            var max = 0L;
            var programText = TextFile("Day7.txt");

            foreach (var perm in Util.Permute(5, 6, 7, 8, 9))
            {
                int intcodeCount = perm.Length;
                Intcode[] intcodes = new Intcode[intcodeCount];
                IEnumerator<long>[] icEnums = new IEnumerator<long>[intcodeCount];
                for (var x = 0; x < intcodeCount; x++)
                {
                    intcodes[x] = new Intcode(programText);
                    intcodes[x].AddInput(perm[x]);
                    icEnums[x] = intcodes[x].Execute().GetEnumerator();
                }
                intcodes[0].AddInput(0);

                bool isTerminated = false;
                while (!isTerminated)
                {
                    for (var x = 0; x <intcodeCount; x++)
                    {
                        var isNext = icEnums[x].MoveNext();
                        if (!isNext)
                        {
                            isTerminated = true;
                            break;
                        }
                        NextIntcode(x).AddInput(icEnums[x].Current);
                    }
                }

                max = Math.Max(max, intcodes[4].LastOutput);

                Intcode NextIntcode(int x) => intcodes[(x + 1) % 5];
            }
            return max;

        }

        static long MaxSignal()
        {
            var max = 0L;
            foreach (var perm in Util.Permute(0, 1, 2, 3, 4))
            {
                long result = 0;
                for (int i=0; i < 5; i++)
                {
                    result = Execute(perm[i], result);
                }
                max = Math.Max(max, result);
            }
            return max;

            long Execute(int phase, long input)
                => (new Intcode()).Execute(TextFile("Day7.txt"), new long[] { phase, input }).First().Value;
        }

        static void Day6()
        {
            var graph = new Dictionary<string, string>();
            foreach(var line in TextFileLines("Day6.txt"))
            {
                string inner = line.Substring(0, 3);
                string outer = line.Substring(4, 3);

                graph.Add(outer, inner);
            }

            string curr;
            var totalCount = 0;
            foreach (var body in graph.Keys)
            {
                curr = body;
                while (!curr.Equals("COM"))
                {
                    totalCount++;
                    curr = graph[curr];
                }
            }

            var sanOrbits = new List<string>();
            curr = "SAN";
            while (!curr.Equals("COM"))
            {
                curr = graph[curr];
                sanOrbits.Add(curr);
            }
            curr = "YOU";
            int transferCount = 0;
            while (!curr.Equals("COM"))
            {
                curr = graph[curr];
                transferCount++;
                if (sanOrbits.Contains(curr))
                {
                    transferCount += sanOrbits.IndexOf(curr) - 1;
                    break;
                }
            }

            Console.WriteLine($"Orbit count: {totalCount}");
            Console.WriteLine($"YOU->SAN transfers: {transferCount}");
        }

        static void Day5()
        {
            Console.WriteLine($"Part 1: {Execute(1)}");
            Console.WriteLine($"Part 2: {Execute(5)}");

            long? Execute(int input) => (new Intcode()).Execute(TextFile("Day5.txt"), new long[] { input }).First();
        }

        static void Day4()
        {
            var start = 130254;
            var end = 678275;

            var count1 = 0;
            var count2 = 0;
            for (var i =start; i < end; i++)
            {
                var val = i.ToString();

                var hasDouble = false;
                var hasDoubleExact = false;
                var isAscending = true;
                for (int c = 0; c < 5; c++)
                {
                    if (val[c] == val[c + 1])
                        hasDouble = true;
                    if (val[c] > val[c + 1])
                        isAscending = false;
                }
                var digits = new int[10];
                var temp = i;
                for (int c = 0; c < 6; c++)
                {
                    digits[temp % 10]++;
                    temp /= 10;
                }
                foreach (var dc in digits)
                    if (dc == 2)
                        hasDoubleExact = true;

                if (hasDouble && isAscending)
                    count1++;
                if (hasDoubleExact && isAscending)
                    count2++;
            }
            Console.WriteLine($"Part 1 count: {count1}");
            Console.WriteLine($"Part 2 count: {count2}");
        }

        static void Day3()
        {
            var data = TextFileLines("Day3.txt").ToArray();
            Console.WriteLine(NearestManhattanIntersectDist(data[0], data[1]));
            Console.WriteLine(NearestWireIntersectDist(data[0], data[1]));
        }

        static int NearestManhattanIntersectDist(string w1, string w2)
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

        static int NearestWireIntersectDist(string w1, string w2)
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

        struct Move
        {
            public char Direction { get; set; }
            public int Distance { get; set; }
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

        static IEnumerable<Move> Moves(string steps)
        {
            var moves = steps.Split(',');
            foreach (var move in moves)
                yield return new Move
                {
                    Direction = move[0],
                    Distance = int.Parse(move.Substring(1))
                };
        }

        static IDictionary<GridLoc, int> BuildGrid(string steps)
        {
            var curr = new GridLoc();
            var result = new Dictionary<GridLoc, int>();
            int moves = 0;
            result.Add(curr, moves++);
            foreach (var move in Moves(steps))
            {
                for (int i=0; i<move.Distance; i++)
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

        static void Day1()
        {
            var total = 0;
            foreach (var massText in TextFileLines("Day1.txt"))
            {
                //total += Day1SimpleFuel(int.Parse(massText));
                total += Day1ComplexFuel(int.Parse(massText));
            }
            Console.WriteLine(total);
        }

        static int Day1ComplexFuel(int mass)
        {
            var simpleFuel = Day1SimpleFuel(mass);
            return simpleFuel > 0 ? simpleFuel + Day1ComplexFuel(simpleFuel) : 0;
        }

        static int Day1SimpleFuel(int mass)
            => mass / 3 - 2;

        static void Day2()
        {
            var rawData = TextFile("Day2.txt").Split(',');

            for (int noun = 0; noun < 100; noun++)
                for (int verb = 0; verb < 100; verb++)
                {
                    var memory = rawData.Select(int.Parse).ToArray();
                    memory[1] = noun;
                    memory[2] = verb;
                    var result = IntcodeExecute(memory);
                    if (result == 19690720)
                        Console.WriteLine(100 * noun + verb);
                }
        }

        static int IntcodeExecute(int[] memory)
        {
            int pc = 0;
            while (true)
            {
                int opCode = Read(pc);
                switch (opCode)
                {
                    case 1:
                        memory[Read(pc + 3)] = ReadAddr(pc + 1) + ReadAddr(pc + 2);
                        pc += 4;
                        break;
                    case 2:
                        memory[Read(pc + 3)] = ReadAddr(pc + 1) * ReadAddr(pc + 2);
                        pc += 4;
                        break;
                    case 99:
                        return Read(0);
                }
            }

            int Read(int x) => memory[x];
            int ReadAddr(int x) => Read(Read(x));

        }


        static IEnumerable<string> TextFileLines(string fileName)
            => File.ReadLines(FILE_PATH + fileName);

        static string TextFile(string fileName)
            => File.ReadAllText(FILE_PATH + fileName);

    }
}
