using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_3
    {
        enum Direction { Right, Up, Left, Down }

        public static string getResult()
        {
            var input = 361527;

            Direction currentDirection = Direction.Right;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var xPos = 0;
            var yPos = 0;
            var step = 1;
            var current = 1;

            // Loop to target
            while (current < input)
            {
                doCurrentDirection(ref xPos, ref yPos, ref currentDirection, ref current, step, input);
                if (current == input)
                {
                    break;
                }
                doCurrentDirection(ref xPos, ref yPos, ref currentDirection, ref current, step, input);

                step++;
            };
            
            var distancePart_1 = Math.Abs(xPos) + Math.Abs(yPos);

            sw.Stop();

            return $"Part 1: {distancePart_1}{Environment.NewLine}Part 2: {Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static void doCurrentDirection(ref int xPos, ref int yPos, ref Direction currentDirection, ref int current, int step, int input)
        {
            for (int i = 0; i < step; i++)
            {
                current++;

                addXorY(ref xPos, ref yPos, currentDirection);
                if (current == input)
                {
                    return;
                }
            }

            currentDirection = getNextDirection(currentDirection);
        }

        private static void addXorY(ref int xPos, ref int yPos, Direction currentDirection)
        {
            switch (currentDirection)
            {
                case Direction.Right:
                    xPos += 1;
                    break;
                case Direction.Up:
                    yPos -= 1;
                    break;
                case Direction.Left:
                    xPos -= 1;
                    break;
                default:
                    yPos += 1;
                    break;
            }
        }

        private static Direction getNextDirection(Direction currentDirection)
        {
            switch (currentDirection)
            {
                case Direction.Right:
                    currentDirection = Direction.Up;
                    break;
                case Direction.Up:
                    currentDirection = Direction.Left;
                    break;
                case Direction.Left:
                    currentDirection = Direction.Down;
                    break;
                default:
                    currentDirection = Direction.Right;
                    break;
            }

            return currentDirection;
        }
    }
}
