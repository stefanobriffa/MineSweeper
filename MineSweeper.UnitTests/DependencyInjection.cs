using Autofac;
using MineSweeper.Classes;
using MineSweeper.Classes.Interfaces;
using MineSweeper.DL;
using MineSweeper.DL.Interfaces;
using MineSweeper.Services;
using MineSweeper.Services.Interfaces;

namespace MineSweeper.UnitTests
{
    public class DependencyInjection
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MineSweeperLogic>().As<IMineSweeperLogic>();
            builder.RegisterType<GameSettings>().As<IGameSettings>();
            builder.RegisterType<HardCodedSettingsRepository>().As<IGameSettingsRepository>();
            return builder.Build();
        }
    }
}
