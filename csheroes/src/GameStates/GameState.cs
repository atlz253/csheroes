using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    public class GameState
    {
        protected List<Control> controls = new();

        public event Action OnStateChange;

        protected void StateChange()
        {
            OnStateChange?.Invoke();
        }

        public void Register()
        {
            foreach (Control control in controls)
            {
                GameWindow.AddControl(control);
            }
        }
    }
}
