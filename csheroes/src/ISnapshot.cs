using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src
{
    public interface ISnapshot
    {
        void Save(BinaryWriter writer);
    }
}
