using System;
using System.Linq;

namespace AoC2019
{
    public class Day02 : DayBase, IDay
    {
        public void Do()
        {
            (var p1, var p2) = DoCalc();
            Console.WriteLine($"Part 1 input: {p1}");
            Console.WriteLine($"Command yielding 19690720: {p2}");
        }

        public (int, int) DoCalc()
            => DoCalc("Day02.txt");

        public (int, int) DoCalc(string filename)
        {
            var part1 = -1;
            var part2 = -1;

            // This problem does not appear to be compatible with the full Intcode
            // computer; this does not use I/O in the same way.  Using the one-off
            // specific to Day 2.
            var rawData = TextFile(filename).Split(',');

            for (int noun = 0; noun < 100; noun++)
                for (int verb = 0; verb < 100; verb++)
                {
                    var memory = rawData.Select(int.Parse).ToArray();
                    memory[1] = noun;
                    memory[2] = verb;
                    var result = IntcodeExecute(memory);

                    if (noun == 12 && verb == 2)
                        part1 = result;
                    if (result == 19690720)
                        part2 = 100 * noun + verb;
                }
            return (part1, part2);
        }

        private static int IntcodeExecute(int[] memory)
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

    }
}
