using Autofac;
using MineSweeper.Classes;
using MineSweeper.Classes.Interfaces;
using MineSweeper.DL;
using MineSweeper.DL.Interfaces;

namespace MineSweeper.Services
{
    public class DependencyInjection
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FileSettingsRepository>().As<IGameSettingsRepository>();
            builder.RegisterType<GameSettings>().As<IGameSettings>();
            return builder.Build();
        }
    }
}
