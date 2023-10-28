﻿using csheroes.form.camp;
using csheroes.src;
using csheroes.src.Saves;
using csheroes.src.Units;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace csheroes.form
{
    public partial class ExploreForm : Form
    {
        private readonly Graphics surface;
        private readonly ExploreMap exploreMap;


        private readonly Arrows[,] arrow = null;

        public ExploreForm()
        {
            InitializeComponent();

            int mapWidth = Width / Global.CellSize;
            int mapHeight = (Height - 22) / Global.CellSize;

            exploreMap = new ExploreMap(mapWidth, mapHeight);

            surface = CreateGraphics();
        }

        public ExploreForm(string fileName) : this()
        {
#if TEST_MAP
            InitAction();
#else
            InitAction(fileName);
#endif
        }

        public ExploreForm(byte[] stream) : this()
        {
            InitAction(stream);
        }

        public void Update(object sender, EventArgs e)
        {
            Invalidate();
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
                g.DrawLine(Global.GridPen, Global.CellSize * i, 0, Global.CellSize * i, Height);
            }

            for (int i = 0; i < exploreMap.Height; i++)
            {
                g.DrawLine(Global.GridPen, 0, Global.CellSize * i, Width, Global.CellSize * i);
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
            if (x < 0 || x > Width / Global.CellSize - 1 || y < 0 || y > Height / Global.CellSize - 2) // TODO: проверить, что за фигня с -1 и -2
            {
                return false;
            }

            return (exploreMap.action[y, x] == null);
        }

        private bool MoveHero(Point dest)
        {
            Queue<Point> visit = new();
            bool[,] used = new bool[Width / Global.CellSize, Height / Global.CellSize];

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

                        WinForm form = new(exploreMap.locationName, score);

                        form.Location = new Point(Location.X, Location.Y);

                        Visible = false;

                        form.ShowDialog();
                        form.Dispose();
                        Close();
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

            for (int i = 0; i < Width / Global.CellSize; i++)
            {
                for (int j = 0; j < Height / Global.CellSize; j++)
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

            Invalidate();
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
            BattleForm battleForm = new(this, exploreMap.hero, enemy, exploreMap.battleMapBackgroundTile);

            battleForm.Location = new Point(Location.X, Location.Y);

            Visible = false;

            battleForm.ShowDialog();
            Location = battleForm.Location;

#if RELEASE
            if (!battleForm.BattleEnd)
            {
                Close(); // не разрешаем считерить и сбежать из битвы
                return;
            }
#endif
            Visible = true;
            UpdateRespect();

            if (exploreMap.hero.Army.Empty)
            {
                DefeatForm form = new();

                form.Location = new Point(Location.X, Location.Y);

                Visible = false;

                form.ShowDialog();
                form.Dispose();
                Close();
            }

            battleForm.Dispose();
        }

        private void CampMenu(object sender, EventArgs e)
        {
            CampForm campForm = new(this, exploreMap.hero);

            campForm.Location = new Point(Location.X, Location.Y);

            Visible = false;

            campForm.ShowDialog();
            Location = campForm.Location;
            campForm.Dispose();

            UpdateRespect();

            Visible = true;
        }

        public void OpenSaveLoadDialog(object sender, EventArgs e)
        {
            SaveLoadDialog dialog = new(this);

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
                Invalidate();
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
            Close();
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
