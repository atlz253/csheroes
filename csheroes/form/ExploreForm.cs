using csheroes.form.camp;
using csheroes.src;
using csheroes.src.unit;
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
    public partial class ExploreForm : Form
    {
        readonly Graphics surface;

        readonly int maxCellWidth;
        readonly int maxCellHeight;

        Rectangle[,] background = null;
        Rectangle battleTile;
        Point winCell;
        IGameObj[,] action = null;
        Arrows[,] arrow = null;

        Hero hero = null;
        Point heroCords;
        string locationName;

        public ExploreForm(string fileName)
        {
            InitializeComponent();

            maxCellWidth = Width / Global.CellSize;
            maxCellHeight = Height / Global.CellSize;

#if TEST_MAP
            InitAction();
#else
            InitAction(fileName);
#endif

            surface = CreateGraphics();
        }

        void Draw()
        {
            surface.Clear(Color.White);

            DrawBackground();
            DrawArrows();
            DrawAction();
            DrawGrid();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Draw();
        }

        void DrawGrid()
        {
            for (int i = 0; i < maxCellWidth; i++)
                surface.DrawLine(Global.GridPen, Global.CellSize * i, 0, Global.CellSize * i, Height);

            for (int i = 0; i < maxCellHeight; i++)
                surface.DrawLine(Global.GridPen, 0, Global.CellSize * i, Width, Global.CellSize * i);
        }

        void DrawBackground()
        {
            for (int i = 0; i < maxCellWidth; i++)
                for (int j = 0; j < maxCellHeight; j++)
                    surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), background[i,j], GraphicsUnit.Pixel);
        }

