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

using CSharpNationV2._0.Configuration;

namespace CSharpNationV2._0.WpfGUI
{
    /// <summary>
    /// Lógica de interacción para ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : UserControl
    {
        public ErrorWindow()
        {
            InitializeComponent();            

            LoadErrors();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ErrorManager.ErrorAdded += UpdateList;
        }

        private void LoadErrors()
        {
            List<string> errors = ErrorManager.ErrorMessages;

            for(int i = 0; i < errors.Count; i++)
            {
                ErrorLogSp.Children.Add(GetLabel(errors[i]));
            }
        }

        public void UpdateList(object sender, EventArgs e)
        {            
            ErrorLogSp.Dispatcher.Invoke(new Action(() => {
                ErrorLogSp.Children.Add(GetLabel(sender));
            }));
        }

        private Label GetLabel(object content)
        {
            return new Label()
            {
                Content = content,
                Foreground = Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(0, 0, 0, 1),
                BorderBrush = new SolidColorBrush(Colors.White),
                Margin = new Thickness(0, 5, 5, 0)
            };
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ErrorLogSp.Children.Clear();
        }
    }
}
