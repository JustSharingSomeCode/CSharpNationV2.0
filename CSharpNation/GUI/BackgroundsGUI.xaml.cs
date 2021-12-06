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
            backgrounds = TexturesConfig.Textures;

            UpdateBackgroundList();
        }

        private CSharpNationController controller;
        private List<Texture> backgrounds;

        private void UpdateBackgroundList()
        {
            BackgroundsStackPnl.Children.Clear();

            for (int i = 0; i < backgrounds.Count; i++)
            {
                HoverButton btn = new HoverButton()
                {
                    Content = "  " +  (i + 1).ToString() + ") " + backgrounds[i].FileName,
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

                _ = BackgroundsStackPnl.Children.Add(btn);
            }
        }

        private void PrevBgBtn_Click(object sender, RoutedEventArgs e)
        {
            controller.PreviousBackground();
        }

        private void NextBgBtn_Click(object sender, RoutedEventArgs e)
        {
            controller.NextBackground();
        }
    }
}
