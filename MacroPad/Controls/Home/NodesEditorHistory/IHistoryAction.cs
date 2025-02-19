namespace MacroPad.Controls.Home.NodesEditorHistory
{
    public interface IHistoryAction
    {
        public void Do();
        public void Undo();
    }
}
