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

using CSharpNation.Config;
using CSharpNation.Textures;
using CSharpNation.Tools;
using WpfHoverControls;

namespace CSharpNation.GUI
{
    /// <summary>
    /// Lógica de interacción para BackgroundsGUI.xaml
    /// </summary>
    public partial class BackgroundsGUI : UserControl
    {
        public BackgroundsGUI(CSharpNationController ctrl)
        {
            InitializeComponent();

            controller = ctrl;

            LoadedFolderbl.Content = GlobalConfig.TexturesPath;
            BackgroundMovementCb.IsChecked = GlobalConfig.BackgroundMovement;
            //backgrounds = TexturesConfig.Textures;

            string[] enumNames = Enum.GetNames(typeof(Texture.Display));

            for (int i = 0; i < enumNames.Length; i++)
            {
                _ = DisplayModeCb.Items.Add(enumNames[i]);
            }

            UpdateBackgroundList();
        }

        private CSharpNationController controller;
        private List<Texture> backgrounds;

        private bool HandledSelection = false;
        private int selectedBackground = 0;

        private void UpdateBackgroundList()
        {
            BackgroundsStackPnl.Children.Clear();

            backgrounds = TexturesConfig.Textures;

            if (backgrounds.Count > 0)
            {
                LoadTextureData(0);
            }

            for (int i = 0; i < backgrounds.Count; i++)
            {
                HoverButton btn = new HoverButton()
                {
                    Content = "  " +  (i + 1).ToString() + ") " + backgrounds[i].FileName,
                    IconText = i.ToString(),
                    Height = 25,
                    CornerRadius = (i == 0) ? new CornerRadius(5, 5, 0, 0) : (i + 1 - backgrounds.Count == 0) ? new CornerRadius(0, 0, 5, 5) : new CornerRadius(0, 0, 0, 0),
                    //BorderThickness = (i == 0) ? new Thickness(1, 1, 1, 0) : (i + 1 - backgrounds.Count == 0) ? new Thickness(1, 0, 1, 1) : new Thickness(1, 0, 1, 0),
                    BorderThickness = new Thickness(0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Background = (i % 2 == 0) ? new SolidColorBrush(Color.FromRgb(100, 100, 100)) : new SolidColorBrush(Color.FromRgb(120, 120, 120)),
                    BackgroundHover = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255)),
                    ShowIcon = false
                };

                btn.Click += Btn_Click;

                _ = BackgroundsStackPnl.Children.Add(btn);
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            int index = Convert.ToInt32(((HoverButton)sender).IconText);

            LoadTextureData(index);
        }

        private void LoadTextureData(int index)
        {
            selectedBackground = index;

            TitleLbl.Content = backgrounds[index].FileName;
            HandledSelection = true;
            DisplayModeCb.SelectedItem = backgrounds[index].DisplayMode.ToString();
            HandledSelection = false;
        }

        private void PrevBgBtn_Click(object sender, RoutedEventArgs e)
        {
            controller.PreviousBackground();
        }

        private void NextBgBtn_Click(object sender, RoutedEventArgs e)
        {
            controller.NextBackground();
        }

        private void DisplayModeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(DisplayModeCb.SelectedItem.ToString())
            {
                case "Fullscreen":
                    LoadImage(@"\FullscreenPreview.jpg");
                    break;
                case "Halfscreen":
                    LoadImage(@"\HalfscreenPreview.jpg");
                    break;
                case "MirroredLeftHalf":
                    LoadImage(@"\MirroredLeftHalfPreview.jpg");
                    break;
                case "MirroredRightHalf":
                    LoadImage(@"\MirroredRightHalfPreview.jpg");
                    break;
            }

            if (!HandledSelection)
            {
                try
                {
                    Texture.Display selectedDm = (Texture.Display)Enum.Parse(typeof(Texture.Display), DisplayModeCb.SelectedValue.ToString());
                    backgrounds[selectedBackground].DisplayMode = selectedDm;
                }
                catch (Exception ex)
                {
                    ErrorLog.AddError(new Error(Error.Type.CriticalError, "Error parsing display enum. Details: " + ex.Message));
                }
            }
        }

        private void LoadImage(string filename)
        {
            try
            {
                PreviewImg.Source = new BitmapImage(new Uri(GlobalConfig.ResourcesDirectoryPath + filename));
            }
            catch (System.IO.FileNotFoundException)
            {
                ErrorLog.AddError(new Error(Error.Type.CriticalError, "Cant find preview image on resources folder. Missing file: " + filename));
            }
            catch (Exception ex)
            {
                ErrorLog.AddError(new Error(Error.Type.CriticalError, "Error loading preview image: " + filename + ", Details: " + ex.Message));
            }
        }

        private void SaveConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalConfig.SaveConfig();
        }

        private void LoadFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalConfig.TexturesPath = TexturesPathTxt.Text;
            UpdateBackgroundList();
        }

        private void BackgroundMovementCb_Click(object sender, RoutedEventArgs e)
        {
            GlobalConfig.BackgroundMovement = BackgroundMovementCb.IsChecked.Value;
        }
    }
}
