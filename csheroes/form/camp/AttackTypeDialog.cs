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
    public partial class AttackTypeDialog : Form
    {
        public AttackType choice;
        public bool choiced;

        public AttackTypeDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            choice = AttackType.RANGE;
            choiced = true;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            choice = AttackType.MELEE;
            choiced = true;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
