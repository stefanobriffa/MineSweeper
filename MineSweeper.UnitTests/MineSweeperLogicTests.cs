using System;
using System.Collections.Generic;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineSweeper.Classes;
using MineSweeper.Classes.CustomExceptions;
using MineSweeper.Classes.Interfaces;
using MineSweeper.DL.Interfaces;
using MineSweeper.Services;
using MineSweeper.Services.Interfaces;

namespace MineSweeper.UnitTests
{
    [TestClass]
    public class MineSweeperLogicTests
    {
        [TestMethod]
        public void correct_no_of_lines_when_valid_settings()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            IGameSettingsRepository _repo = _container.Resolve<IGameSettingsRepository>();
            IMineSweeperLogic _msLogic = _container.Resolve<IMineSweeperLogic>();
            List<string> _settings = _repo.GetGameSettings();
            _msLogic.ValidateRetrievedSettings(_settings);

            Assert.AreEqual(6, _settings.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(MineSweeperException))]
        public void throw_MineSweeperException_when_no_end_of_file_indicator()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            IGameSettingsRepository _repo = _container.Resolve<IGameSettingsRepository>();
            IMineSweeperLogic _msLogic = _container.Resolve<IMineSweeperLogic>();
            List<string> _settings = _repo.GetGameSettings();
            _settings = _settings.GetRange(0, _settings.Count - 1);
            _msLogic.ValidateRetrievedSettings(_settings);
        }

        [TestMethod]
        [ExpectedException(typeof(MineSweeperException))]
        public void throw_MineSweeperException_when_invalid_characters_in_file()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            IGameSettingsRepository _repo = _container.Resolve<IGameSettingsRepository>();
            IMineSweeperLogic _msLogic = _container.Resolve<IMineSweeperLogic>();
            List<string> _settings = _repo.GetGameSettings();
            _settings[1] = "..ss";
            _msLogic.ValidateRetrievedSettings(_settings);
        }

        [TestMethod]
        [ExpectedException(typeof(MineSweeperException))]
        public void throw_MineSweeperException_when_empty_list()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            IMineSweeperLogic _msLogic = _container.Resolve<IMineSweeperLogic>();
            List<string> _settings = new List<string>();
            _msLogic.ValidateRetrievedSettings(_settings);
        }

        [TestMethod]
        [ExpectedException(typeof(MineSweeperException))]
        public void throw_MineSweeperException_when_incorrect_panel_settings()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            IMineSweeperLogic _msLogic = _container.Resolve<IMineSweeperLogic>();
            List<string> _settings = new List<string>() { "....", "..t." };
            _msLogic.ValidateFieldPanelSettings(_settings);
        }

        [TestMethod]
        public void correct_panel_settings()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            IMineSweeperLogic _msLogic = _container.Resolve<IMineSweeperLogic>();
            List<string> _settings = new List<string>() { "....", "..*." };
            _msLogic.ValidateFieldPanelSettings(_settings);
        }

        [TestMethod]
        public void set_field_size_correct_settings()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            MineSweeperLogic _msLogic = new MineSweeperLogic();
            IGameSettings _gSettings = _container.Resolve<IGameSettings>();

            _gSettings = _msLogic.SetFieldSize(_gSettings, "4 4");

            Assert.IsTrue(_gSettings.Height == _gSettings.Width && _gSettings.Height == 4);
        }

        [TestMethod]
        [ExpectedException(typeof(MineSweeperException))]
        public void throw_MineSweeperException_when_incorrect_field_size_settings()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            MineSweeperLogic _msLogic = new MineSweeperLogic();
            IGameSettings _gSettings = _container.Resolve<IGameSettings>();

            _gSettings = _msLogic.SetFieldSize(_gSettings, "4 4 6");
        }

        [TestMethod]
        public void set_field_panels()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            MineSweeperLogic _msLogic = new MineSweeperLogic();
            IGameSettings _gSettings = _container.Resolve<IGameSettings>();

            _gSettings = _msLogic.SetFieldSize(_gSettings, "2 2");
            _gSettings = _msLogic.SetFieldPanels(_gSettings, new List<string>() { "**", "*." });
            Assert.IsTrue(_gSettings.FieldPanels[0][0] is Mine &&
                          _gSettings.FieldPanels[1][0] is Mine &&
                          _gSettings.FieldPanels[0][1] is Mine &&
                          !(_gSettings.FieldPanels[1][1] is Mine));
        }

        [TestMethod]
        public void CheckBottom_true()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            MineSweeperLogic _msLogic = new MineSweeperLogic();
            IGameSettings _gSettings = _container.Resolve<IGameSettings>();

            _gSettings = _msLogic.SetFieldSize(_gSettings, "2 2");
            _gSettings = _msLogic.SetFieldPanels(_gSettings, new List<string>() { "**", "*." });
            Assert.IsTrue(_msLogic.CheckBottom(_gSettings, _gSettings.FieldPanels[0][0]));
        }

        [TestMethod]
        public void CheckBottom_false()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            MineSweeperLogic _msLogic = new MineSweeperLogic();
            IGameSettings _gSettings = _container.Resolve<IGameSettings>();

            _gSettings = _msLogic.SetFieldSize(_gSettings, "2 2");
            _gSettings = _msLogic.SetFieldPanels(_gSettings, new List<string>() { "**", "*." });
            
            Assert.IsFalse(_msLogic.CheckBottom(_gSettings, _gSettings.FieldPanels[1][0]));
        }

        [TestMethod]
        public void CheckTop_false()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            MineSweeperLogic _msLogic = new MineSweeperLogic();
            IGameSettings _gSettings = _container.Resolve<IGameSettings>();

            _gSettings = _msLogic.SetFieldSize(_gSettings, "2 2");
            _gSettings = _msLogic.SetFieldPanels(_gSettings, new List<string>() { "**", "*." });

            Assert.IsFalse(_msLogic.CheckTop(_gSettings.FieldPanels[0][0]));
        }

        [TestMethod]
        public void CheckTop_true()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            MineSweeperLogic _msLogic = new MineSweeperLogic();
            IGameSettings _gSettings = _container.Resolve<IGameSettings>();

            _gSettings = _msLogic.SetFieldSize(_gSettings, "2 2");
            _gSettings = _msLogic.SetFieldPanels(_gSettings, new List<string>() { "**", "*." });

            Assert.IsTrue(_msLogic.CheckTop(_gSettings.FieldPanels[1][0]));
        }

        [TestMethod]
        public void CheckLeft_true()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            MineSweeperLogic _msLogic = new MineSweeperLogic();
            IGameSettings _gSettings = _container.Resolve<IGameSettings>();

            _gSettings = _msLogic.SetFieldSize(_gSettings, "2 2");
            _gSettings = _msLogic.SetFieldPanels(_gSettings, new List<string>() { "**", "*." });

            Assert.IsTrue(_msLogic.CheckLeft(_gSettings.FieldPanels[0][1]));
        }

        [TestMethod]
        public void CheckLeft_false()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            MineSweeperLogic _msLogic = new MineSweeperLogic();
            IGameSettings _gSettings = _container.Resolve<IGameSettings>();

            _gSettings = _msLogic.SetFieldSize(_gSettings, "2 2");
            _gSettings = _msLogic.SetFieldPanels(_gSettings, new List<string>() { "**", "*." });

            Assert.IsFalse(_msLogic.CheckLeft(_gSettings.FieldPanels[0][0]));
        }

        [TestMethod]
        public void CheckRight_true()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            MineSweeperLogic _msLogic = new MineSweeperLogic();
            IGameSettings _gSettings = _container.Resolve<IGameSettings>();

            _gSettings = _msLogic.SetFieldSize(_gSettings, "2 2");
            _gSettings = _msLogic.SetFieldPanels(_gSettings, new List<string>() { "**", "*." });

            Assert.IsTrue(_msLogic.CheckRight(_gSettings, _gSettings.FieldPanels[0][0]));
        }

        [TestMethod]
        public void CheckRight_false()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            MineSweeperLogic _msLogic = new MineSweeperLogic();
            IGameSettings _gSettings = _container.Resolve<IGameSettings>();

            _gSettings = _msLogic.SetFieldSize(_gSettings, "2 2");
            _gSettings = _msLogic.SetFieldPanels(_gSettings, new List<string>() { "**", "*." });

            Assert.IsFalse(_msLogic.CheckRight(_gSettings, _gSettings.FieldPanels[0][1]));
        }

        [TestMethod]
        public void compute_Adjacent_mine_count()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            MineSweeperLogic _msLogic = new MineSweeperLogic();
            IGameSettings _gSettings = _container.Resolve<IGameSettings>();

            _gSettings = _msLogic.SetFieldSize(_gSettings, "2 3");
            _gSettings = _msLogic.SetFieldPanels(_gSettings, new List<string>() { "**.", "*.." });
            _msLogic.ComputeAdjacentMineCount(_gSettings);
            Assert.IsTrue(!(_gSettings.FieldPanels[0][2] is Mine) && _gSettings.FieldPanels[0][2].AdjacentMines == 1);
            Assert.IsTrue(!(_gSettings.FieldPanels[1][1] is Mine) && _gSettings.FieldPanels[1][1].AdjacentMines == 3);
            Assert.IsTrue(!(_gSettings.FieldPanels[1][2] is Mine) && _gSettings.FieldPanels[1][2].AdjacentMines == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(MineSweeperException))]
        public void cannnot_have_more_than_8_adj_m()
        {
            IContainer _container = DependencyInjection.BuildContainer();
            MineSweeperLogic _msLogic = new MineSweeperLogic();
            IGameSettings _gSettings = _container.Resolve<IGameSettings>();

            _gSettings = _msLogic.SetFieldSize(_gSettings, "2 3");
            _gSettings = _msLogic.SetFieldPanels(_gSettings, new List<string>() { "**.", "*.." });
            _msLogic.ComputeAdjacentMineCount(_gSettings);
            _gSettings.FieldPanels[1][1].AdjacentMines = 9;
            _gSettings.FieldPanels[1][1].Validate();


        }
    }
}
