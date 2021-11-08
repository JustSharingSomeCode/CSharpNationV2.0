using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNation.Textures
{
    class Texture
    {
        public enum Display
        {
            NotFound,
            Fullscreen,
            Halfscreen,
            MirroredLeftHalf,
            MirroredRightHalf
        }

        public string Path { get; set; }
        public Display DisplayMode { get; set; }

        public string FileName
        {
            get
            {
                return Path.Split((char)92).Last();
            }
        }

        public void LoadTexture()
        {
            
        }

        public override string ToString()
        {
            return FileName + "|" + DisplayMode.ToString();
        }
    }
}
