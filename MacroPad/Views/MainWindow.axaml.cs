using Avalonia.Controls;
using MacroPad.Core;

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