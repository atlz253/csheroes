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

        Rectangle[,] background = null;
        IGameObj[,] action = null;
        Arrows[,] arrow = null;

        Hero hero = null;
        Point heroCords;

        public ExploreForm(string fileName)
        {
            InitializeComponent();

            InitBackground();
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
            for (int i = 0; i < Width / Global.CellSize; i++)
                surface.DrawLine(Global.GridPen, Global.CellSize * i, 0, Global.CellSize * i, Height);

            for (int i = 0; i < Height / Global.CellSize; i++)
                surface.DrawLine(Global.GridPen, 0, Global.CellSize * i, Width, Global.CellSize * i);
        }

        void InitBackground()
        {
            background = new Rectangle[Width / Global.CellSize, Height / Global.CellSize];

            for (int i = 0; i < Width / Global.CellSize; i++)
                for (int j = 0; j < Height / Global.CellSize; j++)
                    background[i, j] = new Rectangle(Global.CellSize * Global.Rand.Next(0, 2), Global.CellSize * Global.Rand.Next(0, 2), Global.CellSize, Global.CellSize);
        }

        void DrawBackground()
        {
            for (int i = 0; i < Width / Global.CellSize; i++)
                for (int j = 0; j < Height / Global.CellSize; j++)
                    surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), background[i,j], GraphicsUnit.Pixel);
        }

#if TEST_MAP
        void InitAction()
        {
            action = new IGameObj[Width / Global.CellSize, Height / Global.CellSize];

            hero = new Hero(new Army(new Unit[] { new Unit(UnitType.ABBITURENT), new Unit(UnitType.ABBITURENT) }));
            heroCords = new Point(0, 0);
            action[heroCords.Y, heroCords.X] = hero;
            UpdateRespect();

            action[0, 5] = new Obstacle(ObstacleType.MOUNTAIN_1);
            action[1, 5] = new Obstacle(ObstacleType.MOUNTAIN_1);
            action[2, 5] = new Obstacle(ObstacleType.MOUNTAIN_1);
            action[3, 5] = new Obstacle(ObstacleType.MOUNTAIN_1);
            action[3, 4] = new Obstacle(ObstacleType.MOUNTAIN_1);
            action[3, 3] = new Obstacle(ObstacleType.MOUNTAIN_1);
            action[3, 2] = new Obstacle(ObstacleType.MOUNTAIN_1);
            action[3, 1] = new Obstacle(ObstacleType.MOUNTAIN_1);
            action[3, 0] = new Army(new Unit[] { new Unit(UnitType.ABBITURENT), new Unit(UnitType.ABBITURENT) });
            action[4, 0] = new Army(new Unit[] { new Unit(UnitType.ABBITURENT), new Unit(UnitType.ABBITURENT) });
        }
#else

        void InitAction(string fileName)
        {
            action = new IGameObj[Width / Global.CellSize, Height / Global.CellSize];

            using (BinaryReader reader = new(File.Open(fileName, FileMode.Open)))
            {
                for (int i = 0; i < Width / Global.CellSize; i++)
                    for (int j = 0; j < Height / Global.CellSize; j++)
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
                            Unit[] units = new Unit[7];
                            for (int k = 0; k < 7; k++)
                            {
                                string unitName = reader.ReadString();
                                UnitType type = UnitType.ABBITURENT;

                                if (unitName == "NoUnit")
                                    continue;

                                switch (unitName)
                                {
                                    case "Абитурент":
                                        type = UnitType.ABBITURENT;
                                        break;
                                    case "Технарь":
                                        type = UnitType.TECHNAR;
                                        break;
                                    case "Гуманитарий":
                                        type = UnitType.GUMANITARIY;
                                        break;
                                }

                                Unit unit = new(type);
                                unit.Hp = reader.ReadInt32();
                                unit.Exp = reader.ReadInt32();

                                units[k] = unit;
                            }

                            hero = new Hero(new Army(units), respect);
                            action[i, j] = hero;
                            heroCords = new Point(i, j);
                        }
                        else if (name == "Army")
                        {
                            Unit[] units = new Unit[7];
                            for (int k = 0; k < 7; k++)
                            {
                                string unitName = reader.ReadString();
                                UnitType type = UnitType.ABBITURENT;

                                if (unitName == "NoUnit")
                                    continue;

                                switch (unitName)
                                {
                                    case "Абитурент":
                                        type = UnitType.ABBITURENT;
                                        break;
                                    case "Технарь":
                                        type = UnitType.TECHNAR;
                                        break;
                                    case "Гуманитарий":
                                        type = UnitType.GUMANITARIY;
                                        break;
                                }

                                Unit unit = new(type);
                                unit.Hp = reader.ReadInt32();
                                unit.Exp = reader.ReadInt32();

                                units[k] = unit;
                            }

                            action[i, j] = new Army(units);
                        }
                    }
            }
        }
