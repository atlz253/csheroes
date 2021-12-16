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
        IGameObj[,] action = null;
        Arrows[,] arrow = null;

        Hero hero = null;
        Point heroCords;

        public ExploreForm(string fileName)
        {
            InitializeComponent();

            maxCellWidth = Width / Global.CellSize;
            maxCellHeight = Height / Global.CellSize;

            //InitBackground();

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

        //void InitBackground()
        //{
        //    background = new Rectangle[maxCellWidth, maxCellHeight];

        //    for (int i = 0; i < maxCellWidth; i++)
        //        for (int j = 0; j < maxCellHeight; j++)
        //            background[i, j] = new Rectangle(Global.CellSize * Global.Rand.Next(0, 2), Global.CellSize * Global.Rand.Next(0, 2), Global.CellSize, Global.CellSize);
        //}

        void DrawBackground()
        {
            for (int i = 0; i < maxCellWidth; i++)
                for (int j = 0; j < maxCellHeight; j++)
                    surface.DrawImage(Global.Texture, new Rectangle(Global.CellSize * j, Global.CellSize * i, Global.CellSize, Global.CellSize), background[i,j], GraphicsUnit.Pixel);
        }

        void InitAction()
        {
            background = new Rectangle[maxCellWidth, maxCellHeight];
            battleTile = new Rectangle(0, 0, Global.CellSize, Global.CellSize);

            for (int i = 0; i < maxCellWidth; i++)
                for (int j = 0; j < maxCellHeight; j++)
                    background[i, j] = new Rectangle(0, 0, Global.CellSize, Global.CellSize);

            action = new IGameObj[maxCellWidth, maxCellHeight];

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

            action[0, 8] = new Army(true, new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST));
            action[0, 2] = new Army(true, new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK));
            action[2, 21] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[2, 13] = new Army(true, new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP));
            action[2, 17] = new Army(true, new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK));
            action[4, 0] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP) });
            action[4, 9] = new Army(true, new Unit[] { new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK) });
            action[4, 21] = new Army(true, new Unit(UnitTemplate.CREEP));
            action[3, 20] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP) });
            action[3, 13] = new Army(true, new Unit[] { new Unit(UnitTemplate.STALKER_1), new Unit(UnitTemplate.STALKER_2) });
            action[5, 20] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP) });
            action[6, 2] = new Army(true, new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK));
            action[6, 12] = new Army(true, new Unit(UnitTemplate.PHYSIC_RANGE), new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE));
            action[6, 24] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP) });
            action[8, 2] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHYSIC_RANGE), new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE) });
            action[8, 8] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.CREEP_RANGE) });
            action[8, 14] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP_RANGE) });
            action[8, 24] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHILOSOPH_TENACIOUS), new Unit(UnitTemplate.PHILOSOPH_STRONG), new Unit(UnitTemplate.PHILOSOPH_STRONG), new Unit(UnitTemplate.PHILOSOPH_STRONG), new Unit(UnitTemplate.PHILOSOPH_STRONG), new Unit(UnitTemplate.PHILOSOPH_TENACIOUS) });
            action[10, 17] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHILOSOPH_STRONG), new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.PHILOSOPH_STRONG) });
            action[10, 21] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHILOSOPH_RANGE), new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.PHILOSOPH_FAST), new Unit(UnitTemplate.PHILOSOPH_STRONG), new Unit(UnitTemplate.PHILOSOPH_TENACIOUS) });
            action[11, 7] = new Army(true, new Unit[] { new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.WEAK) });
            action[11, 23] = new Army(true, new Unit[] { new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST), new Unit(UnitTemplate.ECONOMIST) });
            action[12, 5] = new Army(true, new Unit[] { new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.WEAK) });
            action[13, 14] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHILOSOPH_RANGE), new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.PHILOSOPH_BALANCED), new Unit(UnitTemplate.PHILOSOPH_RANGE) });
            action[13, 21] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHILOSOPH_RANGE), new Unit(UnitTemplate.PHILOSOPH_RANGE), new Unit(UnitTemplate.PHILOSOPH_RANGE), new Unit(UnitTemplate.PHILOSOPH_RANGE) });
            action[14, 1] = new Army(true, new Unit[] { new Unit(UnitTemplate.CREEP_RANGE), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP), new Unit(UnitTemplate.CREEP_RANGE) });
            action[14, 9] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHILOSOPH_FAST), new Unit(UnitTemplate.PHILOSOPH_FAST), new Unit(UnitTemplate.PHILOSOPH_FAST) });
            action[14, 17] = new Army(true, new Unit[] { new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE), new Unit(UnitTemplate.PHYSIC_MELEE) });
            action[16, 13] = new Army(true, new Unit[] { new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST) });
            action[16, 22] = new Army(true, new Unit[] { new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_FAST), new Unit(UnitTemplate.HACKER_MIDDLE) });
            action[17, 10] = new Army(true, new Unit[] { new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_SENIOR) });
            action[18, 5] = new Army(true, new Unit[] { new Unit(UnitTemplate.HACKER_SENIOR), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR) });
            action[18, 17] = new Army(true, new Unit[] { new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR) });
            action[19, 2] = new Army(true, new Unit[] { new Unit(UnitTemplate.SUSLOV), new Unit(UnitTemplate.KERNEL_PANIC), new Unit(UnitTemplate.ERROR), new Unit(UnitTemplate.BUG), new Unit(UnitTemplate.KERNEL_PANIC), new Unit(UnitTemplate.ERROR), new Unit(UnitTemplate.BUG) });
            action[21, 18] = new Army(true, new Unit[] { new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_JUNIOR), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_MIDDLE), new Unit(UnitTemplate.HACKER_MIDDLE) });
            action[22, 2] = new Army(true, new Unit[] { new Unit(UnitTemplate.SQUID_EASY), new Unit(UnitTemplate.SQUID_EASY), new Unit(UnitTemplate.SQUID_MEDIUM), new Unit(UnitTemplate.SQUID_MEDIUM), new Unit(UnitTemplate.SQUID_HARD), new Unit(UnitTemplate.SQUID_RANGE), new Unit(UnitTemplate.SQUID_RANGE) });
            action[22, 24] = new Army(true, new Unit[] { new Unit(UnitTemplate.ANONIMUS), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD), new Unit(UnitTemplate.BITARD) });
            action[24, 1] = new Army(true, new Unit[] { new Unit(UnitTemplate.MATRIX_BALANCED), new Unit(UnitTemplate.MATRIX_STRONG), new Unit(UnitTemplate.MATRIX_FAST)  });
        }

        void InitAction(string fileName)
        {
            action = new IGameObj[Width / Global.CellSize, Height / Global.CellSize];

            using (BinaryReader reader = new(File.Open(fileName, FileMode.Open)))
            {
                for (int i = 0; i < maxCellWidth; i++)
                    for (int j = 0; j < maxCellHeight; j++)
                        background[i, j] = new(reader.ReadInt32(), reader.ReadInt32(), Global.CellSize, Global.CellSize);
                
                battleTile = new(reader.ReadInt32(), reader.ReadInt32(), Global.CellSize, Global.CellSize);

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

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            int destX = e.X / Global.CellSize,
                destY = e.Y / Global.CellSize;
            if (action[destY, destX] != null && action[destY, destX].ToString() == "Obstacle")
                return;

            int tmpX = heroCords.X,
                tmpY = heroCords.Y;
            bool heroMove = true;
            arrow = new Arrows[maxCellWidth, maxCellHeight];

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
                else if (tmpY < destY && tmpY != maxCellHeight - 2 && action[tmpY + 1, tmpX] == null && arrow[tmpY + 1, tmpX] == Arrows.EMPTY)
                {
                    while (tmpY != maxCellHeight - 2 && tmpY != destY && action[tmpY + 1, tmpX] == null && arrow[tmpY + 1, tmpX] == Arrows.EMPTY)
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
                else if (tmpX < destX && tmpX != maxCellWidth - 1 && action[tmpY, tmpX + 1] == null && arrow[tmpY, tmpX + 1] == Arrows.EMPTY)
                {
                    while (tmpX != maxCellWidth - 1 && action[tmpY, tmpX + 1] == null && tmpX != destX)
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
            SaveLoadDialog dialog = new(this);

            dialog.ShowDialog();

            if (dialog.save)
            {
                if (!Directory.Exists("saves"))
                    Directory.CreateDirectory("saves");

                using (BinaryWriter writer = new(File.Open($"saves/{dialog.fileName}", FileMode.OpenOrCreate)))
                {
                    for (int i = 0; i < maxCellWidth; i++)
                        for (int j = 0; j < maxCellHeight; j++)
                        {
                            writer.Write(background[i, j].X);
                            writer.Write(background[i, j].Y);
                        }
                    writer.Write(battleTile.X);
                    writer.Write(battleTile.Y);

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
