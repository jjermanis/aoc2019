using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class AsteroidLoc
    {
        public int X { get; set; }
        public int Y { get; set; }
        public HashSet<(int, int)> VisibleAsteroids { get; set; }
    }

    public class Day10 : DayBase, IDay
    {
        private readonly HashSet<(int, int)> _asteroids;

        public Day10(string filename)
        {
            _asteroids = AsteroidLocations(filename);
        }
        public Day10() : this("Day10.txt")
        {
        }

        public void Do()
        {
            var best = MostVisibleAsteroids();
            Console.WriteLine($"Most visible asteroids: {best.VisibleAsteroids.Count}");
            var destroyCount = 200;
            var asteroidCode = LocationCodeForAsteroid((best.X, best.Y), best.VisibleAsteroids, destroyCount);
            Console.WriteLine($"Location code for asteroid #{destroyCount}: {asteroidCode}");
        }

        public AsteroidLoc MostVisibleAsteroids()
        {
            var result = new AsteroidLoc() { VisibleAsteroids = new HashSet<(int, int)>() };
            foreach (var asteroid in _asteroids)
            {
                var currVis = new HashSet<(int, int)>();
                foreach (var target in _asteroids)
                {
                    if (asteroid == target)
                        continue;

                    var dx = target.Item1 - asteroid.Item1;
                    var dy = target.Item2 - asteroid.Item2;

                    (var stepX, var stepY) = GetDelta(dx, dy);

                    var currLoc = asteroid;
                    var isBlocked = false;
                    while (currLoc != target)
                    {
                        currLoc.Item1 += stepX;
                        currLoc.Item2 += stepY;
                        if (_asteroids.Contains(currLoc) && currLoc != target)
                            isBlocked = true;
                    }
                    if (!isBlocked)
                        currVis.Add(target);
                }
                if (currVis.Count > result.VisibleAsteroids.Count)
                {
                    result.X = asteroid.Item1;
                    result.Y = asteroid.Item2;
                    result.VisibleAsteroids = currVis;
                }
            }
            return result;
        }

        class LocRes
        {
            public double Angle { get; set; }
            public int LocCode { get; set; }
        }

        public int LocationCodeForAsteroid(
            (int, int) location,
            HashSet<(int, int)> asteroids,
            int destroyCount
            )
        {
            var roids = new List<LocRes>();

            foreach (var asteroid in asteroids)
            {
                // This looks weird because we don't want "normal" trig angles.  0 degrees is straight up. 
                // Rotation proceeds clockwise.
                var angle = Math.Atan2(
                    asteroid.Item1 - location.Item1,
                    location.Item2 - asteroid.Item2
                    );
                if (angle < 0)
                    angle += Math.PI * 2;
                roids.Add(new LocRes { Angle = angle, LocCode = asteroid.Item1 * 100 + asteroid.Item2 });
            }
            roids = roids.OrderBy(o => o.Angle).ToList();
            return roids[destroyCount - 1].LocCode;
        }

        private (int, int) GetDelta(int x, int y)
        {
            if (x == 0)
                return (0, Sign(y));
            if (y == 0)
                return (Sign(x), 0);

            int signX = Sign(x);
            int signY = Sign(y);
            (var rx, var ry) = Util.ReduceCommonFactors(Math.Abs(x), Math.Abs(y));
            return (rx * signX, ry * signY);
            int Sign(int v) => v / Math.Abs(v);
        }

        private HashSet<(int, int)> AsteroidLocations(string fileName)
        {
            var result = new HashSet<(int, int)>();

            var y = 0;
            foreach (var line in TextFileLines(fileName))
            {
                for (int x = 0; x < line.Length; x++)
                    if (line[x] == '#')
                        result.Add((x, y));
                y++;
            }
            return result;
        }

    }
}
