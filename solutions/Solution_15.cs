using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_15
    {
        public static string getResult()
        {
            var A = 679;
            var B = 771;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getMatchingPairs(A, B);
            var solutionPart_2 = getMatchingPairsPartTwo(A, B);

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getMatchingPairs(long a, long b)
        {
            int mPairs = 0;

            for (int i = 0; i < 40000000; i++)
            {
                a = getNextValue(a, 16807);
                b = getNextValue(b, 48271);

                if ((a & 0x000ffff) == (b & 0x000ffff))
                {
                    mPairs++;
                }
            }
            return mPairs;
        }

        private static int getMatchingPairsPartTwo(long a, long b)
        {
            int mPairs = 0;

            for (int i = 0; i < 5000000; i++)
            {
                a = getNextValue(a, 16807, 4);
                b = getNextValue(b, 48271, 8);

                if ((a & 0x000ffff) == (b & 0x000ffff))
                {
                    mPairs++;
                }
            }
            return mPairs;
        }

        private static long getNextValue(long value, long factor)
        {
            long dFactor = 2147483647;

            return (value * factor) % dFactor;
        }

        private static long getNextValue(long value, long factor, long divisor)
        {
            long dFactor = 2147483647;
            long next = (value * factor) % dFactor;

            while (next % divisor != 0)
            {
                next = (next * factor) % dFactor;
            }

            return next;
        }
    }
}
