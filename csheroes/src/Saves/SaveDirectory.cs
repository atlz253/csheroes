using System.IO;

namespace csheroes.src.Saves
{
    public class SaveDirectory
    {
        private const string SAVES_DIRECTORY_NAME = "saves";

        private static readonly DirectoryInfo directoryInfo;

        static SaveDirectory()
        {
            directoryInfo = new(SAVES_DIRECTORY_NAME);
        }

        public static FileStream OpenFileWithName(string name, FileMode mode)
        {
            CreateDirectoryIfNeeded();
            string path = GetPathByFileName(name);
            return File.Open(path, mode);
        }

        public static void CreateDirectoryIfNeeded()
        {
            if (!directoryInfo.Exists)
                directoryInfo.Create();
        }

        public static void DeleteFileWithNameIfExists(string name)
        {
            if (IsFileWithNameExists(name))
                File.Delete(GetPathByFileName(name));
        }

        public static bool IsFileWithNameExists(string name)
        {
            return File.Exists(GetPathByFileName(name));
        }

        private static string GetPathByFileName(string name)
        {
            return $"{directoryInfo.Name}/{name}";
        }

        public static void DeleteDirectoryIfExists()
        {
            if (Directory.Exists(SAVES_DIRECTORY_NAME))
                directoryInfo.Delete(true);
        }
    }
}
