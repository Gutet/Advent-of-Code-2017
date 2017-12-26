using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_25
    {
        public static string getResult()
        {
            var input = 12208951;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getCheckSum(input);
            var solutionPart_2 = 2;

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        public enum State { A, B, C, D, E, F };

        private static int getCheckSum(int input)
        {
            var tape = Enumerable.Repeat(0, 20000).ToArray();
            var cursor = tape.Length / 2;
            State state = State.A;

            for (int i = 0; i < input; i++)
            {
                switch (state)
                {
                    case State.A:
                        if (tape[cursor] == 0)
                        {
                            tape[cursor] = 1;
                            cursor++;
                            state = State.B;
                        }
                        else
                        {
                            tape[cursor] = 0;
                            cursor--;
                            state = State.E;
                        }
                        break;
                    case State.B:
                        if (tape[cursor] == 0)
                        {
                            tape[cursor] = 1;
                            cursor--;
                            state = State.C;
                        }
                        else
                        {
                            tape[cursor] = 0;
                            cursor++;
                            state = State.A;
                        }
                        break;
                    case State.C:
                        if (tape[cursor] == 0)
                        {
                            tape[cursor] = 1;
                            cursor--;
                            state = State.D;
                        }
                        else
                        {
                            tape[cursor] = 0;
                            cursor++;
                            state = State.C;
                        }
                        break;
                    case State.D:
                        if (tape[cursor] == 0)
                        {
                            tape[cursor] = 1;
                            cursor--;
                            state = State.E;
                        }
                        else
                        {
                            tape[cursor] = 0;
                            cursor--;
                            state = State.F;
                        }
                        break;
                    case State.E:
                        if (tape[cursor] == 0)
                        {
                            tape[cursor] = 1;
                            cursor--;
                            state = State.A;
                        }
                        else
                        {
                            tape[cursor] = 1;
                            cursor--;
                            state = State.C;
                        }
                        break;
                    case State.F:
                        if (tape[cursor] == 0)
                        {
                            tape[cursor] = 1;
                            cursor--;
                            state = State.E;
                        }
                        else
                        {
                            tape[cursor] = 1;
                            cursor++;
                            state = State.A;
                        }
                        break;
                }
            }

            return tape.Count(x => x == 1);
        }
    }
}
