using Microsoft.VisualStudio.TestTools.UnitTesting;
using AoC2019;
using System.Linq;

namespace AocC2019Test
{
    [TestClass]
    public class DayTestCases
    {
        [TestMethod]
        public void Day01()
        {
            var d = new Day01();
            Assert.AreEqual(3474920, d.SimpleFuelTotal());
            Assert.AreEqual(5209504, d.ComplexFuelTotal());
        }

        [TestMethod]
        public void Day02()
        {
            var d = new Day02();
            Assert.AreEqual(5098658, d.Execute(12, 2));
            Assert.AreEqual(5064, d.FindInputs(19690720));
        }

        [TestMethod]
        public void Day03()
        {
            var test1 = new Day03("Day03Test1.txt");
            Assert.AreEqual(159, test1.NearestManhattanIntersectDist());
            Assert.AreEqual(610, test1.NearestWireIntersectDist());
            var test2 = new Day03("Day03Test2.txt");
            Assert.AreEqual(135, test2.NearestManhattanIntersectDist());
            Assert.AreEqual(410, test2.NearestWireIntersectDist());

            var d = new Day03();
            Assert.AreEqual(721, d.NearestManhattanIntersectDist());
            Assert.AreEqual(7388, d.NearestWireIntersectDist());
        }

        [TestMethod]
        public void Day04()
        {
            (var p1, var p2) = new Day04().DoCalc();
            Assert.AreEqual(2090, p1);
            Assert.AreEqual(1419, p2);
        }

        [TestMethod]
        public void Day05()
        {
            var d = new Day05();
            Assert.AreEqual(13210611, d.DoCalc(1));
            Assert.AreEqual(584126, d.DoCalc(5));
        }

        [TestMethod]
        public void Day06()
        {
            var test1 = new Day06("Day06Test1.txt");
            Assert.AreEqual(42, test1.TotalStepsToCom());
            var test2 = new Day06("Day06Test2.txt");
            Assert.AreEqual(4, test2.DistanceFromSanToYou());
            var day = new Day06();
            Assert.AreEqual(151345, day.TotalStepsToCom());
            Assert.AreEqual(391, day.DistanceFromSanToYou());
        }

        [TestMethod]
        public void Day07()
        {
            var test1 = new Day07("Day07Test1.txt");
            Assert.AreEqual(43210, test1.MaxSignal());
            var test2 = new Day07("Day07Test2.txt");
            Assert.AreEqual(54321, test2.MaxSignal());
            var test3 = new Day07("Day07Test3.txt");
            Assert.AreEqual(65210, test3.MaxSignal());
            var test4 = new Day07("Day07Test4.txt");
            Assert.AreEqual(139629729, test4.MaxSignalWithFeedback());
            var test5 = new Day07("Day07Test5.txt");
            Assert.AreEqual(18216, test5.MaxSignalWithFeedback());
            var day = new Day07("Day07.txt");
            Assert.AreEqual(19650, day.MaxSignal());
            Assert.AreEqual(35961106, day.MaxSignalWithFeedback());
        }

        [TestMethod]
        public void Day08()
        {
            var d = new Day08();
            Assert.AreEqual(2016, d.Part1Answer());
            var p2 = d.Part2Answer().ToList();
            Assert.AreEqual("#  #    # #  #    # #  # ", p2[1]);
            Assert.AreEqual("####   #  #      #  #  # ", p2[2]);
            Assert.AreEqual("#  # #    #  # #    #  # ", p2[4]);
        }

        [TestMethod]
        public void Day09()
        {
            var test1 = new Day09("Day09Test1.txt");
            Assert.AreEqual(109, test1.Run(0));
            var test2 = new Day09("Day09Test2.txt");
            Assert.AreEqual(1219070632396864, test2.Run(0));
            var test3 = new Day09("Day09Test3.txt");
            Assert.AreEqual(1125899906842624, test3.Run(0));

            var day = new Day09();
            Assert.AreEqual(3765554916, day.Run(1));
            Assert.AreEqual(76642, day.Run(2));
        }

