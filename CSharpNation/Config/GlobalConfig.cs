using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNation.Config
{
    static class GlobalConfig
    {
        static GlobalConfig()
        {
            Lines = 64;
            WaveCount = 9;
            DegreesIncrement = 180f / (Lines - 1);
        }

        public static int Lines { get; private set; }
        public static int WaveCount { get; private set; }
        public static float DegreesIncrement { get; private set; }
    }
}
