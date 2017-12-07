using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_1
    {
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day1.txt");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var intArray = input.Select(c => c - '0').ToArray();
            var captchaSum_1 = 0;
            var captchaSum_2 = 0;
            var step = intArray.Length / 2;

            for (int i = 0; i < intArray.Length - 1; i++)
            {
                captchaSum_1 += (intArray[i] == intArray[i + 1]) ? intArray[i] : 0;

                var posToCheck = (i + step >= intArray.Length) ? i + step - intArray.Length : i + step;
                captchaSum_2 += (intArray[i] == intArray[posToCheck]) ? intArray[i] : 0;
            }

            captchaSum_1 += (intArray[intArray.Length - 1] == intArray[0]) ? intArray[intArray.Length - 1] : 0;
            captchaSum_2 += (intArray[intArray.Length - 1] == intArray[step]) ? intArray[intArray.Length - 1] : 0;

            sw.Stop();

            return $"Part 1: {captchaSum_1}{Environment.NewLine}Part 2: {captchaSum_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }
    }
}
