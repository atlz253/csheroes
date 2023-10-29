using System.IO;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    public class RecordState : GameState
    {
        public RecordState()
        {
            Label label1 = new System.Windows.Forms.Label();
            GameWindow.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(299, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(181, 45);
            label1.TabIndex = 0;
            label1.Text = "Рекордики";
            // 
            // RecordForm
            // 
            controls.Add(label1);
            GameWindow.SetName("Records");
            GameWindow.ResumeLayout();

            LoadRecords();
        }

        private void LoadRecords()
        {
            if (!File.Exists("records.bin"))
            {
                return;
            }

            using (BinaryReader reader = new(File.Open("records.bin", FileMode.Open)))
            {
                int count = 0;
                while (reader.PeekChar() > -1)
                {
                    Label label = new();
                    label.Width = 500;
                    label.Text = $"{reader.ReadString()}     {reader.ReadString()}     {reader.ReadInt32()}";
                    label.Location = new(250, 100 + count * 25);
                    controls.Add(label);
                    count++;
                }
            }

            GameWindow.Clear();
            Register();
        }
    }
}
