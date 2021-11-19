﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src
{
    public class Hero : IGameObj
    {
        private static readonly Rectangle tile = new(224, 0, Global.CellSize, Global.CellSize);
        private readonly Army army;

        int respect = 100;

        public Hero(Army army, int respect = 100)
        {
            this.army = army;
            this.respect = respect;
        }

        public int Respect { get => respect; set => respect = value; }

        internal Army Army => army;

        public Rectangle GetTile()
        {
            return tile;
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(ToString());
            writer.Write(respect);
            army.Save(writer);
        }

        public override string ToString()
        {
            return "Hero";
        }
    }
}
