using csheroes.form;
using csheroes.form.camp;
using csheroes.src.Saves;
using csheroes.src.Units;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    internal class ExploreMapGameState : GameState
    {
        private readonly Graphics surface;
        private readonly ExploreMap exploreMap;


        private readonly Arrows[,] arrow = null;

        ToolStripStatusLabel respectLabel;

        public ExploreMapGameState() 
        {
            StatusStrip statusStrip1 = new System.Windows.Forms.StatusStrip();
            ToolStripStatusLabel toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            respectLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ToolStripStatusLabel toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            ToolStripSplitButton toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            ToolStripSplitButton toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            ToolStripSplitButton toolStripSplitButton3 = new System.Windows.Forms.ToolStripSplitButton();
            ToolStripSplitButton toolStripSplitButton4 = new System.Windows.Forms.ToolStripSplitButton();
            GameWindow.SuspendLayout();
            statusStrip1.SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripStatusLabel2,
            respectLabel,
            toolStripStatusLabel1,
            toolStripSplitButton1,
            toolStripSplitButton2,
            toolStripSplitButton4,
            toolStripSplitButton3});
            statusStrip1.Location = new System.Drawing.Point(0, 802);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(802, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 0;
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
            toolStripStatusLabel1.Size = new System.Drawing.Size(602, 17);
            toolStripStatusLabel1.Spring = true;
            // 
            // toolStripSplitButton1
            // 
            toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripSplitButton1.DropDownButtonWidth = 0;
            toolStripSplitButton1.Image = global::csheroes.Properties.Resources.campico;
            toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripSplitButton1.Name = "toolStripSplitButton1";
            toolStripSplitButton1.Size = new System.Drawing.Size(21, 20);
            toolStripSplitButton1.Text = "toolStripSplitButton1";
            toolStripSplitButton1.ButtonClick += new System.EventHandler(CampMenu);
            // 
            // toolStripSplitButton2
            // 
            toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripSplitButton2.DropDownButtonWidth = 0;
            toolStripSplitButton2.Image = global::csheroes.Properties.Resources.saveico;
            toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripSplitButton2.Name = "toolStripSplitButton2";
            toolStripSplitButton2.Size = new System.Drawing.Size(21, 20);
            toolStripSplitButton2.Text = "toolStripSplitButton2";
            toolStripSplitButton2.ButtonClick += new System.EventHandler(OpenSaveLoadDialog);
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
            // toolStripSplitButton4
            // 
            toolStripSplitButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripSplitButton4.DropDownButtonWidth = 0;
            toolStripSplitButton4.Image = global::csheroes.Properties.Resources.question;
            toolStripSplitButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripSplitButton4.Name = "toolStripSplitButton4";
            toolStripSplitButton4.Size = new System.Drawing.Size(21, 20);
            toolStripSplitButton4.Text = "toolStripSplitButton4";
            toolStripSplitButton4.ButtonClick += new System.EventHandler(toolStripSplitButton4_ButtonClick);
            // 
            // ExploreForm
            // 
            controls.Add(statusStrip1);
            GameWindow.SetName("Exploring");
            GameWindow.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            GameWindow.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            statusStrip1.ResumeLayout();
            GameWindow.ResumeLayout();

            int mapWidth = GameWindow.Width / Global.CellSize;
            int mapHeight = (GameWindow.Height - 22) / Global.CellSize;

            exploreMap = new ExploreMap(mapWidth, mapHeight);

            surface = GameWindow.CreateGraphics();
        }

        public ExploreMapGameState(string fileName) : this()
        {
#if TEST_MAP
            InitAction();
#else
            InitAction(fileName);
#endif
        }

        public ExploreMapGameState(byte[] stream) : this()
        {
            InitAction(stream);
        }

        public void Draw(Graphics g)
        {
            DrawBackground(g);
            DrawAction(g);
            DrawGrid(g);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        private void DrawGrid(Graphics g)
        {
            for (int i = 0; i < exploreMap.Width; i++)
            {
                g.DrawLine(Global.GridPen, Global.CellSize * i, 0, Global.CellSize * i, GameWindow.Height);
            }

            for (int i = 0; i < exploreMap.Height; i++)
            {
                g.DrawLine(Global.GridPen, 0, Global.CellSize * i, GameWindow.Width, Global.CellSize * i);
            }
        }

        private void DrawBackground(Graphics g)
        {
            for (int i = 0; i < exploreMap.Width; i++)
            {
                for (int j = 0; j < exploreMap.Height; j++)
                {
                    g.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), exploreMap.background[i, j], GraphicsUnit.Pixel);
                }
            }
        }

        private void UpdateRespect()
        {
            respectLabel.Text = exploreMap.hero.Respect.ToString();
        }

        private void DrawAction(Graphics g)
        {
            for (int i = 0; i < exploreMap.Width; i++)
            {
                for (int j = 0; j < exploreMap.Height; j++)
                {
                    if (exploreMap.action[i, j] != null)
                    {
                        g.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), exploreMap.action[i, j].Tile.Area, GraphicsUnit.Pixel);
                    }
                }
            }
        }

        private bool CellIsEmpty(int x, int y)
        {
            if (x < 0 || x > GameWindow.Width / Global.CellSize - 1 || y < 0 || y > GameWindow.Height / Global.CellSize - 2) // TODO: проверить, что за фигня с -1 и -2
            {
                return false;
            }

            return (exploreMap.action[y, x] == null);
        }

        private bool MoveHero(Point dest)
        {
            Queue<Point> visit = new();
            bool[,] used = new bool[GameWindow.Width / Global.CellSize, GameWindow.Height / Global.CellSize];

            visit.Enqueue(exploreMap.heroCords);
            used[exploreMap.heroCords.Y, exploreMap.heroCords.X] = true;

            while (visit.Any())
            {
                Point p = visit.Dequeue();

                if (p == dest)
                {
                    MoveHero(dest.X, dest.Y);

                    if (dest.X == exploreMap.winCell.X && dest.Y == exploreMap.winCell.Y)
                    {
                        int score = 0;
                        foreach (Unit unit in exploreMap.hero.Army.Units)
                        {
                            if (unit != null)
                            {
                                score += unit.Damage + unit.Hp + unit.Exp + unit.Range;
                            }
                        }

                        score += exploreMap.hero.Respect;

                        GameWindow.Paint -= new System.Windows.Forms.PaintEventHandler(this.OnPaint);
                        Game.ChangeGameState(new WinState(exploreMap.locationName, score));
                    }

                    return true;
                }


                if (CellIsEmpty(p.X, p.Y - 1) && !used[p.Y - 1, p.X])
                {
                    used[p.Y - 1, p.X] = true;
                    visit.Enqueue(new(p.X, p.Y - 1));
                }
                else if (!CellIsEmpty(dest.X, dest.Y) && p.X == dest.X && p.Y - 1 == dest.Y)
                {
                    switch (exploreMap.action[dest.Y, dest.X].ToString())
                    {
                        case "Army":
                            StartBattle((Army)exploreMap.action[dest.Y, dest.X]);
                            MoveHero(dest.X, dest.Y);
                            break;
                    }

                    return true;
                }

                if (CellIsEmpty(p.X, p.Y + 1) && !used[p.Y + 1, p.X])
                {
                    used[p.Y + 1, p.X] = true;
                    visit.Enqueue(new(p.X, p.Y + 1));
                }
                else if (!CellIsEmpty(dest.X, dest.Y) && p.X == dest.X && p.Y + 1 == dest.Y)
                {
                    switch (exploreMap.action[dest.Y, dest.X].ToString())
                    {
                        case "Army":
                            StartBattle((Army)exploreMap.action[dest.Y, dest.X]);
                            MoveHero(dest.X, dest.Y);
                            break;
                    }

                    return true;
                }

                if (CellIsEmpty(p.X - 1, p.Y) && !used[p.Y, p.X - 1])
                {
                    used[p.Y, p.X - 1] = true;
                    visit.Enqueue(new(p.X - 1, p.Y));
                }
                else if (!CellIsEmpty(dest.X, dest.Y) && p.X - 1 == dest.X && p.Y == dest.Y)
                {
                    switch (exploreMap.action[dest.Y, dest.X].ToString())
                    {
                        case "Army":
                            StartBattle((Army)exploreMap.action[dest.Y, dest.X]);
                            MoveHero(dest.X, dest.Y);
                            break;
                    }

                    return true;
                }

                if (CellIsEmpty(p.X + 1, p.Y) && !used[p.Y, p.X + 1])
                {
                    used[p.Y, p.X + 1] = true;
                    visit.Enqueue(new(p.X + 1, p.Y));
                }
                else if (!CellIsEmpty(dest.X, dest.Y) && p.X + 1 == dest.X && p.Y == dest.Y)
                {
                    switch (exploreMap.action[dest.Y, dest.X].ToString())
                    {
                        case "Army":
                            StartBattle((Army)exploreMap.action[dest.Y, dest.X]);
                            MoveHero(dest.X, dest.Y);
                            break;
                    }

                    return true;
                }
            }

#if DEBUG
            SolidBrush pen = new(Color.FromArgb(128, 0, 0, 255));

            for (int i = 0; i < GameWindow.Width / Global.CellSize; i++)
            {
                for (int j = 0; j < GameWindow.Height / Global.CellSize; j++)
                {
                    if (used[i, j] == true)
                    {
                        surface.FillRectangle(pen, j * Global.CellSize, i * Global.CellSize, Global.CellSize, Global.CellSize);
                    }
                }
            }
#endif

            return false;
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            int destX = e.X / Global.CellSize,
                destY = e.Y / Global.CellSize;
            if (destY < exploreMap.action.GetLength(0) && exploreMap.action[destY, destX] != null && exploreMap.action[destY, destX].ToString() == "Obstacle")
            {
                return;
            }

            MoveHero(new Point(destX, destY));
        }

        private void MoveHero(int x, int y)
        {
            exploreMap.action[exploreMap.heroCords.Y, exploreMap.heroCords.X] = null;

            exploreMap.heroCords.X = x;
            exploreMap.heroCords.Y = y;

            exploreMap.action[exploreMap.heroCords.Y, exploreMap.heroCords.X] = exploreMap.hero;

            GameWindow.Invalidate();
        }

        private void DrawArrow(Arrows direction, int x, int y)
        {
            Rectangle tile = new Rectangle(0, 128, Global.CellSize, Global.CellSize);

            switch (direction)
            {
                case Arrows.EMPTY:
                    return;
                case Arrows.LEFT:
                    tile = new Rectangle(32, 128, Global.CellSize, Global.CellSize);
                    break;
                case Arrows.DOWN:
                    tile = new Rectangle(0, 128, Global.CellSize, Global.CellSize);
                    break;
                case Arrows.RIGHT:
                    tile = new Rectangle(32, 96, Global.CellSize, Global.CellSize);
                    break;
                case Arrows.UP:
                    tile = new Rectangle(0, 96, Global.CellSize, Global.CellSize);
                    break;
            }

            surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * x, Global.CellSize * y, Global.CellSize, Global.CellSize), tile, GraphicsUnit.Pixel);
        }

        private void DrawArrows()
        {
            if (arrow != null)
            {
                for (int i = 0; i < exploreMap.Width; i++)
                {
                    for (int j = 0; j < exploreMap.Height; j++)
                    {
                        DrawArrow(arrow[i, j], j, i);
                    }
                }
            }
        }

        private void StartBattle(Army enemy)
        {
            BattleState battleState = new(this, exploreMap.hero, enemy, exploreMap.battleMapBackgroundTile);

            GameWindow.Paint -= new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            GameWindow.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);

            battleState.OnStateChange += OnBattleEnd;

            Game.ChangeGameState(battleState);

        }

        private void OnBattleEnd()
        {

            GameWindow.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            GameWindow.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);

