using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_17
    {
        public static string getResult()
        {
            var input = 303;

            Debug.Assert(getSolution(3) == 638);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getSolution(input);
            var solutionPart_2 = getSolution_2(input);

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getSolution(int steps)
        {
            List<int> vortex = new List<int>();
            var currentPosition = 0;
            vortex.Add(0);

            for (int i = 1; i <= 2017; i++)
            {
                currentPosition = ((steps + currentPosition) % i) + 1;

                vortex.Insert(currentPosition, i);
            }

            return vortex[vortex.IndexOf(2017) + 1];
        }

        private static int getSolution_2(int steps)
        {
            var currentPosition = 0;
            var lReturn = 0;

            for (int i = 1; i <= 50000000; i++)
            {
                currentPosition = ((steps + currentPosition) % i) + 1;

                if (currentPosition == 1)
                {
                    lReturn = i;
                }
            }

            return lReturn;
        }
    }
}
