using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MacroPad.Views.Settings;

public partial class General : UserControl
{
    public General()
    {
        InitializeComponent();
    }
    public override string ToString() => "General";
}