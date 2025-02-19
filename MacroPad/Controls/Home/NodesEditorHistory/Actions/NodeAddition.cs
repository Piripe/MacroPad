using MacroPad.Core.Config;

namespace MacroPad.Controls.Home.NodesEditorHistory.Actions
{
    public class NodeAddition(NodeLinks node, int nodeId, NodesEditor editor) : IHistoryAction
    {
        public NodeLinks Node { get; set; } = node;
        public int NodeId { get; set; } = nodeId;
        public NodesEditor Editor { get; set; } = editor;

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
