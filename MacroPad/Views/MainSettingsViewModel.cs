using Avalonia;
using Avalonia.Controls;
using MacroPad.ViewModels;
using MacroPad.Views.Settings;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Views
{
    public class MainSettingsViewModel : ViewModelBase
    {
        private bool _openend = false;
        public bool Openend { get => _openend; set => this.RaiseAndSetIfChanged(ref _openend, value); }
        private int _selectedCat = 0;
        public int SelectedCat { get => _selectedCat; set => this.RaiseAndSetIfChanged(ref _selectedCat, value); }

        public void Open() => Openend = true;
        public void Close() => Openend = false;

        public static StyledElement[] SettingsCategories { get; } = [new General() { DataContext = new GeneralViewModel()}];
    }
}
