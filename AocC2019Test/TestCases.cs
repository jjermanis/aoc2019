using Microsoft.VisualStudio.TestTools.UnitTesting;
using AoC2019;
using System.Linq;

namespace AocC2019Test
{
    [TestClass]
    public class TestCases
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
            // Add unit test for visualization
        }

        [TestMethod]
        public void Day12()
        {
            // Add unit test
        }

    }
}
