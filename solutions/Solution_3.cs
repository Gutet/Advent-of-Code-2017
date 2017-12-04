using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_3
    {
        enum Direction { Right, Up, Left, Down }
        static List<Node> nodeList = new List<Node>();
        static int firstValueLargerThanInput = 0;
        static int input = 361527;

        public static string getResult()
        {
            Direction currentDirection = Direction.Right;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var xPos = 0;
            var yPos = 0;
            var step = 1;
            var current = 1;
            
            nodeList.Add(new Node(xPos, yPos, current));

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

            return $"Part 1: {distancePart_1}{Environment.NewLine}Part 2: {firstValueLargerThanInput}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static void doCurrentDirection(ref int xPos, ref int yPos, ref Direction currentDirection, ref int current, int step, int input)
        {
            for (int i = 0; i < step; i++)
            {
                current++;

                addXorY(ref xPos, ref yPos, currentDirection);
                nodeList.Add(new Node(xPos, yPos, getNearbySum(xPos, yPos)));
                if (current == input)
                {
                    return;
                }
            }

            currentDirection = getNextDirection(currentDirection);
        }

        private static int getNearbySum(int xPos, int yPos)
        {
            if (firstValueLargerThanInput == 0)
            {
                var nearbyNodes = from n in nodeList
                                  where
                                    (n.xPos >= (xPos - 1) && n.xPos <= (xPos + 1))
                                    &&
                                    (n.yPos >= (yPos - 1) && n.yPos <= (yPos + 1))
                                  select
                                  n;
                var sum = nearbyNodes.Sum(x => x.value);


                if (sum > input && firstValueLargerThanInput == 0)
                {
                    firstValueLargerThanInput = sum;
                }

                return sum;
            }
            return 0;
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

    class Node
    {
        public int xPos { get; set; }
        public int yPos { get; set; }
        public int value { get; set; }

        public Node(int x, int y, int value)
        {
            this.xPos = x;
            this.yPos = y;
            this.value = value;
        }
    }
}
