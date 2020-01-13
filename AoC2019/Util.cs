using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class Util
    {
        /// <summary>
        /// Returns the smallest integer that is evenly divisible by all provided numbers
        /// </summary>
        public static long LeastCommonMultiple(List<long> vals)
            => vals.Aggregate(LeastCommonMultiple);

        /// <summary>
        /// Returns the smallest integer that is evenly divisible by both numbers
        /// </summary>
        public static long LeastCommonMultiple(long a, long b)
            => Math.Abs(a * b) / GreatestCommonDenom(a, b);

        /// <summary>
        /// Returns the largest integer that is a factor of both numbers
        /// </summary>
        public static long GreatestCommonDenom(long a, long b)
            => b == 0 ? a : GreatestCommonDenom(b, a % b);

        /// <summary>
        /// Enumerates all possible permutations in the input.  This function does NOT
        /// account for duplicates in the input.  For example, vals=[1, 1] will return
        /// [1,1], [1,1]
        /// </summary>
        public static IEnumerable<int[]> Permute(params int[] vals)
        {
            foreach (var val in PermuteWork(vals, 0, vals.Length-1))
                yield return val;
        }

        /// <summary>
        /// Removes all common prime factors from x and y.  If x/y were a fraction,
        /// the result of this function would be the reduced numerator and denominator
        /// for that fraction.
        /// </summary>
        public static (int x, int y) ReduceCommonFactors(int x, int y)
        {
            int resultX = x;
            int resultY = y;

            foreach (var factor in PrimeFactors(x))
            {
                if (resultY % factor == 0)
                {
                    resultX /= factor;
                    resultY /= factor;
                }
            }
            return (resultX, resultY);
        }

        /// <summary>
        /// Enumerates over all prime factors of val, in ascending order.  If a prime
        /// factor is repeated, it will be repeated in the enumeration.  For example,
        /// val=24 returns [2,2,2,3]
        /// </summary>
        public static IEnumerable<int> PrimeFactors(int val)
        {
            if (val >= 2)
            {
                var remaining = val;
                foreach (var prime in Primes())
                {
                    while (remaining % prime == 0)
                    {
                        yield return prime;
                        remaining /= prime;
                    }
                    if (remaining == 1)
                        break;
                }
            }
        }

        /// <summary>
        /// Enumerates over all prime values. This does not stop; there is no
        /// graceful handling for overflow.
        /// </summary>
        public static IEnumerable<int> Primes()
        {
            yield return 2;
            yield return 3;
            var curr = 5;
            while (true)
            {
                if (IsPrime(curr))
                    yield return curr;
                curr += 2;
                if (IsPrime(curr))
                    yield return curr;
                curr += 4;
            }
        }

        /// <summary>
        /// Returns true if and only if val is a prime number
        /// </summary>
        public static bool IsPrime(int val)
        {
            if (val < 2)
                return false;
            if (val == 2)
                return true;
            if (val % 2 == 0)
                return false;
            for (var x = 3; (x * x <= val); x += 2)
                if (val % x == 0)
                    return false;
            return true;
        }

        private static IEnumerable<int[]> PermuteWork(int[] vals, int k, int m)
        {
            int i;
            if (k == m)
            {
                var result = new int[vals.Length];
                Array.Copy(vals, result, vals.Length);
                yield return result;
            }
            else
                for (i = k; i <= m; i++)
                {
                    Swap(ref vals[k], ref vals[i]);
                    foreach (var val in PermuteWork(vals, k + 1, m))
                        yield return val;
                    Swap(ref vals[k], ref vals[i]);
                }
        }
        private static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        public static void PermuteTest(params int[] vals)
        {
            foreach (var val in Permute(vals))
                Console.WriteLine(string.Join("", val));
        }
        public static void PrimesTest(int count)
        {
            var x = 0;
            foreach (var prime in Primes())
            {
                Console.WriteLine(prime);
                if (++x >= count)
                    break;
            }
        }
        public static void PrimeFactorsTest(int val)
        {
            Console.WriteLine($"Factors for {val}");
            var factors = PrimeFactors(val);
            foreach (var prime in factors)
                Console.WriteLine($"Factor: {prime}");
        }

        public static void ReduceFactorsTest(int x, int y)
        {
            Console.WriteLine($"x={x} y={y}");
            (x, y) = ReduceCommonFactors(x, y);
            Console.WriteLine($"x={x} y={y}");
        }
    }
}
