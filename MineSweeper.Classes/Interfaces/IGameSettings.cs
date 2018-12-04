namespace MineSweeper.Classes.Interfaces
{
    public interface IGameSettings
    {
        int Width { get; set; }
        int Height { get; set; }
        IFieldPanel[][] FieldPanels { get; set; }
        void Validate();
    }
}
