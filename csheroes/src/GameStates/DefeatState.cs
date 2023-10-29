using System;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    public class DefeatState : GameState
    {
        public DefeatState() 
        {
            Label label1 = new System.Windows.Forms.Label();
            Label label2 = new System.Windows.Forms.Label();
            Button button1 = new System.Windows.Forms.Button();
            GameWindow.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            label1.Location = new System.Drawing.Point(259, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(243, 45);
            label1.TabIndex = 0;
            label1.Text = "Вас отчислили!";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            label2.Location = new System.Drawing.Point(100, 54);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(612, 37);
            label2.TabIndex = 1;
            label2.Text = "Ваша дальнейшая судьба туманна и неизвестна";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(319, 742);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(163, 23);
            button1.TabIndex = 2;
            button1.Text = "Стойте! Еще один шанс...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(button1_Click);
            // 
            // DefeatForm
            // 
            controls.Add(button1);
            controls.Add(label2);
            controls.Add(label1);
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
