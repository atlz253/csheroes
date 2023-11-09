using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows.Forms;
using View;

namespace WindowsFormsView
{
    internal class WindowsFormsWindow : Window
    {
        private readonly Form form;

        public WindowsFormsWindow()
        {
            form = new Form()
            {
                Width = 818,
                Height = 863,
                AutoScaleMode = AutoScaleMode.None,
                ClientSize = new System.Drawing.Size(802, 824),
                MaximumSize = new System.Drawing.Size(818, 863),
                MinimumSize = new System.Drawing.Size(818, 863),
                MaximizeBox = false,
                MinimizeBox = false
            };

            form.Paint += OnPaint;
            form.MouseClick += OnMouseClick;
            form.KeyDown += OnKeyDown;

            SetDoubleBuffered(form);
        }

        private static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            //Taxes: Remote Desktop Connection and painting
            //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }

        public int Width => form.Width;
        public int Height => form.Height;

        public void Show()
        {
            Application.Run(form); // FIXME: I don't like this moment
        }

        public void Close()
        {
            form.Close();
        }

        public void AddControl(Control control)
        {
            form.Controls.Add(control);
        }

        public void RemoveControl(Control control) 
        { 
            form.Controls.Remove(control);
        }

        public void SuspendLayout()
        {
            form.SuspendLayout();
        }

        public void ResumeLayout()
        {
            form.ResumeLayout();
        }

        public void SetBackgroundImage(Bitmap bitmap) // TODO: Move from here?
        {
            form.BackgroundImage = bitmap;
        }

        public void SetName(string name)
        {
            form.Name = name;
            form.Text = name;
        }

        public void Clear()
        {
            form.Controls.Clear();
        }

        public event PaintEventHandler Paint;

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Paint?.Invoke(sender, e);
        }

        public event MouseEventHandler MouseClick;

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            MouseClick?.Invoke(sender, e);
        }

        public event KeyEventHandler KeyDown;

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            KeyDown?.Invoke(sender, e);
        }

        public Graphics CreateGraphics()
        {
            return form.CreateGraphics();
        }

        public void Invalidate()
        {
            form.Invalidate();
        }
    }
}
