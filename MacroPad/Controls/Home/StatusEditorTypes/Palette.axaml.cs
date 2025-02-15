using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System.Globalization;
using System;
using Avalonia.Media;
using MacroPad.Core.Device;
using MacroPad.Core.BasePlugin.Device;
using MacroPad.Core.Config;

namespace MacroPad.Controls.Home.StatusEditorTypes;

public partial class Palette : UserControl
{
    public PaletteValue[]? Colors { get; set; }
    public int SelectedColor { get; set; }
    public DeviceCore Device { get; set; } = new DeviceCore(new BaseDevice());
    public DeviceLayoutButton Button { get; set; }
    public ButtonConfig ButtonConfig { get; set; } = new ButtonConfig();

    public Palette()
    {
        InitializeComponent();

    }

    private void ColorSelector_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        ButtonConfig.Status.Value = ColorSelector.SelectedIndex;
        Device.SetButtonContent(Button, ColorSelector.SelectedIndex);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        ColorSelector.SelectionChanged -= ColorSelector_SelectionChanged;

        if (Colors != null)
        {
            ColorSelector.ItemsSource = Colors;
            ColorSelector.SelectedIndex = SelectedColor;

            ColorSelector.SelectionChanged += ColorSelector_SelectionChanged;
        }
    }
}
public class ColorConverter : IValueConverter
{
    public static readonly ColorConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter,
                                                            CultureInfo culture)
    {
        if (value is uint sourceColor && targetType.IsAssignableTo(typeof(IBrush)))
        {
            return new SolidColorBrush(Color.FromUInt32(sourceColor));
        }
        // converter used for the wrong type
        return new BindingNotification(new InvalidCastException(),
                                                BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType,
                                object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}