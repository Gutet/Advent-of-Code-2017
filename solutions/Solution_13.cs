using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace AdventOfCode.solutions
{
    public static class Solution_13
    {
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day13.txt");

            List<FireWallNode> fireWall = getFireWallFromInput(input);

            Debug.Assert(getTripSeverity(getFireWallFromInput(@"0: 3
1: 2
4: 4
6: 4")) == 24);

            Debug.Assert(getMinDelay(getFireWallFromInput(@"0: 3
1: 2
4: 4
6: 4"), 0) == 10);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            fireWall = getFireWallFromInput(input);
            var solutionPart_1 = getTripSeverity(fireWall);
            fireWall = getFireWallFromInput(input);
            var solutionPart_2 = getMinDelay(fireWall, 0);

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getMinDelay(List<FireWallNode> fireWall, int startDelay)
        {
            var delay = startDelay;

            if (delay > 0)
            {
                for (int i = 0; i < delay; i++)
                {
                    fireWall = moveScanners(fireWall);
                }
            }

            while (true)
            {
                delay++;
                fireWall = moveScanners(fireWall);

                if (getTripSeverity(fireWall, true) == 0)
                {
                    break;
                }

                if (delay % 100000 == 0)
                {
                    Console.WriteLine($"Still here ({delay})");
                }
            }

            return delay;
        }

        private static int getTripSeverity(List<FireWallNode> fireWallIn, bool forceBreak = false)
        {
            var severity = 0;
            
            List<FireWallNode> fireWall = fireWallIn.ConvertAll(x => new FireWallNode(x.Nodes, x.Position, x.DirectionUp));

            for (int i = 0; i < fireWall.Count; i++)
            {
                // Check Severity - caught if top position
                if (fireWall[i].Position == 1)
                {
                    severity += i * fireWall[i].Nodes;
                    if (forceBreak)
                    {
                        return 1;
                    }
                }

                // Move scanners
                fireWall = moveScanners(fireWall);
            }

            return severity;
        }

        private static List<FireWallNode> moveScanners(List<FireWallNode> fireWall)
        {
            foreach (var scanner in fireWall)
            {
                if (scanner.Nodes > 0)
                {
                    if (scanner.DirectionUp)
                    {
                        scanner.Position--;
                        if (scanner.Position == 1)
                        {
                            scanner.DirectionUp = false;
                        }
                    }
                    else
                    {
                        scanner.Position++;
                        if (scanner.Position == scanner.Nodes)
                        {
                            scanner.DirectionUp = true;
                        }
                    }
                }
            }
            return fireWall;
        }

        private static List<FireWallNode> getFireWallFromInput(string input)
        {
            List<FireWallNode> lReturn = new List<FireWallNode>();

            var inputArray = input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var i = 0;

            foreach (var row in inputArray)
            {
                var rowSplit = row.Split(':').Select(s => int.Parse(s.Trim())).ToArray();
                FireWallNode lTemp; ;
                if (rowSplit[0] == i)
                {
                    lTemp = new FireWallNode(rowSplit[1]);
                    i++;
                }
                else
                {
                    while (i < rowSplit[0])
                    {
                        lReturn.Add(new FireWallNode(0));
                        i++;
                    }
                    lTemp = new FireWallNode(rowSplit[1]);
                    i++;
                }

                lReturn.Add(lTemp);
            }

            return lReturn;
        }
    }

    public class FireWallNode
    {
        public int Nodes { get; set; }
        public int Position { get; set; }
        public bool DirectionUp { get; set; }

        public FireWallNode(int depth)
        {
            this.Nodes = depth;
            this.Position = (depth > 0) ? 1 : -1;
            this.DirectionUp = false;
        }

        public FireWallNode(int depth, int position, bool directionUp)
        {
            this.Nodes = depth;
            this.Position = position;
            this.DirectionUp = directionUp;
        }
    }
}
