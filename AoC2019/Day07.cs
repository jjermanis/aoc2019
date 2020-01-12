using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class Day07 : DayBase, IDay
    {
        private readonly string _program;

        public Day07(string filename)
        {
            _program = TextFile(filename);
        }

        public Day07() : this("Day07.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"Highest signal: {MaxSignal()}");
            Console.WriteLine($"Highest signal with feedback: {MaxSignalWithFeedback()}");
        }

        public long MaxSignal()
        {
            var max = 0L;
            foreach (var perm in Util.Permute(0, 1, 2, 3, 4))
            {
                long result = 0;
                for (int i = 0; i < 5; i++)
                {
                    result = Execute(perm[i], result);
                }
                max = Math.Max(max, result);
            }
            return max;

            long Execute(int phase, long input)
                => (new Intcode()).Execute(_program, new long[] { phase, input }).First().Value;
        }

        public long MaxSignalWithFeedback()
        {
            var max = 0L;
            foreach (var perm in Util.Permute(5, 6, 7, 8, 9))
            {
                int intcodeCount = perm.Length;
                Intcode[] intcodes = new Intcode[intcodeCount];
                IEnumerator<long>[] icEnums = new IEnumerator<long>[intcodeCount];
                for (var x = 0; x < intcodeCount; x++)
                {
                    intcodes[x] = new Intcode(_program);
                    intcodes[x].AddInputs(perm[x]);
                    icEnums[x] = intcodes[x].Execute().GetEnumerator();
                }
                intcodes[0].AddInputs(0);

                bool isTerminated = false;
                while (!isTerminated)
                {
                    for (var x = 0; x < intcodeCount; x++)
                    {
                        var isNext = icEnums[x].MoveNext();
                        if (!isNext)
                        {
                            isTerminated = true;
                            break;
                        }
                        NextIntcode(x).AddInputs(icEnums[x].Current);
                    }
                }

                max = Math.Max(max, intcodes[4].LastOutput);

                Intcode NextIntcode(int x) => intcodes[(x + 1) % 5];
            }
            return max;

        }

    }
}
