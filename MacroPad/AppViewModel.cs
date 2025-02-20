using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using MacroPad.Views;
using Avalonia;
using ReactiveUI;
using System.Reactive;
using MacroPad.Core;

namespace MacroPad
{
    public class AppViewModel
    {
        private readonly MainWindow _mainWindow;

        public AppViewModel()
        {
            _mainWindow = new MainWindow() { DataContext = new MainWindowViewModel()};
            ExitCommand = ReactiveCommand.Create(Exit);
        }

        
        public void ShowWindow()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow ??= _mainWindow;
            }

            _mainWindow.WindowState = WindowState.Normal;
            _mainWindow.Show();
            _mainWindow.BringIntoView();
            _mainWindow.Focus();

        }

        public ReactiveCommand<Unit, Unit> ExitCommand { get; }
        public static void Exit()
        {
            DeviceManager.Config.SaveConfig();
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
            {
                application.Shutdown();
            }
        }
    }
}
