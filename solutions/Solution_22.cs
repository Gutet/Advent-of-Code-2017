using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.solutions
{
    public static class Solution_22
    {
        enum State { Clean, Weakened, Infected, Flagged }
        public static Dictionary<Tuple<int, int>, char> grid;
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day22.txt");
            grid = new Dictionary<Tuple<int, int>, char>();

            Debug.Assert(getInfectedBursts(@"..#
#..
...", 10000) == 5587);

            Debug.Assert(getInfectedBursts(@"..#
#..
...", 100, true) == 26);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getInfectedBursts(input, 10000);
            var solutionPart_2 = getInfectedBursts(input, 10000000, true);

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        static int x = 0;
        static int y = 0;
        static int infectedBursts = 0;
        enum Direction { Up, Right, Down, Left };
        static Direction d = Direction.Up;

        public static int getInfectedBursts(string map, int iterations, bool part2 = false)
        {
            infectedBursts = 0;
            d = Direction.Up;
            setGridFromInput(map);

            for (int i = 0; i < iterations; i++)
            {
                if (part2)
                {
                    DoBurstPart2();
                }
                else
                {
                    DoBurst();
                }
            }
            
            return infectedBursts;
        }

        private static void DoBurstPart2()
        {
            char infected = grid.GetOrCreate(new Tuple<int, int>(x, y));
            TurnChangeStateAndMovePart2(infected);
        }

        private static void TurnChangeStateAndMovePart2(char infected)
        {
            switch (infected)
            {
                case '\0':
                case '.':
                    grid[new Tuple<int, int>(x, y)] = 'W';
                    TurnAndMoveLeft();
                    break;
                case '#':
                    grid[new Tuple<int, int>(x, y)] = 'F';
                    TurnAndMoveRight();
                    break;
                case 'F':
                    grid[new Tuple<int, int>(x, y)] = '.';
                    TurnAndMoveBack();
                    break;
                default:
                    grid[new Tuple<int, int>(x, y)] = '#';
                    MoveForward();
                    infectedBursts++;
                    break;

            }
        }

        private static void MoveForward()
        {
            switch (d)
            {
                case Direction.Up:
                    y--;
                    break;
                case Direction.Right:
                    x++;
                    break;
                case Direction.Down:
                    y++;
                    break;
                default:
                    x--;
                    break;
            }
        }

        private static void TurnAndMoveBack()
        {
            switch (d)
            {
                case Direction.Up:
                    d = Direction.Down;
                    y++;
                    break;
                case Direction.Right:
                    d = Direction.Left;
                    x--;
                    break;
                case Direction.Down:
                    d = Direction.Up;
                    y--;
                    break;
                default:
                    d = Direction.Right;
                    x++;
                    break;
            }
        }

        private static void TurnAndMoveRight()
        {
            switch (d)
            {
                case Direction.Up:
                    d = Direction.Right;
                    x++;
                    break;
                case Direction.Right:
                    d = Direction.Down;
                    y++;
                    break;
                case Direction.Down:
                    d = Direction.Left;
                    x--;
                    break;
                default:
                    d = Direction.Up;
                    y--;
                    break;
            }
        }

        private static void TurnAndMoveLeft()
        {
            switch (d)
            {
                case Direction.Up:
                    d = Direction.Left;
                    x--;
                    break;
                case Direction.Right:
                    d = Direction.Up;
                    y--;
                    break;
                case Direction.Down:
                    d = Direction.Right;
                    x++;
                    break;
                default:
                    d = Direction.Down;
                    y++;
                    break;
            }
        }

        private static void DoBurst()
        {
            char infected = grid.GetOrCreate(new Tuple<int, int>(x, y));
            TurnChangeStateAndMove(infected);
        }

        private static void TurnChangeStateAndMove(char infected)
        {
            if (infected == '#')
            {
                grid[new Tuple<int, int>(x, y)] = '.';
                TurnAndMoveRight();
            }
            else
            {
                grid[new Tuple<int, int>(x, y)] = '#';
                infectedBursts++;
                TurnAndMoveLeft();
            }
        }

        private static void setGridFromInput(string map)
        {
            grid = new Dictionary<Tuple<int, int>, char>();
            var lines = map.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var xPos = 0;
            var yPos = 0;
            foreach (var row in lines)
            {
                foreach (var pos in row)
                {
                    grid.Add(new Tuple<int, int>(xPos, yPos ), pos);
                    xPos++;
                }
                yPos++;
                xPos = 0;
            }
            x = lines[0].Length / 2;
            y = lines.Length / 2;
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
    where TValue : new()
        {
            TValue val;

            if (!dict.TryGetValue(key, out val))
            {
                val = new TValue();
                dict.Add(key, val);
            }

            return val;
        }
    }
}