using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MacroPad.Core;
using MacroPad.Core.Device;
using MacroPad.Views;
using System.Diagnostics;
using System.Linq;

namespace MacroPad
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                DeviceManager.Init();

                DataContext = new AppViewModel();
                if (!(desktop.Args?.Contains("-m")??true)) (DataContext as AppViewModel)?.ShowWindow();
                desktop.Exit += Desktop_Exit;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void Desktop_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
        {
            foreach (DeviceCore device in DeviceManager.ConnectedDevices)
            {
                device.Disconnect();
            }
        }
    }
}