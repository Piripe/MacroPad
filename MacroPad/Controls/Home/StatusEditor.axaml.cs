using Avalonia;
using Avalonia.Controls;
using MacroPad.Core.BasePlugin.Device;
using MacroPad.Core.Config;
using MacroPad.Core.Device;
using MacroPad.Shared.Device;
using Newtonsoft.Json.Linq;

namespace MacroPad.Controls.Home;

public partial class StatusEditor : UserControl
{
    public DeviceCore Device { get; set; } = new DeviceCore(new BaseDevice());
    public DeviceLayoutButton? Button { get; set; }
    public ButtonConfig ButtonConfig { get; set; } = new ButtonConfig();
    public StatusEditor()
    {
        InitializeComponent();
    }
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        if (Device.Layout != null && Button != null && Device.Layout.OutputTypes.TryGetValue(Button.Output, out DeviceOutput? output))
        {
            switch (output.OutputType)
            {
                case OutputType.Palette:
                    int selectedColor = 0;
                    if (ButtonConfig.Status.Value != null) selectedColor = ButtonConfig.Status.Value.Value<int?>() ?? 0;
                    StatusContainer.Content = new StatusEditorTypes.Palette() { Colors = (PaletteValue[])output.Palette, SelectedColor = selectedColor, Device = Device, Button = Button, ButtonConfig = ButtonConfig };
                    break;
            }
        }
    }
}