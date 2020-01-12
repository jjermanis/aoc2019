using System;
using System.Linq;

namespace AoC2019
{
    public class Day04 : DayBase, IDay
    {
        private readonly int _min;
        private readonly int _max;

        public Day04(string filename)
        {
            var vals = TextFile(filename).Split('-').Select(x => int.Parse(x)).ToArray();
            _min = vals[0];
            _max = vals[1];
        }
        public Day04() : this("Day04.txt")
        {
        }

        public void Do()
        {
            (var part1, var part2) = DoCalc();
            Console.WriteLine($"Part 1 answer: {part1}");
            Console.WriteLine($"Part 2 answer: {part2}");
        }

        public (int, int) DoCalc()
        {
            var count1 = 0;
            var count2 = 0;
            for (var i = _min; i < _max; i++)
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
            return (count1, count2);
        }
    }
}