#endif

        void UpdateRespect()
        {
            respectLabel.Text = hero.Respect.ToString();
        }

        void DrawAction()
        {
            for (int i = 0; i < Width / Global.CellSize; i++)
                for (int j = 0; j < Height / Global.CellSize; j++)
                    if (action[i, j] != null)
                        surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), action[i, j].GetTile(), GraphicsUnit.Pixel);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            int destX = e.X / Global.CellSize,
                destY = e.Y / Global.CellSize;
            if (action[destY, destX] != null && action[destY, destX].ToString() == "Obstacle")
                return;

            int tmpX = heroCords.X,
                tmpY = heroCords.Y;
            bool heroMove = true;
            arrow = new Arrows[Width / Global.CellSize, Height / Global.CellSize];

            while (heroMove)
            {
                if ((tmpX == destX && tmpY == destY) || (action[destY, destX] != null && (tmpX == destX && (tmpY + 1 == destY || tmpY - 1 == destY) || (tmpX + 1 == destX || tmpX - 1 == destX))))
                {
                    if (action[destY, destX] != null)
                        switch (action[destY, destX].ToString())
                        {
                            case "Army":
                            StartBattle((Army) action[destY, destX]);
                            break;
                        }

                    MoveHero(destX, destY);
                    heroMove = false;
                }
                else if (tmpY < destY && tmpY != Height / Global.CellSize - 2 && action[tmpY + 1, tmpX] == null && arrow[tmpY + 1, tmpX] == Arrows.EMPTY)
                {
                    while (tmpY != Height / Global.CellSize - 2 && tmpY != destY && action[tmpY + 1, tmpX] == null && arrow[tmpY + 1, tmpX] == Arrows.EMPTY)
                    {
#if DEBUG
                        DrawArrow(Arrows.DOWN, tmpX, tmpY);
#endif
                        arrow[tmpY, tmpX] = Arrows.DOWN;
                        tmpY++;
                    }
                }
                else if (tmpY != destY && tmpY != 0 && action[tmpY - 1, tmpX] == null && arrow[tmpY - 1, tmpX] == Arrows.EMPTY)
                {
                    while (tmpY != 0 && tmpY != destY && action[tmpY - 1, tmpX] == null && arrow[tmpY - 1, tmpX] == Arrows.EMPTY)
                    {
#if DEBUG
                        DrawArrow(Arrows.UP, tmpX, tmpY);
#endif

                        arrow[tmpY, tmpX] = Arrows.UP;
                        tmpY--;
                    }
                }
                else if (tmpX < destX && tmpX != Width / Global.CellSize - 1 && action[tmpY, tmpX + 1] == null && arrow[tmpY, tmpX + 1] == Arrows.EMPTY)
                {
                    while (tmpX != Width / Global.CellSize - 1 && action[tmpY, tmpX + 1] == null && tmpX != destX)
                    {
#if DEBUG
                        DrawArrow(Arrows.RIGHT, tmpX, tmpY);
#endif

                        arrow[tmpY, tmpX] = Arrows.RIGHT;
                        tmpX++;
                    }
                }
                else if (tmpX != 0 && (action[tmpY, tmpX - 1] == null || (tmpX - 1 != 0 && action[tmpY, tmpX - 1] == null))) // TODO: переписать еще одним else if
                {
                    bool turn = false;

                    while (tmpX != 0 && action[tmpY, tmpX - 1] == null && tmpX != destX)
                    {
#if DEBUG
                        DrawArrow(Arrows.LEFT, tmpX, tmpY);
#endif

                        arrow[tmpY, tmpX] = Arrows.LEFT;
                        tmpX--;
                        turn = true;
                    }

                    if (!turn)
                    {
#if DEBUG
                        DrawArrow(Arrows.LEFT, tmpX, tmpY);
#endif
                        arrow[tmpY, tmpX] = Arrows.LEFT;
                        tmpX -= 2;
                    }
                }
                else
                {
                    Draw();
                    heroMove = false;
                }
        }
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
                for (int i = 0; i < Width / Global.CellSize; i++)
                    for (int j = 0; j < Height / Global.CellSize; j++)
                        DrawArrow(arrow[i, j], j, i);
        }

        void StartBattle(Army enemy)
        {
            BattleForm battleForm = new(this, hero, enemy);

            battleForm.Location = new Point(Location.X, Location.Y);

            Visible = false;

            battleForm.ShowDialog();
            battleForm.Dispose();

            Visible = true;
            UpdateRespect();

            if (hero.Army.Empty)
                return; // TODO: поражение
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
            SaveDialog dialog = new();

            dialog.ShowDialog();

            if (dialog.save)
            {
                if (!Directory.Exists("saves"))
                    Directory.CreateDirectory("saves");

                using (BinaryWriter writer = new(File.Open($"saves/{dialog.fileName}", FileMode.OpenOrCreate)))
                {
                    for (int i = 0; i < Width / Global.CellSize; i++)
                        for (int j = 0; j < Height / Global.CellSize; j++)
                            if (action[i, j] == null)
                                writer.Write("NullObj");
                            else
                                action[i, j].Save(writer);
                }
            }
        }
    }
}
