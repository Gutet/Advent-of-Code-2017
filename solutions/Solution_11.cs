using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_11
    {
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day11.txt");

            var maxDistance = 0;
            Debug.Assert(getHexDistance("ne,ne,ne", ref maxDistance) == 3);
            Debug.Assert(getHexDistance("ne,ne,sw,sw", ref maxDistance) == 0);
            Debug.Assert(getHexDistance("ne,ne,s,s", ref maxDistance) == 2);
            Debug.Assert(getHexDistance("se,sw,se,sw,sw", ref maxDistance) == 3);

            Stopwatch sw = new Stopwatch();
            sw.Start();           

            // Solution goes here
            var solutionPart_1 = getHexDistance(input, ref maxDistance);
            var solutionPart_2 = maxDistance;
            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getHexDistance(string path, ref int maxDistance)
        {
            var pathArray = path.Split(',').Select(s => s).ToArray();

            var xPos = 0;
            var yPos = 0;
            var zPos = 0;

            foreach (var direction in pathArray)
            {
                setNexPos(ref xPos, ref yPos, ref zPos, direction);
                maxDistance = (getHexDistance(0, 0, 0, xPos, yPos, zPos) > maxDistance) ? getHexDistance(0, 0, 0, xPos, yPos, zPos) : maxDistance;
            }

            var distance = getHexDistance(0, 0, 0, xPos, yPos, zPos);

            return distance;
        }

        private static int getHexDistance(int x1, int y1, int z1, int xPos, int yPos, int zPos)
        {
            return (Math.Abs(x1 - xPos) + Math.Abs(y1 - yPos) + Math.Abs(z1 - zPos)) / 2;
        }

        private static void setNexPos(ref int xPos, ref int yPos, ref int zPos, string direction)
        {
            switch (direction)
            {
                case "nw":
                    yPos += 1;
                    zPos -= 1;
                    break;
                case "sw":
                    yPos += 1;
                    xPos -= 1;
                    break;
                case "s":
                    zPos += 1;
                    xPos -= 1;
                    break;
                case "se":
                    zPos += 1;
                    yPos -= 1;
                    break;
                case "ne":
                    xPos += 1;
                    yPos -= 1;
                    break;
                case "n":
                    xPos += 1;
                    zPos -= 1;
                    break;
                default:
                    throw new Exception($"Unknown direction: {direction}");
            }
        }
    }
}
