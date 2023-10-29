using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows.Forms;

namespace csheroes.src
{
    internal static class GameWindow
    {
        private static readonly Form form;

        static GameWindow()
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

        public static int Width => form.Width;
        public static int Height => form.Height;

        public static void Show()
        {
            Application.Run(form); // FIXME: I don't like this moment
        }

        public static void Close()
        {
            form.Close();
        }

        public static void AddControl(Control control)
        {
            form.Controls.Add(control);
        }

        public static void RemoveControl(Control control) 
        { 
            form.Controls.Remove(control);
        }

        public static void SuspendLayout()
        {
            form.SuspendLayout();
        }

        public static void ResumeLayout()
        {
            form.ResumeLayout();
        }

        public static void SetBackgroundImage(Bitmap bitmap) // TODO: Move from here?
        {
            form.BackgroundImage = bitmap;
        }

        public static void SetName(string name)
        {
            form.Name = name;
            form.Text = name;
        }

        public static void Clear()
        {
            form.Controls.Clear();
        }

        public static event PaintEventHandler Paint;

        private static void OnPaint(object sender, PaintEventArgs e)
        {
            Paint?.Invoke(sender, e);
        }

        public static event MouseEventHandler MouseClick;

        private static void OnMouseClick(object sender, MouseEventArgs e)
        {
            MouseClick?.Invoke(sender, e);
        }

        public static event KeyEventHandler KeyDown;

        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            KeyDown?.Invoke(sender, e);
        }

        public static Graphics CreateGraphics()
        {
            return form.CreateGraphics();
        }

        public static void Invalidate()
        {
            form.Invalidate();
        }
    }
}
