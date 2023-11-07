using System;
using System.Drawing;

namespace csheroes.src.UI
{
    public class Button: UIObject
    {
        System.Windows.Forms.Button button = new();
        
        public event EventHandler Click;

        public Size Size
        {
            set => button.Size = value;
        }

        public string Text
        {
            set => button.Text = value;
        }

        public string Name
        {
            get => button.Name;

            set => button.Name = value;
        }

        public System.Windows.Forms.Control Control => button;

        public Button()
        {
            button.UseVisualStyleBackColor = true;
            button.Click += OnButtonClick;

            ScenePosition position = AddScript<ScenePosition>();
            position.OnPositionChange += OnGameObjectPositionChange;
        }

        private void OnGameObjectPositionChange(IReadOnlyVector vector)
        {
            button.Location = new(vector.X, vector.Y);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            Click?.Invoke(sender, e);
        }
    }
}
