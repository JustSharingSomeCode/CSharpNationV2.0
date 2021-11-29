using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using CSharpNation.Visualizer;

namespace CSharpNation.Config
{
    static class WaveConfig
    {
        public static List<Wave> Waves { get; private set; } = new List<Wave>();
        //private static List<string> config;

        public static int LoadedWaves = 0;

        private static string path = GlobalConfig.ConfigDirectoryPath + @"\Waves.txt";

        static WaveConfig()
        {
            LoadConfig();
        }

        private static void LoadConfig()
        {            
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                Waves = DefaultWaves();
                LoadedWaves = 9;
            }
            else
            {
                string[] config = File.ReadAllLines(path);

                string[] waveData;

                for (int i = 0; i < config.Length; i++)
                {

                    waveData = config[i].Split('|');
                    Waves.Add(new Wave(Convert.ToInt32(waveData[0]), Convert.ToInt32(waveData[1]), Convert.ToInt32(waveData[2]), Convert.ToInt32(waveData[3]), Convert.ToInt32(waveData[4])));
                    LoadedWaves++;
                }
            }            
        }

        public static List<Wave> DefaultWaves()
        {
            Wave[] waves = new Wave[9];

            waves[0] = new Wave(255, 255, 255, 2, 2);
            waves[1] = new Wave(255, 255, 0, 2, 3);
            waves[2] = new Wave(255, 150, 0, 2, 4);
            waves[3] = new Wave(255, 0, 0, 2, 5);
            waves[4] = new Wave(255, 100, 255, 2, 6);
            waves[5] = new Wave(50, 50, 155, 2, 7);
            waves[6] = new Wave(0, 0, 255, 2, 8);
            waves[7] = new Wave(50, 200, 255, 2, 9);
            waves[8] = new Wave(0, 255, 0, 2, 10);            

            return waves.ToList();
        }

        public static void SaveConfig()
        {
            string[] config = new string[Waves.Count];

            for(int i = 0; i < Waves.Count; i++ )
            {
                config[i] = Waves[i].ToString();
            }

            File.WriteAllLines(path, config);
        }
    }
}
