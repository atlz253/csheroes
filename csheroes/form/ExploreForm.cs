using csheroes.form.camp;
using csheroes.src;
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
        private readonly int maxCellWidth;
        private readonly int maxCellHeight;
        private Rectangle[,] background = null;
        private Rectangle battleTile;
        private Point winCell;
        private IGameObj[,] action = null;
        private readonly Arrows[,] arrow = null;
        private Hero hero = null;
        private Point heroCords;
        private string locationName;

        public ExploreForm(string fileName)
        {
            InitializeComponent();

            maxCellWidth = Width / Global.CellSize;
            maxCellHeight = (Height - 22) / Global.CellSize;

#if TEST_MAP
            InitAction();
#else
            InitAction(fileName);
#endif

            surface = CreateGraphics();
        }

        public ExploreForm(byte[] stream)
        {
            InitializeComponent();

            maxCellWidth = Width / Global.CellSize;
            maxCellHeight = (Height - 22) / Global.CellSize;

            InitAction(stream);

            surface = CreateGraphics();
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
            for (int i = 0; i < maxCellWidth; i++)
            {
                g.DrawLine(Global.GridPen, Global.CellSize * i, 0, Global.CellSize * i, Height);
            }

            for (int i = 0; i < maxCellHeight; i++)
            {
                g.DrawLine(Global.GridPen, 0, Global.CellSize * i, Width, Global.CellSize * i);
            }
        }

        private void DrawBackground(Graphics g)
        {
            for (int i = 0; i < maxCellWidth; i++)
            {
                for (int j = 0; j < maxCellHeight; j++)
                {
                    g.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), background[i, j], GraphicsUnit.Pixel);
                }
            }
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

            action[3, 6] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.COSMONAUT_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.COSMONAUT), UnitFactory.GetUnitByTemplate(UnitTemplate.COSMONAUT), UnitFactory.GetUnitByTemplate(UnitTemplate.COSMONAUT), UnitFactory.GetUnitByTemplate(UnitTemplate.COSMONAUT), UnitFactory.GetUnitByTemplate(UnitTemplate.COSMONAUT), UnitFactory.GetUnitByTemplate(UnitTemplate.COSMONAUT_RANGE));
            action[3, 19] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.BOXER_ALI), UnitFactory.GetUnitByTemplate(UnitTemplate.BOXER_STUDENT), UnitFactory.GetUnitByTemplate(UnitTemplate.BOXER_STUDENT), UnitFactory.GetUnitByTemplate(UnitTemplate.BOXER_STUDENT), UnitFactory.GetUnitByTemplate(UnitTemplate.BOXER_STUDENT), UnitFactory.GetUnitByTemplate(UnitTemplate.BOXER_STUDENT), UnitFactory.GetUnitByTemplate(UnitTemplate.BOXER_ROCKY));
            action[9, 0] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP));
            action[13, 0] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP));
            action[11, 3] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP));
            action[9, 5] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.PHILOSOPH_FAST), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP));
            action[13, 7] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP));
            action[13, 8] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP));
            action[9, 10] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP));
            action[12, 12] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP_RANGE));
            action[11, 13] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP_RANGE));
            action[9, 15] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.BITARD), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP_RANGE));
            action[12, 18] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.BITARD), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.CREEP_RANGE));
            action[12, 18] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.NORMAL), UnitFactory.GetUnitByTemplate(UnitTemplate.PHILOSOPH_FAST), UnitFactory.GetUnitByTemplate(UnitTemplate.PHILOSOPH_FAST), UnitFactory.GetUnitByTemplate(UnitTemplate.PHILOSOPH_FAST), UnitFactory.GetUnitByTemplate(UnitTemplate.PHILOSOPH_FAST), UnitFactory.GetUnitByTemplate(UnitTemplate.PHILOSOPH_FAST), UnitFactory.GetUnitByTemplate(UnitTemplate.PHILOSOPH_FAST));
            action[11, 19] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.NORMAL), UnitFactory.GetUnitByTemplate(UnitTemplate.NORMAL), UnitFactory.GetUnitByTemplate(UnitTemplate.NORMAL), UnitFactory.GetUnitByTemplate(UnitTemplate.NORMAL), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK), UnitFactory.GetUnitByTemplate(UnitTemplate.WEAK));
            action[11, 22] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.NORMAL), UnitFactory.GetUnitByTemplate(UnitTemplate.NORMAL), UnitFactory.GetUnitByTemplate(UnitTemplate.NORMAL), UnitFactory.GetUnitByTemplate(UnitTemplate.NORMAL), UnitFactory.GetUnitByTemplate(UnitTemplate.NORMAL), UnitFactory.GetUnitByTemplate(UnitTemplate.NORMAL), UnitFactory.GetUnitByTemplate(UnitTemplate.NORMAL));
            action[20, 6] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.SOLDIER_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.SOLDIER_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.SOLDIER_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.SOLDIER_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.SOLDIER_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.SOLDIER_RANGE), UnitFactory.GetUnitByTemplate(UnitTemplate.SOLDIER_RANGE));
            action[20, 19] = new Army(true, UnitFactory.GetUnitByTemplate(UnitTemplate.MIGAS), UnitFactory.GetUnitByTemplate(UnitTemplate.VANYA), UnitFactory.GetUnitByTemplate(UnitTemplate.DIMA), UnitFactory.GetUnitByTemplate(UnitTemplate.JULIA_B), UnitFactory.GetUnitByTemplate(UnitTemplate.JULIA_S), UnitFactory.GetUnitByTemplate(UnitTemplate.MISHA), UnitFactory.GetUnitByTemplate(UnitTemplate.FEDOR));
        }