#if RELEASE
            if (!battleForm.BattleEnd)
            {
                Close(); // не разрешаем считерить и сбежать из битвы
                return;
            }
#endif
            UpdateRespect();

            if (exploreMap.hero.Army.Empty)
            {
                GameWindow.Paint -= new System.Windows.Forms.PaintEventHandler(this.OnPaint);
                Game.ChangeGameState(new DefeatState());
            }
        }

        private void CampMenu(object sender, EventArgs e)
        {
            CampState campState = new CampState(this, exploreMap.hero);

            GameWindow.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            GameWindow.Paint -= new System.Windows.Forms.PaintEventHandler(this.OnPaint);

            campState.OnStateChange += AfterCampMenuExit;

            Game.ChangeGameState(campState);
        }

        private void AfterCampMenuExit()
        {
            UpdateRespect();

            GameWindow.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            GameWindow.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
        }

        public void OpenSaveLoadDialog(object sender, EventArgs e)
        {
            SaveLoadDialog dialog = new();

            dialog.ShowDialog();

            if (dialog.save)
            {
                SaveFile.WriteSaveableObjectToFileWithName(exploreMap, dialog.fileName);
            }
            else if (dialog.load)
            {
                InitAction(dialog.fileName);
            }
        }

        public void InitAction(string fileName)
        {
            try
            {
                SaveFile.RestoreSaveableObjectStateFromFileWithName(exploreMap, fileName);
                GameWindow.Invalidate();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(
                $"Сохранения {fileName} не существует",
                "Сохранение не найдено",
                MessageBoxButtons.OK
                );
            }
        }

        public void InitAction(byte[] stream)
        {
            using BinaryReader reader = new(new MemoryStream(stream));
            exploreMap.ReadSave(reader);
        }

