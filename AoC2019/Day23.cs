using System;
using System.Collections.Generic;

namespace AoC2019
{
    public class Day23 : DayBase, IDay
    {
        private const int NO_OUTPUT_VAL = -1498;
        private const int INTCODE_COUNT = 50;
        private const int NAT_ADDRESS = 255;
        private const int IDLE_PERIOD = 4000;

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
            (var part1, var part2) = ProcessNetwork();
            Console.WriteLine($"First Y sent to addr 255: {part1}");
            Console.WriteLine($"First Y sent twic by NAT: {part2}");
        }

        public (long, long) ProcessNetwork()
        {
            // Initialize intcodes.  Need to run in single step mode, where
            // Execute() returns after each intcode op.
            var intCodes = new List<NetworkIntcode>();
            var enumerators = new List<IEnumerator<long>>();
            for (var i=0; i < INTCODE_COUNT; i++)
            {
                var curr = new NetworkIntcode(_program);
                curr.AddInputs(i);
                intCodes.Add(curr);
                enumerators.Add(curr.Execute(
                    isSingleStep: true, 
                    singleStepDefaultReturn: NO_OUTPUT_VAL)
                    .GetEnumerator());
            }

            long? firstYToNat = null;
            var natX = -1L;
            var natY = -1L;
            var lastYSent = -1L;
            var tLastPacketSent = 0;
            // Infinite loop.  Keep the machine running, 1 op at a time. Check for 
            // network idle periods; have the NAT send to "restart" the network.
            // The arbitrary IDLE_PERIOD means this probably doesn't run on all cases.
            for (int t=0; true; t++)
            {
                for (int i = 0; i < INTCODE_COUNT; i++)
                {
                    var enumer = enumerators[i];
                    enumer.MoveNext();
                    var curr = (int)enumer.Current;

                    if (curr != NO_OUTPUT_VAL)
                    {
                        var x = AwaitOutputValue(enumer);
                        var y = AwaitOutputValue(enumer);
                        if (curr == NAT_ADDRESS)
                        {
                            natX = x;
                            natY = y;
                            if (!firstYToNat.HasValue)
                                firstYToNat = natY;
                        }
                        else
                        {
                            intCodes[curr].AddInputs(x, y);
                            tLastPacketSent = t;
                        }
                    }
                }
                if (t - IDLE_PERIOD > tLastPacketSent)
                {
                    if (natY == lastYSent)
                        return (firstYToNat.Value, lastYSent);
                    intCodes[0].AddInputs(natX, natY);
                    tLastPacketSent = t;
                    lastYSent = natY;
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
