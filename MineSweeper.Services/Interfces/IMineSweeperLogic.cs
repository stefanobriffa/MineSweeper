using MineSweeper.Classes.Interfaces;
using System.Collections.Generic;

namespace MineSweeper.Services.Interfaces
{
    public interface IMineSweeperLogic
    {
        List<string> GetGameSettings(string ConnectionSetting);
        void ValidateRetrievedSettings(List<string> SettingsToValidate);
        void ValidateFieldPanelSettings(List<string> FieldSettings);
        List<IGameSettings> BuildGameFields(List<string> AllFieldsSettings);
        IGameSettings ComputeAdjacentMineCount(IGameSettings CurrentGameSettings);
    }
}
