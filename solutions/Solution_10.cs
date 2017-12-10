using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_10
    {
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day10.txt");

            
            int[] knotArray = getKnotArray(5);
            int[] inputArray = "3,4,1,5".Split(',').Select(s => int.Parse(s)).ToArray();
            int[] asciiArray;

            Debug.Assert(getArrayScore(knotArray, inputArray) == 12);
            
            knotArray = getKnotArray(256);
            asciiArray = getAsciiArray("1,2,3");
            Debug.Assert(getArrayHash(knotArray, asciiArray) == "3efbe78a8d82f29979031a4aa0b16a9d");
            knotArray = getKnotArray(256);
            asciiArray = getAsciiArray("AoC 2017");
            Debug.Assert(getArrayHash(knotArray, asciiArray) == "33efeb34ea91902bb2f59c9920caa6cd");
            knotArray = getKnotArray(256);
            asciiArray = getAsciiArray("");
            Debug.Assert(getArrayHash(knotArray, asciiArray) == "a2582a3a0e66e6e86e3812dcb672a272");
            knotArray = getKnotArray(256);
            asciiArray = getAsciiArray("1,2,4");
            Debug.Assert(getArrayHash(knotArray, asciiArray) == "63960835bcdc130f0b66d7ff4f6a5a8e");
            knotArray = getKnotArray(256);
            asciiArray = getAsciiArray("rimbuod");
            Debug.Assert(getArrayHash(knotArray, asciiArray) == "044392e3fec2fac3209c2f5c22717cd3");
            knotArray = getKnotArray(256);
            asciiArray = getAsciiArray("Rimbuod");
            Debug.Assert(getArrayHash(knotArray, asciiArray) == "46093e85204d0b5f2372895c515cbbff");
            knotArray = getKnotArray(256);
            asciiArray = getAsciiArray("RIMBUOD");
            Debug.Assert(getArrayHash(knotArray, asciiArray) == "8279c6a413685a576570b14906c73bf8");

            knotArray = getKnotArray(256);
            inputArray = input.Split(',').Select(s => int.Parse(s)).ToArray();
            asciiArray = getAsciiArray(input);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getArrayScore(knotArray, inputArray);

            knotArray = getKnotArray(256);
            inputArray = input.Split(',').Select(s => int.Parse(s)).ToArray();
            asciiArray = getAsciiArray(input);
            var solutionPart_2 = getArrayHash(knotArray, asciiArray);

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static string getArrayHash(int[] knotArray, int[] asciiArray)
        {
            string hash = "";

            var skip = 0;
            var position = 0;

            for (int i = 0; i < 64; i++)
            {
                foreach (var length in asciiArray)
                {
                    if (length > knotArray.Length)
                    {
                        continue;
                    }

                    knotArray = reverseArrayPart(knotArray, position, length);

                    position += length + skip++;
                    while (position > knotArray.Length)
                    {
                        position -= knotArray.Length;
                    }
                }
            }


            int[] denseHash = getDenseHash(knotArray);
            hash = getHash(denseHash);
            return hash;
        }

        private static string getHash(int[] denseHash)
        {
            string lReturn = "";

            for (int i = 0; i < denseHash.Length; i++)
            {
                lReturn += denseHash[i].ToString("X2").ToLower();
            }

            return lReturn;
        }

        private static int[] getDenseHash(int[] knotArray)
        {
            int[] lReturn = new int[16];
            var i = 0;
            for (int j = 0; j < 16; j++)
            {
                lReturn[j] = knotArray[i] ^ knotArray[i + 1] ^ knotArray[i + 2] ^ knotArray[i + 3] ^ 
                    knotArray[i + 4] ^ knotArray[i + 5] ^ knotArray[i + 6] ^ knotArray[i + 7] ^ 
                    knotArray[i + 8] ^ knotArray[i + 9] ^ knotArray[i + 10] ^ knotArray[i + 11] ^ 
                    knotArray[i + 12] ^ knotArray[i + 13] ^ knotArray[i + 14] ^ knotArray[i + 15];

                i += 16;
            }
            return lReturn;
        }

        private static int[] getAsciiArray(string input)
        {
            int[] lReturn = new int[input.Length + 5];
            int i = 0;
            foreach (char c in input)
            {
                lReturn[i++] = (int)c;
            }

            lReturn[i++] = 17;
            lReturn[i++] = 31;
            lReturn[i++] = 73;
            lReturn[i++] = 47;
            lReturn[i++] = 23;

            return lReturn;
        }

        private static int getArrayScore(int[] knotArray, int[] inputArray)
        {
            var skip = 0;
            var position = 0;

            foreach (var length in inputArray)
            {
                if (length > knotArray.Length)
                {
                    continue;
                }

                knotArray = reverseArrayPart(knotArray, position, length);

                position += length + skip++;
                while (position > knotArray.Length)
                {
                    position -= knotArray.Length;
                }
            }

            return knotArray[0] * knotArray[1];
        }

        private static int[] reverseArrayPart(int[] knotArray, int position, int length)
        {
            int[] arrayToReverse = new int[length];
            var loopPosition = position;
            for (int i = 0; i < length; i++)
            {
                if (loopPosition >= knotArray.Length)
                {
                    loopPosition = 0;
                }
                arrayToReverse[i] = knotArray[loopPosition++];
            }

            loopPosition = position;

            for (int i = arrayToReverse.Length - 1; i >= 0; i--)
            {
                if (loopPosition >= knotArray.Length)
                {
                    loopPosition = 0;
                }
                knotArray[loopPosition++] = arrayToReverse[i];
            }
            return knotArray;
        }

        private static int[] getKnotArray(int size)
        {
            int[] returnArray = new int[size];
            for (int i = 0; i < size; i++)
            {
                returnArray[i] = i;
            }
            return returnArray;
        }
    }
}
