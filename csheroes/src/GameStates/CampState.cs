using csheroes.form.camp;
using csheroes.src.Units;
using System.Drawing.Drawing2D;
using System.Drawing;
using System;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    public class CampState: GameState
    {
        private readonly GameState parent;
        private readonly Hero hero;
        private Label[] dmgLabels,
                hpLabels,
                rangeLabels;
        private Button[] healBtns,
                 expBtns,
                 newUnitBtns;

        private ToolStripStatusLabel respectLabel;

        public CampState() 
        {
            StatusStrip statusStrip1 = new System.Windows.Forms.StatusStrip();
            ToolStripStatusLabel toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            respectLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ToolStripStatusLabel toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            ToolStripSplitButton toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            ToolStripSplitButton toolStripSplitButton3 = new System.Windows.Forms.ToolStripSplitButton();
            statusStrip1.SuspendLayout();
            GameWindow.SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripStatusLabel2,
            respectLabel,
            toolStripStatusLabel1,
            toolStripSplitButton1,
            toolStripSplitButton3});
            statusStrip1.Location = new System.Drawing.Point(0, 802);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(802, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new System.Drawing.Size(57, 17);
            toolStripStatusLabel2.Text = "Влияние:";
            // 
            // respectLabel
            // 
            respectLabel.Name = "respectLabel";
            respectLabel.Size = new System.Drawing.Size(13, 17);
            respectLabel.Text = "0";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(675, 17);
            toolStripStatusLabel1.Spring = true;
            // 
            // toolStripSplitButton1
            // 
            toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripSplitButton1.DropDownButtonWidth = 0;
            toolStripSplitButton1.Image = global::csheroes.Properties.Resources.question;
            toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripSplitButton1.Name = "toolStripSplitButton1";
            toolStripSplitButton1.Size = new System.Drawing.Size(21, 20);
            toolStripSplitButton1.Text = "toolStripSplitButton1";
            toolStripSplitButton1.ButtonClick += new System.EventHandler(toolStripSplitButton1_ButtonClick);
            // 
            // toolStripSplitButton3
            // 
            toolStripSplitButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripSplitButton3.DropDownButtonWidth = 0;
            toolStripSplitButton3.Image = global::csheroes.Properties.Resources.exitico;
            toolStripSplitButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripSplitButton3.Name = "toolStripSplitButton3";
            toolStripSplitButton3.Size = new System.Drawing.Size(21, 20);
            toolStripSplitButton3.Text = "toolStripSplitButton3";
            toolStripSplitButton3.ButtonClick += new System.EventHandler(toolStripSplitButton3_ButtonClick);
            // 
            // CampForm
            // 
            GameWindow.SetBackgroundImage(Properties.Resources.camp);
            controls.Add(statusStrip1);
            GameWindow.SetName("Camp");
            GameWindow.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            statusStrip1.ResumeLayout();
            GameWindow.ResumeLayout();
        }

        public CampState(GameState parent, Hero hero): this()
        {
            this.parent = parent;

            this.hero = hero;
            UpdateRespect();

            InitCards();
        }

        private void InitCards()
        {
            dmgLabels = new Label[7];
            hpLabels = new Label[7];
            rangeLabels = new Label[7];
            healBtns = new Button[7];
            expBtns = new Button[7];
            newUnitBtns = new Button[7];

            for (int i = 0; i < 7; i++)
            {
                if (hero.Army.Units[i] != null)
                {
                    Unit unit = hero.Army.Units[i];

                    hpLabels[i] = new();
                    hpLabels[i].Text = "HP: " + unit.Hp.ToString();
                    hpLabels[i].Width = 60;
                    hpLabels[i].Location = new Point(100 / 8 * (i + 1) + i * 100 + 5, 685);
                    controls.Add(hpLabels[i]);
                    GameWindow.AddControl(hpLabels[i]);

                    dmgLabels[i] = new();
                    dmgLabels[i].Text = "DMG: " + unit.Damage.ToString();
                    dmgLabels[i].Width = 60;
                    dmgLabels[i].Location = new Point(100 / 8 * (i + 1) + i * 100 + 5, 705);
                    controls.Add(dmgLabels[i]);
                    GameWindow.AddControl(dmgLabels[i]);

                    rangeLabels[i] = new();
                    rangeLabels[i].Text = "RNG: " + unit.Range.ToString();
                    rangeLabels[i].Width = 60;
                    rangeLabels[i].Location = new Point(100 / 8 * (i + 1) + i * 100 + 5, 725);
                    controls.Add(rangeLabels[i]);
                    GameWindow.AddControl(rangeLabels[i]);

                    if (unit.Hp != unit.MaxHp)
                    {
                        healBtns[i] = new();
                        healBtns[i].Text = "HP";
                        healBtns[i].Name = $"healBtn{i}";
                        healBtns[i].Size = new Size(25, 25);
                        healBtns[i].Location = new Point(100 / 8 * (i + 1) + (i + 1) * 100 - 30, 685);
                        healBtns[i].Click += new EventHandler(Heal);
                        controls.Add(healBtns[i]);
                        GameWindow.AddControl(healBtns[i]);
                    }

                    if (unit.Exp >= unit.NextLevel)
                    {
                        expBtns[i] = new();
                        expBtns[i].Text = "EXP";
                        expBtns[i].Name = $"expBtn{i}";
                        expBtns[i].Size = new Size(25, 25);
                        expBtns[i].Location = new Point(100 / 8 * (i + 1) + (i + 1) * 100 - 30, 715);
                        expBtns[i].Click += new EventHandler(Upgrade);
                        controls.Add(expBtns[i]);
                        GameWindow.AddControl(expBtns[i]);
                    }
                }
                else
                {
                    newUnitBtns[i] = new();
                    newUnitBtns[i].Text = "+";
                    newUnitBtns[i].Name = $"newUnitBtns{1}";
                    newUnitBtns[i].Size = new Size(25, 25);
                    newUnitBtns[i].Location = new Point(100 / 8 * (i + 1) + i * 100 + 10, 610);
                    newUnitBtns[i].Click += new EventHandler(AddUnit);
                    controls.Add(newUnitBtns[i]);
                    GameWindow.AddControl(newUnitBtns[i]);
                }
            }
        }

        private void Draw(Graphics g)
        {
            DrawCards(g);
            DrawAvatars(g);
        }

        private void DrawCards(Graphics g)
        {
            SolidBrush brush = new(Color.FromArgb(255, 240, 240, 240));

            for (int i = 0; i < 7; i++)
            {
                g.DrawLines(Global.GridPen, new PointF[] { new PointF((float)100 / 8 * (i + 1) + i * 100, 600), new PointF((float)100 / 8 * (i + 1) + i * 100, 750), new PointF((float)100 / 8 * (i + 1) + (i + 1) * 100, 750), new PointF((float)100 / 8 * (i + 1) + (i + 1) * 100, 600), new PointF((float)100 / 8 * (i + 1) + i * 100, 600) });
                g.FillRectangle(brush, new RectangleF((float)100 / 8 * (i + 1) + i * 100, 600, 100, 150));
            }
        }

        private void DrawAvatars(Graphics g)
        {
            for (int i = 0; i < 7; i++)
            {
                if (hero.Army.Units[i] != null)
                {
                    g.DrawImage(Global.Texture, new Rectangle(100 / 8 * (i + 1) + i * 100 + 15, 605, 75, 75), hero.Army.Units[i].Tile.Area, GraphicsUnit.Pixel);
                }
            }
        }

        private void AddUnit(object sender, EventArgs e)
        {
            for (int i = 0; i < 7; i++)
            {
                if (newUnitBtns[i] != null && (sender as Button).Name == newUnitBtns[i].Name)
                {
                    BoolDialog dialog = new("Завербовать нового абитурента (100 респекта)?");

                    dialog.ShowDialog();

                    if (dialog.choice)
                    {
                        if (hero.Army.Units[i] == null)
                        {
                            if (hero.Respect == 0 || hero.Respect < 100)
                            {
                                break;
                            }

                            hero.Respect -= 100;
                            UpdateRespect();

                            hero.Army.Units[i] = new(true);

                            controls.Remove(newUnitBtns[i]);
                            GameWindow.RemoveControl(newUnitBtns[i]);
                            newUnitBtns[i] = null;

                            Unit unit = hero.Army.Units[i];

                            hpLabels[i] = new();
                            hpLabels[i].Text = "HP: " + unit.Hp.ToString();
                            hpLabels[i].Width = 60;
                            hpLabels[i].Location = new Point(100 / 8 * (i + 1) + i * 100 + 5, 685);
                            controls.Add(hpLabels[i]);
                            GameWindow.RemoveControl(hpLabels[i]);

                            dmgLabels[i] = new();
                            dmgLabels[i].Text = "DMG: " + unit.Damage.ToString();
                            dmgLabels[i].Width = 60;
                            dmgLabels[i].Location = new Point(100 / 8 * (i + 1) + i * 100 + 5, 705);
                            controls.Add(dmgLabels[i]);
                            GameWindow.RemoveControl(dmgLabels[i]);

                            rangeLabels[i] = new();
                            rangeLabels[i].Text = "RNG: " + unit.Range.ToString();
                            rangeLabels[i].Width = 60;
                            rangeLabels[i].Location = new Point(100 / 8 * (i + 1) + i * 100 + 5, 725);
                            controls.Add(rangeLabels[i]);
                            GameWindow.RemoveControl(rangeLabels[i]);

                            GameWindow.Invalidate();
                            return;
                        }
                    }

                    return;
                }
            }
        }

        private void Heal(object sender, EventArgs e)
        {
            for (int i = 0; i < 7; i++)
            {
                if (healBtns[i] != null && (sender as Button).Name == healBtns[i].Name)
                {
                    int healCost = 0;
                    for (int j = 0; j < hero.Army.Units[i].MaxHp - hero.Army.Units[i].Hp; j++)
                    {
                        healCost += (int)(10 + healCost * 0.05);
                    }

                    //int healCost = (hero.Army.Units[i].MaxHp - hero.Army.Units[i].Hp) * 5 * hero.Army.Units[i].Level;
                    BoolDialog dialog = new($"Вы слишком зачастили к нам (- {healCost} респекта)");

                    dialog.ShowDialog();

                    if (dialog.choice)
                    {
                        if (hero.Respect == 0 || hero.Respect < healCost)
                        {
                            return;
                        }

                        hero.Respect -= healCost;
                        hero.Army.Units[i].Hp = hero.Army.Units[i].MaxHp;

                        controls.Remove(healBtns[i]);
                        GameWindow.RemoveControl(healBtns[i]);
                        healBtns[i] = null;

                        hpLabels[i].Text = "HP:   " + hero.Army.Units[i].Hp.ToString();
                        UpdateRespect();
                    }

                    GameWindow.Invalidate();
                    return;
                }
            }
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Перед вами меню привала. Здесь вы можете посмотреть и улучшить характеристики ваших персонажей, нанять новых юнитов и вылечить раненых.",
                "Question",
                MessageBoxButtons.OK
                );
        }

        private void Upgrade(object sender, EventArgs e)
        {
            for (int i = 0; i < 7; i++)
            {
                if (expBtns[i] != null && (sender as Button).Name == expBtns[i].Name)
                {
                    if (hero.Army.Units[i].Range == 5)
                    {
                        AttackTypeDialog upgradeDialog = new();

                        upgradeDialog.ShowDialog();

                        if (upgradeDialog.choiced)
                        {
                            if (hero.Respect < 500)
                            {
                                return;
                            }

                            hero.Respect -= 500;
                            UpdateRespect();

                            hero.Army.Units[i].Level += 1;
                            hero.Army.Units[i].Attack = upgradeDialog.choice;

                            hero.Army.Units[i].Range += 1;
                            rangeLabels[i].Text = "RNG: " + hero.Army.Units[i].Range.ToString();

                            hero.Army.Units[i].NextLevel = hero.Army.Units[i].NextLevel * 2;

                            controls.Remove(expBtns[i]);
                            GameWindow.RemoveControl(expBtns[i]);
                            expBtns[i] = null;

                            if (hero.Army.Units[i].Exp >= hero.Army.Units[i].NextLevel)
                            {
                                expBtns[i] = new();
                                expBtns[i].Text = "EXP";
                                expBtns[i].Name = $"expBtn{i}";
                                expBtns[i].Size = new Size(25, 25);
                                expBtns[i].Location = new Point(100 / 8 * (i + 1) + (i + 1) * 100 - 30, 715);
                                expBtns[i].Click += new EventHandler(Upgrade);
                                controls.Add(expBtns[i]);
                                GameWindow.AddControl(expBtns[i]);
                            }
                        }

                        upgradeDialog.Dispose();
                    }
                    else
                    {
                        UpgradeStatsDialog upgradeDialog = new();

                        upgradeDialog.ShowDialog();

                        if (upgradeDialog.choiced)
                        {
                            int upgradeCost = hero.Army.Units[i].Level * 100;
                            BoolDialog acceptDialog = new($"Это будет стоить {upgradeCost} респекта");

                            acceptDialog.ShowDialog();



                            if (acceptDialog.choice && hero.Respect >= upgradeCost)
                            {
                                switch (upgradeDialog.choice)
                                {
                                    case UnitStats.HP:
                                        hero.Army.Units[i].MaxHp += 3;
                                        hero.Army.Units[i].Hp += 3;
                                        hpLabels[i].Text = "HP: " + hero.Army.Units[i].Hp.ToString();
                                        break;
                                    case UnitStats.DAMAGE:
                                        hero.Army.Units[i].Damage += 1;
                                        dmgLabels[i].Text = "DMG: " + hero.Army.Units[i].Damage.ToString();
                                        break;
                                    case UnitStats.RANGE:
                                        hero.Army.Units[i].Range += 1;
                                        rangeLabels[i].Text = "RNG: " + hero.Army.Units[i].Range.ToString();
                                        break;
                                }

                                hero.Army.Units[i].Level += 1;
                                hero.Respect -= upgradeCost;
                                UpdateRespect();
                                hero.Army.Units[i].NextLevel = hero.Army.Units[i].NextLevel * 2;

                                controls.Remove(expBtns[i]);
                                GameWindow.RemoveControl(expBtns[i]);
                                expBtns[i] = null;

                                if (hero.Army.Units[i].Exp >= hero.Army.Units[i].NextLevel)
                                {
                                    expBtns[i] = new();
                                    expBtns[i].Text = "EXP";
                                    expBtns[i].Name = $"expBtn{i}";
                                    expBtns[i].Size = new Size(25, 25);
                                    expBtns[i].Location = new Point(100 / 8 * (i + 1) + (i + 1) * 100 - 30, 715);
                                    expBtns[i].Click += new EventHandler(Upgrade);
                                    controls.Add(expBtns[i]);
                                    GameWindow.AddControl(expBtns[i]);
                                }
                            }


                            acceptDialog.Dispose();
                        }

                        GameWindow.Invalidate();
                        upgradeDialog.Dispose();
                    }
                }
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            Draw(e.Graphics);
        }

        private void UpdateRespect()
        {
            respectLabel.Text = hero.Respect.ToString();
        }

        private void toolStripSplitButton3_ButtonClick(object sender, EventArgs e)
        {
            GameWindow.Paint -= new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            Game.ChangeGameState(parent);
            StateChange();
        }
    }
}
