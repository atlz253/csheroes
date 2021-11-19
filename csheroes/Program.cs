using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using csheroes.form;
using csheroes.src;
using csheroes.src.unit;

namespace csheroes
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if FIRST_MAP
            Application.Run(new ExploreForm("../../../Resources/Maps/FirstMap"));
#elif TEST_MAP
            Application.Run(new ExploreForm(""));
#elif TEST_BATTLE
            Application.Run(new BattleForm(null, new Hero(new Army(false, new Unit[] { new Unit(UnitType.ABBITURENT), new Unit(UnitType.TECHNAR), new Unit(UnitType.GUMANITARIY) })), new Army(true, new Unit[] { new Unit(UnitType.ABBITURENT), new Unit(UnitType.ABBITURENT), new Unit(UnitType.ABBITURENT) })));
#else
            Application.Run(new MainMenuForm());
#endif
        }
    }
}
