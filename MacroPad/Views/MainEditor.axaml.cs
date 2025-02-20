using Avalonia;
using Avalonia.Controls;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Navigation;
using MacroPad.Controls;
using MacroPad.Controls.Home;
using MacroPad.Core.Config;
using MacroPad.Core.Device;
using MacroPad.Pages;
using MacroPad.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MacroPad.Views
{
    public partial class MainEditor : UserControl
    {
        public MainEditor()
        {
            InitializeComponent();
        }

        public void DeviceViewer_ButtonPressed(object? sender, DeviceViewerButtonPressedEventArgs e)
        {
            // Temp code

            if (DataContext is MainEditorViewModel editor && editor.Device.CurrentProfile != null && e.Button != null)
            {
                if (!editor.Device.CurrentProfile.ButtonsConfig.ContainsKey(e.Button.X)) editor.Device.CurrentProfile.ButtonsConfig.Add(e.Button.X, []);
                Dictionary<int, ButtonConfig> buttonColumn = editor.Device.CurrentProfile.ButtonsConfig[e.Button.X];
                if (!buttonColumn.ContainsKey(e.Button.Y)) buttonColumn.Add(e.Button.Y, new ButtonConfig());
                ButtonConfig buttonConfig = buttonColumn[e.Button.Y];
                NodesEditorContainer.Child = new NodesEditor() { ButtonConfig = buttonConfig, Button = e.Button, Device = editor.Device };
                ButtonStatusEditorContainer.Child = new StatusEditor() { ButtonConfig = buttonConfig, Button = e.Button, Device = editor.Device };
            }
        }
    }
}
