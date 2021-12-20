using System;
using System.Windows.Forms;

namespace csheroes.form
{
    public partial class LoadDialog : Form
    {
        public string fileName;
        public bool load;

        public LoadDialog()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            load = true;
            fileName = textBox1.Text;
            Close();
        }
    }
}
