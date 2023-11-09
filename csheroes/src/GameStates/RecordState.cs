using System.IO;
using System.Windows.Forms;

namespace csheroes.src.GameStates
{
    public class RecordState : GameState
    {
        public RecordState()
        {
            Game.Window.SuspendLayout();
            // 
            // label1
            // 
            UI.Label label1 = new()
            {
                FontSize = 24F,
                Location = new System.Drawing.Point(299, 9),
                Text = "Рекордики",
                FontColor = System.Drawing.Color.White
            };
            // 
            // RecordForm
            // 
            controls.Add(label1.Control);
            Game.Window.SetName("Records");
            Game.Window.ResumeLayout();

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
                    UI.Label label = new()
                    {
                        Width = 500,
                        Text = $"{reader.ReadString()}     {reader.ReadString()}     {reader.ReadInt32()}",
                        Location = new(250, 100 + count * 25),
                        FontColor = System.Drawing.Color.White
                    };
                    controls.Add(label.Control);
                    count++;
                }
            }

            Game.Window.Clear();
            Register();
        }
    }
}
