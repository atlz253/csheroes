using System.IO;

namespace csheroes.src
{
    public interface ISnapshot
    {
        void Save(BinaryWriter writer);
    }
}
