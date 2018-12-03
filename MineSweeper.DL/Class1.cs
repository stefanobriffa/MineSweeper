using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.DL
{
    public interface ISettingsRepository<T> where T : class
    {
        T GetGameSettings();
    }
}
