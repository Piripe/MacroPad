using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using DynamicData.Kernel;
using FluentAvalonia.UI.Controls;
using MacroPad.Controls.Home.NodesEditorHistory;
using MacroPad.Controls.Home.NodesEditorHistory.Actions;
using MacroPad.Core;
using MacroPad.Core.Config;
using MacroPad.Core.Device;
using MacroPad.Shared.Device;
using MacroPad.Views;
using Newtonsoft.Json;
using Splat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MacroPad.Controls.Home;

public partial class NodesEditor : UserControl
{
    public ButtonConfig ButtonConfig { get; set; } = new ButtonConfig();
    public DeviceLayoutButton? Button { get; set; }
    public DeviceCore? Device { get; set; }
    public NodeScript CurrentScript => _currentScript;
    public Dictionary<Path, NodeLineData> LinesData { get; set; } = new Dictionary<Path, NodeLineData>();
    public Path? TracingLine { get; set; }
    public bool TracingLineOut { get; set; }
    public bool TracingLineRunner { get; set; }
    public NodeLinksDisplay? TracingLineBase { get; set; }
    public int TracingLineBaseIndex { get; set; }
    public object? TracingLineStartLinks { get; set; }
    public int TracingLineStartIndex { get; set; }

    private NodeScript _currentScript = new NodeScript();

    public NodeLinksDisplay? EventStartNodeLink => _eventStartNodeLink;
    private NodeLinksDisplay? _eventStartNodeLink;
    public Dictionary<NodeLinks, NodeLinksDisplay> CurrentScriptNodeLinks => _nodeLinks;
    private Dictionary<NodeLinks, NodeLinksDisplay> _nodeLinks = new Dictionary<NodeLinks, NodeLinksDisplay>();

    private List<IHistoryAction> _history = new List<IHistoryAction>();
    private int _historyIndex = -1;

    private Point _addNodePoint;

    public Flyout NodePickerFlyout => _nodePickerFlyout;
    private Flyout _nodePickerFlyout;
    private NodePicker.NodePicker _nodePicker;

    public NodesEditor()
    {
        InitializeComponent();
        EventSelector.SelectionChanged += EventSelector_SelectionChanged;
        PointerMoved += NodesEditor_PointerMoved;
        PointerReleased += NodesEditor_PointerReleased;
        PointerPressed += NodesEditor_PointerPressed;
        ZoomAndPan.AddHandler(PointerWheelChangedEvent, ZoomAndPan_PointerWheelChanged, RoutingStrategies.Direct | RoutingStrategies.Tunnel | RoutingStrategies.Bubble, true);

        _nodePicker = new NodePicker.NodePicker();
        _nodePickerFlyout = new Flyout() { Content = _nodePicker };
    }

    private void ZoomAndPan_PointerWheelChanged(object? sender, Avalonia.Input.PointerWheelEventArgs e)
    {
        if (e.Delta.Y > 0 && ZoomAndPan.ZoomX >= 1)
            e.Handled = true;
    }

    private void ZoomAndPan_ZoomChanged(object sender, Avalonia.Controls.PanAndZoom.ZoomChangedEventArgs e)
    {

        if (ZoomAndPan.Background is VisualBrush visualBrush) visualBrush.DestinationRect = new RelativeRect(e.OffsetX*-1, e.OffsetY*-1, 20, 20,RelativeUnit.Absolute);

    }

