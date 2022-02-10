using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;

using CSharpNation.Tools;

namespace CSharpNation.Config
{
    static class GlobalConfig
    {
        static GlobalConfig()
        {
            LoadConfig();
           
            DegreesIncrement = 180f / (Lines - 1.0f);

            CheckConfigDirectory();
            CheckResourcesDirectory();
            TexturesConfig.Initialize();
        }

        private static int lines;
        public static int Lines
        {
            get
            {
                return lines;
            }
            set
            {
                lines = value;
                DegreesIncrement = 180f / (Lines - 1.0f);
            }
        }
        public static float DegreesIncrement { get; private set; }

        public static float Fps { get; private set; }

        public static bool AutoBackgroundChange { get; set; }
        public static int BackgroundTime { get; set; } //seconds
        public static int BackgroundDim { get; set; }
        public static bool BackgroundMovement { get; set; }

        public static bool EnableShaking { get; set; } = true;
        public static bool EnableGlow { get; set; } = true;
        public static float GlowSize { get; set; } = 20.0f;
        public static float GlowMaxAlphaAtSize { get; set; } = 50.0f;
        public static int GlowMaxAlpha { get; set; } = 80;
        public static bool EnableReplayBuffer { get; set; } = true;
        public static int ReplayBufferSize { get; set; } = 6;


        public static string ConfigDirectoryPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\CSharpNation";
        private static string ConfigTxtPath = ConfigDirectoryPath + @"\Config.txt";
        public static string ResourcesDirectoryPath { get; } = ConfigDirectoryPath + @"\Resources";

        private static string texturesPath;
        public static string TexturesPath
        {
            get => texturesPath;
            set
            {
                texturesPath = value;
                TexturesConfig.LoadTextureFolder();
            }
        }

        #region CheckConfigDirectory
        private static void CheckConfigDirectory()
        {
            if (!Directory.Exists(ConfigDirectoryPath))
            {
                _ = Directory.CreateDirectory(ConfigDirectoryPath);
                WriteConfig(DefaultConfig());
            }
        }
        #endregion

        #region CheckResourcesDirectory
        private static void CheckResourcesDirectory()
        {
            if (!Directory.Exists(ResourcesDirectoryPath))
            {
                _ = Directory.CreateDirectory(ResourcesDirectoryPath);

                Bitmap logo = new Bitmap(Properties.Resources.Logo);
                logo.Save(ResourcesDirectoryPath + @"\Logo.png");
                logo.Dispose();

                Bitmap particle = new Bitmap(Properties.Resources.Particle);
                particle.Save(ResourcesDirectoryPath + @"\Particle.png");
                particle.Dispose();

                Bitmap a = new Bitmap(Properties.Resources.FullscreenPreview);
                a.Save(ResourcesDirectoryPath + @"\FullscreenPreview.jpg");
                a.Dispose();

                Bitmap b = new Bitmap(Properties.Resources.HalfscreenPreview);
                b.Save(ResourcesDirectoryPath + @"\HalfscreenPreview.jpg");
                b.Dispose();

                Bitmap c = new Bitmap(Properties.Resources.MirroredLeftHalfPreview);
                c.Save(ResourcesDirectoryPath + @"\MirroredLeftHalfPreview.jpg");
                c.Dispose();

                Bitmap d = new Bitmap(Properties.Resources.MirroredRightHalfPreview);
                d.Save(ResourcesDirectoryPath + @"\MirroredRightHalfPreview.jpg");
                d.Dispose();
            }
        }
        #endregion

        private static void WriteConfig(string[] config)
        {
            File.WriteAllLines(ConfigTxtPath, config);
        }

        private static string[] DefaultConfig()
        {
            return new string[] {
                "50", //lines
                "", //backgrounds
                "60.0", //fps
                "60", //backgrounds seconds
                "True", //auto background change
                "150", //background dim
                "True", //background movement
                "True", //enable shaking
                "True", //enable glow
                "20.0", //glow size
                "50.0", //glow max alpha at size
                "80", //glow max alpha
                "True", //enable replay buffer
                "6" //replay buffer size
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
                BackgroundDim.ToString(), //background dim
                BackgroundMovement.ToString(),
                EnableShaking.ToString(),
                EnableGlow.ToString(),
                GlowSize.ToString(),
                GlowMaxAlphaAtSize.ToString(),
                GlowMaxAlpha.ToString(),
                EnableReplayBuffer.ToString(),
                ReplayBufferSize.ToString()
            };
        }

        public static void SaveConfig()
        {
            try
            {
                WriteConfig(GetCurrentConfig());
                TexturesConfig.SaveConfig();
                WaveConfig.SaveConfig();

                ErrorLog.AddError(new Error(Error.Type.Information, "Actual config saved successfully"));
            }
            catch(Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                ErrorLog.AddError(new Error(Error.Type.CriticalError, "Error saving actual config: " + ex.Message));
            }
        }

        private static void LoadConfig()
        {
            if (File.Exists(ConfigTxtPath))
            {
                try
                {
                    string[] config = File.ReadAllLines(ConfigTxtPath);
                    Lines = Convert.ToInt32(config[0]);
                    TexturesPath = config[1];
                    Fps = Convert.ToInt32(float.Parse(config[2]));
                    BackgroundTime = Convert.ToInt32(config[3]);
                    AutoBackgroundChange = config[4] == "True";
                    BackgroundDim = Convert.ToInt32(config[5]);
                    BackgroundMovement = config[6] == "True";
                    EnableShaking = config[7] == "True";
                    EnableGlow = config[8] == "True";
                    GlowSize = float.Parse(config[9]);
                    GlowMaxAlphaAtSize = float.Parse(config[10]);
                    GlowMaxAlpha = Convert.ToInt32(config[11]);
                    EnableReplayBuffer = config[12] == "True";
                    ReplayBufferSize = Convert.ToInt32(config[13]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    WriteConfig(DefaultConfig());
                    LoadConfig();
                }
            }
            else
            {
                WriteConfig(DefaultConfig());
                LoadConfig();
            }
        }
    }
}
