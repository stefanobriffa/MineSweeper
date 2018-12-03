using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Classes.CustomExceptions
{
    public class MineSweeperException :Exception
    {
        public MineSweeperException() : base() { }

        public MineSweeperException(string message) : base(message) { }

        public MineSweeperException(string message, Exception e) : base(message, e) { }
    }
}
