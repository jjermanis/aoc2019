using Microsoft.VisualStudio.TestTools.UnitTesting;
using AoC2019;

namespace AocC2019Test
{
    [TestClass]
    public class TestCases
    {
        [TestMethod]
        public void Day01()
        {
            (var s, var c) = new Day01().DoCalc();
            Assert.AreEqual(3474920, s);
            Assert.AreEqual(5209504, c);
        }

        [TestMethod]
        public void Day02()
        {
            (var p1, var p2) = new Day02().DoCalc();
            Assert.AreEqual(5098658, p1);
            Assert.AreEqual(5064, p2);
        }

        [TestMethod]
        public void Day03()
        {
            var d = new Day03();

            Assert.AreEqual(159, d.NearestManhattanIntersectDist("Day03Test1.txt"));
            Assert.AreEqual(135, d.NearestManhattanIntersectDist("Day03Test2.txt"));
            Assert.AreEqual(610, d.NearestWireIntersectDist("Day03Test1.txt"));
            Assert.AreEqual(410, d.NearestWireIntersectDist("Day03Test2.txt"));
            Assert.AreEqual(721, d.NearestManhattanIntersectDist("Day03.txt"));
            Assert.AreEqual(7388 , d.NearestWireIntersectDist("Day03.txt"));
        }


    }
}
