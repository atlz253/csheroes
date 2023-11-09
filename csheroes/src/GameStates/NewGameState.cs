using csheroes.src.UI;
using System;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    internal class NewGameState : GameState
    {
        public byte[] level;

        public NewGameState()
        {
            Game.Window.SuspendLayout();
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
            UI.Button button1 = ButtonFactory.InstantiateButton(ButtonTemplate.MainMenuButton);
            button1.Text = "Переполох в новом корпусе";
            button1.SetPosition(5, 46);
            button1.Click += button1_Click;
            controls.Add(button1.Control);
            // 
            // button2
            // 
            UI.Button button2 = ButtonFactory.InstantiateButton(ButtonTemplate.MainMenuButton);
            button2.Text = "Коварные очереди мака";
            button2.SetPosition(5, 146);
            button2.Click += new System.EventHandler(button2_Click);
            controls.Add(button2.Control);
            // 
            // button3
            // 
            UI.Button button3 = ButtonFactory.InstantiateButton(ButtonTemplate.MainMenuButton);
            button3.Text = "Великие испытания в старом корпусе";
            button3.SetPosition(5, 246);
            button3.Click += new System.EventHandler(this.button3_Click);
            controls.Add(button3.Control);
            // 
            // LevelMenu
            // 
            Game.Window.SetName("LevelMenu");
            Game.Window.ResumeLayout();
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
