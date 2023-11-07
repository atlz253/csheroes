using System.IO;
using System;
using System.Windows.Forms;
using csheroes.src.UI;

namespace csheroes.src.GameStates
{
    public class WinState : GameState
    {
        private readonly string locationName;
        private readonly int score;

        private TextBox textBox1;

        public WinState()
        {
            textBox1 = new System.Windows.Forms.TextBox();
            GameWindow.SuspendLayout();
            // 
            // label1
            // 
            UI.Label label1 = new()
            {
                BackColor = System.Drawing.Color.Transparent,
                FontSize = 26F,
                FontColor = System.Drawing.SystemColors.ButtonHighlight,
                Location = new System.Drawing.Point(308, 9),
                Text = "Победа!"
            };
            // 
            // label2
            // 
            UI.Label label2 = new()
            {
                BackColor = System.Drawing.Color.Transparent,
                FontSize = 20F,
                Location = new System.Drawing.Point(58, 56),
                FontColor = System.Drawing.SystemColors.ButtonHighlight,
                Text = "Вашей группе без лишних вопросов закрыли сессию!"
            };
            // 
            // button1
            // 
            UI.Button button1 = ButtonFactory.InstantiateButton(ButtonTemplate.MainMenuButton);
            button1.Text = "ok";
            button1.GetScript<ScenePosition>().Position = new(506, 757);
            button1.Click += new System.EventHandler(button1_Click);
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(213, 757);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(287, 23);
            textBox1.TabIndex = 3;
            textBox1.Text = "Как вас зовут, студент?";
            // 
            // WinForm
            // 
            
            GameWindow.SetBackgroundImage(Properties.Resources.win);
            controls.Add(textBox1);
            controls.Add(button1.Control);
            controls.Add(label2.Control);
            controls.Add(label1.Control);
            GameWindow.SetName("WinForm");
            GameWindow.ResumeLayout();
        }

        public WinState(string locationName, int score): this()
        {
            this.locationName = locationName;
            this.score = score;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (BinaryWriter writer = new(File.Open("records.bin", FileMode.Append)))
            {
                writer.Write(locationName);
                writer.Write(textBox1.Text);
                writer.Write(score);
            }

            Game.ChangeGameState(new MainMenuState());
        }
    }
}
