using System;

namespace csheroes.src
{
    internal class GameObjectException : Exception
    {
        public GameObjectException(string message) : base(message) { }
    }
}
