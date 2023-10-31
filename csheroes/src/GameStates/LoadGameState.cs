using System;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    public class LoadGameState : GameState
    {
        private TextBox textBox1;

        public LoadGameState()
        {
            textBox1 = new System.Windows.Forms.TextBox();
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
            UI.Label label1 = new()
            {
                Location = new System.Drawing.Point(37, 9),
                Text = "Введите имя сохранения"
            };
            // 
            // button1
            // 
            UI.Button button1 = new()
            {
                Location = new System.Drawing.Point(12, 56),
                Name = "button1",
                Size = new System.Drawing.Size(75, 23),
                Text = "Загрузить",
            };
            button1.Click += new System.EventHandler(button1_Click);
            // 
            // button2
            // 
            UI.Button button2 = new()
            {
                Location = new System.Drawing.Point(131, 56),
                Name = "button2",
                Size = new System.Drawing.Size(75, 23),
                Text = "Отмена",
            };
            button2.Click += new System.EventHandler(button2_Click);
            // 
            // LoadDialog
            // 
            controls.Add(button2.Control);
            controls.Add(button1.Control);
            controls.Add(label1.Control);
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
