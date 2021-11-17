using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src
{
    interface IGameObj
    {
        Rectangle GetTile(); // TODO: свойство

        void Save(BinaryWriter writer);
    }
}
