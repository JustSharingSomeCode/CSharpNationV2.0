using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;

namespace CSharpNation.Config
{
    static class GlobalConfig
    {
        static GlobalConfig()
        {
            LoadConfig();

            //Lines = 64;
            //WaveCount = 9;
            DegreesIncrement = 180f / (Lines - 1);
            //TexturesPath = @"D:\Backgrounds";
            //Fps = 60.0f;
            //BackgroundTime = 5;
            //AutoBackgroundChange = true;
            //BackgroundDim = 150;            

            CheckConfigFolder();
            CheckResourcesDirectory();
            TexturesConfig.Initialize();
        }

        public static int Lines { get; private set; }
        //public static int WaveCount { get; private set; }
        public static float DegreesIncrement { get; private set; }

        public static float Fps { get; private set; }

        public static bool AutoBackgroundChange { get; private set; }
        public static int BackgroundTime { get; private set; } //seconds
        public static int BackgroundDim { get; private set; }

        public static string ConfigDirectoryPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\CSharpNation";
        private static string ConfigTxtPath = ConfigDirectoryPath + @"\Config.txt";
        private static string resourcesDirectoryPath = ConfigDirectoryPath + @"\Resources";
        public static string TexturesPath { get; private set; }

        private static void CheckConfigFolder()
        {
            if(!Directory.Exists(ConfigDirectoryPath))
            {
                Directory.CreateDirectory(ConfigDirectoryPath);
                WriteConfig(DefaultConfig());
            }
        }

        private static void CheckResourcesDirectory()
        {
            if (!Directory.Exists(resourcesDirectoryPath))
            {
                Directory.CreateDirectory(resourcesDirectoryPath);

                Bitmap logo = new Bitmap(Properties.Resources.Logo);
                logo.Save(resourcesDirectoryPath + @"\Logo.png");
                logo.Dispose();

                Bitmap particle = new Bitmap(Properties.Resources.Particle);
                particle.Save(resourcesDirectoryPath + @"\Particle.png");
                particle.Dispose();
            }
        }

        private static void WriteConfig(string[] config)
        {
            File.WriteAllLines(ConfigTxtPath, config);
        }

        private static string[] DefaultConfig()
        {
            return new string[] {
                "64", //lines
                "", //backgrounds
                "60.0f", //fps
                "60", //backgrounds seconds
                "True", //auto background change
                "150" //background dim
            };
        }

        private static string[] GetCurrentConfig()
        {
            return new string[] {
                Lines.ToString(), //lines
                TexturesPath, //backgrounds
                Fps.ToString(), //fps
                BackgroundTime.ToString(), //backgrounds seconds
                AutoBackgroundChange.ToString(), //auto background change
                BackgroundDim.ToString() //background dim
            };
        }

        public static void SaveConfig()
        {
            try
            {
                WriteConfig(GetCurrentConfig());
                TexturesConfig.SaveConfig();
                WaveConfig.SaveConfig();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private static void LoadConfig()
        {
            if (File.Exists(ConfigTxtPath))
            {
                string[] config = File.ReadAllLines(ConfigTxtPath);
                Lines = Convert.ToInt32(config[0]);
                TexturesPath = config[1];
                Fps = Convert.ToInt32(config[2]);
                BackgroundTime = Convert.ToInt32(config[3]);
                AutoBackgroundChange = config[4] == "True";
                BackgroundDim = Convert.ToInt32(config[5]);               
            }
            else
            {
                WriteConfig(DefaultConfig());
            }
        }
    }
}