        [TestMethod]
        public void Day10()
        {
            Day10TestPart1("Day10Test1.txt", 8);
            Day10TestPart1("Day10Test2.txt", 33);
            Day10TestPart1("Day10Test3.txt", 35);
            Day10TestPart1("Day10Test4.txt", 41);

            Day10TestFull(new Day10("Day10Test5.txt"), 210, 802);
            Day10TestFull(new Day10(), 276, 1321);
        }

        private void Day10TestPart1(string filename, int expected)
            => Assert.AreEqual(expected, new Day10(filename).MostVisibleAsteroids().VisibleAsteroids.Count);

        private void Day10TestFull(
            Day10 day,
            int expectedCount,
            int expectedCode)
        {
            var best = day.MostVisibleAsteroids();
            Assert.AreEqual(expectedCount, best.VisibleAsteroids.Count);
            var code = day.LocationCodeForAsteroid((best.X, best.Y), best.VisibleAsteroids, 200);
            Assert.AreEqual(expectedCode, code);
        }

        [TestMethod]
        public void Day11()
        {
            var day = new Day11();
            Assert.AreEqual(2184, day.GetPaintedSpaceCount(0));
            var p2 = day.PaintingVisualization(1).ToList();
            Assert.AreEqual("  ##  #  #  ##  #  # #### #### ###  #  #", p2[0]);
            Assert.AreEqual(" #  # #### #    ####   #  ###  #  # ##", p2[2]);
            Assert.AreEqual(" #### #  # #    #  #  #   #    ###  # #", p2[3]);
            Assert.AreEqual(" #  # #  # #  # #  # #    #    #    # #", p2[4]);
        }

        [TestMethod]
        public void Day12()
        {
            var test1 = new Day12("Day12Test1.txt");
            Assert.AreEqual(179, test1.CalcEnergy(10));
            Assert.AreEqual(2772, test1.CyclePeriod());

            var test2 = new Day12("Day12Test2.txt");
            Assert.AreEqual(1940, test2.CalcEnergy(100));
            Assert.AreEqual(4686774924, test2.CyclePeriod());

            var day = new Day12();
            Assert.AreEqual(12490, day.CalcEnergy(1000));
            Assert.AreEqual(392733896255168, day.CyclePeriod());
        }

        [TestMethod]
        public void Day13()
        {
            var day = new Day13();
            Assert.AreEqual(260, day.BlockCountExit());
            Assert.AreEqual(12952, day.PlayGame());
        }

        [TestMethod]
        public void Day14()
        {
            var test1 = new Day14("Day14Test1.txt");
            Assert.AreEqual(31, test1.OreNeeded());
            var test2 = new Day14("Day14Test2.txt");
            Assert.AreEqual(165, test2.OreNeeded());
            var test3 = new Day14("Day14Test3.txt");
            Assert.AreEqual(13312, test3.OreNeeded());
            Assert.AreEqual(82892753, test3.MaxFuel());
            var test4 = new Day14("Day14Test4.txt");
            Assert.AreEqual(180697, test4.OreNeeded());
            Assert.AreEqual(5586022, test4.MaxFuel());
            var test5 = new Day14("Day14Test5.txt");
            Assert.AreEqual(2210736, test5.OreNeeded());
            Assert.AreEqual(460664, test5.MaxFuel());

            var day = new Day14();
            Assert.AreEqual(1920219, day.OreNeeded());
            Assert.AreEqual(1330066, day.MaxFuel());
        }

        [TestMethod]
        public void Day15()
        {
            var day = new Day15();
            (var p1, var p2) = day.TraverseMaze();
            Assert.AreEqual(218, p1);
            Assert.AreEqual(544, p2);
        }

