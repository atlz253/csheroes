using csheroes.form;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    internal class MainMenuState : GameState
    {
        public MainMenuState()
        {
            Label label1 = new Label();
            Button button1 = new Button();
            Button button2 = new Button();
            Button button3 = new Button();
            Button button4 = new Button();
            Label label2 = new Label();
            GameWindow.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            label1.Location = new System.Drawing.Point(12, 785);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(230, 30);
            label1.TabIndex = 0;
            label1.Text = "И504Б Алексеев Ф. В.";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(272, 194);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(250, 100);
            button1.TabIndex = 1;
            button1.Text = "Новая игра";
            button1.UseVisualStyleBackColor = true;
            button1.Click += ChangeGameStateToNewGame;
            controls.Add(button1);
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(272, 314);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(250, 100);
            button2.TabIndex = 2;
            button2.Text = "Загрузить игру";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new System.EventHandler(LoadGameDialog);
            controls.Add(button2);
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(272, 434);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(250, 100);
            button3.TabIndex = 3;
            button3.Text = "Рекорды";
            button3.UseVisualStyleBackColor = true;
            button3.Click += new System.EventHandler(this.RecordsMenu);
            controls.Add(button3);
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(272, 555);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(250, 100);
            button4.TabIndex = 4;
            button4.Text = "Выход";
            button4.UseVisualStyleBackColor = true;
            button4.Click += Exit;
            controls.Add(button4);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            label2.Location = new System.Drawing.Point(170, 56);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(503, 54);
            label2.TabIndex = 5;
            label2.Text = "ГЕРОИ ЗАЧЕТА И СЕССИИ";
            controls.Add(label2);
            // 
            // MainMenuForm
            // 
            GameWindow.SetBackgroundImage(Properties.Resources.Menu);
            GameWindow.SetName("MainMenuForm");
        }

        private void Exit(object sender, EventArgs e)
        {
            GameWindow.Close();
        }

        private void ChangeGameStateToNewGame(object sender, EventArgs e)
        {
            Game.ChangeGameState(new NewGameState());
        }

        private void LoadGameDialog(object sender, EventArgs e)
        {
            Game.ChangeGameState(new LoadGameState());
        }

        private void RecordsMenu(object sender, EventArgs e)
        {
            Game.ChangeGameState(new RecordState());
        }
    }
}
