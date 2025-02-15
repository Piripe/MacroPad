using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using MacroPad.Core.Config;
using MacroPad.Core.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Controls.Home.NodesEditorHistory.Actions
{
    public class NodeLineDeletion : IHistoryAction
    {
        public object Links { get; set; }
        public bool IsRunner { get; set; }
        public int Index { get; set; }
        public NodeLine NodeLine { get; set; }
        public NodesEditor Editor { get; set; }
        internal int nodeLineId;
        public NodeLineDeletion(object links, bool isRunner, int index, NodeLine nodeLine, NodesEditor editor)
        {
            Links = links;
            IsRunner = isRunner;
            Index = index;
            NodeLine = nodeLine;
            Editor = editor;
        }

        private (NodeLinksDisplay, object, NodeLinksDisplay, int)? GetFullDatas()
        {
            NodeLinksDisplay? startLinksDisplay = Editor.GetNodeLinksDisplay(Links);
            object endLinks = Editor.GetNodeLineEnd(NodeLine, IsRunner);
            NodeLinksDisplay? endLinksDisplay = Editor.GetNodeLinksDisplay(endLinks);

            if (startLinksDisplay == null || endLinksDisplay == null) return null;

            int endIndex = Editor.GetNodeLineEndIndex(NodeLine);

            return (startLinksDisplay, endLinks, endLinksDisplay, endIndex);
        }

        public void Do()
        {
            var fullData = GetFullDatas();
            if (fullData == null) return;

            var (startLinksDisplay, endLinks, endLinksDisplay, endIndex) = fullData.Value;

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

            var (startLinksDisplay, endLinks, endLinksDisplay, endIndex) = fullData.Value;

            IBrush? brush;
            Point start = new Point(0,0);
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
