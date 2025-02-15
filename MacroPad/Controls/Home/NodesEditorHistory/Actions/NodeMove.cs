using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroPad.Controls.Home.NodesEditorHistory.Actions
{
    public class NodeMove : IHistoryAction
    {
        public Point From { get; set; }
        public Point To { get; set; }
        public int NodeLinksId { get; set; }
        public NodesEditor Editor { get; set; }
        public NodeMove(Point from, Point to, int nodeLinksId, NodesEditor editor) {
            From = from;
            To = to;
            NodeLinksId = nodeLinksId;
            Editor = editor;
        }
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
