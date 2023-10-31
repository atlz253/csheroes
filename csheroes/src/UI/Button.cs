using System;
using System.Drawing;

namespace csheroes.src.UI
{
    public class Button: UIObject
    {
        System.Windows.Forms.Button button = new();
        
        public event EventHandler Click;

        public void SetPosition(int x, int y)
        {
            button.Location = new(x, y);
        }

        public Point Location
        {
            set => button.Location = value;
        }

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
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            Click?.Invoke(sender, e);
        }
    }
}
