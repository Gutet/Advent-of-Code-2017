using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.solutions
{
    public static class Solution_21
    {
        public static List<Rule> rules;
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day21.txt");
//            input = @"../.# => ##./#../...
//.#./..#/### => #..#/..../..../#..#";
            rules = new List<Rule>();
            foreach (var row in input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                rules.Add(new Rule(row));
            }
            
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getLitPixels(5);
            var solutionPart_2 = getLitPixels(18);

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getLitPixels(int iterations)
        {
            var startString = ".#./..#/###";
            char[,] matrix = GetMatrixFromString(startString);
            
            List<char[,]> unmergedArrays;
            List<string> unmergedArrayStrings;

            for (int iter = 0; iter < iterations; iter++)
            {
                Console.WriteLine(iter);
                unmergedArrays = new List<char[,]>();
                unmergedArrayStrings = new List<string>();
                var size = matrix.GetLength(0);

                if (matrix.GetLength(0) % 2 == 0)
                {
                    for (int qRows = 0; qRows < size; qRows += 2)
                    {
                        for (int qCols = 0; qCols < size; qCols += 2)
                        {
                            //Get new partarray
                            char[,] temp = new char[2, 2];
                            var tRow = 0;
                            var tCol = 0;
                            for (int row = qRows; row < qRows + 2; row++)
                            {
                                for (int column = qCols; column < qCols + 2; column++)
                                {
                                    temp[tRow, tCol] = matrix[row, column];
                                    tCol++;
                                }
                                tCol = 0;
                                tRow++;
                            }
                            var matchString = GetStringFromMatrix(temp);
                            var newArrayString = rules.Where(x => x.MatchPattern.Contains(matchString)).Select(x => x.ArrayPattern).First();
                            temp = GetMatrixFromString(newArrayString.ToString());
                            unmergedArrays.Add(temp);
                            unmergedArrayStrings.Add(newArrayString);
                        }
                    }
                }
                else
                {
                    for (int qRows = 0; qRows < size; qRows += 3)
                    {
                        for (int qCols = 0; qCols < size; qCols += 3)
                        {
                            //Get new partarray
                            char[,] temp = new char[3, 3];
                            var tRow = 0;
                            var tCol = 0;
                            for (int row = qRows; row < qRows + 3; row++)
                            {
                                for (int column = qCols; column < qCols + 3; column++)
                                {
                                    temp[tRow, tCol] = matrix[row, column];
                                    tCol++;
                                }
                                tCol = 0;
                                tRow++;
                            }
                            var matchString = GetStringFromMatrix(temp);
                            var newArrayString = rules.Where(x => x.MatchPattern.Contains(matchString)).Select(x => x.ArrayPattern).First();
                            temp = GetMatrixFromString(newArrayString.ToString());
                            unmergedArrays.Add(temp);
                            unmergedArrayStrings.Add(newArrayString);
                        }
                    }
                }

                // MergeArrays into matrix
                matrix = MergeArrays(unmergedArrays);

            }

            var countMatrix = GetStringFromMatrix(matrix);
            var hashCount = countMatrix.Where(x => x == '#').Count();
            return hashCount;
        }

        private static char[,] MergeArrays(List<char[,]> unmergedArrays)
        {
            if (unmergedArrays.Count == 1)
            {
                return unmergedArrays[0];
            }

            var partSize = unmergedArrays[0].GetLength(0);
            var newSize = partSize * Convert.ToInt32(Math.Sqrt(unmergedArrays.Count));

            var parts = newSize / partSize;
            var partCol = 0;
            var partRow = 0;

            char[,] returnMatrix = new char[newSize, newSize];

            var tRow = 0;
            var tCol = 0;

            for (int part = 0; part < unmergedArrays.Count; part++)
            {
                tCol = 0 + (partCol * partSize);
                tRow = 0 + (partRow * partSize);

                for (int pRow = 0; pRow < unmergedArrays[part].GetLength(0); pRow++)
                {
                    for (int pCol = 0; pCol < unmergedArrays[part].GetLength(1); pCol++)
                    {
                        returnMatrix[tRow, tCol] = unmergedArrays[part][pRow, pCol];
                        tCol++;
                    }
                    tRow++;
                    tCol -= partSize;
                }

                // Find new Row and Col Group
                partCol++;
                if (partCol >= (newSize / partSize))
                {
                    partCol = 0;
                    partRow++;
                }
            }

            return returnMatrix;
        }

        private static char[,] GetMatrixFromString(string matrixString)
        {
            var lines = matrixString.Split('/');
            char[,] matrix = new char[lines.Length, lines.Length];

            int row = 0;
            foreach (string line in lines)
            {
                int column = 0;
                foreach (char character in line)
                {
                    matrix[row, column] = character;
                    column++;
                }
                row++;
            }

            return matrix;
        }

        public static string GetStringFromMatrix(char[,] tArray)
        {
            string lReturn = "";

            for (int i = 0; i < tArray.GetLength(0); i++)
            {
                for (int j = 0; j < tArray.GetLength(1); j++)
                {
                    lReturn += tArray[i, j];
                }
                lReturn += "/";
            }

            lReturn = lReturn.Substring(0, lReturn.Length - 1);
            return lReturn;
        }

        private static char[,] RotateMatrix(char[,] matrix)
        {
            var n = matrix.GetLength(0);
            char[,] ret = new char[n, n];

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    ret[i, j] = matrix[n - j - 1, i];
                }
            }

            return ret;
        }

        public static char[,] FlipMatrix(char[,] tArray, bool Horisontal)
        {
            char[,] flippedArray = new char[tArray.GetLength(0), tArray.GetLength(0)];

            for (int i = 0; i < tArray.GetLength(0); i++)
            {
                for (int j = 0; j < tArray.GetLength(1); j++)
                {
                    if (Horisontal)
                    {
                        flippedArray[i, j] = tArray[(tArray.GetLength(0) - 1) - i, j];
                    }
                    else
                    {
                        flippedArray[i, j] = tArray[i, (tArray.GetLength(1) - 1) - j];
                    }
                }
            }

            return flippedArray;
        }

        public class Rule
        {
            public int Size { get; set; }
            public List<string> MatchPattern { get; set; }
            public string ArrayPattern { get; set; }

            public Rule(string ruleRow)
            {

                var ruleSplit = ruleRow.Split(new string[] { " => " }, StringSplitOptions.RemoveEmptyEntries);

                this.MatchPattern = new List<string>();
                this.ArrayPattern = ruleSplit[1].Trim();
                this.Size = ruleSplit[0].Where(x => x == '/').Count() + 1;

                // Add all flipped / rotated arrays to MatchPattern
                char[,] tArray = GetMatrixFromString(ruleSplit[0]);

                for (int i = 0; i < 4; i++)
                {
                    // Add tArray
                    MatchPattern.Add(GetStringFromMatrix(tArray));

                    // Flip & add horizontal
                    var fArray = FlipMatrix(tArray, true);
                    MatchPattern.Add(GetStringFromMatrix(fArray));

                    // Flip & add vertical
                    fArray = FlipMatrix(tArray, false);
                    MatchPattern.Add(GetStringFromMatrix(fArray));

                    // Rotate
                    tArray = RotateMatrix(tArray);
                }
            }
        }
    }
}