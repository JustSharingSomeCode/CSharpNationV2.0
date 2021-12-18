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
    /// Lógica de interacción para ErrorViewer.xaml
    /// </summary>
    public partial class ErrorViewer : UserControl
    {
        public ErrorViewer(Error err)
        {
            InitializeComponent();

            error = err;

            UpdateStyle();
        }

        private Error error;

        private void UpdateStyle()
        {
            switch (error.ErrorType)
            {
                case Error.Type.Information:
                    MainBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 102, 255));
                    IconLbl.Foreground = new SolidColorBrush(Color.FromRgb(0, 102, 255));
                    IconLbl.Content = "\uE171";
                    break;
                case Error.Type.NonCriticalError:
                    MainBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 0));
                    IconLbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 0));
                    IconLbl.Content = "\uE7BA";
                    break;
                case Error.Type.CriticalError:
                    MainBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    IconLbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    IconLbl.Content = "\uEBE8";
                    break;
            }

            MessageTxt.Text = error.Message;
        }
    }
}
