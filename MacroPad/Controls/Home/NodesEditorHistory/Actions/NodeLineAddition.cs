using MacroPad.Core.Config;

namespace MacroPad.Controls.Home.NodesEditorHistory.Actions
{
    internal class NodeLineAddition : IHistoryAction
    {
        public NodeLineDeletion LineDeletion { get; set; }
        public IHistoryAction? ExtraAction { get; set; }
        public int NodeLineId { get => LineDeletion.nodeLineId; set => LineDeletion.nodeLineId = value; }
        public NodeLineAddition(object links, bool isRunner, int index, NodeLine nodeLine, NodesEditor editor, int nodeLineId)
        {
            LineDeletion = new NodeLineDeletion(links, isRunner, index, nodeLine, editor);
            NodeLineId = nodeLineId;
        }
        public void Do()
        {
            ExtraAction?.Do();
            LineDeletion.Undo();
        }

        public void Undo()
        {
            LineDeletion.Do();
            ExtraAction?.Undo();
        }
    }
}
