using System;
using System.Collections.Generic;

namespace AoC2019
{
    public class Day22 : DayBase, IDay
    {
        private readonly IEnumerable<string> _moves;

        public Day22(string filename)
        {
            _moves = TextFileLines(filename);
        }
        public Day22() : this("Day22.txt")
        {
        }

        public void Do()
        {
            int value = 2019;
            Console.WriteLine($"Index of {value}: {GetIndex(10_007, value)}");
            long count = 119315717514047L;
            value = 2020;
            long shuffles = 101741582076661L;
            Console.WriteLine($"Index of {value} after {shuffles}: {GetIndexNtimes(count, value, shuffles)}");
        }

        public long GetIndex(long count, long value)
            => TrackCard(count, value);

        public long GetIndexNtimes(long count, long value, long shuffles)
        {
            var seen = new HashSet<long>();
            var pos = value;
            for (var i=0L; i < shuffles; i++)
            {
                if (seen.Contains(pos))
                    throw new Exception("Wheeee!");
                seen.Add(pos);
                pos = TrackCard(count, pos);
            }
            return pos;
        }

        private long TrackCard(long count, long pos)
        {
            foreach (var move in _moves)
            {
                var command = move.Split(' ');
                switch (command[0])
                {
                    case "cut":
                        // Adding count is to make sure that the result of the mod op is a positive
                        // value.
                        pos = (count + pos - int.Parse(command[1])) % count;
                        break;
                    case "deal":
                        switch (command[1])
                        {
                            case "into":
                                pos = count - pos - 1;
                                break;
                            case "with":
                                pos = (pos * int.Parse(command[3])) % count;
                                break;
                            default:
                                throw new Exception($"Unrecognized move: {move}");
                        }
                        break;
                    default:
                        throw new Exception($"Unrecognized move: {move}");
                }
            }
            return pos;
        }

        /*
        // This was part of an earlier implementation to actual model all the cards instead of
        // tracking the card of interest.
        private List<int> Shuffle(int count)
        {
            var deck = GetDeck(count);

            foreach (var move in _moves)
            {
                var command = move.Split(' ');
                switch (command[0])
                {
                    case "cut":
                        deck = Cut(deck, int.Parse(command[1]));
                        break;
                    case "deal":
                        switch (command[1])
                        {
                            case "into":
                                deck.Reverse();
                                break;
                            case "with":
                                deck = DealWithIncrement(deck, int.Parse(command[3]));
                                break;
                            default:
                                throw new Exception($"Unrecognized move: {move}");
                        }
                        break;
                    default:
                        throw new Exception($"Unrecognized move: {move}");
                }
            }

            return deck;
        }

        private List<int> Cut(List<int> deck, int val)
        {
            if (val < 0)
                val = deck.Count + val;
            for (int i = 0; i < val; i++)
            {
                var curr = deck[0];
                deck.RemoveAt(0);
                deck.Add(curr);
            }
            return deck;
        }

        private List<int> DealWithIncrement(List<int> deck, int increment)
        {
            var count = deck.Count;
            var nd = new int[count];
            for (int i = 0; i < count; i++)
                nd[(i * increment) % count] = deck[i];
            return nd.ToList();
        }


        private List<int> GetDeck(int count)
        {
            var result = new List<int>();
            for (var x = 0; x < count; x++)
                result.Add(x);
            return result;
        }
        */
    }
}
