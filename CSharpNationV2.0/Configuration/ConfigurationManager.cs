using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpNationV2._0.Visualizer;
using CSharpNationV2._0.Textures;

using System.IO;
using System.Drawing;

namespace CSharpNationV2._0.Configuration
{
    public static class ConfigurationManager
    {
        static ConfigurationManager()
        {
            CheckConfigDirectory();

            CheckResourcesDirectory();

            CheckBackgroundsDirectory();

            LoadConfig();

            LoadBackgrounds();
        }
        
        public static List<string> BackgroundsConfig { get; private set; } = new List<string>();        

        public static readonly string configDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\CSharpNationV2.0";
        public static readonly string resourcesDirectoryPath = configDirectoryPath + @"\Resources";
        public static readonly string backgroundsDirectoryPath = configDirectoryPath + @"\Backgrounds";
        public static readonly string wavesFilePath = configDirectoryPath + @"\Waves.txt";
        public static readonly string backgroundsFilePath = configDirectoryPath + @"\Backgrounds.txt";
        public static readonly string configFilePath = configDirectoryPath + @"\Config.txt";

        public static string BackgroundsPath { get; set; }

        public static int VisualizerWidth { get; set; } = 1280;
        public static int VisualizerHeight { get; set; } = 720;

        public static int ParticlesOnScreen { get; set; } = 500;
        public static int BackgroundDim { get; set; } = 100;
        public static bool BackgroundMovement { get; set; } = false;
        public static int BackgroundDuration { get; set; }

        public static float ParticleScale
        {
            get
            {
                return VisualizerHeight / 720.0f;
            }
        }

        private static void CheckConfigDirectory()
        {
            if(!Directory.Exists(configDirectoryPath))
            {
                Directory.CreateDirectory(configDirectoryPath);
            }
        }

        private static void CheckBackgroundsDirectory()
        {
            if(!Directory.Exists(backgroundsDirectoryPath))
            {
                Directory.CreateDirectory(backgroundsDirectoryPath);

                //copy backgrounds
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
            }
        }        

        private static void DefaultConfig()
        {
            BackgroundsPath = backgroundsDirectoryPath;
            ParticlesOnScreen = 500;
            BackgroundDuration = 30;
            BackgroundDim = 200;
            BackgroundMovement = true;
        }        

        public static void LoadConfig()
        {
            if(File.Exists(configFilePath))
            {                
                try
                {
                    string[] config = File.ReadAllLines(configFilePath);

                    BackgroundsPath = config[0];
                    ParticlesOnScreen = int.Parse(config[1]);
                    BackgroundDuration = int.Parse(config[2]);
                    BackgroundDim = int.Parse(config[3]);
                    BackgroundMovement = bool.Parse(config[4]);
                }
                catch (Exception ex)
                {
                    DefaultConfig();
                    ErrorManager.AddErrorMessage("Error loading config file: " + ex.Message);
                }
            }
            else
            {                
                DefaultConfig();
            }
        }

        public static SpectrumWave[] LoadWaves()
        {
            if (File.Exists(wavesFilePath))
            {
                string[] config = File.ReadAllLines(wavesFilePath);
                SpectrumWave[] waves = new SpectrumWave[config.Length];

                string[] data;

                for (int i = 0; i < config.Length; i++)
                {
                    data = config[i].Split('|');

                    SpectrumWave wave = new SpectrumWave()
                    {
                        WaveColor = Color.FromArgb(255, int.Parse(data[0]), int.Parse(data[1]),
                        int.Parse(data[2])),
                        Increment = float.Parse(data[3]),
                        BarsInfluence = int.Parse(data[4]),
                        PromLoops = int.Parse(data[5])
                    };

                    waves[i] = wave;
                }

                return waves;
            }
            else
            {
                return GetDefaultWaves();
            }
        }

        private static void LoadBackgrounds()
        {
            if (File.Exists(backgroundsFilePath))
            {
                string[] config = File.ReadAllLines(backgroundsFilePath);
                BackgroundsConfig = new List<string>(config);
            }
        }

        public static void SaveConfig()
        {            
            string[] config = new string[] { BackgroundsPath, ParticlesOnScreen.ToString(), BackgroundDuration.ToString(), BackgroundDim.ToString(), BackgroundMovement.ToString() };
            File.WriteAllLines(configFilePath, config);
        }

        public static void SaveWaves(SpectrumWave[] waves)
        {
            string[] config = new string[waves.Length];

            for (int i = 0; i < waves.Length; i++)
            {
                config[i] = waves[i].GetConfig();
            }

            File.WriteAllLines(wavesFilePath, config);
        }

        public static void SaveBackgrounds(List<TextureData> textures)
        {
            bool exists = false;
            int index = 0;

            for (int i = 0; i < textures.Count; i++)
            {
                exists = false;

                for (int j = 0; j < BackgroundsConfig.Count; j++)
                {
                    if (BackgroundsConfig[j].Contains(textures[i].FileName))
                    {
                        exists = true;
                        index = j;
                        break;
                    }
                }

                if (!exists)
                {
                    BackgroundsConfig.Add(textures[i].GetConfig());
                }
                else
                {
                    BackgroundsConfig[index] = textures[i].GetConfig();
                }
            }

            File.WriteAllLines(backgroundsFilePath, BackgroundsConfig.ToArray());
        }        

        #region DefaultWaves
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
        #endregion

        public static TextureManager.DisplayMode GetDisplayMode(string fileName)
        {
            try
            {
                for (int i = 0; i < BackgroundsConfig.Count; i++)
                {
                    if (BackgroundsConfig[i].Contains(fileName))
                    {
                        string dm = BackgroundsConfig[i].Split('|')[1];

                        return (TextureManager.DisplayMode)Enum.Parse(typeof(TextureManager.DisplayMode), dm);
                    }
                }

                return TextureManager.DisplayMode.NotFound;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return TextureManager.DisplayMode.NotFound;
            }
        }
    }
}
