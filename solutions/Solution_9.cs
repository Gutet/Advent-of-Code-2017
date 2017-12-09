using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_9
    {
        static int garbageCharacters;
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day9.txt");

            Debug.Assert(getScoreCount("{}") == 1);
            Debug.Assert(getScoreCount("{{{}}}") == 6);
            Debug.Assert(getScoreCount("{{},{}}") == 5);
            Debug.Assert(getScoreCount("{{{},{},{{}}}}") == 16);
            Debug.Assert(getScoreCount("{<a>,<a>,<a>,<a>}") == 1);
            Debug.Assert(getScoreCount("{{<ab>},{<ab>},{<ab>},{<ab>}}") == 9);
            Debug.Assert(getScoreCount("{{<!!>},{<!!>},{<!!>},{<!!>}}") == 9);
            Debug.Assert(getScoreCount("{{<a!>},{<a!>},{<a!>},{<ab>}}") == 3);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getScoreCount(input);

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {garbageCharacters}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getScoreCount(string input)
        {
            garbageCharacters = 0;
            var groupCount = 0;
            var isGarbage = false;
            var level = 0;
            var skip = false;

            foreach (var c in input)
            {
                if (skip)
                {
                    skip = false;
                    continue;
                }
                else if (isGarbage)
                {
                    switch (c)
                    {
                        case '!':
                            skip = true;
                            break;
                        case '>':
                            isGarbage = false;
                            break;
                        default:
                            garbageCharacters++;
                            break;
                    }
                }
                else if (!isGarbage)
                {
                    switch (c)
                    {
                        case '<':
                            isGarbage = true;
                            break;
                        case '{':
                            level++;
                            break;
                        case '}':
                            groupCount += level;
                            level--;
                            break;
                    }
                }
            }

            return groupCount;
        }
    }
}