#if TEST_MAP
        void InitAction()
        {
            locationName = "Коварные очереди в маке";
            background = new Rectangle[maxCellWidth, maxCellHeight];
            battleTile = new Rectangle(0, 0, Global.CellSize, Global.CellSize);
            winCell = new Point(0, 0);

            for (int i = 0; i < maxCellWidth; i++)
                for (int j = 0; j < maxCellHeight; j++)
                    if (i % 2 == 0)
                        if (j % 2 == 0)
                            background[i, j] = new Rectangle(32, 0, Global.CellSize, Global.CellSize);
                        else
                            background[i, j] = new Rectangle(32, 32, Global.CellSize, Global.CellSize);
                    else
                        if (j % 2 == 0)
                        background[i, j] = new Rectangle(32, 32, Global.CellSize, Global.CellSize);
                    else
                        background[i, j] = new Rectangle(32, 0, Global.CellSize, Global.CellSize);

            action = new IGameObj[maxCellWidth, maxCellHeight];

            for(int i = 0; i < 17; i++)
            {
                action[4, i + 4] = new Obstacle(ObstacleType.MAC_WALL);
                action[5, i + 4] = new Obstacle(ObstacleType.MAC_WALL);
            }

            for (int i = 1; i < maxCellWidth; i++)
                action[10, i] = new Obstacle(ObstacleType.MAC_WALL);
            


            hero = new Hero(new Army(false, new Unit[] { new Unit(), new Unit() }));
            heroCords = new Point(24, 0);
            action[heroCords.Y, heroCords.X] = hero;
            UpdateRespect();

            //action[0, 8] = new Army(true, new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST));
            //action[0, 2] = new Army(true, new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK));
            //action[2, 21] = new Army(true, new Unit(UnitTemplate.CREEP));
            //action[2, 13] = new Army(true, new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP));
            //action[2, 17] = new Army(true, new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK));
            //action[4, 0] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP) });
            //action[4, 9] = new Army(true, new Unit[] { new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK) });
            //action[4, 21] = new Army(true, new Unit(UnitTemplate.CREEP));
            //action[3, 20] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP) });
            //action[3, 13] = new Army(true, new Unit[] { new Unit(UnitTemplate.STALKER_1), new Unit(UnitTemplate.STALKER_2) });
            //action[5, 20] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP) });
            //action[6, 2] = new Army(true, new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK));
            //action[6, 12] = new Army(true, new Unit(UnitTemplate.PHYSIC_RANGE), new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE));
            //action[6, 24] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP) });
            //action[8, 2] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHYSIC_RANGE), new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE) });
            //action[8, 8] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.CREEP_RANGE) });
            //action[8, 14] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP_RANGE) });
            //action[8, 24] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHILOSOPH_TENACIOUS), new Unit(UnitTemplate.PHILOSOPH_STRONG), new Unit(UnitTemplate.PHILOSOPH_STRONG), new Unit(UnitTemplate.PHILOSOPH_STRONG), new Unit(UnitTemplate.PHILOSOPH_STRONG), new Unit(UnitTemplate.PHILOSOPH_TENACIOUS) });
            //action[10, 17] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHILOSOPH_STRONG), new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.PHILOSOPH_STRONG) });
            //action[10, 21] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHILOSOPH_RANGE), new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.PHILOSOPH_FAST), new Unit(UnitTemplate.PHILOSOPH_STRONG), new Unit(UnitTemplate.PHILOSOPH_TENACIOUS) });
            //action[11, 7] = new Army(true, new Unit[] { new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK) });
            //action[11, 23] = new Army(true, new Unit[] { new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST) });
            //action[12, 5] = new Army(true, new Unit[] { new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK) });
            //action[13, 14] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHILOSOPH_RANGE), new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.PHILOSOPH_RANGE) });
            //action[13, 21] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHILOSOPH_RANGE), new Unit(UnitTemplate.PHILOSOPH_RANGE), new Unit(UnitTemplate.PHILOSOPH_RANGE), new Unit(UnitTemplate.PHILOSOPH_RANGE) });
            //action[14, 1] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP_RANGE) });
            //action[14, 9] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHILOSOPH_FAST), new Unit(UnitTemplate.PHILOSOPH_FAST), new Unit(UnitTemplate.PHILOSOPH_FAST) });
            //action[14, 17] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE) });
            //action[16, 13] = new Army(true, new Unit[] { new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST) });
            //action[16, 22] = new Army(true, new Unit[] { new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_MIDDLE) });
            //action[17, 10] = new Army(true, new Unit[] { new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR) });
            //action[18, 5] = new Army(true, new Unit[] { new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR) });
            //action[18, 17] = new Army(true, new Unit[] { new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR) });
            //action[19, 2] = new Army(true, new Unit[] { new Unit(UnitTemplate.SUSLOV), new Unit(UnitTemplate.KERNEL_PANIC), new Unit(UnitTemplate.ERROR), new Unit(UnitTemplate.BUG), new Unit(UnitTemplate.KERNEL_PANIC), new Unit(UnitTemplate.ERROR), new Unit(UnitTemplate.BUG) });
            //action[21, 18] = new Army(true, new Unit[] { new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_MIDDLE) });
            //action[22, 2] = new Army(true, new Unit[] { new Unit(UnitTemplate.SQUID_EASY), new Unit(UnitTemplate.SQUID_EASY), new Unit(UnitTemplate.SQUID_MEDIUM), new Unit(UnitTemplate.SQUID_MEDIUM), new Unit(UnitTemplate.SQUID_HARD), new Unit(UnitTemplate.SQUID_RANGE), new Unit(UnitTemplate.SQUID_RANGE) });
            //action[22, 24] = new Army(true, new Unit[] { new Unit(UnitTemplate.ANONIMUS), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD) });
            //action[24, 1] = new Army(true, new Unit[] { new Unit(UnitTemplate.MATRIX_BALANCED), new Unit(UnitTemplate.MATRIX_STRONG), new Unit(UnitTemplate.MATRIX_FAST)  });
        }
