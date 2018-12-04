using Autofac;
using MineSweeper.Classes;
using MineSweeper.Classes.CustomExceptions;
using MineSweeper.Classes.Interfaces;
using MineSweeper.DL.Interfaces;
using MineSweeper.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MineSweeper.Services
{
    public class MineSweeperLogic : IMineSweeperLogic
    {
        private IContainer _container;
        IGameSettingsRepository _repo;
        bool _settingsInitialised = false;

        public MineSweeperLogic()
        {
            _container = DependencyInjection.BuildContainer();
            _repo = _container.Resolve<IGameSettingsRepository>();
        }

        public List<string> GetGameSettings(string ConnectionSetting)
        {
            _repo.connectionString = ConnectionSetting;
            List<string> _fileContents = _repo.GetGameSettings();
            ValidateRetrievedSettings(_fileContents);

            return _fileContents;
        }

        public void ValidateRetrievedSettings(List<string> SettingsToValidate)
        {
            Regex regex = new Regex(@"^[0-9.\* ]+$");

            if (SettingsToValidate.Any())
            {
                if (!SettingsToValidate.All(s => regex.IsMatch(s)))
                    throw new MineSweeperException("Invalid characters in configuration");

                if (SettingsToValidate.Last().Replace(" ", "") != "00")
                    throw new MineSweeperException("Invalid End of configuration");
            }
            else
                throw new MineSweeperException("No settings received");
        }

        public void ValidateFieldPanelSettings(List<string> FieldSettings)
        {
            Regex regex = new Regex(@"^[.*]+$");
            if (!FieldSettings.All(s => regex.IsMatch(s)))
                throw new MineSweeperException("Invalid characters in field configuration");
        }

        public List<IGameSettings> BuildGameFields(List<string> AllFieldsSettings)
        {
            List<IGameSettings> _fieldList = new List<IGameSettings>();
            var _nextField = 0;

            while (AllFieldsSettings[_nextField].Replace(" ", "").Trim() != "00")
            {
                var _gameSettings = _container.Resolve<IGameSettings>();
                _settingsInitialised = false;
                _gameSettings = SetFieldSize(_gameSettings, AllFieldsSettings[_nextField]);
                //to validate width and height for board
                _gameSettings.Validate();
                ValidateFieldPanelSettings(AllFieldsSettings.GetRange(_nextField + 1, _gameSettings.Height));
                _gameSettings = SetFieldPanels(_gameSettings, AllFieldsSettings.GetRange(_nextField + 1, _gameSettings.Height));
                _gameSettings = ComputeAdjacentMineCount(_gameSettings);
                //to validate the whole object
                _gameSettings.Validate();

                _fieldList.Add(_gameSettings);

                _nextField = _nextField + 1 + _gameSettings.Height;
            }

            return _fieldList;
        }

        public IGameSettings SetFieldPanels(IGameSettings GameSettings, List<string> Panels)
        {
            GameSettings.FieldPanels = new IFieldPanel[GameSettings.Height][];

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

                GameSettings.FieldPanels[i] = _currLine;
            }

            _settingsInitialised = true;
            return GameSettings;
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

        public bool CheckBottom(IGameSettings CurrentGameSettings, IFieldPanel CurrPanel)
        {
            if (!_settingsInitialised)
                return false;
            else
                return (CurrPanel.Y < CurrentGameSettings.FieldPanels.Count() - 1) ? true : false;
        }

        public bool CheckTop(IFieldPanel CurrPanel)
        {
            if (!_settingsInitialised)
                return false;
            else
                return (CurrPanel.Y > 0) ? true : false; ;
        }

        public bool CheckRight(IGameSettings CurrentGameSettings, IFieldPanel CurrPanel)
        {
            if (!_settingsInitialised)
                return false;
            else
                return (CurrPanel.X < CurrentGameSettings.FieldPanels[CurrPanel.Y].Count() - 1) ? true : false;
        }

        public bool CheckLeft(IFieldPanel CurrPanel)
        {
            if (!_settingsInitialised)
                return false;
            else
                return (CurrPanel.X > 0) ? true : false;
        }

        public IGameSettings ComputeAdjacentMineCount(IGameSettings CurrentGameSettings)
        {
            if (_settingsInitialised)
            {
                for (int i = 0; i < CurrentGameSettings.FieldPanels.Count(); i++)
                {
                    for (int j = 0; j < CurrentGameSettings.FieldPanels[i].Count(); j++)
                    {
                        var _adjMines = 0;
                        var _currPanel = CurrentGameSettings.FieldPanels[i][j];

                        if (!(_currPanel is Mine))
                        {
                            if (CheckTop(_currPanel) && CurrentGameSettings.FieldPanels[i - 1][j] is Mine)
                                _adjMines += 1;

                            if (CheckBottom(CurrentGameSettings, _currPanel) && CurrentGameSettings.FieldPanels[i + 1][j] is Mine)
                                _adjMines += 1;

                            if (CheckLeft(_currPanel) && CurrentGameSettings.FieldPanels[i][j - 1] is Mine)
                                _adjMines += 1;

                            if (CheckRight(CurrentGameSettings, _currPanel) && CurrentGameSettings.FieldPanels[i][j + 1] is Mine)
                                _adjMines += 1;

                            if (CheckTop(_currPanel) && CheckLeft(_currPanel) && CurrentGameSettings.FieldPanels[i - 1][j - 1] is Mine)
                                _adjMines += 1;

                            if (CheckTop(_currPanel) && CheckRight(CurrentGameSettings, _currPanel) && CurrentGameSettings.FieldPanels[i - 1][j + 1] is Mine)
                                _adjMines += 1;

                            if (CheckBottom(CurrentGameSettings, _currPanel) && CheckLeft(_currPanel) && CurrentGameSettings.FieldPanels[i + 1][j - 1] is Mine)
                                _adjMines += 1;

                            if (CheckBottom(CurrentGameSettings, _currPanel) && CheckRight(CurrentGameSettings, _currPanel) && CurrentGameSettings.FieldPanels[i + 1][j + 1] is Mine)
                                _adjMines += 1;
                        }
                        _currPanel.AdjacentMines = _adjMines;

                        _currPanel.Validate();
                    }
                }
            }
            return CurrentGameSettings;
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
