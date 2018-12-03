using Autofac;
using MineSweeper.Classes;
using MineSweeper.Classes.CustomExceptions;
using MineSweeper.Classes.Interfaces;
using MineSweeper.DL;
using MineSweeper.DL.Interfaces;
using MineSweeper.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MineSweeper.Services
{
    public class MineSweeperLogic : IMineSweeperLogic
    {
        private IContainer _container;
        IGameSettingsRepository _repo;
        IGameSettings _gameSettings;

        public MineSweeperLogic()
        {
            _container = DependencyInjection.BuildContainer();
            _repo = _container.Resolve<IGameSettingsRepository>();
            _gameSettings = _container.Resolve<IGameSettings>();
        }
        public IGameSettings GetGameSettings(string ConnectionSetting)
        {
            _repo.connectionString = ConnectionSetting;
            List<string> _fileContents = _repo.GetGameSettings();
            //Regex regex = new Regex(@"[\d]");

            if (_fileContents.Any())
            {
                SetFieldSize(_gameSettings, _fileContents[0]);
                SetFieldPanels(_fileContents.GetRange(1, _gameSettings.Height));
            }

            return _gameSettings;
        }

        public void SetFieldPanels(List<string> Panels)
        {
            _gameSettings.FieldPanels = new IFieldPanel[_gameSettings.Height][];

            //use for loops as they are more performant than foreach
            for (int i = 0; i < Panels.Count; i++)
            {
                var _panelStrings = Panels[i].ToCharArray();

                IFieldPanel[] _currLine = new IFieldPanel[_panelStrings.Count()];
                for (int j = 0; j < _panelStrings.Count(); j++)
                {
                    if (_panelStrings[j] == '.')
                        _currLine[j] = new FieldPanel() { Y = i, X = j };
                    else
                        _currLine[j] = new Mine() { Y = i, X = j };
                }

                _gameSettings.FieldPanels[i] = _currLine;
            }
        }


        public IGameSettings SetFieldSize(IGameSettings GameSettings, string settings)
        {
            var _firstLine = settings.Split(' ');

            if (_firstLine.Count() != 2)
                throw new MineSweeperException("Invalid board dimensions (too many values)");

            GameSettings.Height = StringToInt(_firstLine[0]);
            GameSettings.Width = StringToInt(_firstLine[1]);

            return GameSettings;
        }

        public int StringToInt(string toConvert)
        {
            if (string.IsNullOrEmpty(toConvert))
                return -1;
            else
            {
                if (int.TryParse(toConvert, out int _intValue))
                    return _intValue;
                else
                    return -1;
            }
        }

        
    }
}
