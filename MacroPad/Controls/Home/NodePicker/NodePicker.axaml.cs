using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MacroPad.Controls.Home.NodesEditorHistory.Actions;
using MacroPad.Core;
using MacroPad.Core.Config;
using MacroPad.Core.Device;
using MacroPad.Shared.Plugin;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MacroPad.Controls.Home.NodePicker;

public partial class NodePicker : UserControl
{
    public Point AddNodePos { get; set; }
    public bool AutoPos { get; set; } = false;
    public NodesEditor? Editor { get; set; }
    public NodePicker()
    {
        InitializeComponent();
    }

    private List<NodePickerCategory> _categories = [];
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        if (CategoriesPanel.Children.Count != PluginLoader.nodeCategories.Count && Editor != null && Editor.Button != null)
        {
            DeviceOutput? output = null;
            if (Editor.Device != null && Editor.Device.Layout != null && Editor.Device.Layout.OutputTypes.TryGetValue(Editor.Button.Output, out DeviceOutput? value)) output = value;
            CategoriesPanel.Children.Clear();
            foreach (INodeCategory category in PluginLoader.nodeCategories)
            {
                var categoryDisplay = new NodePickerCategory() { Category = category, Button = Editor.Button, DeviceOutput = output };

                categoryDisplay.NodeSelected += (object? sender, NodeSelectedEventArgs e) =>
                {
                    if (Editor != null)
                    {
                        Point? nodePosition;
                        if (AutoPos)
                        {
                            Point? centerScreen = Editor.ZoomAndPan.TranslatePoint(new Point(Editor.ZoomAndPan.Bounds.Width / 2, Editor.ZoomAndPan.Bounds.Height / 2), Editor.DisplayCanvas);
                            if (centerScreen.HasValue) nodePosition = new Point(centerScreen.Value.X/32, centerScreen.Value.Y/32);
                            else nodePosition = new Point(0,0);
                        }
                        else nodePosition = AddNodePos;

                        if (nodePosition.HasValue && e.Id != null)
                        {
                            Debug.WriteLine($"Adding node: {e.Id} at {nodePosition.Value.X}/{nodePosition.Value.Y}");

                            var newLinks = new NodeLinks() { Id = e.Id, X = (int)(nodePosition.Value.X), Y = (int)(nodePosition.Value.Y) };
                            var action = new NodeAddition(newLinks, Editor.CurrentScript.NodesLinks.Count == 0 ? 0 : Editor.CurrentScript.NodesLinks.Keys.Max() + 1, Editor);

                            action.Do();
                            Editor.RecordAction(action);

                            Editor.NodePickerFlyout.Hide();
                        }
                    }
                };

                _categories.Add(categoryDisplay);
                CategoriesPanel.Children.Add(categoryDisplay);
            }
            CategoriesPanel.Children.Last().Loaded += (object? sender, RoutedEventArgs e) =>
            {
                if (SearchBox.Text != "") Search(SearchBox.Text);
            };
        }
    }

    private void SearchBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        Search(SearchBox.Text);
    }
    private void Search(string? query)
    {
        foreach (NodePickerCategory category in _categories)
        {
            if (string.IsNullOrEmpty(query))
            {
                category.IsVisible = true;
                category.ShowAll();
                continue;
            }
            category.IsVisible = category.Search(query);
        }
    }
}