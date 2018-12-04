using MineSweeper.DL.Interfaces;
using System.Collections.Generic;

namespace MineSweeper.DL
{
    //Repository to be used for testing
    public class HardCodedSettingsRepository : IGameSettingsRepository
    {
        public string connectionString { get; set; }

        public List<string> GetGameSettings() 
        {
            List<string> _settings = new List<string>()
            {
                "4 4", "***.", "*.*.", "***.", "....", "0 0"
            };

            return _settings;
        }
    }
}
