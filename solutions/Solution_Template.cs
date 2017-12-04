using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_Template
    {
        public static string getResult()
        {
            var input = @"";

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here

            sw.Stop();

            return $"Part 1: {Environment.NewLine}Part 2: {Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }
    }
}
