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
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void Exit(object sender, EventArgs e)
        {
            Close();
        }

        private void NewGame(object sender, EventArgs e)
        {
            LevelMenu dialog = new();

            dialog.ShowDialog();

            LoadGame(dialog.level);

            dialog.Dispose();
        }

        private void LoadGameDialog(object sender, EventArgs e)
        {
            LoadDialog dialog = new();

            dialog.ShowDialog();

            if (dialog.load)
                LoadGame($"saves/{dialog.fileName}");

            dialog.Dispose();
        }

        void LoadGame(string path)
        {
            if (!File.Exists(path))
                return;

            ExploreForm exploreForm = new(path);

            exploreForm.Location = new Point(Location.X, Location.Y);

            Visible = false;

            exploreForm.ShowDialog();
            exploreForm.Dispose();

            Visible = true;
        }

        private void RecordsMenu(object sender, EventArgs e)
        {
            RecordForm recordForm = new();
            recordForm.Location = new Point(Location.X, Location.Y);

            Visible = false;

            recordForm.ShowDialog();
            Location = recordForm.Location;
            recordForm.Dispose();

            Visible = true;
        }
    }
}
