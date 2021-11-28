using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace CSharpNation.Config
{
    static class GlobalConfig
    {
        static GlobalConfig()
        {
            Lines = 64;
            WaveCount = 9;
            DegreesIncrement = 180f / (Lines - 1);
            TexturesPath = @"D:\USB\Backgrounds";
            Fps = 60.0f;
            BackgroundTime = 5;
            AutoBackgroundChange = true;
            BackgroundDim = 150;

            CheckConfigFolder();
            TexturesConfig.Initialize();
        }

        public static int Lines { get; private set; }
        public static int WaveCount { get; private set; }
        public static float DegreesIncrement { get; private set; }

        public static float Fps { get; private set; }

        public static bool AutoBackgroundChange { get; private set; }
        public static int BackgroundTime { get; private set; } //seconds
        public static int BackgroundDim { get; private set; }

        public static string ConfigDirectoryPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\CSharpNation";
        public static string TexturesPath { get; private set; }

        private static void CheckConfigFolder()
        {
            if(!Directory.Exists(ConfigDirectoryPath))
            {
                Directory.CreateDirectory(ConfigDirectoryPath);
            }
        }
    }
}
