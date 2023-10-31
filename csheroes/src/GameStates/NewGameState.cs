using System;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    internal class NewGameState : GameState
    {
        public byte[] level;

        public NewGameState()
        {
            GameWindow.SuspendLayout();
            // 
            // label1
            // 
            UI.Label label1 = new()
            {
                Location = new System.Drawing.Point(111, 9),
                Text = "Выберите приключение"
            };
            controls.Add(label1.Control);
            // 
            // button1
            // 
            UI.Button button1 = new()
            {
                Location = new System.Drawing.Point(5, 46),
                Name = "button1",
                Size = new System.Drawing.Size(112, 66),
                Text = "Переполох в новом корпусе",
            };
            button1.Click += new System.EventHandler(button1_Click);
            controls.Add(button1.Control);
            // 
            // button2
            // 
            UI.Button button2 = new()
            {
                Location = new System.Drawing.Point(123, 46),
                Name = "button2",
                Size = new System.Drawing.Size(120, 66),
                Text = "Коварные очереди мака",
            };
            button2.Click += new System.EventHandler(button2_Click);
            controls.Add(button2.Control);
            // 
            // button3
            // 
            UI.Button button3 = new()
            {
                Location = new System.Drawing.Point(249, 46),
                Name = "button3",
                Size = new System.Drawing.Size(131, 66),
                Text = "Великие испытания в старом корпусе",
            };
            button3.Click += new System.EventHandler(this.button3_Click);
            controls.Add(button3.Control);
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