        [TestMethod]
        public void Day16()
        {
            var test0 = new Day16("Day16Test0.txt");
            Assert.AreEqual(1029498, test0.CalcFft(4));
            var test1 = new Day16("Day16Test1.txt");
            Assert.AreEqual(24176176, test1.CalcFft(100));
            var test2 = new Day16("Day16Test2.txt");
            Assert.AreEqual(73745418, test2.CalcFft(100));
            var test3 = new Day16("Day16Test3.txt");
            Assert.AreEqual(52432133, test3.CalcFft(100));

            var day = new Day16();
            Assert.AreEqual(85726502, day.CalcFft(100));
            // TODO solve 16-2
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Day17()
        {
            var day = new Day17();
            Assert.AreEqual(9544, day.SumAlignment());
            Assert.AreEqual(1499679, day.NavigateMaze());
        }

        [TestMethod]
        public void Day18()
        {
            var test1 = new Day18("Day18Test1.txt");
            Assert.AreEqual(86, test1.SolveMaze());
            var test2 = new Day18("Day18Test2.txt");
            Assert.AreEqual(132, test2.SolveMaze());
            // TODO - this case is too slow - runs in about 2 minutes
            //var test3 = new Day18("Day18Test3.txt");
            //Assert.AreEqual(136, test3.SolveMaze());
            var test4 = new Day18("Day18Test4.txt");
            Assert.AreEqual(81, test4.SolveMaze());

            // TODO solve part 1
            Assert.IsTrue(false);
            // TODO solve part 2
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Day19()
        {
            var day = new Day19();
            Assert.AreEqual(131, day.LocalPointCount());
            Assert.AreEqual(15231022, day.FindFirstSquare(100));
        }

        [TestMethod]
        public void Day20()
        {
            var test1 = new Day20("Day20Test1.txt");
            Assert.AreEqual(23, test1.ShortestPathLength());
            Assert.AreEqual(26, test1.ShortestPathLengthRecursive());

            var test2 = new Day20("Day20Test2.txt");
            Assert.AreEqual(58, test2.ShortestPathLength());

            var test3 = new Day20("Day20Test3.txt");
            Assert.AreEqual(396, test3.ShortestPathLengthRecursive());

            var day = new Day20();
            Assert.AreEqual(556, day.ShortestPathLength());
            Assert.AreEqual(6532, day.ShortestPathLengthRecursive());
        }

        [TestMethod]
        public void Day21()
        {
            var day = new Day21();
            Assert.AreEqual(19359996, day.Walk());
            // TODO solve part 2
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Day22()
        {
            var test1 = new Day22("Day22Test1.txt");
            Assert.AreEqual(8, test1.GetIndex(10, 4));
            var test2 = new Day22("Day22Test2.txt");
            Assert.AreEqual(3, test2.GetIndex(10, 4));
            var test3 = new Day22("Day22Test3.txt");
            Assert.AreEqual(4, test3.GetIndex(10, 4));
            var test4 = new Day22("Day22Test4.txt");
            Assert.AreEqual(5, test4.GetIndex(10, 4));

            var day = new Day22();
            Assert.AreEqual(6431, day.GetIndex(10007, 2019));
            // TODO solve part 2
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Day23()
        {
            var day = new Day23();
            var results = day.ProcessNetwork();
            Assert.AreEqual(27182, results.Item1);
            Assert.AreEqual(19285, results.Item2);
        }

        [TestMethod]
        public void Day24()
        {
            var test1 = new Day24("Day24Test1.txt");
            Assert.AreEqual(2129920, test1.FindDupRating());
            Assert.AreEqual(99, test1.RecursiveGridBugCount(10));

            var day = new Day24();
            Assert.AreEqual(13500447, day.FindDupRating());
            Assert.AreEqual(2120, day.RecursiveGridBugCount(200));
        }

        [TestMethod]
        public void Day25()
        {
            var day = new Day25();
            Assert.AreEqual(537165825, day.PlayGuided());

            // Day 25 doesn't have a part 2.  Its star is automatically granted after
            // solving the other 49 puzzles
        }
    }
}