#endif

        void InitAction(string fileName)
        {
            background = new Rectangle[maxCellWidth, maxCellHeight];

            action = new IGameObj[Width / Global.CellSize, Height / Global.CellSize];

            using (BinaryReader reader = new(File.Open(fileName, FileMode.Open)))
            {
                locationName = reader.ReadString();
                for (int i = 0; i < maxCellWidth; i++)
                    for (int j = 0; j < maxCellHeight; j++)
                        background[i, j] = new(reader.ReadInt32(), reader.ReadInt32(), Global.CellSize, Global.CellSize);
                
                battleTile = new(reader.ReadInt32(), reader.ReadInt32(), Global.CellSize, Global.CellSize);
                winCell = new(reader.ReadInt32(), reader.ReadInt32());

                for (int i = 0; i < maxCellWidth; i++)
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
                                    continue;


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
                                    continue;


                                Unit unit = new(new UnitSnapshot(reader));
                                units[k] = unit;
                            }

                            action[i, j] = new Army(ai, units);
                        }
                    }
            }
        }

        void UpdateRespect()
        {
            respectLabel.Text = hero.Respect.ToString();
        }

        void DrawAction()
        {
            for (int i = 0; i < maxCellWidth; i++)
                for (int j = 0; j < maxCellHeight; j++)
                    if (action[i, j] != null)
                        surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), action[i, j].GetTile(), GraphicsUnit.Pixel);
        }

        bool CellIsEmpty(int x, int y)
        {
            if (x < 0 || x > Width / Global.CellSize - 1 || y < 0 || y > Height / Global.CellSize - 2) // TODO: проверить, что за фигня с -1 и -2
                return false;

            return (action[y, x] == null);
        }

        bool MoveHero(Point dest)
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
                                if (unit != null)
                                    score += unit.Damage + unit.Hp + unit.Exp + unit.Range;
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
                for (int j = 0; j < Height / Global.CellSize; j++)
                    if (used[i, j] == true)
                        surface.FillRectangle(pen, j * Global.CellSize, i * Global.CellSize, Global.CellSize, Global.CellSize);
#endif

            return false;
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            int destX = e.X / Global.CellSize,
                destY = e.Y / Global.CellSize;
            if (action[destY, destX] != null && action[destY, destX].ToString() == "Obstacle")
                return;

            MoveHero(new Point(destX, destY));
        }

        void MoveHero(int x, int y)
        {
            action[heroCords.Y, heroCords.X] = null;

            heroCords.X = x;
            heroCords.Y = y;

            action[heroCords.Y, heroCords.X] = hero;

            Draw();
        }

        void DrawArrow(Arrows direction, int x, int y)
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

        void DrawArrows()
        {
            if (arrow != null)
                for (int i = 0; i < maxCellWidth; i++)
                    for (int j = 0; j < maxCellHeight; j++)
                        DrawArrow(arrow[i, j], j, i);
        }

        void StartBattle(Army enemy)
        {
            BattleForm battleForm = new(this, hero, enemy, battleTile);

            battleForm.Location = new Point(Location.X, Location.Y);

            Visible = false;

            battleForm.ShowDialog();

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
                    Directory.CreateDirectory("saves");

                using (BinaryWriter writer = new(File.Open($"saves/{dialog.fileName}", FileMode.OpenOrCreate)))
                {
                    writer.Write(locationName);
                    for (int i = 0; i < maxCellWidth; i++)
                        for (int j = 0; j < maxCellHeight; j++)
                        {
                            writer.Write(background[i, j].X);
                            writer.Write(background[i, j].Y);
                        }
                    writer.Write(battleTile.X);
                    writer.Write(battleTile.Y);
                    writer.Write(winCell.X);
                    writer.Write(winCell.Y);

                    ISnapshot[,] actionstate = new ISnapshot[maxCellWidth, maxCellHeight];

                    for (int i = 0; i < maxCellWidth; i++)
                        for (int j = 0; j < maxCellHeight; j++)
                            if (action[i, j] != null)
                                action[i, j].MakeSnapshot().Save(writer);
                            else
                                writer.Write("NullObj");
                }
            }

            if (dialog.load)
            {
                if (!File.Exists($"saves/{dialog.fileName}"))
                    return;

                InitAction($"saves/{dialog.fileName}");
                Draw();
            }
        }

        private void toolStripSplitButton3_ButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
