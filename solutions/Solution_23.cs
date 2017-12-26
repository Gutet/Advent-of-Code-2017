using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_23
    {
        public static Dictionary<string, long> regList;
        public static List<Queue<long>> programQueues;
        public static long lastSound;
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day23.txt");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getMulTimes(input, 0);
            var temp = getMulTimes(input, 1);
            var solutionPart_2 = regList["h"];

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static long getMulTimes(string data, int aInitialValue)
        {
            regList = new Dictionary<string, long>();
            regList.Add("a", aInitialValue);
            regList.Add("b", 0);
            regList.Add("c", 0);
            regList.Add("d", 0);
            regList.Add("e", 0);
            regList.Add("f", 0);
            regList.Add("g", 0);
            regList.Add("h", 0);

            var commandList = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var command = "";
            var arguments = "";
            var mulTimes = 0;
            var position = 0;

            while (position >= 0 && position < commandList.Length)
            {
                command = commandList[position].Substring(0, 3);
                arguments = commandList[position].Substring(4);

                switch (command)
                {
                    case "set":
                        setRegistry(arguments, regList);
                        position++;
                        break;
                    case "sub":
                        subRegistry(arguments, regList);
                        position++;
                        break;                    
                    case "mul":
                        mulRegistry(arguments, regList);
                        position++;
                        mulTimes++;
                        break;
                    case "jnz":
                        if (getNumeric(arguments.Split(' ')[0], regList) != 0)
                        {
                            position += Convert.ToInt32(arguments.Split(' ')[1]);                            
                        }
                        else
                        {
                            position++;
                        }
                        break;
                    default:
                        throw new ArgumentException("Invalid operator");
                }
            }

            return mulTimes;
        }

        private static void mulRegistry(string values, Dictionary<string, long> regList)
        {
            var registry = values.Split(' ')[0];
            var value = getNumeric(values.Split(' ')[1], regList);

            createRegIfNeeded(registry, regList);

            regList[registry] *= value;
        }

        private static void setRegistry(string values, Dictionary<string, long> regList)
        {
            var registry = values.Split(' ')[0];
            var value = getNumeric(values.Split(' ')[1], regList);

            regList[registry] = value;
        }

        private static void subRegistry(string values, Dictionary<string, long> regList)
        {
            var registry = values.Split(' ')[0];
            var value = getNumeric(values.Split(' ')[1], regList);

            createRegIfNeeded(registry, regList);

            regList[registry] -= value;
        }

        private static long getNumeric(string value, Dictionary<string, long> regList)
        {
            long numvalue = 0;

            if (long.TryParse(value, out numvalue) == false)
            {
                createRegIfNeeded(value, regList);
                numvalue = regList[value];
            }

            return numvalue;
        }

        private static void createRegIfNeeded(string registry, Dictionary<string, long> regList)
        {
            if (regList.ContainsKey(registry) == false)
            {
                regList[registry] = 0;
            }
        }
    }
}
