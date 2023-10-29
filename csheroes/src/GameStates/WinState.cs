using System.IO;
using System;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    public class WinState : GameState
    {
        private readonly string locationName;
        private readonly int score;

        private TextBox textBox1;

        public WinState()
        {
            Label label1 = new System.Windows.Forms.Label();
            Label label2 = new System.Windows.Forms.Label();
            Button button1 = new System.Windows.Forms.Button();
            textBox1 = new System.Windows.Forms.TextBox();
            GameWindow.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            label1.Location = new System.Drawing.Point(308, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(151, 47);
            label1.TabIndex = 0;
            label1.Text = "Победа!";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            label2.Location = new System.Drawing.Point(58, 56);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(683, 37);
            label2.TabIndex = 1;
            label2.Text = "Вашей группе без лишних вопросов закрыли сессию!";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(506, 757);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "ok";
            button1.UseVisualStyleBackColor = true;
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
            controls.Add(button1);
            controls.Add(label2);
            controls.Add(label1);
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
