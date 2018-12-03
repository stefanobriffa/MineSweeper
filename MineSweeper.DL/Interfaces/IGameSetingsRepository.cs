using System.Collections.Generic;

namespace MineSweeper.DL.Interfaces
{
    public interface IGameSettingsRepository : ISettingsRepository<List<string>>
    {
        string connectionString { get; set; }
    }
}
