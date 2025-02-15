using Avalonia;
using Avalonia.Controls;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Navigation;
using MacroPad.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MacroPad.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            if (IsVisible)
            {
                DeviceManager.Config.SaveConfig();
                e.Cancel = true;
                Hide();
            }
        }
    }
}