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
    public partial class HireDialog : Form
    {
        public bool choice = false;

        public HireDialog()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            choice = true;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            choice = false;
            Close();
        }
    }
}
