using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.solutions
{
    public static class Solution_20
    {
        public static List<Particle> particleList;
        public static int remainingNodes;
        public static string getResult()
        {
            var input = Helpers.getInputFromFile("day20.txt");
            remainingNodes = 0;

            Debug.Assert(getClosestParticle(@"p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>
p=< 4,0,0>, v=< 0,0,0>, a=<-2,0,0>", 500) == 0);

            var temp = getClosestParticle(@"p=<-6,0,0>, v=< 3,0,0>, a=< 0,0,0>
p=<-4,0,0>, v=< 2,0,0>, a=< 0,0,0>
p=<-2,0,0>, v=< 1,0,0>, a=< 0,0,0>
p=< 3,0,0>, v=<-1,0,0>, a=< 0,0,0>", 500);

            Debug.Assert(remainingNodes == 1);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Solution goes here
            var solutionPart_1 = getClosestParticle(input, 500);
            var solutionPart_2 = remainingNodes;

            sw.Stop();

            return $"Part 1: {solutionPart_1}{Environment.NewLine}Part 2: {solutionPart_2}{Environment.NewLine}Completion time: {sw.ElapsedMilliseconds}ms";
        }

        private static int getClosestParticle(string input, int loopLength)
        {
            particleList = getListFromInput(input);

            for (int i = 0; i < loopLength; i++)
            {
                foreach (var p in particleList)
                {
                    p.MoveParticles();
                }

                // Mark Duplicates
                var duplicates = particleList
                    .Where(x => x.IsDeleted == false)
                    .GroupBy(p => p.Position)
                    .Where(g => g.Count() > 1);

                foreach (var d in duplicates)
                {
                    foreach (var particle in d)
                    {
                        particle.IsDeleted = true;
                    }
                }
            }

            remainingNodes = particleList.Where(x => x.IsDeleted == false).Count();

            return getNodeClosestToZero(particleList);
        }

        private static int getNodeClosestToZero(List<Particle> particleList)
        {
            var minDistance = Int32.MaxValue;
            var minParticle = 0;
            var tempDistance = 0;
            var i = 0;
            foreach (var p in particleList)
            {
                tempDistance = Math.Abs(p.Position.Item1) + Math.Abs(p.Position.Item2) + Math.Abs(p.Position.Item3);
                if (tempDistance < minDistance)
                {
                    minDistance = tempDistance;
                    minParticle = i;
                }
                i++;
            }

            return minParticle;
        }

        private static List<Particle> getListFromInput(string input)
        {
            List<Particle> lReturnList = new List<Particle>();
            foreach (var line in input.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var numbers = Regex.Replace(line, "[^0-9,-]", "");
                int[] tN = numbers.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

                lReturnList.Add(new Particle(new Tuple<int, int, int>(tN[0], tN[1], tN[2]), new Tuple<int, int, int>(tN[3], tN[4], tN[5]), new Tuple<int, int, int>(tN[6], tN[7], tN[8])));
            }
            return lReturnList;
        }

        public class Particle
        {
            public bool IsDeleted { get; set; }
            public Tuple<int, int, int> Position { get; set; }
            public Tuple<int, int, int> Velocity { get; set; }
            public Tuple<int, int, int> Acceleration { get; set; }

            public Particle(Tuple<int, int, int> p, Tuple<int, int, int> v, Tuple<int, int, int> a)
            {
                this.Position = p;
                this.Velocity = v;
                this.Acceleration = a;
            }

            public void MoveParticles()
            {
                Tuple<int, int, int> tempTuple =
                    new Tuple<int, int, int>(
                        this.Velocity.Item1 + this.Acceleration.Item1,
                        this.Velocity.Item2 + this.Acceleration.Item2,
                        this.Velocity.Item3 + this.Acceleration.Item3
                        );

                this.Velocity = tempTuple;

                tempTuple =
                    new Tuple<int, int, int>(
                        this.Position.Item1 + this.Velocity.Item1,
                        this.Position.Item2 + this.Velocity.Item2,
                        this.Position.Item3 + this.Velocity.Item3
                        );

                this.Position = tempTuple;
            }
        }
    }
}