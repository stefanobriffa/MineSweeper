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

        public bool IsValid()
        {
            var validationContext = new ValidationContext(this, null, null);
            var results = new List<ValidationResult>();
            var _isValid = Validator.TryValidateObject(this, validationContext, results, true);

            if (!_isValid)
                foreach (var r in results)
                    Console.WriteLine(r.ErrorMessage);
                

            return _isValid;
        }
    }
}
