using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

using CSharpNationV2._0.Configuration;

namespace CSharpNationV2._0.Textures
{
    public class TextureManager
    {        
        public TextureManager()
        {
            LoadFolder(ConfigurationManager.BackgroundsPath);
        }

        public enum DisplayMode
        {
            NotFound,
            Fullscreen,
            Halfscreen,
            MirroredLeftHalf,
            MirroredRightHalf
        }

        public static string LoadedFolder { get; private set; }                       

        public int LoadedTexturesCount
        {
            get
            {
                return LoadedTextures.Count;
            }
        }

        public int actualBackground = 0;        
        public List<TextureData> LoadedTextures { get; private set; } = new List<TextureData>();        

        public void Clean()
        {
            LoadedTextures.Clear();
            actualBackground = 0;
        }        

        public void LoadFolder(string folder)
        {            
            /*
            if(!Directory.Exists(folder))
            {
                return;
            }
            */
            Clean();

            LoadTextureData(folder, "*.jpg");
            LoadTextureData(folder, "*.png");

            LoadedFolder = folder;

            ConfigurationManager.BackgroundsPath = folder;
        }

        public void LoadTextureData(string path, string extension)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            string[] backgroundPaths = Directory.GetFiles(path, extension);

            for(int i = 0; i < backgroundPaths.Length; i++)
            {
                LoadedTextures.Add(new TextureData(backgroundPaths[i]));
            }
        }

        public void LoadBackgrounds(float screenWidth, float screenHeight)
        {
            for (int i = 0; i < LoadedTextures.Count; i++)
            {
                LoadedTextures[i].LoadTexture();                
            }

            UpdateScales(screenWidth, screenHeight);
        }       

        public void UpdateScales(float screenWidth, float screenHeight)
        {
            for (int i = 0; i < LoadedTextures.Count; i++)
            {                
                LoadedTextures[i].UpdateScale(screenWidth, screenHeight);
            }
        }

        public static Bitmap GetBitmap(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                return new Bitmap(path);
            }
            catch
            {
                ErrorManager.AddErrorMessage("Can't load file: " + path);
                return null;
            }
        }

        public static int LoadTexture(string path)
        {
            return LoadTexture(GetBitmap(path));
        }

        public static int LoadTexture(Bitmap bitmap, DisplayMode displayMode = DisplayMode.Fullscreen)
        {            
            try
            {                
                GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

                GL.GenTextures(1, out int tex);
                GL.BindTexture(TextureTarget.Texture2D, tex);

                BitmapData data = null;

                switch(displayMode)
                {
                    case DisplayMode.Halfscreen:
                    case DisplayMode.Fullscreen:
                        data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        break;
                    case DisplayMode.MirroredLeftHalf:
                        data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width / 2, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        break;
                    case DisplayMode.MirroredRightHalf:
                        data = bitmap.LockBits(new Rectangle(bitmap.Width / 2, 0, bitmap.Width / 2, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        break;                        
                }
                    
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                bitmap.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                bitmap.Dispose();

                return tex;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
                return -1;
            }
        }

        public int GetActualBackground()
        {            
            return LoadedTextures[actualBackground].Texture;
        }

        public static void DrawTexture(int texture, float x, float y, float xMax, float yMax, int a, int r, int g, int b)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Color4(Color.FromArgb(a, r, g, b));
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 1);
            GL.Vertex2(x, y);

            GL.TexCoord2(0, 0);
            GL.Vertex2(x, yMax);

            GL.TexCoord2(1, 0);
            GL.Vertex2(xMax, yMax);

            GL.TexCoord2(1, 1);
            GL.Vertex2(xMax, y);

            GL.End();
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
        }

        public void DrawBackground(float x, float y, float xMax, float yMax, float power, int a, int r, int g, int b)
        {
            TextureData td = LoadedTextures[actualBackground];

            float wp = power * td.WidthRatio;
            float hp = wp * td.HeightRatio;

            switch (td.DisplayMode)
            {
                case DisplayMode.Fullscreen:
                    DrawTexture(td.Texture, x - td.FillX - wp, y - td.FillY - hp, xMax + td.FillX + wp, yMax + td.FillY + hp, a, r, g, b);                    
                    break;

                case DisplayMode.Halfscreen:
                case DisplayMode.MirroredLeftHalf:
                case DisplayMode.MirroredRightHalf:                    

                    DrawTexture(td.Texture, x - (td.FillX * 2) - (wp * 2), y - td.FillY - hp, xMax / 2, yMax + td.FillY + hp, a, r, g, b);
                    DrawTexture(td.Texture, xMax + (td.FillX * 2) + (wp * 2), 0 - td.FillY - hp, xMax / 2, yMax + td.FillY + hp, a, r, g, b);
                    break;
            }
        }

        public void SetBackground(int index)
        {
            if(index >= 0 && index < LoadedTexturesCount)
            {
                actualBackground = index;
            }
        }

        public void NextBackground()
        {
            if(LoadedTextures.Count <= 0)
            {
                return;
            }

            actualBackground++;

            if(actualBackground >= LoadedTextures.Count)
            {
                actualBackground = 0;
            }            

            if(GetActualBackground() == -1)
            {
                NextBackground();
            }
        }

        public void PreviousBackground()
        {
            if (LoadedTextures.Count <= 0)
            {
                return;
            }

            actualBackground--;

            if (actualBackground < 0)
            {
                actualBackground = LoadedTextures.Count - 1;
            }

            if (GetActualBackground() == -1)
            {
                PreviousBackground();
            }
        }

        public string[] GetFileNames()
        {
            string[] names = new string[LoadedTexturesCount];

            for(int i = 0; i < LoadedTexturesCount; i++)
            {
                names[i] = LoadedTextures[i].FileName;
            }

            return names;
        }               
    }
}
