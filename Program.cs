using AdventOfCode.solutions;
using System;
using System.Reflection;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] solvedDays = new int[] { 21 };

            foreach (var day in solvedDays)
            {
                 showResult(day, Type.GetType($"AdventOfCode.solutions.Solution_{day}").GetMethod($"getResult").Invoke(null, null).ToString());
            }

            Console.ReadLine();
        }

        private static void showResult(int day, string result)
        {
            Console.WriteLine($"December {day}{Environment.NewLine}{result}{Environment.NewLine}");
        }
    }
}
