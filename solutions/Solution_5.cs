using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_5
    {
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day5.txt");

            Debug.Assert(getResult(@"0
3
0
1
-3", false) == 5);
            Debug.Assert(getResult(@"0
3
0
1
-3", true) == 10);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            

            // Solution goes here
            var firstPart = getResult(input, false);
            var secondPart = getResult(input, true);

            sw.Stop();

            return $"Part 1: {firstPart}{Environment.NewLine}Part 2: {secondPart}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getResult(string input, bool secondPart)
        {
            var inputArray = getInputArray(input);

            var current = 0;
            var next = 0;
            var step = 0;

            while (next < inputArray.Count())
            {
                step++;
                next = current + inputArray[current]++;
                if (secondPart && inputArray[current] > 3)
                {
                    inputArray[current] = inputArray[current] - 2;
                }
                current = next;
            }

            return step;
        }

        private static int[] getInputArray(string input)
        {
            return input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }
    }
}
