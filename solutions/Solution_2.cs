using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_2
    {
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day2.txt");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var totalCheckSum_part1 = 0;
            var totalCheckSum_part2 = 0;

            foreach (var item in input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var intRow = item.Split('\t').Select(s => int.Parse(s)).ToArray();

                // Part 1
                totalCheckSum_part1 += getCheckSumPart(intRow);

                // Part 2
                totalCheckSum_part2 += getCheckSumDivisiblePart(intRow);
            }

            sw.Stop();

            return $"Part 1: {totalCheckSum_part1}{Environment.NewLine}Part 2: {totalCheckSum_part2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getCheckSumDivisiblePart(int[] intRow)
        {
            var lMax = 0;
            do
            {
                lMax = intRow.Max();
                intRow = intRow.Where(val => val != lMax).ToArray();
                foreach (var item in intRow)
                {
                    if (lMax % item == 0)
                    {
                        return lMax / item;
                    }
                }

            } while (true);
        }

        public static int getCheckSumPart(int[] intRow)
        {
            return (intRow.Max() - intRow.Min());
        }
    }
}
