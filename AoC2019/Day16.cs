using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class Day16 : DayBase, IDay
    {
        private readonly sbyte[] _BASEPAT = new sbyte[] { 0, 1, 0, -1 };

        private readonly byte[] _digits;

        public Day16(string filename)
        {
            _digits = TextFile(filename).Select(x => (byte)(x - '0')).ToArray();
        }
        public Day16() : this("Day16.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"Start after 100 phases: {CalcFft(100)}");
        }

        public int CalcFft(int phaseCount)
        {
            var dataLen = _digits.Length;
            var data = new byte[dataLen];
            var next = new byte[dataLen];

            Array.Copy(_digits, 0, data, 0, _digits.Length);

            for (int p = 0; p < phaseCount; p++)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    long sum = 0;

                    for (int k = i; k < dataLen; k++)
                    {
                        var factor = _BASEPAT[((k + 1) / (i + 1)) % _BASEPAT.Length];

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

    }
}
