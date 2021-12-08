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

using CSharpNation.Tools;

namespace CSharpNation.GUI
{
    /// <summary>
    /// Lógica de interacción para ErrorsGUI.xaml
    /// </summary>
    public partial class ErrorsGUI : UserControl
    {
        public ErrorsGUI()
        {
            InitializeComponent();

            ErrorLog.OnErrorAdded += UpdateError;
        }

        public void UpdateError(object sender, EventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                _ = Test.Children.Add(new Label() { Content = (string)sender });
            }));
        }

        private void HoverButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorLog.AddError("test message");
        }
    }
}