#if TEST_MAP
        void InitAction()
        {
            locationName = "Великие испытания в старом корпусе";
            background = new Rectangle[maxCellWidth, maxCellHeight];
            battleTile = new Rectangle(64, 0, Global.CellSize, Global.CellSize);
            winCell = new Point(24, 11);

            for (int i = 0; i < maxCellWidth; i++)
                for (int j = 0; j < maxCellHeight; j++)
                    background[i, j] = new Rectangle(0, 32, Global.CellSize, Global.CellSize);
            background[11, 24] = new Rectangle(0, 64, Global.CellSize, Global.CellSize);

            action = new IGameObj[maxCellWidth, maxCellHeight];

            for (int i = 0; i < action.GetLength(0); i++)
            {
                action[8, i] = new Obstacle(ObstacleType.OLD_KORPUS_WALL);
                action[14, i] = new Obstacle(ObstacleType.OLD_KORPUS_WALL);
            }
            for (int i = 0; i < 8; i++)
                action[i, 12] = new Obstacle(ObstacleType.OLD_KORPUS_WALL);
            for (int i = 0; i < 10; i++)
                action[15+ i, 12] = new Obstacle(ObstacleType.OLD_KORPUS_WALL);
            action[8, 6] = null;
            action[8, 19] = null;
            action[14, 6] = null;
            action[14, 19] = null;


            hero = new Hero(new Army(false, new Unit[] { new Unit(true), new Unit(true) }));
            heroCords = new Point(0, 12);
            action[heroCords.Y, heroCords.X] = hero;
            UpdateRespect();
        }
#endif

        private void toolStripSplitButton3_ButtonClick(object sender, EventArgs e)
        {
            GameWindow.Paint -= new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            GameWindow.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            Game.ChangeGameState(new MainMenuState());
        }

        private void ExploreForm_Load(object sender, EventArgs e)
        {

        }

        private void toolStripSplitButton4_ButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Перед вами режим исследования. Выберите врага, на которого вы хотите напасть и кликните на него.\nВ нижней части экрана вы можете видеть вспомогательное меню. В левой части панели отображается количество вашего влияния для совершения различных действий: прокачка, покупка и лечение персонажей. В правой части находятся небольшое меню действий: кнопка привала (значок костра) позволит вам взаимодействовать с вашим отрядом, кнопка сохранения (дискета) вызовет диалог сохранения, последняя кнопка в панели отвечает за выход.",
                "Question",
                MessageBoxButtons.OK
                );
        }
    }
}
