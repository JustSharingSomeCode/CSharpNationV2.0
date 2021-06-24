using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.IO;
using System.Drawing;

using CSharpNationV2._0.Visualizer;
using CSharpNationV2._0.Textures;

namespace CSharpNationV2._0.Configuration
{
    public static class ConfigManager
    {        
        private static string[] configData;

        public static readonly string configDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\CSharpNationV2.0";
        private static readonly string configFilePath = configDirectoryPath + @"\Config.txt";

        public static void LoadConfigFile()
        {            
            try
            {
                CheckLocalDirectory();                

                configData = File.ReadAllText(configFilePath).Split((char)124);                

                if(configData.Length <= 1 || configData[0] == "")
                {
                    SaveConfig("Default", GetWaveConfig(), new List<TextureData>());
                    LoadConfigFile();
                }                
            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message);
            }            
        }        

        public static string BackgroundsFolder
        {
            get
            {
                return configData[0];
            }
            set
            {
                configData[0] = value;
            }
        }

        public static string[] GetBackgroundConfig()
        {
            return configData[2].Split((char)47);
        }

        public static TextureManager.DisplayMode GetDisplayMode(string path)
        {
            try
            {
                string[] config = GetBackgroundConfig();

                for (int i = 0; i < config.Length; i++)
                {
                    if (config[i].Contains(path))
                    {
                        string dp = config[i].Split(sep.ToCharArray())[1];

                        return (TextureManager.DisplayMode)Enum.Parse(typeof(TextureManager.DisplayMode), dp);
                    }
                }

                return TextureManager.DisplayMode.NotFound;
            }
            catch
            {
                return TextureManager.DisplayMode.NotFound;
            }
        }

        public static SpectrumWave[] GetWaveConfig()
        {
            try
            {
                string[] waveData = configData[1].Split((char)47);

                SpectrumWave[] waves = new SpectrumWave[waveData.Length];

                string[] data;

                for (int i = 0; i < waveData.Length; i++)
                {
                    data = waveData[i].Split(sep.ToCharArray());
                    waves[i] = new SpectrumWave()
                    {
                        WaveColor = Color.FromArgb(int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2])),
                        Increment = float.Parse(data[3]),
                        BarsInfluence = int.Parse(data[4]),
                        PromLoops = int.Parse(data[5])
                    };
                }

                return waves;
            }
            catch
            {
                return GetDefaultWaves();
            }
        }

        public static SpectrumWave[] GetDefaultWaves()
        {
            SpectrumWave[] Waves = new SpectrumWave[9];

            Waves[0] = new SpectrumWave
            {
                WaveColor = Color.White,
                Increment = 0,
                BarsInfluence = 0,
                PromLoops = 0
            };

            Waves[1] = new SpectrumWave
            {
                WaveColor = Color.Yellow,
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 1
            };

            Waves[2] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(255, 150, 0),
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 2
            };

            Waves[3] = new SpectrumWave
            {
                WaveColor = Color.Red,
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 3
            };

            Waves[4] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(255, 100, 255),
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 4
            };

            Waves[5] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(50, 50, 155),
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 5
            };

            Waves[6] = new SpectrumWave
            {
                WaveColor = Color.Blue,
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 6
            };

            Waves[7] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(50, 205, 255),
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 7
            };

            Waves[8] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(0, 255, 0),
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 8
            };

            return Waves;
        }

        private static void CreateLocalDirectory()
        {
            Directory.CreateDirectory(configDirectoryPath);
        }

        private static void CreateBackgroundsDirectory()
        {
            Directory.CreateDirectory(configDirectoryPath + @"\Backgrounds");
        }

        private static void CreateParticlesDirectory()
        {
            Directory.CreateDirectory(configDirectoryPath + @"\Particles");

            Bitmap logo = new Bitmap(Properties.Resources.Logo);
            logo.Save(configDirectoryPath + @"\Particles\Logo.png");
            logo.Dispose();
        }

        private static void CreateConfigFile()
        {
            File.Create(configFilePath).Close();
        }

        private static void CheckLocalDirectory()
        {
            if (!Directory.Exists(configDirectoryPath))
            {
                CreateLocalDirectory();
            }

            CheckConfigFile();
            CheckBackgroundsDirectory();
            CheckParticlesDirectory();
        }

        private static void CheckConfigFile()
        {
            if (!File.Exists(configFilePath))
            {
                CreateConfigFile();
            }
        }

        private static void CheckBackgroundsDirectory()
        {
            if (!Directory.Exists(configDirectoryPath + @"\Backgrounds"))
            {
                CreateBackgroundsDirectory();
            }
        }

        private static void CheckParticlesDirectory()
        {
            if (!Directory.Exists(configDirectoryPath + @"\Particles"))
            {
                CreateParticlesDirectory();
            }
        }

        private static readonly string sep = "#";

        public static void SaveConfig(string backgroundsPath, SpectrumWave[] waves, List<TextureData> textures)
        {
            string config = "";

            if(backgroundsPath == "Default")
            {
                config += configDirectoryPath + @"\Backgrounds";
            }
            else
            {
                config += backgroundsPath;
            }

            config += "|";            

            for(int i = 0; i < waves.Length; i++)
            {
                if (i > 0)
                {
                    config += "/";
                }

                config += waves[i].WaveColor.R + sep + waves[i].WaveColor.G + sep + waves[i].WaveColor.B
                    + sep + waves[i].Increment + sep + waves[i].BarsInfluence + sep + waves[i].PromLoops;                
            }

            config += "|";

            for(int i = 0; i < textures.Count; i++)
            {
                if (i > 0)
                {
                    config += "/";
                }

                config += textures[i].Path + sep + textures[i].DisplayMode.ToString();
            }

            CheckLocalDirectory();

            File.WriteAllText(configFilePath, config);
        }
    }
}
