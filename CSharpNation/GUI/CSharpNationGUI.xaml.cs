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
        }

        CSharpNationController controller = new CSharpNationController();

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
    }
}
