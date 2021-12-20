using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csheroes.form
{
    public partial class LevelMenu : Form
    {
        public string level;

        public LevelMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
#if RELEASE
            level = "Maps/FirstMap";
#else
            level = "../../../Resources/Maps/FirstMap";
#endif
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
#if RELEASE
            level = "Maps/SecondMap";
#else
            level = "../../../Resources/Maps/SecondMap";
#endif
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
#if RELEASE
            level = "Maps/ThirdMap";
#else
            level = "../../../Resources/Maps/ThirdMap";
#endif
            Close();
        }
    }
}
