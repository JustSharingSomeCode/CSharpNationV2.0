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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CSharpNation.GUI
{
    /// <summary>
    /// Lógica de interacción para CSharpNationGUI.xaml
    /// </summary>
    public partial class CSharpNationGUI : Window
    {
        public CSharpNationGUI()
        {
            InitializeComponent();

            waves = new WavesGUI();
            backgrounds = new BackgroundsGUI();
            errors = new ErrorsGUI();
        }

        CSharpNationController controller = new CSharpNationController();

        private WavesGUI waves;
        private BackgroundsGUI backgrounds;
        private ErrorsGUI errors;

        private bool IsMenuExpanded = false;

        private void StartTemp_Click(object sender, RoutedEventArgs e)
        {
            controller.StartVisualizer();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controller.Cleanup();
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            controller.PauseCapture();
        }

        private void ResumeBtn_Click(object sender, RoutedEventArgs e)
        {
            controller.ResumeCapture();
        }

        private void PreviousBtn_Click(object sender, RoutedEventArgs e)
        {
            controller.PreviousBackground();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            controller.NextBackground();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TopMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ExpandBtn_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation()
            {
                From = LeftMenu.ActualWidth,
                To = IsMenuExpanded ? 50 : 150,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new CubicEase()
            };

            LeftMenu.BeginAnimation(WidthProperty, anim);

            IsMenuExpanded = !IsMenuExpanded;
        }

        private void WavesBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeControl(waves);
        }

        private void BackgroundsBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeControl(backgrounds);
        }

        private void ErrorsBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeControl(errors);
        }

        private void ChangeControl(FrameworkElement ui)
        {
            ContentGrid.Children.Clear();

            ContentGrid.Children.Add(ui);
        }
    }
}
