using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using CSharpNation.Textures;

namespace CSharpNation.Config
{
    static class TexturesConfig
    {
        public static List<Texture> Textures { get; private set; } = new List<Texture>();
        private static List<string> config;

        public static void Initialize()
        {
            LoadConfig();
            LoadTextureFolder();
        }

        public static void LoadTextureFolder()
        {
            string path = GlobalConfig.TexturesPath;

            if(!Directory.Exists(path))
            {
                return;
            }

            Textures.Clear();

            string[] jpgFiles = Directory.GetFiles(path, "*.jpg");
            string[] pngFiles = Directory.GetFiles(path, "*.png");

            for(int i = 0; i < jpgFiles.Length; i++)
            {
                Texture t = new Texture()
                {
                    Path = jpgFiles[i],
                };

                t.DisplayMode = SearchDisplayMode(t.FileName);

                Textures.Add(t);
            }

            for (int i = 0; i < pngFiles.Length; i++)
            {
                Texture t = new Texture()
                {
                    Path = pngFiles[i],                    
                };

                t.DisplayMode = SearchDisplayMode(t.FileName);

                Textures.Add(t);
            }

            Console.WriteLine(Textures.Count);
        }

        private static void LoadConfig()
        {
            string path = GlobalConfig.ConfigDirectoryPath + @"\Textures.txt";

            if(!File.Exists(path))
            {
                File.Create(path).Close();
            }

            config = File.ReadAllLines(path).ToList();
        }

        public static void SaveConfig()
        {
            string path = GlobalConfig.ConfigDirectoryPath + @"\Textures.txt";

            bool exist;

            for (int i = 0; i < Textures.Count; i++)
            {
                exist = false;

                for(int j = 0; j < config.Count; j++)
                {
                    if(config[j].Contains(Textures[i].FileName))
                    {
                        exist = true;

                        config[j] = Textures[i].ToString();

                        break;
                    }
                }

                if(!exist)
                {
                    config.Add(Textures[i].ToString());
                }                
            }

            File.WriteAllLines(path, config);
        }

        private static Texture.Display SearchDisplayMode(string fileName)
        {
            if (config == null)
            {
                return Texture.Display.NotFound;
            }

            for(int i = 0; i < config.Count; i++)
            {
                if(config[i].Contains(fileName))
                {
                    string dm = config[i].Split('|')[1];

                    return (Texture.Display)Enum.Parse(typeof(Texture.Display), dm);
                }
            }

            return Texture.Display.NotFound;
        }
    }
}
