using System;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    public class DefeatState : GameState
    {
        public DefeatState() 
        {
            GameWindow.SuspendLayout();
            // 
            // label1
            // 
            UI.Label label1 = new()
            {
                FontSize = 24F,
                FontColor = System.Drawing.SystemColors.ButtonHighlight,
                Location = new System.Drawing.Point(259, 9),
                Text = "Вас отчислили!"
            };
            // 
            // label2
            // 
            UI.Label label2 = new()
            {
                FontSize = 20F,
                FontColor = System.Drawing.SystemColors.ButtonHighlight,
                Location = new System.Drawing.Point(100, 54),
                Text = "Ваша дальнейшая судьба туманна и неизвестна"
            };
            // 
            // button1
            // 
            UI.Button button1 = new()
            {
                Location = new System.Drawing.Point(319, 742),
                Name = "button1",
                Size = new System.Drawing.Size(163, 23),
                Text = "Стойте! Еще один шанс...",
            };
            button1.Click += new System.EventHandler(button1_Click);
            // 
            // DefeatForm
            // 
            controls.Add(button1.Control);
            controls.Add(label2.Control);
            controls.Add(label1.Control);
            GameWindow.SetBackgroundImage(Properties.Resources.defeat);
            GameWindow.SetName("Defeat");
            GameWindow.ResumeLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game.ChangeGameState(new MainMenuState());
        }
    }
}
