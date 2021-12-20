using csheroes.src.unit;
using System;
using System.Windows.Forms;

namespace csheroes.form.camp
{
    public partial class UpgradeStatsDialog : Form
    {
        public UnitStats choice;
        public bool choiced;

        public UpgradeStatsDialog()
        {
            InitializeComponent();
        }

        private void HealthUpgrade(object sender, EventArgs e)
        {
            choice = UnitStats.HP;
            choiced = true;
            Close();
        }

        private void RangeUpgrade(object sender, EventArgs e)
        {
            choice = UnitStats.RANGE;
            choiced = true;
            Close();
        }

        private void DamageUpgrade(object sender, EventArgs e)
        {
            choice = UnitStats.DAMAGE;
            choiced = true;
            Close();
        }

        private void CancelBtn(object sender, EventArgs e)
        {
            Close();
        }
    }
}
