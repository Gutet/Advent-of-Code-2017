using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.solutions
{
    public static class Solution_16
    {
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day16.txt");

            Debug.Assert(getOrder("abcde", "s1,x3/4,pe/b,") == "baedc");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getOrder("abcdefghijklmnop", input);
            var solutionPart_2 = getOrderPartTwo("abcdefghijklmnop", input);

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static string getOrderPartTwo(string dancers, string input)
        {
            int dances = 1;
            string originalPosition = dancers;
            dancers = getOrder(dancers, input);

            while (dancers != originalPosition)
            {
                dancers = getOrder(dancers, input);
                dances++;
            }

            var neededDances = 1000000000 % dances;
            dancers = originalPosition;

            for (int i = 0; i < neededDances; i++)
            {
                dancers = getOrder(dancers, input);
            }

            return dancers;
        }

        private static string getOrder(string dancers, string moves)
        {
            foreach (var item in moves.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                switch(item[0])
                {
                    case 's':
                        dancers = spinDancers(dancers, Convert.ToInt32(item.Substring(1)));
                        break;
                    case 'x':
                        dancers = exchangeDancers(dancers, item.Substring(1));
                        break;
                    case 'p':
                        dancers = swapPartners(dancers, item.Substring(1));
                        break;
                    default:
                        throw new ArgumentException("Invalid operation");
                }
            }
            return dancers;
        }

        private static string swapPartners(string dancers, string partners)
        {
            var p1 = partners.Split('/')[0];
            var p2 = partners.Split('/')[1];

            dancers = dancers.Replace(p1, "#");
            dancers = dancers.Replace(p2, p1);
            return dancers.Replace("#", p2);
        }

        private static string exchangeDancers(string dancers, string exchange)
        {
            var pos1 = Convert.ToInt32(exchange.Split('/')[0]);
            var pos2 = Convert.ToInt32(exchange.Split('/')[1]);

            string temp = dancers[pos1].ToString();
            return dancers.Remove(pos1, 1).Insert(pos1, dancers[pos2].ToString()).Remove(pos2, 1).Insert(pos2, temp);
        }

        private static string spinDancers(string dancers, int length)
        {
            var s = length % dancers.Length;
            return $"{dancers.Substring(dancers.Length - s)}{dancers.Substring(0, dancers.Length - s)}";
        }
    }
}
