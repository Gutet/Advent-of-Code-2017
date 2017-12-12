using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_12
    {
        static List<int> connectedHouses = new List<int>();
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day12.txt");

            Debug.Assert(getConnectedCount(@"0 <-> 2
1 <-> 1
2 <-> 0, 3, 4
3 <-> 2, 4
4 <-> 2, 3, 6
5 <-> 6
6 <-> 4, 5", 0) == 6);
            connectedHouses = new List<int>();

            Debug.Assert(getGroupCount(@"0 <-> 2
1 <-> 1
2 <-> 0, 3, 4
3 <-> 2, 4
4 <-> 2, 3, 6
5 <-> 6
6 <-> 4, 5", 0) == 2);
            connectedHouses = new List<int>();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getConnectedCount(input, 0);
            connectedHouses = new List<int>();
            var solutionPart_2 = getGroupCount(input, 0);

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getGroupCount(string input, int group)
        {
            List<House> programList = getHouseList(input);
            var totalGroupCount = 0;

            while (true)
            {
                setConnectedList(programList, group);
                totalGroupCount++;

                if (connectedHouses.Distinct().Count() == programList.Count)
                {
                    return totalGroupCount;
                }

                var newStartNode = programList.Where(x => connectedHouses.Contains(x.Id) == false).FirstOrDefault();
                group = newStartNode.Id;
            }
        }

        private static int getConnectedCount(string input, int group)
        {
            List<House> programList = getHouseList(input);

            setConnectedList(programList, group);

            return connectedHouses.Distinct().Count();
        }

        private static void setConnectedList(List<House> programList, int startGroup)
        {
            if (connectedHouses.Contains(startGroup) == false)
            {
                connectedHouses.Add(startGroup);
            }

            var thisHouse = programList.Where(x => x.Id == startGroup).FirstOrDefault();

            var neighbours = thisHouse.Neighbour.Except(connectedHouses).ToList();
            foreach (var item in neighbours)
            {
                setConnectedList(programList, item);
            }
            
        }

        private static List<House> getHouseList(string input)
        {
            List<House> lReturn = new List<House>();
            
            foreach (var row in input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var lTemp = row.Split(new string[] { " <-> " }, StringSplitOptions.RemoveEmptyEntries);
                lReturn.Add(new House(lTemp[0], lTemp[1]));
            }

            return lReturn;
        }
    }

    public class House
    {
        public int Id { get; set; }
        public List<int> Neighbour { get; set; }

        public House (string i, string n)
        {
            this.Id = Convert.ToInt32(i);
            this.Neighbour = n.Split(',').Select(s => int.Parse(s.Trim())).ToList<int>();
        }
    }
}
