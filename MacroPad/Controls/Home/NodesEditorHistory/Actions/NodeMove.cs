using Avalonia;

namespace MacroPad.Controls.Home.NodesEditorHistory.Actions
{
    public class NodeMove(Point from, Point to, int nodeLinksId, NodesEditor editor) : IHistoryAction
    {
        public Point From { get; set; } = from;
        public Point To { get; set; } = to;
        public int NodeLinksId { get; set; } = nodeLinksId;
        public NodesEditor Editor { get; set; } = editor;

        private NodeLinksDisplay? GetNodeLinksDisplay()
        {
            if (NodeLinksId == -1) return Editor.EventStartNodeLink;
            return Editor.CurrentScriptNodeLinks[Editor.CurrentScript.NodesLinks[NodeLinksId]];
        }
        public void Do()
        {

            GetNodeLinksDisplay()?.MoveNode((int)To.X, (int)To.Y);
        }

        public void Undo()
        {
            GetNodeLinksDisplay()?.MoveNode((int)From.X, (int)From.Y);
        }
    }
}
