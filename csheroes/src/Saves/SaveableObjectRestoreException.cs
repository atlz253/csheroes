using System;

namespace csheroes.src.Saves
{
    public class SaveableObjectRestoreException : Exception
    {
        public SaveableObjectRestoreException(string message) : base(message) { }
    }
}
