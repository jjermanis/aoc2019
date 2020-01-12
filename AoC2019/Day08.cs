using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2019
{
    public class Day08 : DayBase, IDay
    {
        private readonly string _image;
        private readonly int _width;
        private readonly int _height;

        public Day08(string filename, int width, int height)
        {
            _image = TextFile(filename);
            _width = width;
            _height = height;
        }
        public Day08() : this("Day08.txt", 25, 6)
        {
        }

        public void Do()
        {
            Console.WriteLine($"Part 1: {Part1Answer()}");
            Console.WriteLine("Part 2:");
            foreach (var line in Part2Answer())
                Console.WriteLine(line);
        }

        public int Part1Answer()
        {
            var size = _width * _height;
            var layers = _image.Length / size;
            var min = int.MaxValue;
            IDictionary<int, int> minProfile = null;
            for (int layer = 0; layer < layers; layer++)
            {
                var curr = GetLayerProfile(_image, layer, size);
                if (curr[0] < min)
                {
                    min = curr[0];
                    minProfile = curr;
                }
            }
            return minProfile[1] * minProfile[2];
        }

        public IEnumerable<string> Part2Answer()
        {
            var result = new List<string>();
            var size = _width * _height;
            for (var y = 0; y < _height; y++)
            {
                var line = new char[_width];
                for (var x = 0; x < _width; x++)
                {
                    var layer = 0;
                    while (Curr() == '2')
                        layer++;
                    line[x] = (Curr() == '0' ? ' ' : '#');

                    char Curr() => _image[layer * size + _width * y + x];
                }
                result.Add(new string(line));
            }
            return result;
        }



        static IDictionary<int, int> GetLayerProfile(
            string image,
            int layer,
            int size)
        {
            var result = new Dictionary<int, int>();
            for (var x = 0; x < 10; x++)
                result[x] = 0;
            for (int x = layer * size; x < (layer + 1) * size; x++)
            {
                result[image[x] - '0']++;
            }
            return result;
        }
    }
}
