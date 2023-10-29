using System.Windows.Forms;
using csheroes.src.GameStates;

namespace csheroes.src
{
    internal static class Game
    {

        private static GameState currentGameState;

        public static GameState CurrentGameState => currentGameState;

        static Game()
        {
            ChangeGameState(new MainMenuState());
        }

        public static void Start()
        {
            GameWindow.Show();
        }

        public static void ChangeGameState(GameState gameState)
        {
            GameWindow.Clear();
            GameWindow.Invalidate();
            gameState.Register();
            currentGameState = gameState;
        }
    }
}
