using System;
using System.Windows.Forms;

namespace csheroes.form
{
    public partial class LevelMenu : Form
    {
        public byte[] level;

        public LevelMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            level = Properties.Resources.FirstMap;

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            level = Properties.Resources.SecondMap;

            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            level = Properties.Resources.ThirdMap;

            Close();
        }
    }
}
