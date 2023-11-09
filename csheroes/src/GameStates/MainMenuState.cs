using csheroes.src.UI;
using System;
using System.Drawing;

namespace csheroes.src.GameStates
{
    internal class MainMenuState : GameState
    {
        public MainMenuState()
        {
            Game.Window.SuspendLayout();
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
            Button newGameButton = ButtonFactory.InstantiateButton(ButtonTemplate.MainMenuButton);
            newGameButton.Text = "Новая игра";
            newGameButton.SetPosition(272, 194);
            newGameButton.Click += ChangeGameStateToNewGame;
            controls.Add(newGameButton.Control);
            // 
            // button2
            // 
            Button loadGameButton = ButtonFactory.InstantiateButton(ButtonTemplate.MainMenuButton);
            loadGameButton.Text = "Загрузить игру";
            loadGameButton.SetPosition(272, 314);
            loadGameButton.Click += LoadGameDialog;
            controls.Add(loadGameButton.Control);
            // 
            // button3
            // 
            Button recordsButton = ButtonFactory.InstantiateButton(ButtonTemplate.MainMenuButton);
            recordsButton.SetPosition(272, 434);
            recordsButton.Text = "Рекорды";
            recordsButton.Click += new System.EventHandler(this.RecordsMenu);
            controls.Add(recordsButton.Control);
            // 
            // button4
            // 
            Button exitButton = ButtonFactory.InstantiateButton(ButtonTemplate.MainMenuButton);
            exitButton.SetPosition(272, 555);
            exitButton.Text = "Выход";
            exitButton.Click += Exit;
            controls.Add(exitButton.Control);
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
            Game.Window.SetBackgroundImage(Properties.Resources.Menu);
            Game.Window.SetName("MainMenuForm");
        }

        private void Exit(object sender, EventArgs e)
        {
            Game.Window.Close();
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
