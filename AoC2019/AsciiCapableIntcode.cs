using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2019
{
    public class AsciiCapableIntcode : Intcode
    {
        private const long NEWLINE = 10;

        public AsciiCapableIntcode(string textMemory) : base(textMemory)
        {
        }

        public AsciiCapableIntcode(long[] memory) : base(memory)
        {
        }

        public void InputLine(string input)
        {
            AddInputs(input.Select(x => (long)x).ToArray());
            AddInputs(NEWLINE);
        }

        protected override long GetInput()
        {
            if (InputQueue.Count > 0)
                return base.GetInput();
            else
            {
                var input = Console.ReadLine();
                InputLine(input);
                return GetInput();
            }
        }

        public IEnumerable<string> OutputLines()
        {
            var sb = new StringBuilder();
            foreach (var output in Execute())
            {
                if (output == NEWLINE)
                {
                    yield return sb.ToString();
                    sb = new StringBuilder();
                }
                else
                    sb.Append((char)output);
            }

        }

        public long AnimateOutputNonAscii(bool animate=true)
        {
            foreach (var output in Execute())
            {
                if (output < 127)
                {
                    if (animate)
                        Console.Write((char)output);
                }
                else
                    return output;
            }
            return 0;
        }
    }
}
