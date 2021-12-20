using csheroes.form;
using System;
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
            Application.Run(new BattleForm(null, new Hero(new Army(false, new Unit(100, 3, 1, AttackType.RANGE))), new Army(true, new Unit(UnitTemplate.CREEP_RANGE)), new Rectangle(0, 0, Global.CellSize, Global.CellSize)));
#elif CAMPMENU
            Application.Run(new CampForm(null, new Hero(new Army(false, new Unit(), new Unit(new UnitSnapshot(5, 10, 1, 3, 1, 1, new(256, 0), AttackType.MELEE))), 1000)));
#else
            Application.Run(new MainMenuForm());
#endif
        }
    }
}
