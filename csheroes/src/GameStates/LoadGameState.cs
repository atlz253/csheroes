using System;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace csheroes.src.GameStates
{
    public class LoadGameState: GameState
    {
        private TextBox textBox1;

        public LoadGameState() 
        {
            textBox1 = new System.Windows.Forms.TextBox();
            Label label1 = new System.Windows.Forms.Label();
            Button button1 = new System.Windows.Forms.Button();
            Button button2 = new System.Windows.Forms.Button();
            GameWindow.SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(12, 27);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(194, 23);
            textBox1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(37, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(143, 15);
            label1.TabIndex = 1;
            label1.Text = "Введите имя сохранения";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(12, 56);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "Загрузить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(button1_Click);
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(131, 56);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 3;
            button2.Text = "Отмена";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(button2_Click);
            // 
            // LoadDialog
            // 
            controls.Add(button2);
            controls.Add(button1);
            controls.Add(label1);
            controls.Add(textBox1);
            GameWindow.SetName("Load");
            GameWindow.ResumeLayout();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Game.ChangeGameState(new MainMenuState());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = textBox1.Text;

            Game.ChangeGameState(new ExploreMapGameState(fileName));
        }
    }
}
