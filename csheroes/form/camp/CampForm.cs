﻿using csheroes.src;
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

        public CampForm(ExploreForm parent, Hero hero)
        {
            InitializeComponent();

            surface = CreateGraphics();

            this.hero = hero;
            this.parent = parent;
        }

        void Draw()
        {
            DrawCards();
        }

        void DrawCards()
        {
            for (int i = 0; i < 7; i++)
            {

                surface.DrawLines(Global.GridPen, new PointF[] { new PointF((float) 100 / 8 * (i + 1) + i * 100, 600), new PointF((float) 100 / 8 * (i + 1) + i * 100, 750), new PointF((float) 100 / 8 * (i + 1) + (i + 1) * 100, 750), new PointF((float)100 / 8 * (i + 1) + (i + 1) * 100, 600), new PointF((float)100 / 8 * (i + 1) + i * 100, 600) });
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Draw();
        }

        private void Hire(object sender, EventArgs e)
        {
            HireDialog dialog = new();

            dialog.ShowDialog();

            if (dialog.choice)
                for (int i = 0; i < 7; i++)
                {
                    if (hero.Army.Units[i] == null)
                    {
                        hero.Army.Units[i] = new(UnitType.ABBITURENT);
                        hero.Respect -= 100;
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
