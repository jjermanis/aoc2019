using System;
using System.Collections.Generic;

namespace AoC2019
{
    public class Day06 : DayBase, IDay
    {
        private readonly IDictionary<string, string> _graph;

        public Day06() : this("Day06.txt")
        {
        }

        public Day06(string filename)
        {
            _graph = BuildGraph(filename);
        }

        public void Do()
        {
            Console.WriteLine($"Total number of direct and indirect orbits: {TotalStepsToCom()}");
            Console.WriteLine($"Number of orbital transfers from YOU to SAN: {DistanceFromSanToYou()}");
        }

        private IDictionary<string, string> BuildGraph(string filename)
        {
            var result = new Dictionary<string, string>();
            foreach (var line in TextFileLines(filename))
            {
                string inner = line.Substring(0, 3);
                string outer = line.Substring(4, 3);

                result.Add(outer, inner);
            }
            return result;
        }

        public int TotalStepsToCom()
        {
            var result = 0;
            foreach (var body in _graph.Keys)
            {
                string curr = body;
                while (!curr.Equals("COM"))
                {
                    result++;
                    curr = _graph[curr];
                }
            }
            return result;
        }

        public int DistanceFromSanToYou()
        {
            // First, find distances of all orbits between SAN and COM
            var sanOrbits = new List<string>();
            string curr = "SAN";
            while (!curr.Equals("COM"))
            {
                curr = _graph[curr];
                sanOrbits.Add(curr);
            }

            // Next, find distance from YOU to something found on the 
            // previous path
            curr = "YOU";
            int transferCount = 0;
            while (!curr.Equals("COM"))
            {
                curr = _graph[curr];
                transferCount++;
                if (sanOrbits.Contains(curr))
                {
                    // Return distance from SAN to middle, and middle to YOU (without
                    // doubling counting middle)
                    return transferCount + sanOrbits.IndexOf(curr) - 1;
                }
            }
            throw new Exception("Route not found");
        }
    }
}
