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

            action[0, 3] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[0, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[0, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[0, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[0, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[1, 3] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[1, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[1, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[1, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[1, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 0] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 2] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 3] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 4] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 6] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 8] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 10] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 12] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 14] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 16] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 18] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 20] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 22] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 23] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[2, 24] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 1] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 2] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 3] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 4] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 6] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 8] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 10] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 12] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 14] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 16] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 18] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 20] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 22] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 23] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[4, 24] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[5, 1] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[5, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[5, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[5, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[5, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[6, 1] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[6, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[6, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[6, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[6, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 1] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 2] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 3] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 4] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 5] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 6] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 8] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 9] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 10] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 12] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 13] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 14] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 16] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 17] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 18] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 20] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 21] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 22] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 23] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[7, 24] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[8, 1] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[8, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[8, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[8, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[8, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[9, 1] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[9, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[9, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[9, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[9, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 1] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 2] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 3] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 4] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 6] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 8] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 10] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 12] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 14] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 16] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 17] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 18] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 20] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 22] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 23] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[10, 24] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 0] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 2] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 3] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 4] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 6] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 8] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 10] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 12] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 14] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 16] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 18] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 20] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 22] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[12, 23] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[13, 3] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[13, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[13, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[13, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[13, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[13, 23] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[14, 3] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[14, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[14, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[14, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[14, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[14, 23] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            for (int i = 0; i < 24; i++)
                action[15, i] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[16, 3] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[16, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[16, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[16, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[16, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[16, 23] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[17, 3] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[17, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[17, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[17, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[17, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[17, 23] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 0] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 2] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 3] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 4] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 6] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 8] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 10] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 12] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 14] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 16] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 18] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 20] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 22] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[18, 23] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 1] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 2] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 3] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 4] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 6] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 8] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 10] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 12] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 14] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 16] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 18] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 20] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 22] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 23] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[20, 24] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[21, 1] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[21, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[21, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[21, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[21, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[22, 1] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[22, 7] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[22, 11] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[22, 15] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            action[22, 19] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);
            for (int i = 1; i < 25; i++)
                action[23, i] = new Obstacle(ObstacleType.NEW_KORPUS_WALL);

            hero = new Hero(new Army(false, new Unit[] { new Unit(), new Unit() }));
            heroCords = new Point(24, 0);
            action[heroCords.Y, heroCords.X] = hero;
            UpdateRespect();

            action[0, 8] = new Army(true, new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST));
            action[2, 21] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[2, 13] = new Army(true, new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP));
            action[4, 21] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[3, 20] = new Army(true, new Unit[] { new Unit(3, 3, 1), new Unit(3, 3, 1), new Unit(3, 3, 1), new Unit(3, 3, 1), new Unit(3, 3, 1), new Unit(3, 3, 1), new Unit(3, 3, 1) });
            action[3, 13] = new Army(true, new Unit[] { new Unit(UnitTemplate.STALKER_1), new Unit(UnitTemplate.STALKER_2) });
            action[5, 20] = new Army(true, new Unit[] { new Unit(3, 3, 1), new Unit(3, 3, 1) });
            action[6, 24] = new Army(true, new Unit[] { new Unit(3, 3, 1), new Unit(3, 3, 1), new Unit(3, 3, 1) });
            action[6, 12] = new Army(true, new Unit(UnitTemplate.PHYSIC_RANGE), new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_RANGE));
            action[4, 0] = new Army(true, new Unit[] { new Unit(3, 3, 1), new Unit(3, 3, 1), new Unit(3, 3, 1) });
        }
#else

        void InitAction(string fileName)
        {
            action = new IGameObj[Width / Global.CellSize, Height / Global.CellSize]; // FIXME: чтение с файла

            //using (BinaryReader reader = new(File.Open(fileName, FileMode.Open)))
            //{
            //    for (int i = 0; i < Width / Global.CellSize; i++)
            //        for (int j = 0; j < Height / Global.CellSize; j++)
            //        {
            //            string name = reader.ReadString();

            //            if (name == "NullObj")
            //            {
            //                continue;
            //            }
            //            else if (name == "Obstacle")
            //            {
            //                action[i, j] = new Obstacle(reader.ReadInt32(), reader.ReadInt32());
            //            }
            //            else if (name == "Hero")
            //            {
            //                int respect = reader.ReadInt32();

            //                reader.ReadString(); // считываем строку "Army"
            //                Unit[] units = new Unit[7];
            //                for (int k = 0; k < 7; k++)
            //                {
            //                    string unitName = reader.ReadString();
            //                    UnitType type = UnitType.ABBITURENT;

            //                    if (unitName == "NoUnit")
            //                        continue;

            //                    switch (unitName)
            //                    {
            //                        case "Абитурент":
            //                            type = UnitType.ABBITURENT;
            //                            break;
            //                        case "Технарь":
            //                            type = UnitType.TECHNAR;
            //                            break;
            //                        case "Гуманитарий":
            //                            type = UnitType.GUMANITARIY;
            //                            break;
            //                    }

            //                    Unit unit = new(type);
            //                    unit.Hp = reader.ReadInt32();
            //                    unit.Exp = reader.ReadInt32();

            //                    units[k] = unit;
            //                }

            //                hero = new Hero(new Army(false, units), respect);
            //                action[i, j] = hero;
            //                heroCords = new Point(i, j);
            //            }
            //            else if (name == "Army")
            //            {
            //                Unit[] units = new Unit[7];
            //                for (int k = 0; k < 7; k++)
            //                {
            //                    string unitName = reader.ReadString();
            //                    UnitType type = UnitType.ABBITURENT;

            //                    if (unitName == "NoUnit")
            //                        continue;

            //                    switch (unitName)
            //                    {
            //                        case "Абитурент":
            //                            type = UnitType.ABBITURENT;
            //                            break;
            //                        case "Технарь":
            //                            type = UnitType.TECHNAR;
            //                            break;
            //                        case "Гуманитарий":
            //                            type = UnitType.GUMANITARIY;
            //                            break;
            //                    }

            //                    Unit unit = new(type);
            //                    unit.Hp = reader.ReadInt32();
            //                    unit.Exp = reader.ReadInt32();

            //                    units[k] = unit;
            //                }

            //                action[i, j] = new Army(true, units);
            //            }
            //        }
            //}
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

        private void toolStripSplitButton3_ButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
