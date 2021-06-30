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
using System.Windows.Shapes;

using System.Windows.Media.Animation;

namespace CSharpNationV2._0.WpfGUI
{
    /// <summary>
    /// Lógica de interacción para CSharpNationManager.xaml
    /// </summary>
    public partial class CSharpNationManager : Window
    {
        public CSharpNationManager()
        {
            InitializeComponent();

            windowManager = new WindowManager();

            ChangeWindows(windowManager.GetWaveWindow());
        }

        private WindowManager windowManager;        

        private bool IsMenuExpanded = false;

        private void TitleGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.WindowState = WindowState.Normal;
                DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ExpandBtn_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation()
            {
                From = MenuGrid.ActualWidth,
                To = IsMenuExpanded ? 50 : 150,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new CubicEase()
            };

            MenuGrid.BeginAnimation(WidthProperty, anim);

            IsMenuExpanded = !IsMenuExpanded;
        }

        private void MenuGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Margin = new Thickness(MenuGrid.Width, 0, 0, 0);
        }

        private void WavesBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeWindows(windowManager.GetWaveWindow());
        }

        private void TexturesBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeWindows(windowManager.GetTexturesWindow());
        }

        private void ChangeWindows(FrameworkElement window)
        {
            ContentGrid.Children.Clear();

            ContentGrid.Children.Add(window);
        }
    }
}
