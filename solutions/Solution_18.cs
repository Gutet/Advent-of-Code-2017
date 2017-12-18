using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_18
    {
        public static Dictionary<string, long> regListPartOne;
        public static List<Dictionary<string, long>> regLists;
        public static List<Queue<long>> programQueues;
        public static long lastSound;
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day18.txt");

            Debug.Assert(getFirstFrequency(@"set a 1
add a 2
mul a a
mod a 5
snd a
set a 0
rcv a
jgz a -1
set a 1
jgz a -2") == 4);

            Debug.Assert(getSndCount(@"snd 1
snd 2
snd p
rcv a
rcv b
rcv c
rcv d") == 3);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getFirstFrequency(input);
            var solutionPart_2 = getSndCount(input);

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getSndCount(string input)
        {
            regLists = new List<Dictionary<string, long>>();
            regLists.Add(new Dictionary<string, long>());
            regLists.Add(new Dictionary<string, long>());
            programQueues = new List<Queue<long>>();
            programQueues.Add(new Queue<long>());
            programQueues.Add(new Queue<long>());

            regLists[0]["p"] = 0;
            regLists[1]["p"] = 1;

            var previousPos = 0;
            var tempPos = 0;

            bool switchProgram = false;

            var currentPos = 0;
            var currentProgram = 0;

            var commandList = input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var command = "";
            var arguments = "";

            bool deadlocked = false;
            bool possibleDeadLock = false;
            bool doJump = false;
            int p1SndCount = 0;

            while (!deadlocked)
            {
                switchProgram = false;
                command = commandList[currentPos].Substring(0, 3);
                arguments = commandList[currentPos].Substring(4);

                switch (command)
                {
                    case "snd":
                        if (currentProgram == 1)
                        {
                            p1SndCount++;
                        }
                        programQueues[currentProgram].Enqueue(getNumeric(arguments, regLists[currentProgram]));
                        possibleDeadLock = false;
                        break;
                    case "set":
                        setRegistry(arguments, regLists[currentProgram]);
                        break;
                    case "add":
                        addRegistry(arguments, regLists[currentProgram]);
                        break;
                    case "mul":
                        multiplyRegistry(arguments, regLists[currentProgram]);
                        break;
                    case "mod":
                        modRegistry(arguments, regLists[currentProgram]);
                        break;
                    case "rcv":
                        //Check other queue
                        var queueToCheck = (currentProgram == 0) ? 1 : 0;

                        //if empty - switch programs mark possibleDeadlock
                        if (programQueues[queueToCheck].Count == 0)
                        {
                            switchProgram = true;
                            currentProgram = queueToCheck;
                            tempPos = currentPos;
                            currentPos = previousPos;
                            previousPos = tempPos;
                            if (possibleDeadLock)
                            {
                                deadlocked = true;
                                break;
                            }

                            possibleDeadLock = true;
                        }
                        else
                        {
                            // Receive input
                            possibleDeadLock = false;
                            regLists[currentProgram][arguments] = programQueues[queueToCheck].Dequeue();
                        }

                        break;
                    case "jgz":
                        if (getNumeric(arguments.Split(' ')[0], regLists[currentProgram]) > 0)
                        {
                            currentPos += Convert.ToInt32(getNumeric(arguments.Split(' ')[1], regLists[currentProgram]));
                            doJump = true;
                        }
                        break;
                    default:
                        throw new ArgumentException("Invalid operator");
                }

                if (!switchProgram)
                {
                    if (!doJump)
                    {
                        currentPos++;
                    }
                }
                doJump = false;
            }

            return p1SndCount;
        }

        private static long getFirstFrequency(string data)
        {
            regListPartOne = new Dictionary<string, long>();
            var commandList = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var command = "";
            var arguments = "";

            for (int i = 0; i < commandList.Length; i++)
            {
                command = commandList[i].Substring(0, 3);
                arguments = commandList[i].Substring(4);

                switch (command)
                {
                    case "snd":
                        lastSound = regListPartOne[arguments];
                        break;
                    case "set":
                        setRegistry(arguments, regListPartOne);
                        break;
                    case "add":
                        addRegistry(arguments, regListPartOne);
                        break;
                    case "mul":
                        multiplyRegistry(arguments, regListPartOne);
                        break;
                    case "mod":
                        modRegistry(arguments, regListPartOne);
                        break;
                    case "rcv":
                        if (regListPartOne[arguments] != 0)
                        {
                            return lastSound;
                        }
                        break;
                    case "jgz":
                        if (getNumeric(arguments.Split(' ')[0], regListPartOne) > 0)
                        {
                            i += Convert.ToInt32(arguments.Split(' ')[1]);
                            if (Convert.ToInt32(arguments.Split(' ')[1]) > 0)
                            {
                                i++;
                            }
                            else
                            {
                                i--;
                            }
                        }
                        break;
                    default:
                        throw new ArgumentException("Invalid operator");
                }
            }

            return 0;
        }

        private static void modRegistry(string values, Dictionary<string, long> regList)
        {
            var registry = values.Split(' ')[0];
            var value = getNumeric(values.Split(' ')[1], regList);

            createRegIfNeeded(registry, regList);

            regList[registry] = regList[registry] % value;
        }

        private static void multiplyRegistry(string values, Dictionary<string, long> regList)
        {
            var registry = values.Split(' ')[0];
            var value = getNumeric(values.Split(' ')[1], regList);

            createRegIfNeeded(registry, regList);

            regList[registry] *= value;
        }

        private static void addRegistry(string values, Dictionary<string, long> regList)
        {
            var registry = values.Split(' ')[0];
            var value = getNumeric(values.Split(' ')[1], regList);

            createRegIfNeeded(registry, regList);

            regList[registry] += value;
        }

        private static void setRegistry(string values, Dictionary<string, long> regList)
        {
            var registry = values.Split(' ')[0];
            var value = getNumeric(values.Split(' ')[1], regList);

            regList[registry] = value;
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
