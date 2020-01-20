using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC2019
{
    // TODO This handles MY game.  Change this to handle anyone's game

    public class Day25 : DayBase, IDay
    {
        private readonly IReadOnlyList<string> _startMoves = new List<string>()
            {
            "north",
            "take polygon",
            "north",
            "take astrolabe",
            "south",
            "south",
            "west",
            "take hologram",
            "north",
            "east",
            "take space law space brochure",
            "west",
            "north",
            "take prime number",
            "south",
            "south",
            "east",
            "south",
            "east",
            "take weather machine",
            "west",
            "south",
            "take manifold",
            "west",
            "take mouse",
            "north",
            "north"
        };

        private readonly IReadOnlyList<string> _items = new List<string>()
        {
            "polygon",
            "astrolabe",
            "hologram",
            "space law space brochure",
            "prime number",
            "weather machine",
            "manifold",
            "mouse",
        };

        private readonly string _program;

        public Day25(string filename)
        {
            _program = TextFile(filename);
        }

        public Day25() : this("Day25.txt")
        {
        }

        public void Do()
        {
            PlayInteractive();
            Console.WriteLine($"Password: {PlayGuided()}");
        }

        public void PlayInteractive()
        {
            var comp = new AsciiCapableIntcode(_program);
            while (true)
            {
                comp.AnimateOutputNonAscii(true);
            }
        }

        public long PlayGuided()
        {
            var comp = new AsciiCapableIntcode(_program);
            // Start with scripted move to get everything
            foreach (var move in _startMoves)
                comp.InputLine(move);

            // Try all the inventory combinations
            for (var bitField = 1; bitField < 256; bitField++)
            {
                var curr = bitField;
                for (int i=0; i < 8; i++)
                {
                    var verb = curr % 2 == 0 ? "drop" : "take";
                    var line = $"{verb} {_items[i]}";
                    comp.InputLine(line);
                    curr /= 2;
                }
                comp.InputLine("east");
            }

            foreach (var line in comp.OutputLines())
            {
                if (line.Contains("You should be able to get in"))
                    return long.Parse(Regex.Match(line, @"\d+").Value);
            }
            throw new Exception("Never gets here");
        }
    }
}
