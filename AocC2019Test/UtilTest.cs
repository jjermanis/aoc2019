using Microsoft.VisualStudio.TestTools.UnitTesting;
using AoC2019;
using System.Linq;
using System.Collections.Generic;

namespace AocC2019Test
{
    [TestClass]
    public class UtilTest
    {
        [TestMethod]
        public void TestIsPrime()
        {
            Assert.IsFalse(Util.IsPrime(-2));
            Assert.IsFalse(Util.IsPrime(0));
            Assert.IsFalse(Util.IsPrime(1));
            Assert.IsTrue(Util.IsPrime(2));
            Assert.IsTrue(Util.IsPrime(3));
            Assert.IsFalse(Util.IsPrime(4));
            Assert.IsTrue(Util.IsPrime(7919));
            Assert.IsFalse(Util.IsPrime(947 * 953));
        }

        [TestMethod]
        public void TestPrimesEnumerator()
        {
            var count = 0;
            foreach (var prime in Util.Primes())
            {
                if (count++ > 100)
                    break;
                if (count == 100)
                    Assert.AreEqual(541, prime);
                Assert.IsTrue(Util.IsPrime(prime));
            }
        }

        [TestMethod]
        public void TestPrimeFactors()
        {
            Assert.AreEqual(0, Util.PrimeFactors(0).Count());
            Assert.AreEqual(0, Util.PrimeFactors(1).Count());
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { 7 }, Util.PrimeFactors(7)));
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { 2, 2, 3 }, Util.PrimeFactors(12)));
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { 2, 2, 2, 2, 2, 2, 2, 2 }, Util.PrimeFactors(256)));
            Assert.IsTrue(Enumerable.SequenceEqual(new[] { 2, 3, 5, 7, 11 }, Util.PrimeFactors(2310)));
        }

        [TestMethod]
        public void TestReduceFactors()
        {
            Assert.AreEqual((1, 5), Util.ReduceCommonFactors(1, 5));
            Assert.AreEqual((0, 5), Util.ReduceCommonFactors(0, 5));
            Assert.AreEqual((2, 5), Util.ReduceCommonFactors(42, 105));
            Assert.AreEqual((13, 19), Util.ReduceCommonFactors(13, 19));
            Assert.AreEqual((8, 15), Util.ReduceCommonFactors(8, 15));
            Assert.AreEqual((6, 35), Util.ReduceCommonFactors(66, 385));
            Assert.AreEqual((14, 15), Util.ReduceCommonFactors(3961202, 4244145));
        }

        [TestMethod]
        public void TestPermute()
        {
            // TODO Make better tests here
            Assert.IsTrue(Util.Permute(3, 7, 13).ToList().Count == 6);
            Assert.IsTrue(Util.Permute(3, 7, 13, 19).ToList().Count == 24);
            Assert.IsTrue(Util.Permute(3, 7, 13, 19, 29).ToList().Count == 120);
        }

        [TestMethod]
        public void TestCommonDenom()
        {
            Assert.AreEqual(1, Util.GreatestCommonDenom(1, 12));
            Assert.AreEqual(2, Util.GreatestCommonDenom(0, 2));
            Assert.AreEqual(2, Util.GreatestCommonDenom(2, 0));
            Assert.AreEqual(1, Util.GreatestCommonDenom(14, 15));
            Assert.AreEqual(4, Util.GreatestCommonDenom(4, 32));
            Assert.AreEqual(1001, Util.GreatestCommonDenom(2002, 7007));
        }

        [TestMethod]
        public void TestLeastCommonMultiple()
        {
            Assert.AreEqual(6, Util.LeastCommonMultiple(2, 3));
            Assert.AreEqual(30, Util.LeastCommonMultiple(6, 15));
            Assert.AreEqual(462, Util.LeastCommonMultiple(42, 66));

            Assert.AreEqual(30, Util.LeastCommonMultiple(new List<long> { 2, 3, 5 }));
            Assert.AreEqual(28_453_973_716, Util.LeastCommonMultiple(new List<long> { 206, 422, 1748, 2996}));
        }
    }
}