using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019
{
    public class Util
    {
        public static long LeastCommonMultiple(List<long> vals)
            => vals.Aggregate(LeastCommonMultiple);
        public static long LeastCommonMultiple(long a, long b)
            => Math.Abs(a * b) / GreatestCommonDenom(a, b);
        public static long GreatestCommonDenom(long a, long b)
            => b == 0 ? a : GreatestCommonDenom(b, a % b);

        public static IEnumerable<int[]> Permute(params int[] vals)
        {
            foreach (var val in PermuteWork(vals, 0, vals.Length-1))
                yield return val;
        }

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

        public static IEnumerable<int> PrimeFactors(int val)
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
