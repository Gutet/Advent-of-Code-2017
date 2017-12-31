using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_24
    {
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day24.txt");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            foreach (var row in input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                componentList.Add(new Component(row));
            }

            BuildBridges(0, componentList, null);

            var max = 0;
            var longestLength = 0;
            var longestStrength = 0;
            foreach (var item in Bridges)
            {
                var t = 0;
                foreach (var component in item)
                {
                    t = t + component.Strength();
                }
                if (t > max)
                {
                    max = t;
                }

                if (item.Count() >= longestLength)
                {                    
                    if (item.Count() == longestLength)
                    {
                        if (longestStrength < t)
                        {
                            longestStrength = t;
                        }
                    }
                    else
                    {
                        longestStrength = t;
                    }

                    longestLength = item.Count();
                }
            }
            var solutionPart_1 = max;
            var solutionPart_2 = longestStrength;

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static void BuildBridges(int value, List<Component> remainingList, List<Component> existingList)
        {
            if (existingList == null)
            {
                existingList = new List<Component>();
            }

            var listToLoop = remainingList.Where(x => x.Contains(value));

            if (listToLoop.Count() == 0)
            {
                Bridges.Add(existingList);
            }

            foreach (var item in listToLoop)
            {
                var newList = new List<Component>(remainingList.Where(x => x != item));
                
                var sendList = new List<Component>(existingList);
                sendList.Add(item);

                BuildBridges(item.Next(value), newList, sendList);
            }            
        }

        static List<List<Component>> Bridges = new List<List<Component>>();
        static List<Component> componentList = new List<Component>();

        class Component
        {
            public int PortA { get; set; }
            public int PortB { get; set; }

            public Component(string ports)
            {
                var split = ports.Split('/').Select(s => int.Parse(s.Trim())).ToList();
                PortA = split[0];
                PortB = split[1];
            }

            public bool Contains(int value)
            {
                return PortA == value || PortB == value;
            }

            public int Next(int value)
            {
                return (PortA == value) ? PortB : PortA;
            }

            public int Strength()
            {
                return PortA + PortB;
            }
        }
    }
}
