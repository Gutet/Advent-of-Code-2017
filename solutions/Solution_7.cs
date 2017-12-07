using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_7
    {
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day7.txt");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<programNode> programNodes = new List<programNode>();
            var inputRows = input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in inputRows)
            {
                programNodes.Add(new programNode(item));
            }

            // Solution goes here
            var solutionPart_1 = getBottomName(programNodes);
            var solutionPart_2 = getWeightAbnormality(programNodes, solutionPart_1.ToString());

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static string getWeightAbnormality(List<programNode> programNodes, string startProgram)
        {
            string lReturn = "";
            setParentsAndWeight(programNodes, startProgram, "");

            var weightLinq = from p in programNodes
                             group p by p.parentName into g
                             select new { Name = g.Key, weights = g.ToList() };

            foreach (var item in weightLinq)
            {
                var weightShouldBe = item.weights[0].programWeight;
                for (int i = 1; i < item.weights.Count; i++)
                {
                    if (item.weights[i].programWeight != weightShouldBe)
                    {
                        var originalProgramWeight = item.weights[i].programWeight - item.weights[i].legWeight;
                        lReturn = (originalProgramWeight - (Math.Abs(weightShouldBe - item.weights[i].programWeight))).ToString();
                    }
                }
            }

            return lReturn;
        }

        private static int setParentsAndWeight(List<programNode> programNodes, string startProgram, string parentName)
        {
            programNode thisNode = programNodes.Where(x => x.programName == startProgram).First();

            thisNode.parentName = parentName;

            if (thisNode.subPrograms != null)
            {
                foreach (var item in thisNode.subPrograms)
                {
                    thisNode.legWeight += setParentsAndWeight(programNodes, item, startProgram);
                }
            }

            thisNode.programWeight += thisNode.legWeight;

            return thisNode.programWeight;
        }

        private static object getBottomName(List<programNode> nodes)
        {
            List<string> subPrograms = new List<string>();
            foreach (var item in nodes)
            {
                if (item.subPrograms != null)
                {
                    foreach (var subitem in item.subPrograms)
                    {
                        subPrograms.Add(subitem);
                    }
                }
            }
            return nodes.Where(x => subPrograms.Contains(x.programName) == false).Select(x => x.programName).FirstOrDefault();
        }
    }

    class programNode
    {
        public string programName { get; set; }
        public int programWeight { get; set; }
        public int legWeight { get; set; }
        public string parentName { get; set; }
        public List<string> subPrograms;

        public programNode(string programRow)
        {
            string[] rowParts = programRow.Split(new string[] { "-> " }, StringSplitOptions.RemoveEmptyEntries);
            string[] mainPart = rowParts[0].Split(' ');
            this.programName = mainPart[0].Trim();
            this.programWeight = Convert.ToInt32(mainPart[1].Trim().Replace("(", "").Replace(")",""));

            if (rowParts.Length == 2)
            {
                //SubPrograms
                subPrograms = new List<string>();
                foreach (var item in rowParts[1].Split(','))
                {
                    subPrograms.Add(item.Trim());
                }
            }
        }
    }
}
