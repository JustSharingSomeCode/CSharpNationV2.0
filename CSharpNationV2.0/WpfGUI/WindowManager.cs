using CSharpNationV2._0.Textures;
using CSharpNationV2._0.Visualizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace CSharpNationV2._0.WpfGUI
{
    public class WindowManager
    {
        public WindowManager()
        {
            wavesWindow = new WavesWindow();
            texturesWindow = new TexturesWindow();
        }

        private WavesWindow wavesWindow;
        private TexturesWindow texturesWindow;

        public WavesWindow GetWaveWindow()
        {
            return wavesWindow;
        }

        public TexturesWindow GetTexturesWindow()
        {
            return texturesWindow;
        }

        public SpectrumWave[] GetWaves()
        {
            return wavesWindow.Waves;
        }

        public TextureManager GetTextureManager()
        {
            return texturesWindow.textureManager;
        }
    }
}
