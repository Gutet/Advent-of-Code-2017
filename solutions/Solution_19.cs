using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_19
    {
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day19.txt");
            int steps = 0;

            Debug.Assert(getPath(@"    |         
    |  +--+   
    A  |  C   
F---|----E|--+
    |  |  |  D
    +B-+  +--+", ref steps) == "ABCDEF");
            Debug.Assert(steps == 38);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            steps = 0;

            // Solution goes here
            var solutionPart_1 = getPath(input, ref steps);
            var solutionPart_2 = steps;

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        enum direction { Up, Right, Down, Left };

        private static string getPath(string input, ref int steps)
        {
            char[,] mapArray = getMapArray(input);

            direction d = direction.Down;

            var path = "";
            var xPos = getStartingPosition(mapArray);
            var yPos = 0;
            
            if (xPos == -1)
            {
                throw new Exception("Invalid starting position");
            }

            while (true)
            {
                if (outsideBounds(mapArray, xPos, yPos) || mapArray[yPos, xPos] == ' ')
                {
                    break;
                }

                switch (mapArray[yPos, xPos])
                {
                    case '|':
                    case '-':
                        break;
                    case '+':
                        d = changeDirection(d, mapArray, xPos, yPos);
                        break;
                    default:
                        path += mapArray[yPos, xPos].ToString();
                        break;
                }
                getNewPosition(d, ref xPos, ref yPos);
                steps++;
            }

            return path.Trim();
        }

        private static bool outsideBounds(char[,] mapArray, int xPos, int yPos)
        {
            if (xPos > mapArray.GetLength(1) - 1 || xPos < 0)
                return true;

            if (yPos > mapArray.GetLength(0) - 1 || yPos < 0)
                return true;

            return false;
        }

        private static direction changeDirection(direction d, char[,] mapArray, int xPos, int yPos)
        {
            if (d == direction.Down || d == direction.Up)
            {
                if (outsideBounds(mapArray, xPos - 1, yPos) == true || mapArray[yPos, xPos - 1] == ' ')
                {
                    return direction.Right;
                }
                return direction.Left;
            }
            else
            {
                if (outsideBounds(mapArray, xPos, yPos - 1) == true || mapArray[yPos - 1, xPos] == ' ')
                {
                    return direction.Down;
                }
                return direction.Up;
            }            
        }

        private static void getNewPosition(direction d, ref int xPos, ref int yPos)
        {
            switch (d)
            {
                case direction.Up:
                    yPos--;
                    break;
                case direction.Right:
                    xPos++;
                    break;
                case direction.Down:
                    yPos++;
                    break;
                case direction.Left:
                    xPos--;
                    break;
            }
        }

        private static int getStartingPosition(char[,] mapArray)
        {
            for (int i = 0; i < mapArray.GetLength(1); i++)
            {
                if (mapArray[0, i] == '|')
                    return i;
            }

            return -1;
        }

        private static char[,] getMapArray(string input)
        {
            var lines = input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            char[,] charArray = new char[lines.Length, lines[0].Length];

            int row = 0;
            foreach (string line in lines)
            {
                int column = 0;
                foreach (char character in line)
                {
                    charArray[row, column] = character;
                    column++;
                }
                row++;
            }

            return charArray;
        }
    }
}
