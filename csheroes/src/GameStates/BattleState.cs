using csheroes.form;
using csheroes.src.Units;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    public class BattleState: GameState
    {
        private readonly Graphics surface;
        private readonly GameState parent;
        private Rectangle background;
        private IGameObj[,] action = null;
        private Direction[,] arrow = null;
        private Hero hero;
        private Army firstArmy,
             secondArmy;
        private readonly Point[] firstArmyCords,
                secondArmyCords;
        private int firstArmyTurn = -1,
            secondArmyTurn = -1;
        private bool turn = true,
             close = false,
             ai = true; // включен ли искусственный интелект
        private bool battleEnd = false;

        public bool BattleEnd => battleEnd;

#if DEBUG
        private readonly BattleFormSnapshot[] snapshots;
        private int lastSnapshotIndex = -1;
#endif

        private ToolStripStatusLabel toolStripStatusLabel1;

        public BattleState() 
        {
            Container components = new System.ComponentModel.Container();
            StatusStrip statusStrip1 = new System.Windows.Forms.StatusStrip();
            ToolStripStatusLabel toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            ToolStripSplitButton toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            ToolStripSplitButton toolStripSplitButton3 = new System.Windows.Forms.ToolStripSplitButton();
            ToolStripSplitButton toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            Timer timer1 = new System.Windows.Forms.Timer(components);
            ToolStripSplitButton toolStripSplitButton4 = new System.Windows.Forms.ToolStripSplitButton();
            statusStrip1.SuspendLayout();
            GameWindow.SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripStatusLabel1,
            toolStripSplitButton1,
            toolStripSplitButton3,
            toolStripSplitButton2,
            toolStripSplitButton4});
            statusStrip1.Location = new System.Drawing.Point(0, 802);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(802, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(661, 17);
            toolStripStatusLabel1.Spring = true;
            // 
            // toolStripSplitButton1
            // 
            toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripSplitButton1.DropDownButtonWidth = 0;
            toolStripSplitButton1.Image = global::csheroes.Properties.Resources.aiicon;
            toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripSplitButton1.Name = "toolStripSplitButton1";
            toolStripSplitButton1.Size = new System.Drawing.Size(21, 20);
            toolStripSplitButton1.Text = "toolStripSplitButton1";
            toolStripSplitButton1.ButtonClick += new System.EventHandler(SwitchAIBtn);
            // 
            // toolStripSplitButton3
            // 
            toolStripSplitButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripSplitButton3.DropDownButtonWidth = 0;
            toolStripSplitButton3.Image = global::csheroes.Properties.Resources.turnbackico;
            toolStripSplitButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripSplitButton3.Name = "toolStripSplitButton3";
            toolStripSplitButton3.Size = new System.Drawing.Size(21, 20);
            toolStripSplitButton3.Text = "toolStripSplitButton3";
            toolStripSplitButton3.ButtonClick += new System.EventHandler(TurnBackBtn);
            // 
            // toolStripSplitButton2
            // 
            toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripSplitButton2.DropDownButtonWidth = 0;
            toolStripSplitButton2.Image = global::csheroes.Properties.Resources.waitico;
            toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripSplitButton2.Name = "toolStripSplitButton2";
            toolStripSplitButton2.Size = new System.Drawing.Size(21, 20);
            toolStripSplitButton2.Text = "toolStripSplitButton2";
            toolStripSplitButton2.ButtonClick += new System.EventHandler(WaitBtnClick);
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
            // BattleForm
            // 
            controls.Add(statusStrip1);
            GameWindow.SetName("BattleForm");
            GameWindow.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            GameWindow.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BattleForm_KeyDown);
            GameWindow.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            statusStrip1.ResumeLayout();
            GameWindow.ResumeLayout();
        }

        public BattleState(GameState parent, Hero hero, Army enemy, Rectangle tile): this()
        {
#if RELEASE
            toolStripSplitButton1.Visible = false;
            toolStripSplitButton3.Visible = false;
#endif

            this.hero = hero;
            this.parent = parent;
#if DEBUG
            snapshots = new BattleFormSnapshot[10];
#endif

            background = tile;
            //InitBackground(tile);

            action = new IGameObj[GameWindow.Width / Global.BattleCellSize, GameWindow.Height / Global.BattleCellSize];

            firstArmy = hero.Army;
            LinedArmy(firstArmy, out firstArmyCords, 0);

            secondArmy = enemy;
            LinedArmy(secondArmy, out secondArmyCords, GameWindow.Width / Global.BattleCellSize - 1);

            surface = GameWindow.CreateGraphics();

            NextTurn(firstArmy.Units, ref firstArmyTurn);
        }

        private void Draw(Graphics g)
        {
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            DrawBackground(g);
            DrawAction(g);
            DrawGrid(g);
            DrawHighlight(g);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        private void DrawHighlight(Graphics g)
        {
            Point[] friendCords = turn ? firstArmyCords : secondArmyCords,
                    enemyCords = turn ? secondArmyCords : firstArmyCords;
            int index = turn ? firstArmyTurn : secondArmyTurn;
            Unit unit = turn ? firstArmy.Units[firstArmyTurn] : secondArmy.Units[secondArmyTurn];
            Army enemyArmy = turn ? secondArmy : firstArmy;

            while (unit == null) // если юнит, который должен был ходить, трагически погиб
            {
                if (turn)
                {
                    NextTurn(secondArmy.Units, ref secondArmyTurn);
                }
                else
                {
                    NextTurn(firstArmy.Units, ref firstArmyTurn);
                }

                if (close) // не осталось юнитов
                {
                    return;
                }

                index = turn ? firstArmyTurn : secondArmyTurn;
                unit = turn ? firstArmy.Units[firstArmyTurn] : secondArmy.Units[secondArmyTurn];
            }

            Point tmp = new(friendCords[index].X, friendCords[index].Y);
            g.DrawRectangle(Global.HighlightPen, new Rectangle(tmp.X * Global.BattleCellSize, tmp.Y * Global.BattleCellSize, Global.BattleCellSize, Global.BattleCellSize));

            Queue<Point> visit = new();
            bool[,] used = new bool[GameWindow.Width / Global.BattleCellSize, GameWindow.Height / Global.BattleCellSize];

            visit.Enqueue(tmp);
            used[tmp.Y, tmp.X] = true;

            while (visit.Any())
            {
                Point p = visit.Dequeue();

                if (p.X < 0 || p.Y < 0)
                {
                    continue;
                }

                if (p.Y != 0 && !used[p.Y - 1, p.X])
                {
                    used[p.Y - 1, p.X] = true;

                    if (IsCellInRange(tmp, new(p.X, p.Y - 1), unit.Range))
                    {
                        if (unit.Attack == AttackType.MELEE && !CellIsEmpty(p.X, p.Y - 1))
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                if (enemyArmy.Units[i] != null && enemyArmy.Units[i].Hp != 0 && enemyCords[i].X == p.X && enemyCords[i].Y == p.Y - 1)
                                {
                                    g.FillRectangle(Global.EnemyHighlightBrush, new Rectangle(p.X * Global.BattleCellSize, (p.Y - 1) * Global.BattleCellSize, Global.BattleCellSize, Global.BattleCellSize));
                                }
                            }
                        }
                        else
                        {
                            visit.Enqueue(new(p.X, p.Y - 1));
                            g.FillRectangle(Global.MoveHighlightBrush, new Rectangle(p.X * Global.BattleCellSize, (p.Y - 1) * Global.BattleCellSize, Global.BattleCellSize, Global.BattleCellSize));
                        }
                    }
                }

                if (p.Y < used.GetLength(0) - 1 && !used[p.Y + 1, p.X])
                {
                    used[p.Y + 1, p.X] = true;

                    if (IsCellInRange(tmp, new(p.X, p.Y + 1), unit.Range))
                    {
                        if (unit.Attack == AttackType.MELEE && !CellIsEmpty(p.X, p.Y + 1))
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                if (enemyArmy.Units[i] != null && enemyArmy.Units[i].Hp != 0 && enemyCords[i].X == p.X && enemyCords[i].Y == p.Y + 1)
                                {
                                    g.FillRectangle(Global.EnemyHighlightBrush, new Rectangle(p.X * Global.BattleCellSize, (p.Y + 1) * Global.BattleCellSize, Global.BattleCellSize, Global.BattleCellSize));
                                }
                            }
                        }
                        else
                        {
                            visit.Enqueue(new(p.X, p.Y + 1));
                            g.FillRectangle(Global.MoveHighlightBrush, new Rectangle(p.X * Global.BattleCellSize, (p.Y + 1) * Global.BattleCellSize, Global.BattleCellSize, Global.BattleCellSize));
                        }
                    }
                }

                if (p.X != 0 && !used[p.Y, p.X - 1])
                {
                    used[p.Y, p.X - 1] = true;

                    if (IsCellInRange(tmp, new(p.X - 1, p.Y), unit.Range))
                    {
                        if (unit.Attack == AttackType.MELEE && !CellIsEmpty(p.X - 1, p.Y))
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                if (enemyArmy.Units[i] != null && enemyArmy.Units[i].Hp != 0 && enemyCords[i].X == p.X - 1 && enemyCords[i].Y == p.Y)
                                {
                                    g.FillRectangle(Global.EnemyHighlightBrush, new Rectangle((p.X - 1) * Global.BattleCellSize, p.Y * Global.BattleCellSize, Global.BattleCellSize, Global.BattleCellSize));
                                }
                            }
                        }
                        else
                        {
                            visit.Enqueue(new(p.X - 1, p.Y));
                            g.FillRectangle(Global.MoveHighlightBrush, new Rectangle((p.X - 1) * Global.BattleCellSize, p.Y * Global.BattleCellSize, Global.BattleCellSize, Global.BattleCellSize));
                        }
                    }

                }

                if (p.X < used.GetLength(0) && !used[p.Y, p.X + 1])
                {
                    used[p.Y, p.X + 1] = true;

                    if (IsCellInRange(tmp, new(p.X + 1, p.Y), unit.Range))
                    {
                        if (unit.Attack == AttackType.MELEE && !CellIsEmpty(p.X + 1, p.Y))
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                if (enemyArmy.Units[i] != null && enemyArmy.Units[i].Hp != 0 && enemyCords[i].X == p.X + 1 && enemyCords[i].Y == p.Y)
                                {
                                    g.FillRectangle(Global.EnemyHighlightBrush, new Rectangle((p.X + 1) * Global.BattleCellSize, p.Y * Global.BattleCellSize, Global.BattleCellSize, Global.BattleCellSize));
                                }
                            }
                        }
                        else
                        {
                            visit.Enqueue(new(p.X + 1, p.Y));
                            g.FillRectangle(Global.MoveHighlightBrush, new Rectangle((p.X + 1) * Global.BattleCellSize, p.Y * Global.BattleCellSize, Global.BattleCellSize, Global.BattleCellSize));
                        }
                    }

                }
            }

            if (unit.Attack == AttackType.RANGE)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (enemyArmy.Units[i] != null && enemyArmy.Units[i].Hp != 0)
                    {
                        g.FillRectangle(Global.EnemyHighlightBrush, new Rectangle(enemyCords[i].X * Global.BattleCellSize, enemyCords[i].Y * Global.BattleCellSize, Global.BattleCellSize, Global.BattleCellSize));
                    }
                }
            }
        }

        private void DrawGrid(Graphics g)
        {
            for (int i = 0; i < GameWindow.Width / Global.BattleCellSize; i++)
            {
                g.DrawLine(Global.GridPen, Global.BattleCellSize * i, 0, Global.BattleCellSize * i, GameWindow.Height);
            }

            for (int i = 0; i < GameWindow.Height / Global.BattleCellSize; i++)
            {
                g.DrawLine(Global.GridPen, 0, Global.BattleCellSize * i, GameWindow.Width, Global.BattleCellSize * i);
            }
        }

        private List<Label> hpLabels = new();

        private void DrawAction(Graphics g)
        {
            for (int i = 0; i < GameWindow.Width / Global.BattleCellSize; i++)
            {
                for (int j = 0; j < GameWindow.Height / Global.BattleCellSize; j++)
                {
                    if (action[i, j] != null && action[i, j].ToString() == "Unit")
                    {
                        g.DrawImage(Global.Texture, new Rectangle(Global.BattleCellSize * j, Global.BattleCellSize * i, Global.BattleCellSize, Global.BattleCellSize), action[i, j].Tile.Area, GraphicsUnit.Pixel);

                        Label hp = new();
                        hp.Text = ((Unit)action[i, j]).Hp.ToString();
                        hp.Location = new(Global.BattleCellSize * j + 25, Global.BattleCellSize * i + 37);
                        hp.BackColor = Color.Green;
                        hp.ForeColor = Color.White;
                        hp.Width = 25;
                        hp.Height = 13;
                        hp.TextAlign = ContentAlignment.MiddleCenter;
                        hp.Font = new(FontFamily.GenericSerif, 8);
                        controls.Add(hp);
                        GameWindow.AddControl(hp);
                        hpLabels.Add(hp);
                    }
                }
            }
        }

        private void MovePoint(Direction direction, ref Point src)
        {
#if DEBUG
            DrawArrow(direction, src);
#endif
            switch (direction)
            {
                case Direction.DOWN:
                    src.Y++;
                    break;
                case Direction.LEFT:
                    src.X--;
                    break;
                case Direction.RIGHT:
                    src.X++;
                    break;
                case Direction.UP:
                    src.Y--;
                    break;
            }

            arrow[src.Y, src.X] = direction;
        }

        private void DrawArrow(Direction direction, Point dest)
        {
            Rectangle tile = new Rectangle(0, 128, Global.CellSize, Global.CellSize);

            switch (direction)
            {
                case Direction.NONE:
                    return;
                case Direction.LEFT:
                    tile = new Rectangle(32, 128, Global.CellSize, Global.CellSize);
                    break;
                case Direction.DOWN:
                    tile = new Rectangle(0, 128, Global.CellSize, Global.CellSize);
                    break;
                case Direction.RIGHT:
                    tile = new Rectangle(32, 96, Global.CellSize, Global.CellSize);
                    break;
                case Direction.UP:
                    tile = new Rectangle(0, 96, Global.CellSize, Global.CellSize);
                    break;
            }

            surface.DrawImage(Global.Texture, new Rectangle(Global.BattleCellSize * dest.X, Global.BattleCellSize * dest.Y, Global.BattleCellSize, Global.BattleCellSize), tile, GraphicsUnit.Pixel);
        }

        private bool CellIsEmpty(int x, int y)
        {
            if (x < 0 || x > GameWindow.Width / Global.BattleCellSize - 1 || y < 0 || y > GameWindow.Height / Global.BattleCellSize - 2)
            {
                return false;
            }

            return (action[y, x] == null);
        }

        private bool MoveUnit(Point src, Point dest, Unit unit, Point[] friendCords, int index)
        {
            if (CellIsEmpty(src.X, src.Y))
            {
                return false;
            }

            Queue<Point> visit = new();
            bool[,] used = new bool[GameWindow.Width / Global.BattleCellSize, GameWindow.Height / Global.BattleCellSize];

            visit.Enqueue(src);
            used[src.Y, src.X] = true;

            while (visit.Any())
            {
                Point p = visit.Dequeue();

                if (CellIsEmpty(dest.X, dest.Y))
                {
                    if (p == dest)
                    {
                        friendCords[index] = dest;
                        action[src.Y, src.X] = null;
                        action[dest.Y, dest.X] = unit;
                        return true;
                    }
                }
                else
                {
                    if (CellIsEmpty(dest.X - 1, dest.Y) && p.X == dest.X - 1 && p.Y == dest.Y)
                    {
                        friendCords[index] = new(dest.X - 1, dest.Y);
                        action[src.Y, src.X] = null;
                        action[dest.Y, dest.X - 1] = unit;
                        return true;
                    }
                    else if (CellIsEmpty(dest.X + 1, dest.Y) && p.X == dest.X + 1 && p.Y == dest.Y)
                    {
                        friendCords[index] = new(dest.X + 1, dest.Y);
                        action[src.Y, src.X] = null;
                        action[dest.Y, dest.X + 1] = unit;
                        return true;
                    }
                    else if (CellIsEmpty(dest.X, dest.Y - 1) && p.X == dest.X && p.Y == dest.Y - 1)
                    {
                        friendCords[index] = new(dest.X, dest.Y - 1);
                        action[src.Y, src.X] = null;
                        action[dest.Y - 1, dest.X] = unit;
                        return true;
                    }
                    else if (CellIsEmpty(dest.X, dest.Y + 1) && p.X == dest.X && p.Y == dest.Y + 1)
                    {
                        friendCords[index] = new(dest.X, dest.Y + 1);
                        action[src.Y, src.X] = null;
                        action[dest.Y + 1, dest.X] = unit;
                        return true;
                    }
                }

                if (CellIsEmpty(p.X, p.Y - 1) && !used[p.Y - 1, p.X])
                {
                    used[p.Y - 1, p.X] = true;

                    if (IsCellInRange(src, new(p.X, p.Y - 1), unit.Range))
                    {
                        visit.Enqueue(new(p.X, p.Y - 1));
                    }
                }

                if (CellIsEmpty(p.X, p.Y + 1) && !used[p.Y + 1, p.X])
                {
                    used[p.Y + 1, p.X] = true;

                    if (IsCellInRange(src, new(p.X, p.Y + 1), unit.Range))
                    {
                        visit.Enqueue(new(p.X, p.Y + 1));
                    }
                }

                if (CellIsEmpty(p.X - 1, p.Y) && !used[p.Y, p.X - 1])
                {
                    used[p.Y, p.X - 1] = true;

                    if (IsCellInRange(src, new(p.X - 1, p.Y), unit.Range))
                    {
                        visit.Enqueue(new(p.X - 1, p.Y));
                    }
                }

                if (CellIsEmpty(p.X + 1, p.Y) && !used[p.Y, p.X + 1])
                {
                    used[p.Y, p.X + 1] = true;

                    if (IsCellInRange(src, new(p.X + 1, p.Y), unit.Range))
                    {
                        visit.Enqueue(new(p.X + 1, p.Y));
                    }
                }
            }

#if DEBUG
            SolidBrush pen = new(Color.FromArgb(128, 0, 0, 255));

            for (int i = 0; i < GameWindow.Width / Global.BattleCellSize; i++)
            {
                for (int j = 0; j < GameWindow.Height / Global.BattleCellSize; j++)
                {
                    if (used[i, j] == true)
                    {
                        surface.FillRectangle(pen, j * Global.BattleCellSize, i * Global.BattleCellSize, Global.BattleCellSize, Global.BattleCellSize);
                    }
                }
            }
#endif

            return false;
        }

        private void AttackUnit(Unit enemy, Point pos, Unit damager)
        {
            enemy.Hp -= damager.Damage;

            if (enemy.Hp <= 0)
            {
                enemy.Hp = 0;
                Army enemyArmy = turn ? secondArmy : firstArmy;

                //for (int i = 0; i < enemyArmy.Units.Length; i++)
                //    if (action[pos.Y, pos.X] == enemyArmy.Units[i])
                //        enemyArmy.Units[i] = null;

                action[pos.Y, pos.X] = null;

                if (hero.Army != enemyArmy)
                {
                    hero.Respect += enemy.Level * 100;
                    damager.Exp += 1;
                }

            }
        }

        private bool UnitNearby(Point cords, Unit unit)
        {
            if (cords.Y < action.GetLength(0) && cords.X != GameWindow.Width / Global.BattleCellSize && action[cords.Y, cords.X] != null &&
                ((cords.X != 0 && action[cords.Y, cords.X - 1] == unit) ||
                (action[cords.Y, cords.X + 1] == unit) ||
                (cords.Y != 0 && action[cords.Y - 1, cords.X] == unit) ||
                (cords.Y + 1 < action.GetLength(0) && action[cords.Y + 1, cords.X] == unit)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsCellInRange(Point src, Point dest, int range)
        {
            return Math.Abs(dest.X - src.X) <= range && Math.Abs(dest.Y - src.Y) <= range;
        }

        private bool IsNeedUnitMove(Unit unit, Point src, Point dest)
        {
            bool meleeAttack = ((unit.Attack == AttackType.MELEE && !UnitNearby(dest, unit)) || (unit.Attack == AttackType.RANGE && CellIsEmpty(dest.X, dest.Y))) && IsCellInRange(src, dest, unit.Range);
            return meleeAttack;
        }

        private bool IsReadyAttack(Unit[] friends, int index, Point[] friendCords, Point dest)
        {
            Unit unit = friends[index];

            if (CellIsEmpty(dest.X, dest.Y))
            {
                return false;
            }

            for (int i = 0; i < 7; i++)
            {
                if (dest == friendCords[i] && action[dest.Y, dest.X] == friends[i])
                {
                    return false;
                }
            }

            return (unit.Attack == AttackType.RANGE) || ((unit.Attack == AttackType.MELEE) && UnitNearby(dest, unit));
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            arrow = new Direction[GameWindow.Width / Global.BattleCellSize, GameWindow.Height / Global.BattleCellSize];
            Point dest = new Point(e.X / Global.BattleCellSize, e.Y / Global.BattleCellSize);
            Point[] friendCords = turn ? firstArmyCords : secondArmyCords;
            Army friendArmy = turn ? firstArmy : secondArmy;

            if (!CellIsEmpty(dest.X, dest.Y))
            {
                for (int i = 0; i < 7; i++)
                {
                    if (dest == friendCords[i] && friendArmy.Units[i].Hp != 0) // клик на дружественного юнита
                    {
                        return;
                    }
                }
            }

            int index = turn ? firstArmyTurn : secondArmyTurn;
            Unit unit = friendArmy.Units[index];

            while (unit == null) // если юнит, который должен был ходить, трагически погиб
            {
                if (turn)
                {
                    NextTurn(secondArmy.Units, ref secondArmyTurn);
                }
                else
                {
                    NextTurn(firstArmy.Units, ref firstArmyTurn);
                }

                if (close) // не осталось юнитов
                {
                    return;
                }

                index = turn ? firstArmyTurn : secondArmyTurn;
                unit = turn ? firstArmy.Units[firstArmyTurn] : secondArmy.Units[secondArmyTurn];
            }

            Point tmp = new(friendCords[index].X, friendCords[index].Y);

            bool unitTurn = false;

#if DEBUG
            WriteSnapshot();
#endif
            if (IsNeedUnitMove(unit, tmp, dest))
            {
                unitTurn = MoveUnit(tmp, dest, unit, friendCords, index); // TODO: если на range + 1 противник, то ударить его
            }

            if (IsReadyAttack(friendArmy.Units, index, friendCords, dest))
            {
                AttackUnit((Unit)action[dest.Y, dest.X], dest, unit);

                unitTurn = true;
            }

            if (unitTurn)
            {
                if (turn)
                {
                    NextTurn(secondArmy.Units, ref secondArmyTurn);
                }
                else
                {
                    NextTurn(firstArmy.Units, ref firstArmyTurn);
                }

                turn = !turn;

                Army next = turn ? firstArmy : secondArmy;

                if (ai && next.Ai)
                {
                    AIMove();
                }
            }
            GameWindow.Invalidate();
        }

        private double VectorLenght(Point begin, Point end)
        {
            return Math.Sqrt((begin.X - end.X) * (begin.X - end.X) + (begin.Y - end.Y) * (begin.Y - end.Y));
        }

        private void AIMove()
        {
            arrow = new Direction[GameWindow.Width / Global.BattleCellSize, GameWindow.Height / Global.BattleCellSize];
            Point[] friendCords = turn ? firstArmyCords : secondArmyCords,
                    enemyCords = turn ? secondArmyCords : firstArmyCords;

            int index = turn ? firstArmyTurn : secondArmyTurn;
            if (index == -1) // Если был единственный юнит, который погиб - остановиться
                return;

            Army friendArmy = turn ? firstArmy : secondArmy;
            Unit unit = friendArmy.Units[index];
            Unit[] enemyUnits = turn ? secondArmy.Units : firstArmy.Units;

#if DEBUG
            WriteSnapshot();
#endif
            while (unit == null) // если юнит, который должен был ходить, трагически погиб
            {
                if (turn)
                {
                    NextTurn(secondArmy.Units, ref secondArmyTurn);
                }
                else
                {
                    NextTurn(firstArmy.Units, ref firstArmyTurn);
                }

                if (close) // не осталось юнитов
                {
                    return;
                }

                index = turn ? firstArmyTurn : secondArmyTurn;
                unit = turn ? firstArmy.Units[firstArmyTurn] : secondArmy.Units[secondArmyTurn];
            }

            Point dest = new(-1, -1), tmp = new(friendCords[index].X, friendCords[index].Y);

            for (int i = 0; i < 7; i++)
            {
                if (enemyUnits[i] != null && enemyUnits[i].Hp != 0 && (dest.X == -1 || (VectorLenght(dest, tmp) > VectorLenght(enemyCords[i], tmp))))
                {
                    dest = enemyCords[i];
                }
            }

            if (unit.Attack == AttackType.MELEE && Math.Abs(dest.X - tmp.X) > unit.Range)
            {
                if (dest.X < tmp.X)
                {
                    dest.X = tmp.X - unit.Range;

                    if (dest.X < 0)
                    {
                        dest.X = 0;
                    }
                }
                else // dest.X < tmp.X
                {
                    dest.X = tmp.X + unit.Range;

                    if (dest.X > GameWindow.Width / Global.BattleCellSize - 1)
                    {
                        dest.X = GameWindow.Width / Global.BattleCellSize - 1;
                    }
                }
            }

            if (unit.Attack == AttackType.MELEE && Math.Abs(dest.Y - tmp.Y) > unit.Range)
            {
                if (dest.Y < tmp.Y)
                {
                    dest.Y = tmp.Y - unit.Range;

                    if (dest.Y < 0)
                    {
                        dest.Y = 0;
                    }
                }
                else
                {
                    dest.Y = tmp.Y + unit.Range;

                    if (dest.Y > GameWindow.Height / Global.BattleCellSize - 1)
                    {
                        dest.Y = GameWindow.Height / Global.BattleCellSize - 1;
                    }
                }
            }

            bool unitTurn = false;

            if (IsNeedUnitMove(unit, tmp, dest))
            {
                unitTurn = MoveUnit(tmp, dest, unit, friendCords, index); // TODO: если на range + 1 противник, то ударить его
            }

            if (IsReadyAttack(friendArmy.Units, index, friendCords, dest))
            {
                AttackUnit((Unit)action[dest.Y, dest.X], dest, unit);

                unitTurn = true;
            }

            if (!unitTurn)
            {
                Console.WriteLine("AI Move error");
            }

            if (turn)
            {
                NextTurn(secondArmy.Units, ref secondArmyTurn);
            }
            else
            {
                NextTurn(firstArmy.Units, ref firstArmyTurn);
            }

            turn = !turn;

            GameWindow.Invalidate();
        }

        private void NextTurn(Unit[] units, ref int index)
        {
            if (hpLabels != null) // удаляем старое количество hp
            {
                foreach (Label label in hpLabels)
                {
                    controls.Remove(label);
                    GameWindow.RemoveControl(label);
                }
            }

            hpLabels = new();

            for (int i = index + 1; i < 7; i++) // проверяем остаток массива
            {
                if (units[i] != null && units[i].Hp != 0)
                {
                    index = i;
                    return;
                }
            }

            for (int i = 0; i < index + 1; i++) // начинаем смотреть сначала
            {
                if (units[i] != null && units[i].Hp != 0)
                {
                    index = i;
                    return;
                }
            }

            EndBattle();
        }

        private void SwitchAIBtn(object sender, EventArgs e)
        {
            ai = false;
        }

        private void WaitBtnClick(object sender, EventArgs e)
        {
#if DEBUG
            WriteSnapshot();
#endif

            if (turn)
            {
                NextTurn(secondArmy.Units, ref secondArmyTurn);
            }
            else
            {
                NextTurn(firstArmy.Units, ref firstArmyTurn);
            }

            turn = !turn;

            Army next = turn ? firstArmy : secondArmy;

            if (ai && next.Ai)
            {
                AIMove();
            }
        }

        private void EndBattle()
        {
            GameWindow.Paint -= new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            GameWindow.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.BattleForm_KeyDown);
            GameWindow.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            Game.ChangeGameState(parent);
            StateChange();
            battleEnd = true;
            close = true;
        }

        private void TurnBackBtn(object sender, EventArgs e)
        {
#if DEBUG
            RestoreSnapshot();
#endif
        }

        private void BattleForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
#if DEBUG
                WriteSnapshot();
#endif

                if (turn)
                {
                    NextTurn(secondArmy.Units, ref secondArmyTurn);
                }
                else
                {
                    NextTurn(firstArmy.Units, ref firstArmyTurn);
                }

                turn = !turn;

                Army next = turn ? firstArmy : secondArmy;

                if (ai && next.Ai)
                {
                    AIMove();
                }
            }
        }

        private void toolStripSplitButton4_ButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Перед вами режим боя. В данном режиме ваши юниты ходят по очереди с противником. Подсветка отображает доступные ходы ваших персонажей.\nДля пропуска хода присутствует кнопка на панели внизу (песочные часы), можно воспользоваться горячей клавишей - W.",
                "Question",
                MessageBoxButtons.OK
                );
        }

        private void DrawBackground(Graphics g)
        {
            for (int i = 0; i < GameWindow.Width / Global.BattleCellSize; i++)
            {
                for (int j = 0; j < GameWindow.Height / Global.BattleCellSize; j++)
                {
                    g.DrawImage(Global.Texture, new Rectangle(Global.BattleCellSize * j, Global.BattleCellSize * i, Global.BattleCellSize, Global.BattleCellSize), background, GraphicsUnit.Pixel);
                }
            }
        }

        private void LinedArmy(Army army, out Point[] cordsArr, int column)
        {
            cordsArr = new Point[7];

            for (int i = 0; i < GameWindow.Height && i < 7; i++)
            {
                if (army.Units[i] != null && army.Units[i].Hp != 0)
                {
                    action[i * 2, column] = army.Units[i];
                }

                cordsArr[i] = new Point(column, i * 2);
            }
        }

