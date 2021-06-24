using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

using CSharpNationV2._0.Configuration;

namespace CSharpNationV2._0.Textures
{
    public class TextureData
    {
        public TextureData(string path)
        {
            Path = path;

            LoadConfig();            
        }

        private void LoadConfig()
        {
            DisplayMode = ConfigManager.GetDisplayMode(Path);
            /*
            if(DisplayMode == TextureManager.DisplayMode.NotFound)
            {
                DisplayMode = TextureManager.DisplayMode.Fullscreen;
            }
            */
        }

        public void LoadTexture()
        {
            textureBitmap = TextureManager.GetBitmap(Path);

            if(textureBitmap == null)
            {                
                return;
            }

            OriginalWidth = textureBitmap.Width;
            OriginalHeight = textureBitmap.Height;

            if (DisplayMode == TextureManager.DisplayMode.NotFound)
            {                
                if(OriginalHeight > OriginalWidth)
                {
                    DisplayMode = TextureManager.DisplayMode.Halfscreen;
                }
                else
                {
                    DisplayMode = TextureManager.DisplayMode.Fullscreen;
                }
            }

            Texture = TextureManager.LoadTexture(textureBitmap, DisplayMode);

            textureBitmap = null;
        }

        public string Path { get; private set; }

        private Bitmap textureBitmap;
        public int Texture { get; set; } = -1;
        public TextureManager.DisplayMode DisplayMode { get; set; }   
        
        private int OriginalWidth { get; set; }
        private int OriginalHeight { get; set; }
    }
}
