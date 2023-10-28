using System.IO;

namespace csheroes.src.Saves
{
    public interface ISaveable
    {
        void WriteSave(BinaryWriter writer);
        void ReadSave(BinaryReader reader);
    }
}
