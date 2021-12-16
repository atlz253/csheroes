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
    public partial class WinForm : Form
    {
        readonly string locationName;
        readonly int score;

        public WinForm(string locationName, int score)
        {
            InitializeComponent();

            this.locationName = locationName;
            this.score = score;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (BinaryWriter writer = new(File.Open("records.bin", FileMode.Append)))
            {
                writer.Write(locationName);
                writer.Write(textBox1.Text);
                writer.Write(score);
            }

            Close();
        }
    }
}
