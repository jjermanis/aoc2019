using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class GridSystem
    {
        private readonly RecursiveGridLevel EMPTY_LEVEL = new RecursiveGridLevel();

        public GridSystem()
        {
            System = new Dictionary<int, RecursiveGridLevel>();
        }

        public Dictionary<int, RecursiveGridLevel> System { get; set; }

        public int BugCount()
        {
            int result = 0;
            foreach (var level in System.Values)
                result += level.BugCount();
            return result;
        }

        public GridSystem RunMinute()
        {
            var result = new GridSystem();

            foreach(var level in System.Keys)
            {
                TryRun(result, level);
                TryRun(result, level+1);
                TryRun(result, level-1);
            }
            return result;
        }

        private void TryRun(
            GridSystem result,
            int level)
        {
            if (!result.System.ContainsKey(level))
            {
                var above = System.ContainsKey(level + 1) ? System[level + 1] : EMPTY_LEVEL;
                var below = System.ContainsKey(level - 1) ? System[level - 1] : EMPTY_LEVEL;
                var curr  = System.ContainsKey(level) ? System[level] : EMPTY_LEVEL;
                result.System[level] = curr.RunMinute(above, below);
            }
        }

    }

    public class RecursiveGridLevel
    {
        public RecursiveGridLevel()
        {
            Level = new Dictionary<(int, int), bool>();
        }

        public Dictionary<(int, int), bool> Level { get; set; }

        public int BugCount()
        {
            var result = 0;
            for (int x = 0; x < 5; x++)
                for (int y = 0; y < 5; y++)
                    if (Check(this, x, y)) result++;
            return result;
        }
        public RecursiveGridLevel RunMinute(
            RecursiveGridLevel above,
            RecursiveGridLevel below)
        {
            var result = new RecursiveGridLevel();

            result.Level[(0, 0)] = ResultTopLeft(above);
            result.Level[(4, 0)] = ResultTopRight(above);
            result.Level[(0, 4)] = ResultBottomLeft(above);
            result.Level[(4, 4)] = ResultBottomRight(above);

            for (int i = 1; i < 4; i++)
            {
                result.Level[(i, 0)] = ResultTop(above, i);
                result.Level[(0, i)] = ResultLeft(above, i);
                result.Level[(i, 4)] = ResultBottom(above, i);
                result.Level[(4, i)] = ResultRight(above, i);
            }

            result.Level[(1, 1)] = InnerCorner(1, 1);
            result.Level[(1, 3)] = InnerCorner(1, 3);
            result.Level[(3, 1)] = InnerCorner(3, 1);
            result.Level[(3, 3)] = InnerCorner(3, 3);

            result.Level[(2, 1)] = InnerTop(below);
            result.Level[(3, 2)] = InnerRight(below);
            result.Level[(2, 3)] = InnerBottom(below);
            result.Level[(1, 2)] = InnerLeft(below);

            return result;
        }

        private bool ResultTopLeft(RecursiveGridLevel above)
        {
            var nc = 0;
            if (Check(above, 2, 1)) nc++;
            if (Check(this, 1, 0)) nc++;
            if (Check(this, 0, 1)) nc++;
            if (Check(above, 1, 2)) nc++;
            return Result(0, 0, nc);
        }

        private bool ResultTopRight(RecursiveGridLevel above)
        {
            var nc = 0;
            if (Check(above, 2, 1)) nc++;
            if (Check(above, 3, 2)) nc++;
            if (Check(this, 4, 1)) nc++;
            if (Check(this, 3, 0)) nc++;
            return Result(4, 0, nc);
        }

        private bool ResultBottomLeft(RecursiveGridLevel above)
        {
            var nc = 0;
            if (Check(this, 0, 3)) nc++;
            if (Check(this, 1, 4)) nc++;
            if (Check(above, 2, 3)) nc++;
            if (Check(above, 1, 2)) nc++;
            return Result(0, 4, nc);
        }

        private bool ResultBottomRight(RecursiveGridLevel above)
        {
            var nc = 0;
            if (Check(this, 4, 3)) nc++;
            if (Check(above, 3, 2)) nc++;
            if (Check(above, 2, 3)) nc++;
            if (Check(this, 3, 4)) nc++;
            return Result(4, 4, nc);
        }

        private bool ResultTop(RecursiveGridLevel above, int x)
        {
            var nc = 0;
            if (Check(above, 2, 1)) nc++;
            if (Check(this, x+1, 0)) nc++;
            if (Check(this, x, 1)) nc++;
            if (Check(this, x-1, 0)) nc++;
            return Result(x, 0, nc);
        }

        private bool ResultLeft(RecursiveGridLevel above, int y)
        {
            var nc = 0;
            if (Check(this, 0, y-1)) nc++;
            if (Check(this, 1, y)) nc++;
            if (Check(this, 0, y+1)) nc++;
            if (Check(above, 1, 2)) nc++;
            return Result(0, y, nc);
        }

        private bool ResultBottom(RecursiveGridLevel above, int x)
        {
            var nc = 0;
            if (Check(this, x, 3)) nc++;
            if (Check(this, x + 1, 4)) nc++;
            if (Check(above, 2, 3)) nc++;
            if (Check(this, x - 1, 4)) nc++;
            return Result(x, 4, nc);
        }

        private bool ResultRight(RecursiveGridLevel above, int y)
        {
            var nc = 0;
            if (Check(this, 4, y-1)) nc++;
            if (Check(above, 3, 2)) nc++;
            if (Check(this, 4, y+1)) nc++;
            if (Check(this, 3, y)) nc++;
            return Result(4, y, nc);
        }

        private bool InnerCorner(int x, int y)
        {
            var nc = 0;
            if (Check(this, x, y-1)) nc++;
            if (Check(this, x+1, y)) nc++;
            if (Check(this, x, y+1)) nc++;
            if (Check(this, x-1, y)) nc++;
            return Result(x, y, nc);
        }

        private bool InnerTop(RecursiveGridLevel below)
        {
            var nc = 0;
            if (Check(this, 2, 0)) nc++;
            if (Check(this, 3, 1)) nc++;
            if (Check(this, 1, 1)) nc++;
            for (var i = 0; i < 5; i++)
                if (Check(below, i, 0)) nc++;
            return Result(2, 1, nc);
        }

        private bool InnerRight(RecursiveGridLevel below)
        {
            var nc = 0;
            if (Check(this, 3, 1)) nc++;
            if (Check(this, 4, 2)) nc++;
            if (Check(this, 3, 3)) nc++;
            for (var i = 0; i < 5; i++)
                if (Check(below, 4, i)) nc++;
            return Result(3, 2, nc);
        }

        private bool InnerBottom(RecursiveGridLevel below)
        {
            var nc = 0;
            if (Check(this, 3, 3)) nc++;
            if (Check(this, 2, 4)) nc++;
            if (Check(this, 1, 3)) nc++;
            for (var i = 0; i < 5; i++)
                if (Check(below, i, 4)) nc++;
            return Result(2, 3, nc);
        }

        private bool InnerLeft(RecursiveGridLevel below)
        {
            var nc = 0;
            if (Check(this, 1, 1)) nc++;
            if (Check(this, 1, 3)) nc++;
            if (Check(this, 0, 2)) nc++;
            for (var i = 0; i < 5; i++)
                if (Check(below, 0, i)) nc++;
            return Result(1, 2, nc);
        }

        private bool Check(RecursiveGridLevel grid, int x, int y)
            => grid.Level.ContainsKey((x, y)) && grid.Level[(x, y)];

        private bool Result(int x, int y, int neighborCount)
            => Check(this, x, y) ? (neighborCount == 1) : (neighborCount == 1 || neighborCount == 2);
    }

    public class Day24 : DayBase, IDay
    {
        private readonly IEnumerable<string> _gridData;

        public Day24(string filename)
        {
            _gridData = TextFileLines(filename);
        }
        public Day24() : this("Day24.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"First repeat biodiversity rating: {FindDupRating()}");
            Console.WriteLine($"Bug count: {RecursiveGridBugCount(200)}");
        }

        public int FindDupRating()
        {
            var scoresSeen = new HashSet<int>();
            var grid = InitGrid();
            while (true)
            {
                var rating = BiodiversityRating(grid);
                if (scoresSeen.Contains(rating))
                    return rating;
                scoresSeen.Add(rating);
                grid = RunMinute(grid);
            }
        }

        public int RecursiveGridBugCount(int minutes)
        {
            var gridSys = new GridSystem();
            gridSys.System[0] = new RecursiveGridLevel() { Level = InitGrid() };

            for (int time = 0; time < minutes; time++)
            {
                gridSys = gridSys.RunMinute();
            }
            return gridSys.BugCount();
        }

        static Dictionary<(int, int), bool> RunMinute(IDictionary<(int, int), bool> grid)
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

        private Dictionary<(int, int), bool> InitGrid()
        {
            var result = new Dictionary<(int, int), bool>();
            var lines = _gridData.ToArray();
            for (var y = 0; y < 5; y++)
                for (var x = 0; x < 5; x++)
                    result[(x, y)] = lines[y][x] == '#';
            return result;
        }

    }
}
