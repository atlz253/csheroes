using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using csheroes.form;

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
#else
            Application.Run(new MainMenuForm());
#endif
        }
    }
}
