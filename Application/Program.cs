using Autofac;
using MineSweeper.Classes;
using MineSweeper.Classes.CustomExceptions;
using MineSweeper.Services.Interfaces;
using System;
using System.Linq;

namespace MineSweeper
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IContainer _container = DependencyInjection.BuildContainer();
                IMineSweeperLogic _mineSweeperLogic = _container.Resolve<IMineSweeperLogic>();
                var _settings = _mineSweeperLogic.GetGameSettings("..\\settings.txt");
                var _fields = _mineSweeperLogic.BuildGameFields(_settings);
                var _counter = 1;
                foreach (var field in _fields)
                {
                    Console.WriteLine($"Field #{_counter}");

                    for (int i = 0; i < field.FieldPanels.Count(); i++)
                    {
                        var op = "";
                        for (int j = 0; j < field.FieldPanels[i].Count(); j++)
                        {
                            if (field.FieldPanels[i][j] is Mine)
                                op += "*";
                            else
                                op += field.FieldPanels[i][j].AdjacentMines.ToString();
                        }

                        Console.WriteLine(op);
                    }

                    _counter++;
                    Console.WriteLine();
                }
                
            }
            catch (MineSweeperException mex)
            {
                Console.WriteLine(mex.Message);
            }
            catch (Exception ex)
            {
                //in real life scenarios you would log to event log ar similar
                Console.WriteLine(ex.Message);
            }
        }
    }
}
