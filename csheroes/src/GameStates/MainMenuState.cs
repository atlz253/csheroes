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
            GameWindow.SuspendLayout();
            // 
            // label1
            // 
            UI.Label label = new()
            {
                Text = "И504Б Алексеев Ф. В.",
                Location = new Point(12, 785),
                FontSize = 16F,
                FontColor = SystemColors.ButtonHighlight
            };
            controls.Add(label.Control);
            // 
            // button1
            // 
            UI.Button button1 = new()
            {
                Location = new System.Drawing.Point(272, 194),
                Size = new System.Drawing.Size(250, 100),
                Text = "Новая игра"
            };
            button1.Click += ChangeGameStateToNewGame;
            controls.Add(button1.Control);
            // 
            // button2
            // 
            UI.Button button2 = new()
            {
                Location = new System.Drawing.Point(272, 314),
                Size = new System.Drawing.Size(250, 100),
                Text = "Загрузить игру",
            };
            button2.Click += new System.EventHandler(LoadGameDialog);
            controls.Add(button2.Control);
            // 
            // button3
            // 
            UI.Button button3 = new()
            {
                Location = new System.Drawing.Point(272, 434),
                Size = new System.Drawing.Size(250, 100),
                Text = "Рекорды",
            };
            button3.Click += new System.EventHandler(this.RecordsMenu);
            controls.Add(button3.Control);
            // 
            // button4
            // 
            UI.Button button4 = new()
            {
                Location = new System.Drawing.Point(272, 555),
                Size = new System.Drawing.Size(250, 100),
                Text = "Выход",
            };
            button4.Click += Exit;
            controls.Add(button4.Control);
            // 
            // label2
            //
            UI.Label label2 = new()
            {
                BackColor = System.Drawing.Color.Transparent,
                FontSize = 30F,
                FontColor = System.Drawing.SystemColors.ButtonHighlight,
                Location = new System.Drawing.Point(170, 56),
                Text = "ГЕРОИ ЗАЧЕТА И СЕССИИ"
            };
            controls.Add(label2.Control);
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
