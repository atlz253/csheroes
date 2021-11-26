using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csheroes.form
{
    public partial class SaveLoadDialog : Form
    {
        ExploreForm parent;

        public string fileName;
        public bool save, load;

        public SaveLoadDialog(ExploreForm parent)
        {
            InitializeComponent();

            this.parent = parent;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fileName = textBox1.Text;
            save = true;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fileName = textBox1.Text;
            load = true;
            Close();
        }
    }
}
