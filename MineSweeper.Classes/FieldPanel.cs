using MineSweeper.Classes.CustomExceptions;
using MineSweeper.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Classes
{
    public class FieldPanel : IFieldPanel
    {
        [Range(0, 100)]
        public int X { get; set; }
        [Range(0, 100)]
        public int Y { get; set; }
        [Range(0, 8)]
        public int AdjacentMines { get; set; }

        public void Validate()
        {
            var validationContext = new ValidationContext(this, null, null);
            var results = new List<ValidationResult>();
            var _isValid = Validator.TryValidateObject(this, validationContext, results, true);

            var _errMsg = "";
            if (!_isValid)
            {
                foreach (var r in results)
                    _errMsg += r.ErrorMessage + Environment.NewLine;

                throw new MineSweeperException(_errMsg);
            }
        }
    }
}
