using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.solutions
{
    public static class Solution_8
    {
        public static Dictionary<string, int> registryList = new Dictionary<string, int>();
        public static int totalMax;
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day8.txt");

            Debug.Assert(getHighestValue(@"b inc 5 if a > 1
a inc 1 if b < 5
c dec -10 if a >= 1
c inc -20 if c == 10") == 1);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            registryList = new Dictionary<string, int>();
            var solutionPart_1 = getHighestValue(input);

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {totalMax}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getHighestValue(string input)
        {
            List<Command> commandList = getCommandList(input);

            processCommandList(commandList);
            return registryList.Values.Max();
        }

        private static void processCommandList(List<Command> commandList)
        {
            foreach (var command in commandList)
            {
                createKeysIfNeeded(command);
                if (compareResult(command) == true)
                {
                    performOperation(command);
                }
            }
        }

        private static void performOperation(Command command)
        {
            switch (command.operation)
            {
                case "inc":
                    registryList[command.register] += command.value;
                    break;
                case "dec":
                    registryList[command.register] -= command.value;
                    break;
                default:
                    throw new ArgumentException(command.operation);
            }
            totalMax = (registryList.Values.Max() > totalMax) ? registryList.Values.Max() : totalMax;
        }

        private static bool compareResult(Command command)
        {
            switch (command.compareOperator)
            {
                case "<":
                    return (registryList[command.compareRegister] < command.compareValue);
                case ">":
                    return (registryList[command.compareRegister] > command.compareValue);
                case "<=":
                    return (registryList[command.compareRegister] <= command.compareValue);
                case ">=":
                    return (registryList[command.compareRegister] >= command.compareValue);
                case "==":
                    return (registryList[command.compareRegister] == command.compareValue);
                case "!=":
                    return (registryList[command.compareRegister] != command.compareValue);
                default:
                    throw new ArgumentException(command.compareOperator);
            }
        }

        private static void createKeysIfNeeded(Command command)
        {
            if (registryList.ContainsKey(command.compareRegister) == false)
            {
                registryList.Add(command.compareRegister, 0);
            }
            if (registryList.ContainsKey(command.register) == false)
            {
                registryList.Add(command.register, 0);
            }
        }

        private static List<Command> getCommandList(string input)
        {
            List<Command> lReturn = new List<Command>();
            var inputRows = input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in inputRows)
            {
                lReturn.Add(new Command(item));
            }
            return lReturn;
        }
    }

    class Command
    {
        public string register { get; set; }
        public string operation { get; set; }
        public int value { get; set; }
        public string compareRegister { get; set; }
        public string compareOperator { get; set; }
        public int compareValue { get; set; }

        public Command(string commandLine)
        {
            string[] commandSplit = commandLine.Split(' ');
            this.register = commandSplit[0].Trim();
            this.operation = commandSplit[1].Trim();
            this.value = Convert.ToInt32(commandSplit[2].Trim());
            this.compareRegister = commandSplit[4].Trim();
            this.compareOperator = commandSplit[5].Trim();
            this.compareValue = Convert.ToInt32(commandSplit[6].Trim());
        }
    }
}