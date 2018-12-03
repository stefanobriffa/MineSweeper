using Autofac;
using MineSweeper.Classes;
using MineSweeper.Classes.Interfaces;
using MineSweeper.Services;
using MineSweeper.Services.Interfaces;

namespace MineSweeper
{
    class DependencyInjection
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MineSweeperLogic>().As<IMineSweeperLogic>();
            builder.RegisterType<GameSettings>().As<IGameSettings>();
            return builder.Build();
        }
    }
}
