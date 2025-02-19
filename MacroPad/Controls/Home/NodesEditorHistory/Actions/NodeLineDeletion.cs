using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using MacroPad.Core.Config;
using System.Linq;

namespace MacroPad.Controls.Home.NodesEditorHistory.Actions
{
    public class NodeLineDeletion(object links, bool isRunner, int index, NodeLine nodeLine, NodesEditor editor) : IHistoryAction
    {
        public object Links { get; set; } = links;
        public bool IsRunner { get; set; } = isRunner;
        public int Index { get; set; } = index;
        public NodeLine NodeLine { get; set; } = nodeLine;
        public NodesEditor Editor { get; set; } = editor;
        internal int nodeLineId;

        private (NodeLinksDisplay, NodeLinksDisplay, int)? GetFullDatas()
        {
            NodeLinksDisplay? startLinksDisplay = Editor.GetNodeLinksDisplay(Links);
            object endLinks = Editor.GetNodeLineEnd(NodeLine, IsRunner);
            NodeLinksDisplay? endLinksDisplay = Editor.GetNodeLinksDisplay(endLinks);

            if (startLinksDisplay == null || endLinksDisplay == null) return null;

            int endIndex = NodesEditor.GetNodeLineEndIndex(NodeLine);

            return (startLinksDisplay, endLinksDisplay, endIndex);
        }

        public void Do()
        {
            var fullData = GetFullDatas();
            if (fullData == null) return;
            var (startLinksDisplay, endLinksDisplay, _) = fullData.Value;

            Path? linePath = IsRunner ? startLinksDisplay.RunOutLines[Index] : startLinksDisplay.GetInLines[Index];
            if (linePath == null) return;

            nodeLineId = Editor.LinesData[linePath].Id;

            if (IsRunner) {
                startLinksDisplay.RunOutLines[Index] = null;
                endLinksDisplay.RunInLines.Remove(linePath);

                if (Links is NodeLinks nodeLinks) nodeLinks.Runners.Remove(Index);
            }
            else
            {
                if (!startLinksDisplay.GetInComponents[Index].IsVisible)
                {
                    startLinksDisplay.GetInComponents[Index].IsVisible = true;
                }
                startLinksDisplay.GetInLines[Index] = null;
                endLinksDisplay.GetOutLines[NodeLine.PointIndex].Remove(linePath);

                if (Links is NodeLinks nodeLinks) nodeLinks.Getters.Remove(Index);
            }

            Editor.CurrentScript.NodeLines.Remove(nodeLineId);

            Editor.DisplayCanvas.Children.Remove(linePath);
            Editor.LinesData.Remove(linePath);
        }

        public void Undo()
        {
            var fullData = GetFullDatas();
            if (fullData == null) return;

            var (startLinksDisplay, endLinksDisplay, endIndex) = fullData.Value;

            IBrush? brush;
            Point start = new(0,0);
            Point end = start;


            if (IsRunner)
            {
                brush = Brushes.LightGray;
                Point? startPoint = (startLinksDisplay.RunOutPoints[Index]).TranslatePoint(new Point(6, 6), Editor.DisplayCanvas);
                if (startPoint.HasValue) start = startPoint.Value;
                Point? endPoint = endLinksDisplay.RunInPoint?.TranslatePoint(new Point(6, 6), Editor.DisplayCanvas);
                if (endPoint.HasValue) end = endPoint.Value;
            }
            else
            {
                brush = startLinksDisplay.GetInPoints[Index].Fill;
                Point? startPoint = startLinksDisplay.GetInPoints[Index].TranslatePoint(new Point(6, 6), Editor.DisplayCanvas);
                if (startPoint.HasValue) start = startPoint.Value;
                Point? endPoint = endLinksDisplay.GetOutPoints[endIndex].TranslatePoint(new Point(6, 6), Editor.DisplayCanvas);
                if (endPoint.HasValue) end = endPoint.Value;
            }

            if (Editor.CurrentScript.NodeLines.ContainsKey(nodeLineId)) nodeLineId = Editor.CurrentScript.NodeLines.Keys.Max() + 1;
            Editor.CurrentScript.NodeLines.Add(nodeLineId, NodeLine);

            Path linePath = Editor.AddLine(brush, start, end, nodeLineId, startLinksDisplay.LinksId);

            if (IsRunner)
            {
                startLinksDisplay.RunOutLines[Index] = linePath;
                endLinksDisplay.RunInLines.Add(linePath);

                if (Links is NodeLinks nodeLinks) nodeLinks.Runners.Add(Index, nodeLineId);
            }
            else
            {
                if (startLinksDisplay.GetInComponents[Index].IsVisible)
                {
                    startLinksDisplay.GetInComponents[Index].IsVisible = false;
                }
                startLinksDisplay.GetInLines[Index] = linePath;
                endLinksDisplay.GetOutLines[NodeLine.PointIndex].Add(linePath);

                if (Links is NodeLinks nodeLinks) nodeLinks.Getters.Add(Index, nodeLineId);
            }
        }
    }
}
