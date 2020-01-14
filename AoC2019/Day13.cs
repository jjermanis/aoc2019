using System;
using System.Collections.Generic;

namespace AoC2019
{
    public class Day13 : DayBase, IDay
    {
        private readonly string _program;

        public Day13(string filename)
        {
            _program = TextFile(filename);
        }

        public Day13() : this("Day13.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"Block count: {BlockCountExit()}");
            Console.WriteLine($"Score: {PlayGame()}");
        }

        public int BlockCountExit()
        {
            var intcode = new Intcode(_program);
            var screen = new Dictionary<(long, long), long>();

            using (var enumer = intcode.Execute().GetEnumerator())
            {
                while (enumer.MoveNext())
                {
                    var x = enumer.Current;
                    enumer.MoveNext();
                    var y = enumer.Current;
                    enumer.MoveNext();
                    var tile = enumer.Current;

                    screen[(x, y)] = tile;
                }
                return TileCount(screen, 2);
            }
        }

        public long PlayGame()
        {
            var intcode = new Intcode(_program);
            // Instructions state to put 2 at mem addr 0 for free play
            intcode.Poke(0, 2);
            var screen = new Dictionary<(long, long), long>();

            var ballX = 0L;
            var paddleX = 0L;
            var score = 0L;
            using (var enumer = intcode.Execute().GetEnumerator())
            {
                while (enumer.MoveNext())
                {
                    // Read x, y, and tile from output
                    var x = enumer.Current;
                    enumer.MoveNext();
                    var y = enumer.Current;
                    enumer.MoveNext();
                    var tile = enumer.Current;

                    if (x == -1 && y == 0)
                    {
                        // Special case for score
                        score = tile;
                    }
                    else
                    {
                        // Check if this is paddle or ball
                        screen[(x, y)] = tile;

                        if (tile == 4)
                            ballX = x;
                        if (tile == 3)
                            paddleX = x;
                    }

                    // Update input to match known game state
                    long dx;
                    if (ballX > paddleX)
                        dx = 1;
                    else if (ballX < paddleX)
                        dx = -1;
                    else
                        dx = 0;
                    intcode.UpdateInput(dx);

                }
            }
            return score;
        }

        private int TileCount(Dictionary<(long, long), long> screen, long tile)
        {
            var result = 0;
            foreach (var pixel in screen.Values)
                if (pixel == tile)
                    result++;
            return result;
        }
    }
}
