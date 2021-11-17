using csheroes.src;
using csheroes.src.unit;
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
    public partial class CampForm : Form
    {
        readonly Graphics surface;

        readonly ExploreForm parent;
        readonly Hero hero;

        Label[] labels;
        Button[] healBtns,
                 expBtns;

        public CampForm(ExploreForm parent, Hero hero)
        {
            InitializeComponent();

            surface = CreateGraphics();

            this.hero = hero;
            this.parent = parent;

            InitCards();
        }

        void InitCards()
        {
            labels = new Label[7];
            healBtns = new Button[7];
            expBtns = new Button[7];

            for (int i = 0; i < 7; i++)
                if (hero.Army.Units[i] != null)
                {
                    Unit unit = hero.Army.Units[i];

                    if (labels[i] == null)
                        labels[i] = new();

                    labels[i].Text = unit.ToString();
                    labels[i].Location = new Point(100 / 8 * (i + 1) + i * 100 + 5, 610);
                    labels[i].BackColor = Color.Transparent;
                    Controls.Add(labels[i]);

                    if (unit.Hp != unit.MaxHp)
                    {
                        healBtns[i] = new();
                        healBtns[i].Text = "HP";
                        healBtns[i].Name = $"healBtn{i}";
                        healBtns[i].Size = new Size(25, 25);
                        healBtns[i].Location = new Point(100 / 8 * (i + 1) + (i + 1) * 100 - 30, 700);
                        healBtns[i].Click += new EventHandler(Heal);
                        Controls.Add(healBtns[i]);
                    }

                    if (unit.Exp >= unit.NextLevel)
                    {
                        expBtns[i] = new();
                        expBtns[i].Text = "EXP";
                        expBtns[i].Name = $"expBtn{i}";
                        expBtns[i].Size = new Size(25, 25);
                        expBtns[i].Location = new Point(100 / 8 * (i + 1) + (i + 1) * 100 - 30, 730);
                        expBtns[i].Click += new EventHandler(Upgrade);
                        Controls.Add(expBtns[i]);
                    }
                }
        }

        void Draw()
        {
            DrawCards();
        }

        void DrawCards()
        {
            for (int i = 0; i < 7; i++)
                surface.DrawLines(Global.GridPen, new PointF[] { new PointF((float) 100 / 8 * (i + 1) + i * 100, 600), new PointF((float) 100 / 8 * (i + 1) + i * 100, 750), new PointF((float) 100 / 8 * (i + 1) + (i + 1) * 100, 750), new PointF((float)100 / 8 * (i + 1) + (i + 1) * 100, 600), new PointF((float)100 / 8 * (i + 1) + i * 100, 600) });
        }

        void Heal(object sender, EventArgs e)
        {
            for (int i = 0; i < 7; i++)
                if (healBtns[i] != null && (sender as Button).Name == healBtns[i].Name)
                {
                    BoolDialog dialog = new("Вы слишком зачастили к нам (- 100 респекта)");

                    dialog.ShowDialog();

                    if (dialog.choice)
                    {
                        if (hero.Respect == 0 || hero.Respect < 100)
                            return;

                        hero.Respect -= 100;
                        hero.Army.Units[i].Hp = hero.Army.Units[i].MaxHp;

                        Controls.Remove(healBtns[i]);
                        healBtns[i] = null;
                    }

                    return;
                }
        }

        void Upgrade(object sender, EventArgs e)
        {
            for (int i = 0; i < 7; i++)
                if (expBtns[i] != null && (sender as Button).Name == expBtns[i].Name)
                {
                    if (hero.Army.Units[i].ToString() == "Абитурент")
                    {
                        AbiturentUpgradeDialog upgradeDialog = new();
                        
                        upgradeDialog.ShowDialog();
                        upgradeDialog.Dispose();
                        
                        if (hero.Respect == 0 || hero.Respect < 100)
                            return;

                        hero.Respect -= 100;
                        hero.Army.Units[i] = new Unit(upgradeDialog.choice);

                        Controls.Remove(expBtns[i]);
                        expBtns[i] = null;
                    }
                }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Draw();
        }

        private void Hire(object sender, EventArgs e)
        {
            BoolDialog dialog = new("Завербовать нового абитурента (100 респекта)?");

            dialog.ShowDialog();

            if (dialog.choice)
                for (int i = 0; i < 7; i++)
                {
                    if (hero.Army.Units[i] == null)
                    {
                        if (hero.Respect == 0 || hero.Respect < 100)
                            break;
                        hero.Respect -= 100;

                        hero.Army.Units[i] = new(UnitType.ABBITURENT);
                        break;
                    }
                }
        }

        private void Exit(object sender, EventArgs e)
        {
            parent.Location = new Point(Location.X, Location.Y);

            Close();
        }

        
    }
}
