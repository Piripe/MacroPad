using MacroPad.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Controls.Home.NodesEditorHistory.Actions
{
    public class NodeAddition : IHistoryAction
    {
        public NodeLinks Node { get; set; }
        public int NodeId { get; set; }
        public NodesEditor Editor { get; set; }

        public NodeAddition(NodeLinks node, int nodeId, NodesEditor editor)
        {
            Node = node;
            NodeId = nodeId;
            Editor = editor;
        }

        public void Do()
        {
            Editor.AddNodeLinksDisplay(Node, NodeId);
            Editor.CurrentScript.NodesLinks.Add(NodeId, Node);
        }

        public void Undo()
        {
            Editor.DisplayCanvas.Children.Remove(Editor.CurrentScriptNodeLinks[Node]);
            Editor.CurrentScriptNodeLinks.Remove(Node);
            Editor.CurrentScript.NodesLinks.Remove(NodeId);
        }
    }
}
