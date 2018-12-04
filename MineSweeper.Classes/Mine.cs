using MineSweeper.Classes.Interfaces;

namespace MineSweeper.Classes
{
    public class Mine : IFieldPanel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int AdjacentMines { get; set; }

        public void Explode()
        {
            return;
        }

        public void Validate()
        {
            return;
        }
    }
}
