using System;
using System.Linq;

namespace AoC2019
{
    public class Day02 : DayBase, IDay
    {
        private readonly long[] _programData;

        public Day02(string filename)
        {
            _programData = TextFile(filename).Split(',').Select(long.Parse).ToArray();
        }
        public Day02() : this ("Day02.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"Output for noun=12, verb=2: {Execute(12, 2)}");
            Console.WriteLine($"Command yielding 19690720: {FindInputs(19690720)}");
        }

        public long Execute(int noun, int verb)
        {
            // This was based on an older version of the Intcode machine. There were no
            // I/O opcodes - it was based on direct reads/writes into memory
            long[] memory = new long[_programData.Length];
            Array.Copy(_programData, memory, _programData.Length);
            var intcode = new Intcode(memory);
            intcode.Poke(1, noun);
            intcode.Poke(2, verb);

            // Run to termination
            intcode.Execute().LastOrDefault();
            return intcode.Peek(0);
        }

        public int FindInputs(long desiredOutput)
        {
            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    var curr = Execute(noun, verb);
                    if (curr == desiredOutput)
                        return 100 * noun + verb;
                }
            }
            throw new Exception("No match found");
        }

    }
}