#endif

        private void ReadData(BinaryReader reader)
        {
            locationName = reader.ReadString();
            for (int i = 0; i < maxCellWidth; i++)
            {
                for (int j = 0; j < maxCellHeight; j++)
                {
                    background[i, j] = new(reader.ReadInt32(), reader.ReadInt32(), Global.CellSize, Global.CellSize);
                }
            }

            battleTile = new(reader.ReadInt32(), reader.ReadInt32(), Global.CellSize, Global.CellSize);
            winCell = new(reader.ReadInt32(), reader.ReadInt32());

            for (int i = 0; i < maxCellWidth; i++)
            {
                for (int j = 0; j < maxCellHeight; j++)
                {
                    string name = reader.ReadString();

                    if (name == "NullObj")
                    {
                        continue;
                    }
                    else if (name == "Obstacle")
                    {
                        action[i, j] = new Obstacle(reader.ReadInt32(), reader.ReadInt32());
                    }
                    else if (name == "Hero")
                    {
                        int respect = reader.ReadInt32();

                        reader.ReadString(); // считываем строку "Army"
                        bool ai = reader.ReadBoolean();
                        Unit[] units = new Unit[7];
                        for (int k = 0; k < 7; k++)
                        {
                            string unitName = reader.ReadString();

                            if (unitName == "NoUnit")
                            {
                                continue;
                            }

                            Unit unit = new(new UnitSnapshot(reader));
                            units[k] = unit;
                        }

                        hero = new Hero(new Army(ai, units), respect);
                        action[i, j] = hero;
                        heroCords = new Point(j, i);
                    }
                    else if (name == "Army")
                    {
                        bool ai = reader.ReadBoolean();
                        Unit[] units = new Unit[7];
                        for (int k = 0; k < 7; k++)
                        {
                            string unitName = reader.ReadString();

                            if (unitName == "NoUnit")
                            {
                                continue;
                            }

                            Unit unit = new(new UnitSnapshot(reader));
                            units[k] = unit;
                        }

                        action[i, j] = new Army(ai, units);
                    }
                }
            }
        }

        private void InitAction(string fileName)
        {
            background = new Rectangle[maxCellWidth, maxCellHeight];

            action = new IGameObj[Width / Global.CellSize, Height / Global.CellSize];

            using (BinaryReader reader = new(File.Open(fileName, FileMode.Open)))
            {
                ReadData(reader);
            }
        }

        private void InitAction(byte[] stream)
        {
            background = new Rectangle[maxCellWidth, maxCellHeight];

            action = new IGameObj[Width / Global.CellSize, Height / Global.CellSize];

            using (BinaryReader reader = new(new MemoryStream(stream)))
            {
                ReadData(reader);
            }
        }

        private void UpdateRespect()
        {
            respectLabel.Text = hero.Respect.ToString();
        }

        private void DrawAction(Graphics g)
        {
            for (int i = 0; i < maxCellWidth; i++)
            {
                for (int j = 0; j < maxCellHeight; j++)
                {
                    if (action[i, j] != null)
                    {
                        g.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), action[i, j].Tile.Area, GraphicsUnit.Pixel);
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

            return (action[y, x] == null);
        }

        private bool MoveHero(Point dest)
        {
            Queue<Point> visit = new();
            bool[,] used = new bool[Width / Global.CellSize, Height / Global.CellSize];

            visit.Enqueue(heroCords);
            used[heroCords.Y, heroCords.X] = true;

            while (visit.Any())
            {
                Point p = visit.Dequeue();

                if (p == dest)
                {
                    MoveHero(dest.X, dest.Y);

                    if (dest.X == winCell.X && dest.Y == winCell.Y)
                    {
                        int score = 0;
                        foreach (Unit unit in hero.Army.Units)
                        {
                            if (unit != null)
                            {
                                score += unit.Damage + unit.Hp + unit.Exp + unit.Range;
                            }
                        }

                        score += hero.Respect;

                        WinForm form = new(locationName, score);

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
                    switch (action[dest.Y, dest.X].ToString())
                    {
                        case "Army":
                            StartBattle((Army)action[dest.Y, dest.X]);
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
                    switch (action[dest.Y, dest.X].ToString())
                    {
                        case "Army":
                            StartBattle((Army)action[dest.Y, dest.X]);
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
                    switch (action[dest.Y, dest.X].ToString())
                    {
                        case "Army":
                            StartBattle((Army)action[dest.Y, dest.X]);
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
                    switch (action[dest.Y, dest.X].ToString())
                    {
                        case "Army":
                            StartBattle((Army)action[dest.Y, dest.X]);
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
            if (destY < action.GetLength(0) && action[destY, destX] != null && action[destY, destX].ToString() == "Obstacle")
            {
                return;
            }

            MoveHero(new Point(destX, destY));
        }

        private void MoveHero(int x, int y)
        {
            action[heroCords.Y, heroCords.X] = null;

            heroCords.X = x;
            heroCords.Y = y;

            action[heroCords.Y, heroCords.X] = hero;

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
                for (int i = 0; i < maxCellWidth; i++)
                {
                    for (int j = 0; j < maxCellHeight; j++)
                    {
                        DrawArrow(arrow[i, j], j, i);
                    }
                }
            }
        }

        private void StartBattle(Army enemy)
        {
            BattleForm battleForm = new(this, hero, enemy, battleTile);

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

            if (hero.Army.Empty)
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
            CampForm campForm = new(this, hero);

            campForm.Location = new Point(Location.X, Location.Y);

            Visible = false;

            campForm.ShowDialog();
            Location = campForm.Location;
            campForm.Dispose();

            UpdateRespect();

            Visible = true;
        }

        private void Save(object sender, EventArgs e)
        {
            SaveLoadDialog dialog = new(this);

            dialog.ShowDialog();

            if (dialog.save)
            {
                if (!Directory.Exists("saves"))
                {
                    Directory.CreateDirectory("saves");
                }

                using (BinaryWriter writer = new(File.Open($"saves/{dialog.fileName}", FileMode.OpenOrCreate)))
                {
                    writer.Write(locationName);
                    for (int i = 0; i < maxCellWidth; i++)
                    {
                        for (int j = 0; j < maxCellHeight; j++)
                        {
                            writer.Write(background[i, j].X);
                            writer.Write(background[i, j].Y);
                        }
                    }

                    writer.Write(battleTile.X);
                    writer.Write(battleTile.Y);
                    writer.Write(winCell.X);
                    writer.Write(winCell.Y);

                    ISnapshot[,] actionstate = new ISnapshot[maxCellWidth, maxCellHeight];

                    for (int i = 0; i < maxCellWidth; i++)
                    {
                        for (int j = 0; j < maxCellHeight; j++)
                        {
                            if (action[i, j] != null)
                            {
                                action[i, j].MakeSnapshot().Save(writer);
                            }
                            else
                            {
                                writer.Write("NullObj");
                            }
                        }
                    }
                }
            }

            if (dialog.load)
            {
                if (!File.Exists($"saves/{dialog.fileName}"))
                {
                    return;
                }

                InitAction($"saves/{dialog.fileName}");
                Invalidate();
            }
        }

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
