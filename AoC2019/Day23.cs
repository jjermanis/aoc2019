using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2019
{
    public class Day23 : DayBase, IDay
    {
        private const int NO_OUTPUT_VAL = -1498;

        private readonly string _program;

        public Day23(string filename)
        {
            _program = TextFile(filename);
        }

        public Day23() : this("Day23.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"First Y sent to addr 255: {FirstPacketToAddress(255, 50)}");
        }

        public long FirstPacketToAddress(
            int address,
            int intcodeCount)
        {
            var intCodes = new List<NetworkIntcode>();
            var enumerators = new List<IEnumerator<long>>();
            for (var i=0; i < intcodeCount; i++)
            {
                var curr = new NetworkIntcode(_program);
                curr.AddInputs(i);
                intCodes.Add(curr);
                enumerators.Add(curr.Execute(
                    isSingleStep: true, 
                    singleStepDefaultReturn: NO_OUTPUT_VAL)
                    .GetEnumerator());
            }

            while (true)
            {
                for (int i = 0; i < intcodeCount; i++)
                {
                    var enumer = enumerators[i];
                    enumer.MoveNext();
                    var curr = (int)enumer.Current;

                    if (curr != NO_OUTPUT_VAL)
                    {
                        var x = AwaitOutputValue(enumer);
                        var y = AwaitOutputValue(enumer);
                        if (curr == address)
                            return y;
                        intCodes[curr].AddInputs(x, y);
                    }
                }
            }
        }

        private long AwaitOutputValue(IEnumerator<long> enumer)
        {
            while (true)
            {
                enumer.MoveNext();
                var result = enumer.Current;
                if (result != NO_OUTPUT_VAL)
                    return result;
            }
        }
    }
}
