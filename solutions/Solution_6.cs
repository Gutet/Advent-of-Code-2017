using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_6
    {
        static List<memoryAllocationNode> storedArrays = new List<memoryAllocationNode>();

        public static string getResult()
        {
            var input = @"4	10	4	1	8	4	9	14	5	1	14	15	0	15	3	5";
            var cycles = 0;

            Debug.Assert(getSolution("0	2	7	0", ref cycles) == 5);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getSolution(input, ref cycles);
            
            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {cycles}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getSolution(string input, ref int cycles)
        {
            bool breakLoop = false;
            var steps = 0;

            var memoryArray = getArrayFromInput(input);
            storedArrays.Add(new memoryAllocationNode((int[])memoryArray.Clone(), steps));
            
            var position = 0;

            while (true)
            {
                steps++;
                cycles = steps;
                var maxIndex = memoryArray.ToList().IndexOf(memoryArray.Max());
                var reallocateValue = memoryArray[maxIndex];
                memoryArray[maxIndex] = 0;
                position = maxIndex;

                for (int i = 0; i < reallocateValue; i++)
                {
                    position++;
                    if (position >= memoryArray.Length)
                    {
                        position = 0;
                    }

                    memoryArray[position] += 1;
                }

                if (checkIfArrayExists(memoryArray, storedArrays, ref cycles) == true)
                {
                    breakLoop = true;
                }
                storedArrays.Add(new memoryAllocationNode((int[])memoryArray.Clone(), steps));

                if (breakLoop)
                {
                    break;
                }
            }

            return steps;
        }

        private static bool checkIfArrayExists(int[] memoryArray, List<memoryAllocationNode> storedArrays, ref int cycles)
        {
            bool lReturn = false;

            foreach (var item in storedArrays)
            {
                lReturn = Enumerable.SequenceEqual(memoryArray, item.memoryArray);
                if (lReturn)
                {
                    cycles = cycles - item.arrayPosition;
                    return lReturn;
                }
            }

            return false;
        }

        private static int[] getArrayFromInput(string input)
        {
            return input.Split('\t').Select(s => int.Parse(s)).ToArray();
        }

        class memoryAllocationNode
        {
            public int[] memoryArray { get; set; }
            public int arrayPosition { get; set; }

            public memoryAllocationNode(int[] memoryArray, int arrayPosition)
            {
                this.memoryArray = memoryArray;
                this.arrayPosition = arrayPosition;
            }
        }
    }
}
