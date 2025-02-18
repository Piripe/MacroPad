using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using MacroPad.Controls.Home.NodesEditorHistory.Actions;
using MacroPad.Core;
using MacroPad.Core.Config;
using MacroPad.Core.Device;
using MacroPad.Core.Node;
using MacroPad.Shared.Plugin.Nodes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace MacroPad.Controls.Home;

public partial class NodeLinksDisplay : UserControl
{
    public NodeLinksDisplay()
    {
        InitializeComponent();
        DeleteButton.Click += DeleteButton_Click;
    }

    public bool Deletable { get; set; } = true;
    public object? Links { get; set; }
    public int LinksId { get; set; }
    public NodesEditor? NodesEditor { get; set; }
    public List<Shape> GetInPoints => _getInPoints;
    private List<Shape> _getInPoints = new List<Shape>();
    public List<Shape> GetOutPoints => _getOutPoints;
    private List<Shape> _getOutPoints = new List<Shape>();
    public Shape? RunInPoint => _runInPoint;
    private Shape? _runInPoint;
    public List<Shape> RunOutPoints => _runOutPoints;
    private List<Shape> _runOutPoints = new List<Shape>();
    public Dictionary<int, Path?> GetInLines { get; set; } = new Dictionary<int, Path?>();
    public Dictionary<int, List<Path>> GetOutLines { get; set; } = new Dictionary<int, List<Path>>();
    public List<Path> RunInLines { get; set; } = new List<Path>();
    public Dictionary<int, Path?> RunOutLines { get; set; } = new Dictionary<int, Path?>();
    public List<StackPanel> GetInComponents => _getInComponents;
    private List<StackPanel> _getInComponents = new List<StackPanel>();

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        _getInPoints = new List<Shape>();
        _getOutPoints = new List<Shape>();
        _runOutPoints = new List<Shape>();
        RunInLines.Clear();
        RunOutLines.Clear();
        GetInLines.Clear();
        GetOutLines.Clear();
        InputsContainer.Children.Clear();
        OutputsContainer.Children.Clear();
        int i;

        DeleteButton.IsVisible = Deletable;

        if (Links is ButtonEvent buttonEvent)
        {
                    NodeTitle.Text = buttonEvent switch
                    {
                        ButtonEvent.Init => "Event: Init",
                        ButtonEvent.Pressed => "Event: Pressed",
                        ButtonEvent.Released => "Event: Released",
                        ButtonEvent.ValueChanged => "Event: Value Changed",
                        ButtonEvent.LongPress => "Event: Long Press",
                        _ => "Event: Unknown Event"
                    };
                    AddNodePoint(null, null,0, true, true);

        }
        else if(Links is NodeLinks links)
        {
            string name;
            TypeNamePair[] inputs;
            TypeNamePair[] outputs;
            INodeComponent[] components;
            if (NodeManager.Runners.ContainsKey(links.Id))
            {
                INodeRunner node = NodeManager.Runners[links.Id];
                name = node.Name;
                inputs = node.Inputs;
                outputs = node.Outputs;
                components = node.Components;

                AddNodePoint(null, null, 0, false, true);
                for (i = 0; i < node.RunnerOutputCount; i++)
                {
                    AddNodePoint(null, node.RunnerOutputsName.Length > i ? node.RunnerOutputsName[i] : null, i, true, true);
                }
            }
            else if (NodeManager.Getters.ContainsKey(links.Id))
            {
                INodeGetter node = NodeManager.Getters[links.Id];
                name = node.Name;
                inputs = node.Inputs;
                outputs = node.Outputs;
                components = node.Components;
            }
            else { return; }

            NodeTitle.Text = name;

            foreach (INodeComponent component in components)
            {
                AddNodeComponent(component, new NodeResourceManager(x => { throw new Exception("Can't get value of a component."); }, links.Data));
            }
            i = 0;
            foreach (TypeNamePair node in inputs)
            {
                if (NodeManager.Types.ContainsKey(node.Type)) {
                    INodeComponent[] nodeComponents = NodeManager.Types[node.Type].Components;
                    if (!links.Consts.ContainsKey(i)) links.Consts.Add(i,new Dictionary<string, JToken>());
                }
                AddNodePoint(node.Type, node.Name, i, false, data: links.Consts);
                i++;
            }
            i = 0;
            foreach (TypeNamePair node in outputs)
            {
                AddNodePoint(node.Type, node.Name, i, true);
                i++;
            }
        }

