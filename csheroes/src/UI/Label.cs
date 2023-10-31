using System.Drawing;

namespace csheroes.src.UI
{
    public class Label: UIObject
    {
        private readonly System.Windows.Forms.Label label = new System.Windows.Forms.Label();

        public string Text
        {
            set => label.Text = value;
        }

        public Point Location
        {
            set => label.Location = value;
        }

        public int Width
        {
            set => label.Width = value;
        }

        public int Height
        {
            set => label.Height = value;
        }

        public float FontSize
        {
            set => label.Font = new Font("Segoe UI", value, FontStyle.Regular, GraphicsUnit.Point);
        }

        public Color FontColor
        {
            set => label.ForeColor = value;
        }

        public Color BackColor
        {
            set => label.BackColor = value;
        }

        public ContentAlignment TextAlign
        {
            set => label.TextAlign = value;
        }

        public System.Windows.Forms.Control Control => label;

        public Label()
        {
            label.AutoSize = true;
            label.BackColor = Color.Transparent;
        }
    }
}
