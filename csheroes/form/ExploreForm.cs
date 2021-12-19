using csheroes.form.camp;
using csheroes.src;
using csheroes.src.unit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            maxCellHeight = (Height - 22) / Global.CellSize;

#if TEST_MAP
            InitAction();
#else
            InitAction(fileName);
#endif

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

        void DrawGrid(Graphics g)
        {
            for (int i = 0; i < maxCellWidth; i++)
                g.DrawLine(Global.GridPen, Global.CellSize * i, 0, Global.CellSize * i, Height);

            for (int i = 0; i < maxCellHeight; i++)
                g.DrawLine(Global.GridPen, 0, Global.CellSize * i, Width, Global.CellSize * i);
        }

        void DrawBackground(Graphics g)
        {
            for (int i = 0; i < maxCellWidth; i++)
                for (int j = 0; j < maxCellHeight; j++)
                    g.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), background[i,j], GraphicsUnit.Pixel);
        }

#if TEST_MAP
        void InitAction()
        {
            locationName = "Коварные очереди в маке";
            background = new Rectangle[maxCellWidth, maxCellHeight];
            battleTile = new Rectangle(64, 0, Global.CellSize, Global.CellSize);
            winCell = new Point(24, 24);

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

            for (int i = 0; i < 25; i++)
                action[0, i] = new Obstacle(ObstacleType.MAC_WALL);
            for (int i = 0; i < 17; i++)
            {
                action[4, i + 4] = new Obstacle(ObstacleType.MAC_WALL);
                action[5, i + 4] = new Obstacle(ObstacleType.MAC_WALL);
            }
            action[2, 5] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[2, 17] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[2, 7] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[2, 9] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[2, 11] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[2, 13] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[2, 15] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[2, 17] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[2, 19] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[2, 23] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[2, 24] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[3, 5] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[3, 6] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[3, 7] = new Obstacle(ObstacleType.MAC_TABLE_6);
            action[3, 8] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[3, 9] = new Obstacle(ObstacleType.MAC_TABLE_6);
            action[3, 10] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[3, 11] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[3, 12] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[3, 13] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[3, 14] = new Obstacle(ObstacleType.MAC_TABLE_6);
            action[3, 15] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[3, 16] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[3, 17] = new Obstacle(ObstacleType.MAC_TABLE_6);
            action[3, 18] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[3, 19] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[3, 23] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[3, 24] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[6, 4] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[6, 5] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[6, 6] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[6, 7] = new Obstacle(ObstacleType.MAC_TABLE_2);
            action[6, 8] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[6, 9] = new Obstacle(ObstacleType.MAC_TABLE_4);
            action[6, 10] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[6, 11] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[6, 12] = new Obstacle(ObstacleType.MAC_TABLE_2);
            action[6, 13] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[6, 14] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[6, 15] = new Obstacle(ObstacleType.MAC_TABLE_2);
            action[6, 16] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[6, 17] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[6, 18] = new Obstacle(ObstacleType.MAC_TABLE_2);
            action[6, 19] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[6, 23] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[6, 24] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[7, 3] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[7, 4] = new Obstacle(ObstacleType.MAC_CHAIR_1);
            action[7, 5] = new Obstacle(ObstacleType.MAC_CHAIR_4);
            action[7, 17] = new Obstacle(ObstacleType.MAC_CHAIR_4);
            action[7, 7] = new Obstacle(ObstacleType.MAC_CHAIR_4);
            action[7, 9] = new Obstacle(ObstacleType.MAC_CHAIR_4);
            action[7, 11] = new Obstacle(ObstacleType.MAC_CHAIR_4);
            action[7, 13] = new Obstacle(ObstacleType.MAC_CHAIR_4);
            action[7, 15] = new Obstacle(ObstacleType.MAC_CHAIR_4);
            action[7, 17] = new Obstacle(ObstacleType.MAC_CHAIR_4);
            action[7, 19] = new Obstacle(ObstacleType.MAC_CHAIR_4);
            action[7, 23] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[7, 24] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[8, 4] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[8, 3] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[9, 2] = new Obstacle(ObstacleType.MAC_CHAIR_1);
            action[9, 3] = new Obstacle(ObstacleType.MAC_CHAIR_4);
            action[9, 4] = new Obstacle(ObstacleType.MAC_CHAIR_1);
            action[9, 7] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[9, 8] = new Obstacle(ObstacleType.MAC_TABLE_3);
            action[9, 9] = new Obstacle(ObstacleType.MAC_CHAIR_1);
            action[9, 10] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[9, 11] = new Obstacle(ObstacleType.MAC_TABLE_5);
            action[9, 12] = new Obstacle(ObstacleType.MAC_CHAIR_1);
            action[9, 13] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[9, 14] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[9, 15] = new Obstacle(ObstacleType.MAC_CHAIR_1);
            action[9, 16] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[9, 17] = new Obstacle(ObstacleType.MAC_TABLE_4);
            action[9, 18] = new Obstacle(ObstacleType.MAC_CHAIR_1);
            action[9, 19] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[9, 20] = new Obstacle(ObstacleType.MAC_TABLE_3);
            action[9, 21] = new Obstacle(ObstacleType.MAC_CHAIR_1);
            action[9, 22] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[9, 23] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[9, 24] = new Obstacle(ObstacleType.MAC_CHAIR_1);
            for (int i = 1; i < maxCellWidth; i++)
                action[10, i] = new Obstacle(ObstacleType.MAC_WALL);
            for (int i = 0; i < 9; i++)
            {
                action[i + 11, 7] = new Obstacle(ObstacleType.MAC_WALL);
                action[i + 11, 6] = new Obstacle(ObstacleType.MAC_SOFA);
            }
            action[11, 1] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[11, 4] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[11, 5] = new Obstacle(ObstacleType.MAC_TABLE_3);
            action[11, 8] = new Obstacle(ObstacleType.MAC_WORK_TABLE);
            for (int i = 0; i < 15; i++)
                action[11, 22 - i] = new Obstacle(ObstacleType.MAC_WORK_TABLE);
            action[12, 1] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[12, 2] = new Obstacle(ObstacleType.MAC_CHAIR_1);
            action[12, 8] = new Obstacle(ObstacleType.MAC_WORK_TABLE);
            action[13, 1] = new Obstacle(ObstacleType.MAC_CHAIR_4);
            action[13, 4] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[13, 5] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[13, 8] = new Obstacle(ObstacleType.MAC_WORK_TABLE);
            for (int i = 0; i < 14; i++)
                action[13, 23 - i] = new Obstacle(ObstacleType.MAC_WORK_TABLE);
            action[14, 1] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[14, 8] = new Obstacle(ObstacleType.MAC_WORK_TABLE);
            for (int i = 0; i < 14; i++)
                action[14, 23 - i] = new Obstacle(ObstacleType.MAC_WORK_TABLE);
            action[15, 1] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[15, 2] = new Obstacle(ObstacleType.MAC_CHAIR_1);
            action[15, 4] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[15, 5] = new Obstacle(ObstacleType.MAC_TABLE_3);
            action[16, 1] = new Obstacle(ObstacleType.MAC_CHAIR_4);
            for (int i = 0; i < 14; i++)
                action[16, 23 - i] = new Obstacle(ObstacleType.MAC_WORK_TABLE);
            action[17, 1] = new Obstacle(ObstacleType.MAC_CHAIR_3);
            action[17, 4] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[17, 5] = new Obstacle(ObstacleType.MAC_TABLE_4);
            for (int i = 0; i < 14; i++)
                action[17, 23 - i] = new Obstacle(ObstacleType.MAC_WORK_TABLE);
            action[18, 1] = new Obstacle(ObstacleType.MAC_TABLE_1);
            action[18, 2] = new Obstacle(ObstacleType.MAC_CHAIR_1);
            action[18, 23] = new Obstacle(ObstacleType.MAC_WORK_TABLE);
            action[19, 1] = new Obstacle(ObstacleType.MAC_CHAIR_4);
            action[19, 4] = new Obstacle(ObstacleType.MAC_CHAIR_2);
            action[19, 5] = new Obstacle(ObstacleType.MAC_TABLE_3);
            action[20, 9] = new Obstacle(ObstacleType.MAC_BARRIER);
            action[21, 9] = new Obstacle(ObstacleType.MAC_BARRIER);
            action[22, 9] = new Obstacle(ObstacleType.MAC_BARRIER);
            action[23, 9] = new Obstacle(ObstacleType.MAC_BARRIER);
            for (int i = 0; i < 15; i++)
                action[19, i + 9] = new Obstacle(ObstacleType.MAC_WALL);
            for (int i = 0; i < 24; i++)
                action[24, i] = new Obstacle(ObstacleType.MAC_WALL);

            hero = new Hero(new Army(false, new Unit[] { new Unit(true), new Unit(true) }));
            heroCords = new Point(5, 9);
            action[heroCords.Y, heroCords.X] = hero;
            UpdateRespect();

            action[5, 0] = new Army(true, new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP));
            action[2, 2] = new Army(true, new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK));
            action[1, 19] = new Army(true, new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.HACKER_JUNIOR));
            action[1, 24] = new Army(true, new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK));
            action[2, 6] = new Army(true, new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.PHILOSOPH_FAST), new Unit(UnitTemplate.HACKER_JUNIOR));
            action[2, 10] = new Army(true, new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_JUNIOR));
            action[2, 12] = new Army(true, new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST));
            action[2, 18] = new Army(true, new Unit(UnitTemplate.NORMAL), new Unit(UnitTemplate.NORMAL), new Unit(UnitTemplate.NORMAL), new Unit(UnitTemplate.NORMAL), new Unit(UnitTemplate.NORMAL), new Unit(UnitTemplate.NORMAL), new Unit(UnitTemplate.NORMAL));
            action[4, 22] = new Army(true, new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.CREEP_RANGE));
            action[6, 3] = new Army(true, new Unit(UnitTemplate.NORMAL), new Unit(UnitTemplate.NORMAL), new Unit(UnitTemplate.NORMAL), new Unit(UnitTemplate.NORMAL), new Unit(UnitTemplate.NORMAL), new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.CREEP_RANGE));
            action[7, 6] = new Army(true, new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP));
            action[7, 10] = new Army(true, new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK));
            action[7, 12] = new Army(true, new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP_RANGE));
            action[7, 16] = new Army(true, new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK));
            action[7, 18] = new Army(true, new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK));
            action[8, 2] = new Army(true, new Unit(UnitTemplate.HARD), new Unit(UnitTemplate.HARD), new Unit(UnitTemplate.HARD), new Unit(UnitTemplate.HARD));
            action[8, 7] = new Army(true, new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP));
            action[8, 14] = new Army(true, new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP));
            action[8, 22] = new Army(true, new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK));
            action[10, 0] = new Army(true, new Unit(UnitTemplate.ANONIMUS), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD));
            action[12, 5] = new Army(true, new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_FAST));
            action[12, 14] = new Army(true, new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER));
            action[12, 18] = new Army(true, new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER));
            action[14, 5] = new Army(true, new Unit(UnitTemplate.PHILOSOPH_TENACIOUS), new Unit(UnitTemplate.PHILOSOPH_TENACIOUS), new Unit(UnitTemplate.PHILOSOPH_TENACIOUS), new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.PHILOSOPH_BALANCED));
            action[14, 9] = new Army(true, new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER));
            action[15, 11] = new Army(true, new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER));
            action[15, 15] = new Army(true, new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER));
            action[15, 20] = new Army(true, new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER));
            action[16, 5] = new Army(true, new Unit(UnitTemplate.MATRIX_BALANCED), new Unit(UnitTemplate.MATRIX_FAST), new Unit(UnitTemplate.MATRIX_STRONG));
            action[18, 10] = new Army(true, new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER));
            action[18, 13] = new Army(true, new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER));
            action[18, 17] = new Army(true, new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER));
            action[20, 10] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[20, 13] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[20, 17] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[20, 19] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[21, 10] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[21, 13] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[21, 17] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[21, 20] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[21, 22] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[22, 10] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[22, 13] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[22, 17] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[23, 10] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[23, 11] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[23, 12] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[23, 13] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[23, 14] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[23, 15] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[23, 16] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[23, 17] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[23, 20] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[23, 23] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[24, 24] = new Army(true, new Unit(UnitTemplate.RONALD), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER), new Unit(UnitTemplate.MAC_WORKER));
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

        void DrawAction(Graphics g)
        {
            for (int i = 0; i < maxCellWidth; i++)
                for (int j = 0; j < maxCellHeight; j++)
                    if (action[i, j] != null)
                        g.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), action[i, j].GetTile(), GraphicsUnit.Pixel);
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
            if (destY < action.GetLength(0) && action[destY, destX] != null && action[destY, destX].ToString() == "Obstacle")
                return;

            MoveHero(new Point(destX, destY));
        }

        void MoveHero(int x, int y)
        {
            action[heroCords.Y, heroCords.X] = null;

            heroCords.X = x;
            heroCords.Y = y;

            action[heroCords.Y, heroCords.X] = hero;

            Invalidate();
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
                Invalidate();
            }
        }

        private void toolStripSplitButton3_ButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
