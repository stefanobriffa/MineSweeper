namespace MineSweeper.Classes.Interfaces
{
    public interface IFieldPanel
    {
        int X { get; set; }
        int Y { get; set; }
        int AdjacentMines { get; set; }
        void Validate();
    }
}
