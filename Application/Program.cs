using Autofac;
using MineSweeper.DL;
using MineSweeper.Services;
using MineSweeper.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class Program
    {
        static void Main(string[] args)
        {
            IContainer _container = DependencyInjection.BuildContainer();
            IMineSweeperLogic _mineSweeperLogic = _container.Resolve<IMineSweeperLogic>();
            _mineSweeperLogic.GetGameSettings("C:\\Development\\MineSweeper\\settings.txt");
        }
    }
}
