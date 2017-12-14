using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode.solutions
{
    public static class Solution_14
    {
        static List<DiscNode> disc = new List<DiscNode>();
        public static string getResult()
        {
            var input = @"xlqgujun";

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getUsedSquares(input);
            var solutionPart_2 = getUsedRegions();

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getUsedRegions()
        {
            var lReturn = 0;

            for (int row = 0; row < 128; row++)
            {
                for (int col = 0; col < 128; col++)
                {
                    var node = disc.Where(x => x.X == col && x.Y == row).FirstOrDefault();
                    if (node.Visited == false)
                    {
                        node.Visited = true;
                        if (node.IsChecked)
                        {
                            lReturn++;
                            markAllAdjacentAsChecked(node.X, node.Y);
                        }
                    }
                }
            }

            return lReturn;
        }

        private static void markAllAdjacentAsChecked(int xPos, int yPos)
        {
            var adjacentNodes = disc.Where(x =>
            (
            //Top
            (x.X == xPos && x.Y == yPos - 1 && x.IsChecked == true && x.Visited == false) ||
            //Right
            (x.X == xPos + 1 && x.Y == yPos && x.IsChecked == true && x.Visited == false) ||
            //Left
            (x.X == xPos - 1 && x.Y == yPos && x.IsChecked == true && x.Visited == false) ||
            //Down
            (x.X == xPos && x.Y == yPos + 1 && x.IsChecked == true && x.Visited == false))
            ).ToList();

            foreach (var node in adjacentNodes)
            {
                node.Visited = true;
                markAllAdjacentAsChecked(node.X, node.Y);
            }
        }

        private static int getUsedSquares(string input)
        {
            int[] knotArray = getKnotArray(256);
            int[] asciiArray;
            List<string> lHashList = new List<string>();
            var lReturn = 0;

            for (int i = 0; i < 128; i++)
            {
                knotArray = getKnotArray(256);
                asciiArray = getAsciiArray($"{input}-{i}");
                var binaryString = getBinaryString(getArrayHash(knotArray, asciiArray));
                fillMatrix(i, binaryString);

                lReturn += binaryString.Count(f => f == '1');
            }
            
            return lReturn;
        }

        private static void fillMatrix(int row, string binaryString)
        {
            for (int i = 0; i < 128; i++)
            {
                disc.Add(new DiscNode(i, row, Convert.ToBoolean((int)Char.GetNumericValue(binaryString[i]))));
            }
        }

        private static string getBinaryString(string item)
        {
            var s = String.Join("",
            item.Select(x => Convert.ToString(Convert.ToInt32(x + "", 16), 2).PadLeft(4, '0')));

            return s;
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
            var toBytes = Encoding.ASCII.GetBytes(input).Select(x => (int)x).ToList();
            toBytes.Add(17);
            toBytes.Add(31);
            toBytes.Add(73);
            toBytes.Add(47);
            toBytes.Add(23);
            return toBytes.ToArray();
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

    public class DiscNode
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsChecked { get; set; }
        public bool Visited { get; set; }

        public DiscNode(int x, int y, bool isChecked)
        {
            this.X = x;
            this.Y = y;
            this.Visited = (isChecked) ? false : true;
            this.IsChecked = isChecked;
        }
    }
}
