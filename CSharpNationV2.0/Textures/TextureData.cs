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

        private Bitmap textureBitmap;
        public int Texture { get; set; } = -1;
        public TextureManager.DisplayMode DisplayMode { get; set; }

        public int OriginalWidth { get; private set; }
        public int OriginalHeight { get; private set; }

        public float WidthRatio { get; private set; }
        public float HeightRatio { get; private set; }

        public float ActualWidth { get; private set; }
        public float ActualHeight { get; private set; }

        public float FillY { get; private set; }
        public float FillX { get; private set; }

        private void LoadConfig()
        {            
            DisplayMode = ConfigurationManager.GetDisplayMode(FileName);
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
            else
            {
                if(DisplayMode != TextureManager.DisplayMode.Fullscreen && DisplayMode != TextureManager.DisplayMode.Halfscreen)
                {
                    OriginalWidth /= 2;
                }                
            }

            WidthRatio = (float)OriginalWidth / OriginalHeight;
            HeightRatio = (float)OriginalHeight / OriginalWidth;

            Texture = TextureManager.LoadTexture(textureBitmap, DisplayMode);

            textureBitmap = null;
        }

        public void UpdateScale(float screenWidth, float screenHeight)
        {
            if(DisplayMode != TextureManager.DisplayMode.Fullscreen)
            {
                screenWidth = screenWidth / 2;
            }

            float missingWidth = screenWidth - OriginalWidth;
            float missingHeight = screenHeight - OriginalHeight;

            if(missingWidth > missingHeight)            
            {
                ActualWidth = screenWidth;
                ActualHeight = screenWidth * HeightRatio;
            }
            else
            {
                ActualHeight = screenHeight;
                ActualWidth = screenHeight * WidthRatio;
            }

            if(ActualHeight < screenHeight)
            {
                float scale = screenHeight / ActualHeight;
                ActualHeight *= scale;
                ActualWidth *= scale;
            }

            if (ActualWidth < screenWidth)
            {
                float scale = screenWidth / ActualWidth;
                ActualHeight *= scale;
                ActualWidth *= scale;
            }

            FillY = (ActualHeight - screenHeight) / 2;
            FillX = (ActualWidth - screenWidth) / 2;            
        }

        public string Path { get; private set; }
        public string FileName
        {
            get
            {
                return Path.Split((char)92).Last();
            }
        }        

        public void Write()
        {
            Console.WriteLine("O_Width: {0}, O_Height: {1}", OriginalWidth, OriginalHeight);
            Console.WriteLine("RWidth: {0}, RHeight: {1}", WidthRatio, HeightRatio);
            Console.WriteLine("A_Width: {0}, A_Height: {1}", ActualWidth, ActualHeight);
            Console.WriteLine("CY: {0}, CX: {1}", FillY, FillX);
        }

        public string GetConfig()
        {
            return FileName + "|" + DisplayMode.ToString();
        }
    }
}
