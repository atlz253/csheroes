using csheroes.src.unit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csheroes.form.camp
{
    public partial class AbiturentUpgradeDialog : Form
    {
        public UnitType choice;
        public bool choiced;

        public AbiturentUpgradeDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            choice = UnitType.TECHNAR;
            choiced = true;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            choice = UnitType.GUMANITARIY;
            choiced = true;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
