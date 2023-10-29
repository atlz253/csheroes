using System;
using System.Windows.Forms;

namespace csheroes.form
{
    public partial class SaveLoadDialog : Form
    {
        public string fileName;
        public bool save, load;

        public SaveLoadDialog()
        {
            InitializeComponent();
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
