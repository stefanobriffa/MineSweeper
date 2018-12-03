using Autofac;
using MineSweeper.Classes.Interfaces;
using MineSweeper.DL.Interfaces;
using MineSweeper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.DI
{
    public class Class1
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<IMineSweeperLogic>().As<MineSweeperLogic>();
            builder.RegisterType<IGameSettings>().As<GameSettings>();
            builder.RegisterType<IGameSettingsRepository>().As<FileSettingsRepository>();
            return builder.Build();
        }
    }
}
