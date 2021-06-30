using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using CSharpNationV2._0.Textures;
using CSharpNationV2._0.Configuration;
using WpfLightControls;

namespace CSharpNationV2._0.WpfGUI
{
    /// <summary>
    /// Lógica de interacción para TexturesWindow.xaml
    /// </summary>
    public partial class TexturesWindow : UserControl
    {
        public TexturesWindow()
        {
            InitializeComponent();

            textureManager = new TextureManager();

            BackgroundsFolderTxt.Text = ConfigManager.BackgroundsFolder;
            LoadedFolderTxt.Text = textureManager.LoadedFolder;

            FoundLbl.Content = "Founded files: " + textureManager.LoadedTexturesCount.ToString();

            UpdateFileNamesList();

            string[] enumNames = Enum.GetNames(typeof(TextureManager.DisplayMode));

            for(int i = 0; i < enumNames.Length; i++)
            {
                DisplayModeCb.Items.Add(enumNames[i]);
            }        
            
            if(textureManager.LoadedTexturesCount != 0)
            {
                LoadTextureData(0);
            }
        }

        TextureManager textureManager;
        TextureData textureData;

        private void UpdateFileNamesList()
        {
            FileNamesSp.Children.Clear();

            string[] names = textureManager.GetFileNames();

            for(int i = 0; i < names.Length; i++)
            {
                LightButton btn = new LightButton()
                {
                    Content = (i + 1).ToString() + ") " + names[i],
                    ShowIcon = false,
                    Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                    HorizontalContentAlignment = HorizontalAlignment.Left
                };

                btn.Click += Btn_Click;

                FileNamesSp.Children.Add(btn);
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            LoadTextureData(int.Parse(((LightButton)sender).Content.ToString().Split(')')[0]) - 1);
        }

        private void LoadTextureData(int index)
        {
            textureData = textureManager.LoadedTextures[index];

            if (textureData != null)
            {
                FilePathTxt.Text = textureData.Path;
                DisplayModeCb.SelectedItem = textureData.DisplayMode.ToString();
            }
        }

        private void SaveConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfigurationManager.SaveBackgrounds(textureManager.LoadedTextures);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DisplayModeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textureData.DisplayMode = (TextureManager.DisplayMode)Enum.Parse(typeof(TextureManager.DisplayMode), DisplayModeCb.SelectedValue.ToString());
        }
    }
}
