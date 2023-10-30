using System.Drawing;

namespace csheroes.src.UI
{
    public class Label
    {
        private readonly System.Windows.Forms.Label label = new System.Windows.Forms.Label();

        public string Text
        {
            get => label.Text;

            set => label.Text = value;
        }

        public Point Location
        {
            get => label.Location;

            set => label.Location = value;
        }

        public int Width
        {
            get => label.Width;

            set => label.Width = value;
        }

        public int Height
        {
            get => label.Height;

            set => label.Height = value;
        }

        public float FontSize
        {
            get => label.Font.Size;

            set => label.Font = new Font("Segoe UI", value, FontStyle.Regular, GraphicsUnit.Point);
        }

        public Color FontColor
        {
            get => label.ForeColor;

            set => label.ForeColor = value;
        }

        public Color BackColor
        {
            get => label.BackColor;

            set => label.BackColor = value;
        }

        public ContentAlignment TextAlign
        {
            get => label.TextAlign;

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
