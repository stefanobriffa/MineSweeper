using MineSweeper.Classes.CustomExceptions;
using MineSweeper.Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MineSweeper.Classes
{
    public class GameSettings : IGameSettings
    {
        [Range(1,100)]
        public int Width { get; set; }

        [Range(1, 100)]
        public int Height { get; set; }

        public IFieldPanel[][] FieldPanels { get; set; }

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
