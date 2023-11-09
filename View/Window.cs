using System.Drawing;
using System.Windows.Forms;

namespace View
{
    public interface Window
    {
        int Width { get; }
        int Height { get; }
        void Show();
        void Close();
        void AddControl(Control control);
        void RemoveControl(Control control);
        void SuspendLayout();
        void ResumeLayout();
        void SetBackgroundImage(Bitmap bitmap);
        void SetName(string name);
        void Clear();
        event PaintEventHandler Paint;
        event KeyEventHandler KeyDown;
        event MouseEventHandler MouseClick;
        Graphics CreateGraphics();
        void Invalidate();
    }
}
