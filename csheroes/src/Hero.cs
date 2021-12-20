using System.Drawing;
using System.IO;

namespace csheroes.src
{
    public class HeroSnapshot : ISnapshot
    {
        public readonly ISnapshot army;
        public readonly int respect;

        public HeroSnapshot(ISnapshot army, int respect)
        {
            this.army = army;
            this.respect = respect;
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write("Hero");
            writer.Write(respect);
            army.Save(writer);
        }
    }

    public class Hero : IGameObj
    {
        private static readonly Rectangle tile = new(224, 0, Global.CellSize, Global.CellSize);
        private readonly Army army;
        private int respect = 100;

        public Hero(Army army, int respect = 100)
        {
            this.army = army;
            this.respect = respect;
        }

        public Hero(HeroSnapshot snapshot)
        {
            army = new Army((ArmyShapshot)snapshot.army);
            respect = snapshot.respect;
        }

        public int Respect { get => respect; set => respect = value; }

        internal Army Army => army;

        public Rectangle GetTile()
        {
            return tile;
        }

        public override string ToString()
        {
            return "Hero";
        }

        public ISnapshot MakeSnapshot()
        {
            return new HeroSnapshot(army.MakeSnapshot(), respect);
        }
    }
}
