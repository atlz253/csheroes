using System;
using System.IO;
using System.Windows.Forms;

namespace csheroes.form
{
    public partial class WinForm : Form
    {
        private readonly string locationName;
        private readonly int score;

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
