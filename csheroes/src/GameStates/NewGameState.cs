using System;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    internal class NewGameState : GameState
    {
        public byte[] level;

        public NewGameState()
        {
            Label label1 = new Label();
            Button button1 = new Button();
            Button button2 = new Button();
            Button button3 = new Button();
            GameWindow.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(111, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(141, 15);
            label1.TabIndex = 0;
            label1.Text = "Выберите приключение";
            controls.Add(label1);
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(5, 46);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(112, 66);
            button1.TabIndex = 1;
            button1.Text = "Переполох в новом корпусе";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(button1_Click);
            controls.Add(button1);
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(123, 46);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(120, 66);
            button2.TabIndex = 2;
            button2.Text = "Коварные очереди мака";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(button2_Click);
            controls.Add(button2);
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(249, 46);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(131, 66);
            button3.TabIndex = 3;
            button3.Text = "Великие испытания в старом корпусе";
            button3.UseVisualStyleBackColor = true;
            button3.Click += new System.EventHandler(this.button3_Click);
            controls.Add(button3);
            // 
            // LevelMenu
            // 
            GameWindow.SetName("LevelMenu");
            GameWindow.ResumeLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            level = Properties.Resources.FirstMap;

            Game.ChangeGameState(new ExploreMapGameState(level));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            level = Properties.Resources.SecondMap;

            Game.ChangeGameState(new ExploreMapGameState(level));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            level = Properties.Resources.ThirdMap;

            Game.ChangeGameState(new ExploreMapGameState(level));
        }
    }
}
