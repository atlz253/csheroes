using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace csheroes.form
{
    public partial class RecordForm : Form
    {
        public RecordForm()
        {
            InitializeComponent();
            LoadRecords();
        }

        void LoadRecords()
        {
            if (!File.Exists("records.bin"))
                return;

            using (BinaryReader reader = new(File.Open("records.bin", FileMode.Open)))
            {
                int count = 0;
                while (reader.PeekChar() > -1)
                {
                    Label label = new();
                    label.Width = 500;
                    label.Text = $"{reader.ReadString()}     {reader.ReadString()}     {reader.ReadInt32()}";
                    label.Location = new(250, 100 + count * 25);
                    Controls.Add(label);
                    count++;
                }
            }
        }
    }
}