        NodeTitleBar.PointerPressed += NodeTitleBar_PointerPressed;
        NodeTitleBar.PointerMoved += NodeTitleBar_PointerMoved;
        NodeTitleBar.PointerReleased += NodeTitleBar_PointerReleased;
        this.SizeChanged += NodeLinksDisplay_SizeChanged;
    }

    private void NodeLinksDisplay_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (NodesEditor != null)
        {
            for (int i = 0; i < GetOutPoints.Count; i++)
            {
                foreach (Path line in GetOutLines[i])
                {
                    NodesEditor.LinesData[line].Point2 = GetOutPoints[i].TranslatePoint(new Point(6, 6), NodesEditor.DisplayCanvas) ?? new Point(0, 0);
                    NodesEditor.UpdateLinePath(line);
                }
            }
            for (int i = 0; i < RunOutPoints.Count; i++)
            {
                Path? line = RunOutLines[i];
                if (line != null)
                {
                    NodesEditor.LinesData[line].Point1 = RunOutPoints[i].TranslatePoint(new Point(6, 6), NodesEditor.DisplayCanvas) ?? new Point(0, 0);
                    NodesEditor.UpdateLinePath(line);
                }
            }
            for (int i = 0; i < GetInPoints.Count; i++)
            {
                 if (GetInLines.ContainsKey(i))
                 {
                    Path? line = GetInLines[i];
                    if (line != null)
                    {
                        NodesEditor.LinesData[line].Point1 = GetInPoints[i].TranslatePoint(new Point(6, 6), NodesEditor.DisplayCanvas) ?? new Point(0, 0);
                        NodesEditor.UpdateLinePath(line);
                    }
                 }
            }
            foreach (Path line in RunInLines)
            {
                NodesEditor.LinesData[line].Point2 = RunInPoint?.TranslatePoint(new Point(6, 6), NodesEditor.DisplayCanvas) ?? new Point(0, 0);
                NodesEditor.UpdateLinePath(line);
            }
            
        }
    }

    bool isMoving;
    int moveFromX, moveFromY, moveToX, moveToY;
    Point moveMouseBasePoint;
    private void NodeTitleBar_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            var parent = (Canvas?)Parent;
            if (parent != null)
            {
                if (Links is ButtonEvent buttonEvent)
                {
                    if (NodesEditor != null)
                    {
                        moveFromX = NodesEditor.CurrentScript.StartX;
                        moveFromY = NodesEditor.CurrentScript.StartY;
                    }
                }
                else if (Links is NodeLinks links)
                {
                    moveFromX = links.X;
                    moveFromY = links.Y;
                }
                moveMouseBasePoint = e.GetPosition(parent);
                isMoving = true;
            }
        }
    }
    private void NodeTitleBar_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        if (isMoving)
        {
            var parent = (Canvas?)Parent;
            if (parent != null)
            {
                Point point = e.GetPosition(parent);

                moveToX = (int)(Math.Round((point.X - moveMouseBasePoint.X) / 32)) + moveFromX;
                moveToY = (int)(Math.Round((point.Y - moveMouseBasePoint.Y) / 32)) + moveFromY;

                MoveNode(moveToX,moveToY);
            }
        }
    }
    public void MoveNode(int x, int y)
    {
        bool positionUpdated = false;

        if (Links is ButtonEvent buttonEvent)
        {
                    if (NodesEditor != null)
                    {
                        if (NodesEditor.CurrentScript.StartX != x || NodesEditor.CurrentScript.StartY != y)
                        {
                            positionUpdated = true;
                            NodesEditor.CurrentScript.StartX = x;
                            NodesEditor.CurrentScript.StartY = y;
                        }
                    }
        }
        else if (Links is NodeLinks links)
        {
            if (links.X != x || links.Y != y)
            {
                positionUpdated = true;
                links.X = x;
                links.Y = y;
            }
        }
        if (positionUpdated)
        {
            SetValue(Canvas.LeftProperty, x * 32);
            SetValue(Canvas.TopProperty, y * 32);

            UpdateLines(x, y);
        }
    }
    private void NodeTitleBar_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        if (isMoving)
        {
            if (NodesEditor != null) NodesEditor.RecordAction(new NodeMove(new Point(moveFromX, moveFromY), new Point(moveToX, moveToY), this.LinksId, NodesEditor));
            isMoving = false;
        }
    }
    private void UpdateLines(int x, int y)
    {
        if (NodesEditor != null)
        {
            foreach (KeyValuePair<int, Path?> getInLine in GetInLines)
            {
                if (getInLine.Value == null) continue;
                Point? newPoint = GetInPoints[getInLine.Key].TranslatePoint(new Point(6, 6), this);
                if (newPoint.HasValue) NodesEditor.LinesData[getInLine.Value].Point1 = newPoint.Value.WithX(newPoint.Value.X + x*32).WithY(newPoint.Value.Y+y*32);
                NodesEditor.UpdateLinePath(getInLine.Value);
            }
            foreach (KeyValuePair<int, List<Path>> getOutLines in GetOutLines)
            {
                foreach(Path getOutLine in getOutLines.Value)
                {
                    Point? newPoint = GetOutPoints[getOutLines.Key].TranslatePoint(new Point(6, 6), this);
                    if (newPoint.HasValue) NodesEditor.LinesData[getOutLine].Point2 = newPoint.Value.WithX(newPoint.Value.X + x * 32).WithY(newPoint.Value.Y + y * 32);
                    NodesEditor.UpdateLinePath(getOutLine);
                }
            }
            foreach (KeyValuePair<int, Path?> runOutLine in RunOutLines)
            {
                if (runOutLine.Value == null) continue;
                Point? newPoint = RunOutPoints[runOutLine.Key].TranslatePoint(new Point(6, 6), this);
                if (newPoint.HasValue) NodesEditor.LinesData[runOutLine.Value].Point1 = newPoint.Value.WithX(newPoint.Value.X + x * 32).WithY(newPoint.Value.Y + y * 32);
                NodesEditor.UpdateLinePath(runOutLine.Value);
            }
            foreach (Path runInLine in RunInLines)
            {
                if (RunInPoint == null) continue;
                Point? newPoint = RunInPoint.TranslatePoint(new Point(6, 6), this);
                if (newPoint.HasValue) NodesEditor.LinesData[runInLine].Point2 = newPoint.Value.WithX(newPoint.Value.X + x * 32).WithY(newPoint.Value.Y + y * 32);
                NodesEditor.UpdateLinePath(runInLine);
            }
        }
    }


    public DockPanel GetNodeComponent(INodeComponent component, IResourceManager resource, bool smallMode = false)
    {
        DockPanel dockPanel = new DockPanel()
        {
            Margin = new Thickness(8, 0, 0, 0),
        };

        switch (component)
        {
            case Shared.Plugin.Nodes.Components.TextBox textBox:
                TextBox textBoxControl = new TextBox() { Text = textBox.GetText != null ? textBox.GetText(resource) : "" };
                textBoxControl.TextChanged += (object? sender, TextChangedEventArgs e) => {
                    if (textBox.TextChanged != null) textBox.TextChanged(resource, textBoxControl.Text);
                };
                textBoxControl.Width = 196;
                if (smallMode)
                {
                    textBoxControl.Height = 20d;
                    textBoxControl.Classes.Add("small");
                }
                    dockPanel.Children.Add(textBoxControl);
                break;
            case Shared.Plugin.Nodes.Components.NumericUpDown numericUpDown:
                NumericUpDown numericUpDownControl = new NumericUpDown()
                {
                    Minimum = numericUpDown.Min,
                    Maximum = numericUpDown.Max,
                    Value = numericUpDown.GetValue != null ? numericUpDown.GetValue(resource) : 0
                };
                numericUpDownControl.ValueChanged += (object? sender, NumericUpDownValueChangedEventArgs e) => {
                    if (numericUpDown.ValueChanged != null) numericUpDown.ValueChanged(resource, numericUpDownControl.Value ?? 0);
                };
                numericUpDownControl.Width = 196;
                if (smallMode)
                {
                    numericUpDownControl.Height = 20d;
                    numericUpDownControl.Classes.Add("small");
                }
                dockPanel.Children.Add(numericUpDownControl);
                break;
            case Shared.Plugin.Nodes.Components.ComboBox comboBox:
                ObservableCollection<string> GetItems()
                {
                    if (comboBox.GetItems != null && NodesEditor?.Button != null && NodesEditor.Device?.Layout != null && NodesEditor.Device.Layout.OutputTypes.TryGetValue(NodesEditor.Button.Output, out DeviceOutput? value))
                    {
                        return new ObservableCollection<string>(comboBox.GetItems(resource, NodesEditor.Button, value));
                    }
                    else
                    {
                        return comboBox.Items;
                    }
                }
                ObservableCollection<string> items = GetItems();
                ComboBox comboBoxControl = new() { ItemsSource = items};
                
                void UpdateSelection()
                {
                    int index = 0;
                    if (comboBox.GetSelectedItem != null)
                    {
                        string value = comboBox.GetSelectedItem(resource);
                        index = items.IndexOf(value);
                    }
                    if (comboBox.GetSelection != null)
                    {
                        index = comboBox.GetSelection(resource);
                    }
                    if (index < items.Count && index >= 0) comboBoxControl.SelectedIndex = index;
                }
                UpdateSelection();


                comboBoxControl.Items.CollectionChanged += (s, e) =>
                {
                    UpdateSelection();
                };

                comboBoxControl.SelectionChanged += (object? sender, SelectionChangedEventArgs e) => {
                    if (comboBox.SelectionChanged != null) comboBox.SelectionChanged(resource, comboBoxControl.SelectedIndex);
                };
                comboBoxControl.Width = 196;
                if (smallMode)
                {
                    comboBoxControl.Height = 20;
                    comboBoxControl.Classes.Add("small");
                }
                    dockPanel.Children.Add(comboBoxControl);
                break;
        }

        return dockPanel;
    }

    public void AddNodeComponent(INodeComponent component, IResourceManager resource)
    {
        InputsContainer.Children.Add(GetNodeComponent(component, resource));
    }
    public void AddNodePoint(Type? type, string? name, int index, bool isOutput, bool isRunner = false, Dictionary<int, Dictionary<string, JToken>>? data = null)
    {
        DockPanel dockPanel = new DockPanel();
        Shape linkShape;
        if (isRunner)
        {
            linkShape = new Path() {
                Data = Geometry.Parse("M2,1L6,1L11,6L6,11L2,11Z"),
                Fill = Brushes.LightGray
            };
        }
        else if (type != null)
        {
            Shared.Media.Color color;
            if (NodeManager.Types.ContainsKey(type)) color = NodeManager.Types[type].Color;
            else color = new Shared.Media.Color();
            linkShape = new Ellipse() { Fill = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B)) };
        }
        else { return; }
        linkShape.Stroke = new SolidColorBrush(Color.FromArgb(64, 0, 0, 0));
        linkShape.StrokeThickness = 2;
        linkShape.Width = 12;
        linkShape.Height = 12;
        linkShape.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
        linkShape.Margin = new Thickness(8, 0);
        linkShape.SetValue(DockPanel.DockProperty, isOutput ? Dock.Right : Dock.Left);
        linkShape.HorizontalAlignment = isOutput ? Avalonia.Layout.HorizontalAlignment.Right : Avalonia.Layout.HorizontalAlignment.Left;
        linkShape.Tag = "LinkPoint";

        if (isOutput && isRunner)
        {
            _runOutPoints.Add(linkShape);
            RunOutLines.Add(index, null);
            linkShape.PointerPressed += RunOutLinkShape_PointerPressed;

        }
        else if (isOutput && !isRunner)
        {
            _getOutPoints.Add(linkShape);
            GetOutLines.Add(index, new List<Path>());
            linkShape.PointerPressed += GetOutLinkShape_PointerPressed;
        }
        else if (!isOutput && isRunner)
        {
            _runInPoint = linkShape;
            linkShape.PointerPressed += RunInLinkShape_PointerPressed;
        }
        else if (!isOutput && !isRunner)
        {
            _getInPoints.Add(linkShape);
            GetInLines.Add(index, null);
            linkShape.PointerPressed += GetInLinkShape_PointerPressed;
        }

        dockPanel.Children.Add(linkShape);
        if (name != null) dockPanel.Children.Add(new TextBlock() { Text = name, HorizontalAlignment = isOutput ? Avalonia.Layout.HorizontalAlignment.Right : Avalonia.Layout.HorizontalAlignment.Left});

        if (!isOutput && !isRunner)
        {
            StackPanel componentsPanel = new StackPanel() { Orientation = Avalonia.Layout.Orientation.Vertical, Spacing = 4};
            if (type != null && data != null && NodeManager.Types.ContainsKey(type))
            {
                INodeComponent[] components = NodeManager.Types[type].Components;
                for (int i = 0; i < components.Length; i++)
                {
                    INodeComponent component = components[i];
                    if (!data.ContainsKey(index)) data.Add(index, new Dictionary<string, JToken>());

                    DockPanel componentPanel = GetNodeComponent(component, new NodeResourceManager(x => { throw new Exception("Can't get value of a component."); }, data[index]), true);
                    componentPanel.SetValue(DockPanel.DockProperty, Dock.Right);
                    componentsPanel.Children.Add(componentPanel);
                }
            }
            _getInComponents.Add(componentsPanel);
            dockPanel.Children.Add(componentsPanel);
        }

        if (isOutput) OutputsContainer.Children.Add(dockPanel);
        else InputsContainer.Children.Add(dockPanel);
    }
    private void RunOutLinkShape_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        
            if (NodesEditor != null && sender is Shape linkShape && e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
            {
            
                Point? basePoint = linkShape.TranslatePoint(new Point(6, 6), NodesEditor.DisplayCanvas);
                Point mousePoint = e.GetPosition(NodesEditor.DisplayCanvas);
            if (basePoint.HasValue)
            {
                int index = _runOutPoints.IndexOf(linkShape);
                Path? line = RunOutLines[index];
                if (line == null)
                {
                    line = NodesEditor.AddLine(Brushes.LightGray, basePoint.Value, mousePoint, NodesEditor.CurrentScript.NodeLines.Count == 0 ? 0 : NodesEditor.CurrentScript.NodeLines.Keys.Max() + 1,-1);
                    NodesEditor.TracingLine = line;
                    NodesEditor.TracingLineOut = false;
                    NodesEditor.TracingLineRunner = true;
                    NodesEditor.TracingLineBase = this;
                    NodesEditor.TracingLineBaseIndex = index;
                    NodesEditor.TracingLineStartLinks = null;
                }
                else
                {
                    NodesEditor.TracingLine = line;
                    NodesEditor.TracingLineOut = true;
                    NodesEditor.TracingLineRunner = true;
                    NodeLine nodeLine = NodesEditor.GetLineNodeLine(line);
                    NodesEditor.TracingLineBase = NodesEditor.GetNodeLineEndLinks(nodeLine);
                    NodesEditor.TracingLineBaseIndex = NodesEditor.GetNodeLineEndIndex(nodeLine);
                    NodesEditor.TracingLineStartLinks = Links;
                    NodesEditor.TracingLineStartIndex = index;
                }
            }
        }
    }

    private void GetOutLinkShape_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (NodesEditor != null && sender is Shape linkShape && e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
        {
            Point? basePoint = linkShape.TranslatePoint(new Point(6, 6), NodesEditor.DisplayCanvas);
            Point mousePoint = e.GetPosition(NodesEditor.DisplayCanvas);
            if (basePoint.HasValue)
            {
                int index = _getOutPoints.IndexOf(linkShape);

                Path line = NodesEditor.AddLine(linkShape.Fill, mousePoint, basePoint.Value, NodesEditor.CurrentScript.NodeLines.Count == 0 ? 0 : NodesEditor.CurrentScript.NodeLines.Keys.Max()+1,-1);
                NodesEditor.TracingLine = line;
                NodesEditor.TracingLineOut = false;
                NodesEditor.TracingLineRunner = false;
                NodesEditor.TracingLineBase = this;
                NodesEditor.TracingLineBaseIndex = index;
                NodesEditor.TracingLineStartLinks = null;
            }
        }
    }

    private void RunInLinkShape_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (NodesEditor != null && sender is Shape linkShape && e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
        {
            Point? basePoint = linkShape.TranslatePoint(new Point(6, 6), NodesEditor.DisplayCanvas);
            Point mousePoint = e.GetPosition(NodesEditor.DisplayCanvas);
            if (basePoint.HasValue)
            {
                Path line = NodesEditor.AddLine(Brushes.LightGray, mousePoint, basePoint.Value, NodesEditor.CurrentScript.NodeLines.Count == 0 ? 0 : NodesEditor.CurrentScript.NodeLines.Keys.Max() + 1, -1);
                NodesEditor.TracingLine = line;
                NodesEditor.TracingLineOut = true;
                NodesEditor.TracingLineRunner = true;
                NodesEditor.TracingLineBase = this;
                NodesEditor.TracingLineBaseIndex = 0;
                NodesEditor.TracingLineStartLinks = null;
            }
            }
    }

    private void GetInLinkShape_PointerPressed(object? sender, PointerPressedEventArgs e)
    {        if (NodesEditor != null && Links != null && sender is Shape linkShape && e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
        {
            Point? basePoint = linkShape.TranslatePoint(new Point(6, 6), NodesEditor.DisplayCanvas);
            Point mousePoint = e.GetPosition(NodesEditor.DisplayCanvas);
            if (basePoint.HasValue)
            {
                int index = _getInPoints.IndexOf(linkShape);
                Path? line = GetInLines[index];
                if (line == null)
                {
                    line = NodesEditor.AddLine(linkShape.Fill, basePoint.Value, mousePoint, NodesEditor.CurrentScript.NodeLines.Count == 0 ? 0 : NodesEditor.CurrentScript.NodeLines.Keys.Max() + 1, -1);
                    NodesEditor.TracingLine = line;
                    NodesEditor.TracingLineOut = true;
                    NodesEditor.TracingLineRunner = false;
                    NodesEditor.TracingLineBase = this;
                    NodesEditor.TracingLineBaseIndex = index;
                    NodesEditor.TracingLineStartLinks = null;

                }
                else
                {
                    NodesEditor.TracingLine = line;
                    NodesEditor.TracingLineOut = false;
                    NodesEditor.TracingLineRunner = false;
                    NodeLine nodeLine = NodesEditor.GetLineNodeLine(line);
                    NodesEditor.TracingLineBase = NodesEditor.GetNodeLineEndLinks(nodeLine);
                    NodesEditor.TracingLineBaseIndex = NodesEditor.GetNodeLineEndIndex(nodeLine);
                    NodesEditor.TracingLineStartLinks = Links;
                    NodesEditor.TracingLineStartIndex = index;
                }
            }
            }
    }

    public void LinkShape_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (NodesEditor == null) return;
        if (sender is Shape linkShape)
        {

            bool isOutput = false, isRunner = false;
            if (_runOutPoints.Contains(linkShape))
            {
                isOutput = true;
                isRunner = true;
            }
            else if (_getOutPoints.Contains(linkShape))
            {
                isOutput = true;
            }
            else if (_runInPoint == linkShape)
            {
                isRunner = true;
            }

            if (NodesEditor.TracingLine != null)
            {
                if ((isOutput == NodesEditor.TracingLineOut) && (isRunner == NodesEditor.TracingLineRunner))
                {
                    Point? endPoint = linkShape.TranslatePoint(new Point(6, 6), NodesEditor.DisplayCanvas);
                    if (endPoint.HasValue) NodesEditor.MoveTracingLine(endPoint.Value);
                }
                else
                {
                    NodesEditor.MoveTracingLine(e.GetPosition(NodesEditor.DisplayCanvas));
                }
            }
            else
            {
                // TODO: Change Cursor in function of the condition
            }
        }

    }
    private void DeleteButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Links is NodeLinks nodeLinks && NodesEditor != null)
        {
            var action = new NodeDeletion(nodeLinks, LinksId, NodesEditor);

            action.Do();

            NodesEditor.RecordAction(action);
        }
    }
}