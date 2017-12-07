using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.solutions
{
    public static class Helpers
    {
        public static string getInputFromFile(string filename)
        {
            StreamReader sr = new StreamReader($"./input/{filename}");
            string lReturn = sr.ReadToEnd();
            sr.Close();
            return lReturn;
        }
    }
}