    private void NodesEditor_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(DisplayCanvas).Properties.IsRightButtonPressed)
        {
            if (TracingLine == null)
            {
                if (e.Source == ZoomAndPan)
                {
                     Point displayCanvasPos = e.GetPosition(DisplayCanvas);
                     _addNodePoint = new Point((int)(displayCanvasPos.X / 32), (int)(displayCanvasPos.Y / 32));
                     _nodePicker.AddNodePos = _addNodePoint;
                     _nodePicker.Editor = this;
                     ContextFlyout = _nodePickerFlyout;
                }
                else
                {
                    ContextFlyout = null;
                }
            }
            else
            {
                //TODO: Link automatically line to the new node
            }
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        _history.Clear();
        _historyIndex = -1;
        EventSelector.SelectedIndex = Button.Type switch
        {
            ButtonType.Slider => 3,
            _ => 1
        };
    }

    private void EventSelector_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (!ButtonConfig.EventScripts.ContainsKey((ButtonEvent)EventSelector.SelectedIndex)) ButtonConfig.EventScripts.Add((ButtonEvent)EventSelector.SelectedIndex, new NodeScript());
        _currentScript = ButtonConfig.EventScripts[(ButtonEvent)EventSelector.SelectedIndex];
        DisplayCanvas.Children.Clear();
        _nodeLinks.Clear();
        _history.Clear();
        _historyIndex = -1;

        NodeLinksDisplay startNodeLinks = new NodeLinksDisplay()
        {
            Links = (ButtonEvent)EventSelector.SelectedIndex,
            NodesEditor = this,
            LinksId = -1,
            Deletable = false,
        };
        startNodeLinks.SetValue(Canvas.LeftProperty, CurrentScript.StartX * 32);
        startNodeLinks.SetValue(Canvas.TopProperty, CurrentScript.StartY * 32);
        DisplayCanvas.Children.Add(startNodeLinks);
        _eventStartNodeLink = startNodeLinks;

        foreach (KeyValuePair<int,NodeLinks> links in CurrentScript.NodesLinks)
        {
            AddNodeLinksDisplay(links.Value,links.Key);
        }

        DisplayCanvas.Children.Last().Loaded += (object? sender, RoutedEventArgs e) => UpdateLines();
    }
    public void AddNodeLinksDisplay(NodeLinks links, int linksId)
    {
        NodeLinksDisplay nodeLinks = new NodeLinksDisplay()
        {
            Links = links,
            LinksId = linksId,
            NodesEditor = this,
        };
        nodeLinks.SetValue(Canvas.LeftProperty, links.X * 32);
        nodeLinks.SetValue(Canvas.TopProperty, links.Y * 32);

        DisplayCanvas.Children.Add(nodeLinks);
        _nodeLinks.Add(links, nodeLinks);
    }

    private void NodesEditor_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        IEnumerable<Visual> hoverVisuals = DisplayCanvas.GetVisualsAt(e.GetPosition(DisplayCanvas)).Where((x) => (typeof(Shape).IsInstanceOfType(x) && (((string?)((Shape)x).Tag) == "LinkPoint")));
        if (TracingLine != null)
        {
            if (hoverVisuals.Count() == 0)
            {
                MoveTracingLine(e.GetPosition(DisplayCanvas));
            } else
            {
                Visual hoverVisual = hoverVisuals.First();
                NodeLinksDisplay? hoverNodeLinks = hoverVisual.FindLogicalAncestorOfType<NodeLinksDisplay>(false);

                hoverNodeLinks?.LinkShape_PointerMoved(hoverVisual, e);
            }
        }
    }

    private void NodesEditor_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        IEnumerable<Visual> hoverVisuals = DisplayCanvas.GetVisualsAt(e.GetPosition(DisplayCanvas)).Where((x) => (typeof(Shape).IsInstanceOfType(x) && (((string?)((Shape)x).Tag) == "LinkPoint")));
        if (TracingLine != null && TracingLineBase != null && TracingLineBase.Links != null)
        {
            if (hoverVisuals.Count() == 0)
            {
                DeleteTracingLine();
            }
            else
            {
                Visual hoverVisual = hoverVisuals.First();
                NodeLinksDisplay? hoverNodeLinks = hoverVisual.FindLogicalAncestorOfType<NodeLinksDisplay>(false);
                if (hoverNodeLinks == null || hoverNodeLinks.Links == null) {
                    DeleteTracingLine();
                    return;
                }

                object links;
                int index, endNode, endIndex;
                NodeLine? currentLine = null;
                bool lineAddable;

                if (TracingLineOut)
                {
                    if (TracingLineRunner)
                    {
                        lineAddable = hoverNodeLinks.RunOutPoints.Contains((Shape)hoverVisual);
                        links = hoverNodeLinks.Links;
                        index = hoverNodeLinks.RunOutPoints.IndexOf((Shape)hoverVisual);
                        endNode = TracingLineBase.LinksId;
                        endIndex = TracingLineBaseIndex;
                        if (hoverNodeLinks.Links is ButtonEvent) CurrentScript.NodeLines.TryGetValue(-1, out currentLine);
                        else
                        {
                            Path? line = TracingLineBase.RunOutLines.ContainsKey(endIndex) ? TracingLineBase.RunOutLines[endIndex] : null;
                            if (line != null) currentLine = CurrentScript.NodeLines[LinesData[line].Id];
                        }
                    }
                    else
                    {
                        lineAddable = hoverNodeLinks.GetOutPoints.Contains((Shape)hoverVisual);
                        links = TracingLineBase.Links;
                        index = TracingLineBaseIndex;
                        endNode = hoverNodeLinks.LinksId;
                        endIndex = hoverNodeLinks.GetOutPoints.IndexOf((Shape)hoverVisual);

                        Path? line = hoverNodeLinks.GetInLines.ContainsKey(endIndex) ? hoverNodeLinks.GetInLines[endIndex] : null;
                        if (line != null) currentLine = CurrentScript.NodeLines[LinesData[line].Id];
                    }
                }
                else
                {
                    if (TracingLineRunner)
                    {
                        lineAddable = hoverNodeLinks.RunInPoint == (Shape)hoverVisual;
                        links = TracingLineBase.Links;
                        index = TracingLineBaseIndex;
                        endNode = hoverNodeLinks.LinksId;
                        endIndex = 0;
                        Path? line = TracingLineBase.RunOutLines.ContainsKey(index) ? TracingLineBase.RunOutLines[index] : null;
                        if (line != null) currentLine = CurrentScript.NodeLines[LinesData[line].Id];
                    }
                    else
                    {
                        lineAddable = hoverNodeLinks.GetInPoints.Contains((Shape)hoverVisual);
                        links = hoverNodeLinks.Links;
                        index = hoverNodeLinks.GetInPoints.IndexOf((Shape)hoverVisual);
                        endNode = TracingLineBase.LinksId;
                        endIndex = TracingLineBaseIndex;
                        Path? line = hoverNodeLinks.GetInLines.ContainsKey(index) ? hoverNodeLinks.GetInLines[index] : null;
                        if (line != null) currentLine = CurrentScript.NodeLines[LinesData[line].Id];
                    }
                }

                if (lineAddable)
                {
                    var action = new NodeLineAddition(links, TracingLineRunner, index, new NodeLine() { Node = endNode, PointIndex = endIndex }, this, links is ButtonEvent ? -1 : LinesData[TracingLine].Id); ;
                    if (currentLine != null)
                    {
                        action.ExtraAction = new NodeLineDeletion(links, TracingLineRunner, index, currentLine, this);
                    }

                    DisplayCanvas.Children.Remove(TracingLine);

                    TracingLine = null;
                    TracingLineBase = null;

                    action.Do();
                    RecordAction(action);
                }
                else
                {
                    DeleteTracingLine();
                }

            }
        }
    }
    private void UpdateLines()
    {
        DisplayCanvas.Children.RemoveAll(LinesData.Keys);
        LinesData.Clear();

        foreach (KeyValuePair<int, NodeLinks> links in CurrentScript.NodesLinks)
        {
            foreach (KeyValuePair<int,int> link in links.Value.Getters)
            {
                if (CurrentScript.NodeLines.ContainsKey(link.Value))
                {
                    NodeLine nodeLine = CurrentScript.NodeLines[link.Value];
                    NodeLinksDisplay nodeLinksDisplay = _nodeLinks[links.Value];
                    Point start = nodeLinksDisplay.GetInPoints[link.Key].TranslatePoint(new Point(6, 6), DisplayCanvas) ?? new Point(0, 0);
                    IBrush? stroke = nodeLinksDisplay.GetInPoints[link.Key].Fill;
                    UpdateGetterLine(nodeLinksDisplay, link.Key, nodeLine, link.Value, start, stroke);
                }
            }
            foreach (KeyValuePair<int, int> link in links.Value.Runners)
            {
                if (CurrentScript.NodeLines.ContainsKey(link.Value))
                {
                    NodeLine nodeLine = CurrentScript.NodeLines[link.Value];
                    NodeLinksDisplay nodeLinksDisplay = _nodeLinks[links.Value];
                    Point start = nodeLinksDisplay.RunOutPoints[link.Key].TranslatePoint(new Point(6, 6), DisplayCanvas) ?? new Point(0, 0);
                    UpdateRunnerLine(nodeLinksDisplay,link.Key,nodeLine,link.Value, start);
                }
            }
        }
        if (CurrentScript.NodeLines.ContainsKey(-1) && _eventStartNodeLink != null)
        {
            NodeLine nodeLine = CurrentScript.NodeLines[-1];
            Point start = _eventStartNodeLink.RunOutPoints[0].TranslatePoint(new Point(6, 6), DisplayCanvas) ?? new Point(0, 0);
            UpdateRunnerLine(_eventStartNodeLink,0,nodeLine, -1, start);
        }
    }
    private void UpdateGetterLine(NodeLinksDisplay startNodeLinksDisplay, int index, NodeLine nodeLine, int lineId, Point start, IBrush? stroke)
    {
        NodeLinksDisplay? endNodeLinksDisplay = null;
        int endIndex = 0;
        Visual? visual = null;

            if (nodeLine.Node >= 0)
            {
                if (CurrentScript.NodesLinks.ContainsKey(nodeLine.Node))
                {
                    NodeLinks getterLinks = CurrentScript.NodesLinks[nodeLine.Node];
                if (_nodeLinks.ContainsKey(getterLinks))
                {
                    endNodeLinksDisplay = _nodeLinks[getterLinks];
                    endIndex = nodeLine.PointIndex;
                    visual = endNodeLinksDisplay.GetOutPoints[endIndex];
                }
                }
            }

        if (visual == null || endNodeLinksDisplay == null) return;

            Point end = visual.TranslatePoint(new Point(6, 6), DisplayCanvas) ?? new Point(0, 0);
        Path line = AddLine(stroke,start,end, lineId, startNodeLinksDisplay.LinksId);
        if (startNodeLinksDisplay.GetInComponents[index].IsVisible)
        {
            startNodeLinksDisplay.GetInComponents[index].IsVisible = false;
        }
        startNodeLinksDisplay.GetInLines[index] = line;
        endNodeLinksDisplay.GetOutLines[endIndex].Add(line);
    }
    public Path AddLine(IBrush? stroke, Point start, Point end, int id, int startLinksId)
    {

        Path line = new Path()
        {
            Stroke = stroke,
            StrokeThickness = 4,
            ZIndex = 10,
            Focusable = false,
            IsEnabled = false,
        };
        LinesData.Add(line, new NodeLineData() { Point1 = start, Point2 = end, Id=id, StartLinksId = startLinksId});
        UpdateLinePath(line);
        DisplayCanvas.Children.Add(line);

        return line;
    }

    private void UpdateRunnerLine(NodeLinksDisplay startNodeLinksDisplay, int index, NodeLine nodeLine, int lineId, Point start)
    {
        NodeLinksDisplay? endNodeLinksDisplay = null;
        Visual? visual = null;

        if (nodeLine.Node >= 0)
        {
            if (CurrentScript.NodesLinks.ContainsKey(nodeLine.Node))
            {
                NodeLinks runnerLinks = CurrentScript.NodesLinks[nodeLine.Node];
                if (_nodeLinks.ContainsKey(runnerLinks))
                {
                    endNodeLinksDisplay = _nodeLinks[runnerLinks];
                    visual = _nodeLinks[runnerLinks].RunInPoint;
                }
            }
        }

        if (visual == null || endNodeLinksDisplay == null) return;
        Point end = visual.TranslatePoint(new Point(6, 6), DisplayCanvas) ?? new Point(0, 0);

        Path line = AddLine(Brushes.LightGray, start, end, lineId, startNodeLinksDisplay.LinksId);
        startNodeLinksDisplay.RunOutLines[index] = line;
        endNodeLinksDisplay.RunInLines.Add(line);
    }
    public void UpdateLinePath(Path line)
    {
        line.Data = GetCurvePathData(LinesData[line].Point1, LinesData[line].Point2);
    }
    Geometry GetCurvePathData(Point start, Point end)
    {
        if (start.X > end.X)
        {
            Point end_ = end;
            end = start;
            start = end_;
        }
        double xDiff = end.X - start.X;

        StreamGeometry streamGeometry = new StreamGeometry();
        using (StreamGeometryContext context = streamGeometry.Open())
        {
            context.BeginFigure(start, false);
            context.CubicBezierTo(new Point(start.X + xDiff / 3, start.Y), new Point(end.X - xDiff / 3, end.Y), new Point(end.X, end.Y));
        }
        return streamGeometry;
    }
    public NodeLinksDisplay GetLineEndLinks(Path line)
    {
        return GetNodeLineEndLinks(GetLineNodeLine(line));
    }
    public NodeLinksDisplay GetNodeLineEndLinks(NodeLine lineData)
    {
        return _nodeLinks[CurrentScript.NodesLinks[lineData.Node]];
    }
    public int GetNodeLineEndIndex(NodeLine lineData)
    {
        return lineData.PointIndex;
    }
    public NodeLine GetLineNodeLine(Path line)
    {
        int lineId = LinesData[line].Id;
        return CurrentScript.NodeLines[lineId];
    }
    public void DeleteTracingLine()
    {
        if (TracingLine == null ||TracingLineBase == null) return;

        if (TracingLineStartLinks == null)
        {
            if (TracingLineOut && TracingLineRunner)
            {
                TracingLineBase.RunInLines.Remove(TracingLine);
            }
            else if (!TracingLineOut && TracingLineRunner)
            {
                TracingLineBase.RunOutLines[TracingLineBaseIndex] = null;
            }
            else if (TracingLineOut && !TracingLineRunner)
            {
                TracingLineBase.GetInLines[TracingLineBaseIndex] = null;
            }
            else if (!TracingLineOut && !TracingLineRunner)
            {
                TracingLineBase.GetOutLines[TracingLineBaseIndex].Remove(TracingLine);
            }
            DisplayCanvas.Children.Remove(TracingLine);
            LinesData.Remove(TracingLine);
            TracingLine = null;
            TracingLineBase = null;

            return;
        }

        var action = new NodeLineDeletion(TracingLineStartLinks, TracingLineRunner,TracingLineStartIndex, CurrentScript.NodeLines[LinesData[TracingLine].Id],this);
        action.Do();

        TracingLine = null;

        RecordAction(action);
    }
    public void MoveTracingLine(Point endPoint)
    {
        if (TracingLine == null) return;

        if (LinesData.ContainsKey(TracingLine))
        {
            if ((TracingLineOut && TracingLineRunner) || (!TracingLineOut && !TracingLineRunner))
            {
                LinesData[TracingLine].Point1 = endPoint;
            }
            else
            {
                LinesData[TracingLine].Point2 = endPoint;
            }
            UpdateLinePath(TracingLine);
        }
    }
    private void Undo_Click(object? sender, RoutedEventArgs e)
    {
        Undo();
    }
    private void Redo_Click(object? sender, RoutedEventArgs e)
    {
        Redo();
    }
    public void Undo()
    {
        if (_historyIndex >= 0)
        {
            _history[_historyIndex].Undo();
            _historyIndex--;
        }
    }
    public void Redo()
    {
        if ((_historyIndex + 1) < _history.Count)
        {
            _historyIndex++;
            _history[_historyIndex].Do();
        }
    }
    public void RecordAction(IHistoryAction action)
    {
        Debug.WriteLine($"Recording action: {action.GetType().Name}");

        _historyIndex++;
        if (_historyIndex < _history.Count) _history.RemoveRange(_historyIndex, _history.Count - _historyIndex);
        _history.Add(action);
    }

    public NodeLinksDisplay? GetNodeLinksDisplay(object nodeLinks)
    {
        return nodeLinks switch
        {
            NodeLinks links => _nodeLinks[links],
            ButtonEvent => _eventStartNodeLink,
            _ => null
        };
    }
    public object GetNodeLineEnd(NodeLine nodeLine, bool isRunner)
    {
        if (isRunner)
            return nodeLine.Node switch
            {
                -1 => (ButtonEvent)EventSelector.SelectedIndex,
                _ => CurrentScript.NodesLinks[nodeLine.Node]
            };
        else
            return CurrentScript.NodesLinks[nodeLine.Node];
    }
    private void AddNode_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Control control)
        {
            string? nodeId = control.Tag?.ToString();
            Point? nodePosition = ZoomAndPan.TranslatePoint(new Point(ZoomAndPan.Bounds.Width/2,ZoomAndPan.Bounds.Height/2),DisplayCanvas);

            if (nodePosition.HasValue && nodeId != null)
            {
                Debug.WriteLine($"Adding node: {nodeId} at {nodePosition.Value.X}/{nodePosition.Value.Y}");

                var newLinks = new NodeLinks() { Id = nodeId, X = (int)(nodePosition.Value.X / 32), Y = (int)(nodePosition.Value.Y / 32) };
                var action = new NodeAddition(newLinks, CurrentScript.NodesLinks.Count == 0 ? 0 : CurrentScript.NodesLinks.Keys.Max()+1, this);

                action.Do();
                RecordAction(action);
            }
        }
    }
    object? importWarning;
    private async void Import_Click(object? sender, RoutedEventArgs e)
    {
        if (CurrentScript.NodesLinks.Count > 0)
        {
            if (importWarning == null) TryGetResource("importWarning", null, out importWarning);
            var dialog = new ContentDialog()
            {
                Title = "Import Script",
                IsPrimaryButtonEnabled = true,
                PrimaryButtonText = "Cancel",
                IsSecondaryButtonEnabled = true,
                SecondaryButtonText = "Import",
                Content = importWarning
            };
            ContentDialogResult result = await dialog.ShowAsync();
            dialog.Content = null;
            if (result != ContentDialogResult.Secondary) return;
        }

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions()
        {
            Title = "Import Script",
            AllowMultiple = false,
            FileTypeFilter = new List<FilePickerFileType>() { new FilePickerFileType("MacroPad Script") { Patterns = new List<string>() { "*.mpscript.json" } } }
        });

        if (files.Count >= 1)
        {
            await using var stream = await files[0].OpenReadAsync();
            using var streamReader = new System.IO.StreamReader(stream);
            string fileContent = await streamReader.ReadToEndAsync();

            ButtonConfig.EventScripts[(ButtonEvent)EventSelector.SelectedIndex] = JsonConvert.DeserializeObject<NodeScript>(fileContent) ?? CurrentScript;
            EventSelector_SelectionChanged(this, new SelectionChangedEventArgs(e.RoutedEvent ?? new RoutedEvent("", RoutingStrategies.Direct, typeof(object), typeof(object)), new List<object>(), new List<object>()));
        }
    }
    
    private async void Export_Click(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions()
        {
            Title = "Export Script",
            FileTypeChoices = new List<FilePickerFileType>() { new FilePickerFileType("MacroPad Script") { Patterns = new List<string>() { "*.mpscript.json" } } },
            DefaultExtension = ".mpscript.json",
            ShowOverwritePrompt = true,
        });

        if (file != null)
        { 
            await using var stream = await file.OpenWriteAsync();
            using var streamWriter = new System.IO.StreamWriter(stream);

            streamWriter.Write(JsonConvert.SerializeObject(CurrentScript));
        }
    }
}