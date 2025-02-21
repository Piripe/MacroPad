using Avalonia.Controls;
using MacroPad.Utilities;
using System;
using System.Runtime.Versioning;

namespace MacroPad.Pages.Settings;

public partial class General : UserControl
{
    public General()
    {
        InitializeComponent();
        if (OperatingSystem.IsWindows())
        {
            WindowsUtils.StartupStatus status = WindowsUtils.GetAppStartupStatus();

            RunStartup.IsChecked = status.OnStartup;
            RunStartup.IsCheckedChanged += UpdateStartupSettings_Windows;
            RunMinimized.IsChecked = status.Minimized;
            RunMinimized.IsCheckedChanged += UpdateStartupSettings_Windows;
        }
    }

    [SupportedOSPlatform("windows")]
    private void UpdateStartupSettings_Windows(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        WindowsUtils.SetAppRunOnLaunch(RunStartup.IsChecked??false, RunMinimized.IsChecked??false);
    }
}