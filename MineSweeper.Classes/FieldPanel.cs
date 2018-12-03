using MineSweeper.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Classes
{
    public class FieldPanel : IFieldPanel
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
