using MineSweeper.DL.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace MineSweeper.DL
{
    //Repository Patterns
    public class FileSettingsRepository : IGameSettingsRepository
    {
        public string connectionString { get; set; }

        public List<string> GetGameSettings() 
        {
            List<string> _settings = new List<string>();
            if (File.Exists(connectionString))
            {
                using (FileStream fs = File.Open(connectionString, FileMode.Open))
                using (BufferedStream bs = new BufferedStream(fs))
                using (StreamReader sr = new StreamReader(bs))
                {
                    string _fileLine = "";
                    do
                    {
                        _fileLine = sr.ReadLine();
                        if (_fileLine != null)
                            _settings.Add(_fileLine);

                    } while (_fileLine != null);
                }
            }
            else
                throw new FileNotFoundException("Settings file not found");

            return _settings;
        }
    }
}
