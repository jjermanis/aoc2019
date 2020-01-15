using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
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

        private IDictionary<(int, int), bool> InitGrid()
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
