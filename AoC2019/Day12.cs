using System;
using System.Collections.Generic;

namespace AoC2019
{
    public class Day12 : DayBase, IDay
    {
        private readonly IEnumerable<string> _moonStates;

        public Day12(string filename)
        {
            _moonStates = TextFileLines(filename);
        }

        public Day12() : this("Day12.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"Total energy after 1000 steps: {CalcEnergy(1000)}");
            Console.WriteLine($"Duplicate position seen in {CyclePeriod()} moves");
        }

        public int CalcEnergy(int stepCount)
        {
            var moons = LoadMoons();
            var (locs, velos) = AsteroidMovement(moons, stepCount);
            var result = CalcEnergy(locs, velos);
            return result;
        }

        public long CyclePeriod()
        {
            // Each coordinate is independent. Calculate the cycle period of each,
            // take the LCM of those values.
            var cycles = new List<long>
            {
                CyclePeriod(o => o.X),
                CyclePeriod(o => o.Y),
                CyclePeriod(o => o.Z),
            };

            return Util.LeastCommonMultiple(cycles);
        }

        private long CyclePeriod(Func<Loc12, int> Coord)
        {
            var startMoons = LoadMoons();
            var currMoons = LoadMoons();
            int count = 0;
            foreach (var step in AsteroidMovement(currMoons))
            {
                count++;
                var isCycle = true;
                var locs = step.Item1;
                var velos = step.Item2;
                foreach (var velo in velos)
                    if (Coord(velo) != 0)
                        isCycle = false;
                for (int x = 0; x < locs.Count; x++)
                    if (Coord(locs[x]) != Coord(startMoons[x]))
                        isCycle = false;
                if (isCycle)
                    return count;
            }
            throw new Exception("Never gets here");
        }

        private List<Loc12> LoadMoons()
        {
            var moons = new List<Loc12>();
            foreach (var moon in _moonStates)
            {
                moons.Add(ParseLoc(moon));
            }
            return moons;
        }

        private int CalcEnergy(List<Loc12> moons, List<Loc12> velocities)
        {
            int result = 0;

            for (int i = 0; i < moons.Count; i++)
            {
                int pot = AvSum(moons[i]);
                int kin = AvSum(velocities[i]);
                result += pot * kin;
            }
            return result;

            int AvSum(Loc12 a) => Math.Abs(a.X) + Math.Abs(a.Y) + Math.Abs(a.Z);
        }

        private (List<Loc12>, List<Loc12>) AsteroidMovement(
            List<Loc12> locs,
            int stepCount)
        {
            (List<Loc12>, List<Loc12>) result = (null, null);
            int count = 0;
            foreach (var step in AsteroidMovement(locs))
            {
                result = step;
                if (++count == stepCount)
                    break;
            }
            return result;
        }

        private IEnumerable<(List<Loc12>, List<Loc12>)> AsteroidMovement(List<Loc12> locs)
        {
            var velocities = new List<Loc12>();
            for (int x = 0; x < locs.Count; x++)
                velocities.Add(new Loc12());

            while (true)
            {
                velocities = Gravity(locs, velocities);
                for (int i = 0; i < locs.Count; i++)
                {
                    locs[i] = AddLoc(locs[i], velocities[i]);
                }
                yield return (locs, velocities);
            }
        }

        private Loc12 ParseLoc(string raw)
        {
            raw = raw.Substring(1, raw.Length - 2);
            var vals = raw.Split(", ");
            return new Loc12(GetNum(0), GetNum(1), GetNum(2));

            int GetNum(int index) => int.Parse(vals[index].Substring(2));
        }

        private List<Loc12> Gravity(List<Loc12> moons, List<Loc12> velocities)
        {
            for (int a = 0; a < moons.Count - 1; a++)
            {
                var moonA = moons[a];
                for (int b = a + 1; b < moons.Count; b++)
                {
                    var moonB = moons[b];

                    if (moonA.X > moonB.X)
                    {
                        velocities[a].X--;
                        velocities[b].X++;
                    }
                    else if (moonA.X < moonB.X)
                    {
                        velocities[a].X++;
                        velocities[b].X--;
                    }
                    if (moonA.Y > moonB.Y)
                    {
                        velocities[a].Y--;
                        velocities[b].Y++;
                    }
                    else if (moonA.Y < moonB.Y)
                    {
                        velocities[a].Y++;
                        velocities[b].Y--;
                    }
                    if (moonA.Z > moonB.Z)
                    {
                        velocities[a].Z--;
                        velocities[b].Z++;
                    }
                    else if (moonA.Z < moonB.Z)
                    {
                        velocities[a].Z++;
                        velocities[b].Z--;
                    }
                }
            }
            return velocities;
        }
        private Loc12 AddLoc(Loc12 a, Loc12 b)
            => new Loc12(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        class Loc12
        {
            public Loc12() : this(0, 0, 0) { }

            public Loc12(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public override string ToString()
                => $"{X}, {Y}, {Z}";
        }
    }
}
