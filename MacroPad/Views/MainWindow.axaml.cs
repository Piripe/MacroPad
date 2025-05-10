using Avalonia.Controls;
using MacroPad.Core;
using MacroPad.Views.Navigation;
using System.Diagnostics;

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

        private void ListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            MainEditorViewModel? editor = (DataContext as MainWindowViewModel)?.GetDeviceEditor(((sender as ListBox)?.SelectedItem as DeviceNavViewModel)?.Device);

            if (editor == null) return;

            if (deviceEditorView.Child?.GetType() != typeof(MainEditor)) deviceEditorView.Child = new MainEditor() { DataContext = editor};
            else deviceEditorView.Child.DataContext = editor;
        }
    }
}