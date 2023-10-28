using System.IO;

namespace csheroes.src.Saves
{
    public class SaveFile // TODO: maybe move to abstract class Save?
    {
        public static void WriteSaveableObjectToFileWithName(ISaveable saveable, string fileName)
        {
            using FileStream fileStream = SaveDirectory.OpenFileWithName(fileName, FileMode.OpenOrCreate);
            using BinaryWriter writer = new(fileStream);
            saveable.WriteSave(writer);
        }

        public static void RestoreSaveableObjectStateFromFileWithName(ISaveable saveable, string fileName)
        {
            using FileStream fileStream = SaveDirectory.OpenFileWithName(fileName, FileMode.Open);
            using BinaryReader reader = new(fileStream);
            saveable.ReadSave(reader);
        }
    }
}
