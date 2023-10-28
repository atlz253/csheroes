using csheroes.src.Saves;
using CSHeroesTests.SaveDirectoryUnits.Mocks;

namespace CSHeroesTests.SaveDirectoryUnits
{
    [TestClass]
    public class SaveFileTests
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            SaveDirectory.DeleteDirectoryIfExists();
        }

        [TestMethod]
        public void SaveableObjectFileWriteShouldWorkIfSaveDirectoryNotExists()
        {
            SaveDirectory.DeleteDirectoryIfExists();
            string fileName = nameof(SaveableObjectFileWriteShouldWorkIfSaveDirectoryNotExists);
            FirstSaveableObjectMock mock = InstantiateMockObjectAndSaveToFileWithName(fileName);
            CompareMockAndFileData(mock, fileName);
        }

        [TestMethod]
        public void SaveableObjectFileWriteShouldWorkIfSaveDirectoryExists()
        {
            SaveDirectory.CreateDirectoryIfNeeded();
            string fileName = nameof(SaveableObjectFileWriteShouldWorkIfSaveDirectoryExists);
            FirstSaveableObjectMock mock = InstantiateMockObjectAndSaveToFileWithName(fileName);
            CompareMockAndFileData(mock, fileName);
        }

        [TestMethod]
        public void SaveableObjectFileWriteShouldRewriteFileIfFileExists()
        {
            string fileName = nameof(SaveableObjectFileWriteShouldRewriteFileIfFileExists);
            WriteRandomDataToFileWithName(fileName);
            FirstSaveableObjectMock mock = InstantiateMockObjectAndSaveToFileWithName(fileName);
            CompareMockAndFileData(mock, fileName);
        }

        private static void WriteRandomDataToFileWithName(string fileName)
        {
            using FileStream fileStream = SaveDirectory.OpenFileWithName(fileName, FileMode.OpenOrCreate);
            using BinaryWriter writer = new(fileStream);
            writer.Write("Random file data");
        }

        private static FirstSaveableObjectMock InstantiateMockObjectAndSaveToFileWithName(string fileName)
        {
            FirstSaveableObjectMock mock = new("Hello world", true, 44);
            SaveFile.WriteSaveableObjectToFileWithName(mock, fileName);
            return mock;
        }

        [TestMethod]
        public void SaveableObjectFileWriteShouldWorkIfFileNotExists()
        {
            string fileName = nameof(SaveableObjectFileWriteShouldWorkIfFileNotExists);
            SaveDirectory.DeleteFileWithNameIfExists(fileName);
            FirstSaveableObjectMock mock = InstantiateMockObjectAndSaveToFileWithName(fileName);
            CompareMockAndFileData(mock, fileName);
        }

        private void CompareMockAndFileData(FirstSaveableObjectMock mock, string fileName)
        {
            using FileStream fileStream = SaveDirectory.OpenFileWithName(fileName, FileMode.Open);
            using BinaryReader reader = new(fileStream);
            string savedClassName = reader.ReadString();
            string savedString = reader.ReadString();
            bool savedBool = reader.ReadBoolean();
            int savedInt = reader.ReadInt32();
            Assert.AreEqual(savedClassName, nameof(FirstSaveableObjectMock));
            Assert.AreEqual(mock.testString, savedString);
            Assert.AreEqual(mock.testBool, savedBool);
            Assert.AreEqual(mock.testInt, savedInt);
            Assert.IsTrue(IsEndOfBinaryReader(reader));
        }

        private static bool IsEndOfBinaryReader(BinaryReader reader)
        {
            return reader.BaseStream.Position == reader.BaseStream.Length;
        }

        [TestMethod]
        public void SaveableObjectRestoreFromNonExistingFileShouldThrowException()
        {
            string fileName = nameof(SaveableObjectRestoreFromNonExistingFileShouldThrowException);
            FirstSaveableObjectMock mock = new();
            SaveDirectory.DeleteFileWithNameIfExists(fileName);
            Assert.ThrowsException<FileNotFoundException>(() => SaveFile.RestoreSaveableObjectStateFromFileWithName(mock, fileName));
        }

        [TestMethod]
        public void SaveableObjectRestoreFromNonExistingDirectoryShouldThrowException()
        {
            SaveDirectory.DeleteDirectoryIfExists();
            string fileName = nameof(SaveableObjectRestoreFromNonExistingDirectoryShouldThrowException);
            FirstSaveableObjectMock mock = new();
            SaveDirectory.DeleteFileWithNameIfExists(fileName);
            Assert.ThrowsException<FileNotFoundException>(() => SaveFile.RestoreSaveableObjectStateFromFileWithName(mock, fileName));
        }

        [TestMethod]
        public void SaveableObjectRestoreFromFileWithAnotherSaveableObjectDataShouldThrowException() // FIXME: this test working only for FirstSaveableObjectMock, abstract class for saveable objects needed
        {
            string fileName = nameof(SaveableObjectRestoreFromFileWithAnotherSaveableObjectDataShouldThrowException);
            FirstSaveableObjectMock firstMock = new();
            SecondSaveableObjectMock secondMock = new("Test string");
            SaveFile.WriteSaveableObjectToFileWithName(secondMock, fileName);
            Assert.ThrowsException<SaveableObjectRestoreException>(() => SaveFile.RestoreSaveableObjectStateFromFileWithName(firstMock, fileName));
        }

        [TestMethod]
        public void SaveableObjectRestoreFromEmptyFileShouldThrowException() // FIXME: this test working only for FirstSaveableObjectMock, abstract class for saveable objects needed
        {
            string fileName = nameof(SaveableObjectRestoreFromEmptyFileShouldThrowException);
            FirstSaveableObjectMock mock = new();
            CreateEmptyFileWithName(fileName);
            Assert.ThrowsException<SaveableObjectRestoreException>(() => SaveFile.RestoreSaveableObjectStateFromFileWithName(mock, fileName));
        }

        private static void CreateEmptyFileWithName(string fileName)
        {
            FileStream fileStream = SaveDirectory.OpenFileWithName(fileName, FileMode.Create);
            fileStream.Dispose();
        }

        [TestMethod]
        public void SaveableObjectRestoreFromFileShouldWork()
        {
            string fileName = nameof(SaveableObjectRestoreFromFileShouldWork);
            FirstSaveableObjectMock original = new("Test", false, 777);
            FirstSaveableObjectMock clone = new();
            SaveFile.WriteSaveableObjectToFileWithName(original, fileName);
            SaveFile.RestoreSaveableObjectStateFromFileWithName(clone, fileName);
            CompareMocks(original, clone);
        }

        private static void CompareMocks(FirstSaveableObjectMock a, FirstSaveableObjectMock b)
        {
            Assert.AreEqual(a.testString, b.testString);
            Assert.AreEqual(a.testBool, b.testBool);
            Assert.AreEqual(a.testInt, b.testInt);
        }
    }
}
