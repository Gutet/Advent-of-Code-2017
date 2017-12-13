using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
            var solutionPart_2 = getMinDelay(fireWall, 3897600);

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

                if (getTripSeverity(fireWall) == 0)
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

        private static int getTripSeverity(List<FireWallNode> fireWallIn)
        {
            var severity = 0;
            var gotCaught = false;

            List<FireWallNode> fireWall = new List<FireWallNode>();
            foreach (var item in fireWallIn)
            {
                fireWall.Add(new FireWallNode(item.Nodes, item.Position, item.DirectionUp));
            }

            for (int i = 0; i < fireWall.Count; i++)
            {
                // Check Severity - caught if top position
                if (fireWall[i].Position == 1)
                {
                    severity += i * fireWall[i].Nodes;
                    gotCaught = true;
                }

                // Move scanners
                fireWall = moveScanners(fireWall);
            }

            return (severity == 0 && gotCaught) ? 1 : severity;
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