#if DEBUG
        private BattleFormSnapshot MakeSnapshot()
        {
            return new((HeroSnapshot)hero.MakeSnapshot(), (ArmyShapshot)secondArmy.MakeSnapshot(), firstArmyCords, secondArmyCords, firstArmyTurn, secondArmyTurn, turn, ai);
        }

        private void WriteSnapshot()
        {
            if (lastSnapshotIndex != 9)
            {
                lastSnapshotIndex++;
            }
            else
            {
                lastSnapshotIndex = 0;
            }

            snapshots[lastSnapshotIndex] = MakeSnapshot();
        }

        private void RestoreSnapshot()
        {
            if (hpLabels != null) // удаляем старое количество hp
            {
                foreach (Label label in hpLabels)
                {
                    controls.Remove(label);
                    GameWindow.RemoveControl(label);
                }
            }

            hpLabels = new();

            BattleFormSnapshot snapshot = snapshots[lastSnapshotIndex];
            snapshots[lastSnapshotIndex] = null;

            if (snapshot == null)
            {
                return;
            }

            if (lastSnapshotIndex != 0)
            {
                lastSnapshotIndex--;
            }
            else
            {
                lastSnapshotIndex = 9;
            }

            hero = new Hero(snapshot.hero);
            firstArmy = hero.Army;
            secondArmy = new Army(snapshot.secondArmy);
            action = new IGameObj[GameWindow.Width / Global.BattleCellSize, GameWindow.Height / Global.BattleCellSize];
            arrow = null;

            for (int i = 0; i < 7; i++)
            {
                firstArmyCords[i] = snapshot.firstArmyCords[i];
                if (firstArmy.Units[i] != null)
                {
                    action[firstArmyCords[i].Y, firstArmyCords[i].X] = firstArmy.Units[i];
                }

                secondArmyCords[i] = snapshot.secondArmyCords[i];
                if (secondArmy.Units[i] != null)
                {
                    action[secondArmyCords[i].Y, secondArmyCords[i].X] = secondArmy.Units[i];
                }
            }

            firstArmyTurn = snapshot.firstArmyTurn;
            secondArmyTurn = snapshot.secondArmyTurn;
            turn = snapshot.turn;
            ai = snapshot.ai;

            GameWindow.Invalidate();
        }
#endif
    }

    internal enum Direction
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public class BattleFormSnapshot
    {
        public readonly HeroSnapshot hero;
        public readonly ArmyShapshot secondArmy;
        public readonly Point[] firstArmyCords,
                                secondArmyCords;
        public readonly int firstArmyTurn,
            secondArmyTurn;
        public readonly bool turn, ai;

        public BattleFormSnapshot(HeroSnapshot hero, ArmyShapshot army, Point[] firstArmyCords, Point[] secondArmyCords, int firstArmyTurn, int secondArmyTurn, bool turn, bool ai)
        {
            this.hero = hero;
            secondArmy = army;

            this.firstArmyCords = new Point[7];
            for (int i = 0; i < 7; i++)
            {
                this.firstArmyCords[i] = firstArmyCords[i];
            }

            this.secondArmyCords = new Point[7];
            for (int i = 0; i < 7; i++)
            {
                this.secondArmyCords[i] = secondArmyCords[i];
            }

            this.firstArmyTurn = firstArmyTurn;
            this.secondArmyTurn = secondArmyTurn;
            this.turn = turn;
            this.ai = ai;
        }
    }
}
