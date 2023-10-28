using csheroes.src.Saves;

namespace CSHeroesTests.SaveDirectoryUnits.Mocks
{
    internal class FirstSaveableObjectMock : ISaveable
    {
        public string testString;
        public bool testBool;
        public int testInt;

        public FirstSaveableObjectMock() 
        {
            testString = "";
        }

        public FirstSaveableObjectMock(string testString, bool testBool, int testInt)
        {
            this.testString = testString;
            this.testBool = testBool;
            this.testInt = testInt;
        }

        public void ReadSave(BinaryReader reader) // TODO: Saveable abstract class
        {
            try
            {
                string className = reader.ReadString();

                if (className != nameof(FirstSaveableObjectMock))
                {
                    throw new SaveableObjectRestoreException("");
                }

                testString = reader.ReadString();
                testBool = reader.ReadBoolean();
                testInt = reader.ReadInt32();
            }
            catch (EndOfStreamException)
            {
                throw new SaveableObjectRestoreException("");
            }
        }

        public void WriteSave(BinaryWriter writer)
        {
            writer.Write(nameof(FirstSaveableObjectMock));
            writer.Write(testString);
            writer.Write(testBool);
            writer.Write(testInt);
        }
    }
}
