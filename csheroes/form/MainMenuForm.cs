using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            ExploreForm exploreForm = new("../../../Resources/Maps/FirstMap");

            exploreForm.Location = new Point(Location.X, Location.Y);

            Visible = false;

            exploreForm.ShowDialog();
            exploreForm.Dispose();

            Visible = true;
        }
    }
}
