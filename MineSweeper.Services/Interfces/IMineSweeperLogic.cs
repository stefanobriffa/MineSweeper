using MineSweeper.Classes.Interfaces;

namespace MineSweeper.Services.Interfaces
{
    public interface IMineSweeperLogic
    {
        IGameSettings GetGameSettings(string ConnectionSetting);

    }
}
