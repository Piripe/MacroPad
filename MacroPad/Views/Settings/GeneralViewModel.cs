using MacroPad.Utilities;
using MacroPad.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Views.Settings
{
    public class GeneralViewModel : ViewModelBase
    {
        private bool _runStartup = false;
        public bool RunStartup { get => _runStartup; set => this.RaiseAndSetIfChanged(ref _runStartup, value); }
        private bool _runMin = false;
        public bool RunMin { get => _runMin; set => this.RaiseAndSetIfChanged(ref _runMin, value); }
        public GeneralViewModel()
        {
            if (OperatingSystem.IsWindows())
            {
                WindowsUtils.StartupStatus status = WindowsUtils.GetAppStartupStatus();

                RunStartup = status.OnStartup;
                RunMin = status.Minimized;
            }
            this.PropertyChanged += GeneralViewModel_PropertyChanged;
        }

        private void GeneralViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (OperatingSystem.IsWindows())
            {
                WindowsUtils.SetAppRunOnLaunch(RunStartup, RunMin);
            }
        }
    }
}
