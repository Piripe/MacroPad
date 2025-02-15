using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using MacroPad.Core.Config;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Controls.Home.NodesEditorHistory.Actions
{
    public class NodeDeletion : IHistoryAction
    {
        public NodeLinks Node { get; set; }
        public int NodeId { get; set; }
        public NodesEditor Editor { get; set; }
        private List<NodeLineDeletion> _linesDeletion = new List<NodeLineDeletion>();
        private IHistoryAction _nodeAddition;

        public NodeDeletion(NodeLinks node, int nodeId, NodesEditor editor)
        {
            Node = node;
            NodeId = nodeId;
            Editor = editor;
            _nodeAddition = new NodeAddition(node, nodeId, editor);
        }

        public void Do()
        {
            _linesDeletion.Clear();

            NodeLinksDisplay display = Editor.CurrentScriptNodeLinks[Node];

            foreach (KeyValuePair<int, Path?> line in display.RunOutLines)
            {
                if (line.Value != null) _linesDeletion.Add(new NodeLineDeletion(Node, true, line.Key, Editor.CurrentScript.NodeLines[Editor.LinesData[line.Value].Id], Editor));
            }
            foreach (KeyValuePair<int, List<Path>> lines in display.GetOutLines)
            {
                foreach (Path line in lines.Value)
                {
                    NodeLineData lineData = Editor.LinesData[line];
                    NodeLinksDisplay? links = lineData.StartLinksId == -1 ? Editor.EventStartNodeLink : Editor.CurrentScriptNodeLinks[Editor.CurrentScript.NodesLinks[lineData.StartLinksId]];
                    object? startLinks = links?.Links;
                    if (links != null && startLinks != null) _linesDeletion.Add(new NodeLineDeletion(startLinks, false, links.GetInLines.FirstOrDefault(x => x.Value == line).Key, Editor.CurrentScript.NodeLines[lineData.Id], Editor));
                }
            }
            foreach (Path line in display.RunInLines)
            {
                NodeLineData lineData = Editor.LinesData[line];
                NodeLinksDisplay? links = lineData.StartLinksId == -1 ? Editor.EventStartNodeLink : Editor.CurrentScriptNodeLinks[Editor.CurrentScript.NodesLinks[lineData.StartLinksId]];
                object? startLinks = links?.Links;
                if (links != null && startLinks != null) _linesDeletion.Add(new NodeLineDeletion(startLinks, true, links.RunOutLines.FirstOrDefault(x => x.Value == line).Key, Editor.CurrentScript.NodeLines[lineData.Id], Editor));
            }
            foreach (KeyValuePair<int, Path?> line in display.GetInLines)
            {
                if (line.Value != null) _linesDeletion.Add(new NodeLineDeletion(Node, false, line.Key, Editor.CurrentScript.NodeLines[Editor.LinesData[line.Value].Id], Editor));
            }


            _linesDeletion.ForEach(x=>x.Do());

            _nodeAddition.Undo();
        }

        public void Undo()
        {
            _nodeAddition.Do();
            Editor.CurrentScriptNodeLinks[Node].Loaded += (object? sender, RoutedEventArgs e) =>
            {
                _linesDeletion.ForEach(x => x.Undo());
            };
        }
    }
}
