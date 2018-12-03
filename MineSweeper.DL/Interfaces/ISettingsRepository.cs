namespace MineSweeper.DL.Interfaces
{
    public interface ISettingsRepository<T>
    {
        T GetGameSettings();
    }
}
