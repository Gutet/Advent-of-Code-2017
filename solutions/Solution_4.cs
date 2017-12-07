using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_4
    {
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day4.txt");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Debug.Assert(isValidPassPhrase("aa bb cc dd ee") == 1);
            Debug.Assert(isValidPassPhrase("aa bb cc dd aa") == 0);
            Debug.Assert(isValidPassPhrase("aa bb cc dd aaa") == 1);

            Debug.Assert(isValidPassPhraseTwo("abcde fghij") == 1);
            Debug.Assert(isValidPassPhraseTwo("abcde xyz ecdab") == 0);
            Debug.Assert(isValidPassPhraseTwo("a ab abc abd abf abj") == 1);
            Debug.Assert(isValidPassPhraseTwo("iiii oiii ooii oooi oooo") == 1);
            Debug.Assert(isValidPassPhraseTwo("oiii ioii iioi iiio") == 0);

            var inputList = input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var validPassPhrases_1 = 0;
            var validPassPhrases_2 = 0;

            foreach (var passPhrase in inputList)
            {
                validPassPhrases_1 += isValidPassPhrase(passPhrase);
                validPassPhrases_2 += isValidPassPhraseTwo(passPhrase);
            }

            sw.Stop();

            return $"Part 1: {validPassPhrases_1}{Environment.NewLine}Part 2: {validPassPhrases_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int isValidPassPhraseTwo(string passPhrase)
        {
            var passPhraseList = passPhrase.Split(' ');
            var groupedList = passPhraseList.Select(x => new string (x.OrderBy(y => y).ToArray())).
                GroupBy(s => s).SelectMany(grp => grp.Skip(1));

            return (groupedList.Count() > 0) ? 0 : 1;
        }

        private static int isValidPassPhrase(string passPhrase)
        {
            var passPhraseList = passPhrase.Split(' ');
            var groupedList = passPhraseList.GroupBy(s => s).SelectMany(grp => grp.Skip(1));

            return (groupedList.Count() > 0) ? 0 : 1;
        }
    }
}
