using csheroes.form;
using csheroes.form.camp;
using csheroes.src;
using csheroes.src.Units;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace csheroes
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if FIRST_MAP
            Application.Run(new ExploreForm("../../../Resources/Maps/FirstMap"));
#elif TEST_MAP
            Application.Run(new ExploreForm(""));
#elif TEST_BATTLE
            Application.Run(new BattleForm(null, new Hero(new Army(false, new Unit(100, 3, 1, AttackType.RANGE))), new Army(true, UnitFactory.GetUnitByTemplate("creep range")), new Rectangle(0, 0, Global.CellSize, Global.CellSize)));
#elif CAMPMENU
            Application.Run(new CampForm(null, new Hero(new Army(false, new Unit(100, 3, 1), new Unit(100, 3, 1), new Unit(100, 3, 1), new Unit(100, 3, 1), new Unit(100, 3, 1), new Unit(100, 3, 1), new Unit(100, 3, 1)))));
#else
            Application.Run(new MainMenuForm());
#endif
        }
    }
}
