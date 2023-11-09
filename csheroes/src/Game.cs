using System.Windows.Forms;
using csheroes.src.GameStates;
using View;

namespace csheroes.src
{
    internal static class Game
    {
        private static Window window;
        private static GameState currentGameState;

        public static GameState CurrentGameState => currentGameState;

        public static Window Window => window;

        static Game()
        {
            window = WindowFactory.GetWindow();

            ChangeGameState(new MainMenuState());
        }

        public static void Start()
        {
            window.Show();
        }

        public static void ChangeGameState(GameState gameState)
        {
            window.Clear();
            window.Invalidate();
            gameState.Register();
            currentGameState = gameState;
        }
    }
}
