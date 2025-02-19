using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MacroPad.Core.Device;
using MacroPad.Shared.Plugin;
using MacroPad.Shared.Plugin.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MacroPad.Controls.Home.NodePicker;

public partial class NodePickerCategory : UserControl
{
    public INodeCategory? Category { get; set; }
    public event EventHandler<NodeSelectedEventArgs>? NodeSelected;
    public DeviceLayoutButton? Button { get; set; }
    public DeviceOutput? DeviceOutput { get; set; }
    public NodePickerCategory()
    {
        InitializeComponent();

        Loaded += NodePickerCategory_Loaded;
    }

    private void NodePickerCategory_Loaded(object? sender, RoutedEventArgs e)
    {
        IsVisible = false;
        if (Category != null && DeviceOutput != null && Button != null)
        {
            NodesPanel.Children.Clear();

            CategoryExpander.Header = Category.Name;
            foreach (INodeGetter getter in Category.Getters)
            {
                if (getter.IsVisible(Button, DeviceOutput)) AddNode(getter.Name, getter.Description, getter.Id);
            }
            foreach (INodeRunner runner in Category.Runners)
            {
                if (runner.IsVisible(Button, DeviceOutput)) AddNode(runner.Name, runner.Description, runner.Id);
            }
        }
    }

    private readonly HashSet<Button> _buttons = [];
    private void AddNode(string name, string description, string id)
    {
        var button = new Button()
        {
            Tag = new string[] { id, name, description },
            Margin = new Thickness(2),
            Content = new StackPanel()
            {
                Orientation = Avalonia.Layout.Orientation.Vertical,
                Spacing = 2,
                Children =
                {
                    new TextBlock() { Text = name },
                    new TextBlock() { Text = description, Opacity = 0.75, TextWrapping = Avalonia.Media.TextWrapping.Wrap },
                }
            },
        };

        button.Click += (object? sender, RoutedEventArgs e) =>
        {
            NodeSelected?.Invoke(sender, new NodeSelectedEventArgs(Category?.Id+"."+id));
        };

        _buttons.Add(button);
        NodesPanel.Children.Add(button);

        IsVisible = true;
    }

    public bool Search(string searchText)
    {
        bool visible = false;
        foreach (Button button in _buttons)
        {
            if (button.Tag is string[] tags)
            {
                if (!string.IsNullOrEmpty(tags.FirstOrDefault(x => x.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))))
                {
                    button.IsVisible = true;
                    visible = true;
                }
                else button.IsVisible = false;
            }
        }
        return visible;
    }
    public void ShowAll()
    {
        foreach (Button button in _buttons)
        {
            button.IsVisible = true;
        }
    }
}