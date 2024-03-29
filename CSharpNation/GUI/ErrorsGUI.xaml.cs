﻿using System;
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
                _ = LogStackPnl.Children.Add(new ErrorViewer((Error)sender) { Margin = new Thickness(0, 0, 0, 5) });
                LogScrollViewer.ScrollToBottom();
            }));
        }
    }
}
