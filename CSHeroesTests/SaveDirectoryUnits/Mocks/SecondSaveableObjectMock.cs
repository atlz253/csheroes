using csheroes.src.Saves;

namespace CSHeroesTests.SaveDirectoryUnits.Mocks
{
    internal class SecondSaveableObjectMock: ISaveable
    {
        private string testString;

        public SecondSaveableObjectMock(string testString) 
        {
            this.testString = testString;
        }

        public void ReadSave(BinaryReader reader)
        {
            testString = reader.ReadString();
        }

        public void WriteSave(BinaryWriter writer)
        {
            writer.Write(nameof(SecondSaveableObjectMock));
            writer.Write(testString);
        }
    }
}
