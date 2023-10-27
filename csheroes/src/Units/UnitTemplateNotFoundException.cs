using System;

namespace csheroes.src.Units
{
    public class UnitTemplateNotFoundException: Exception
    {
        public UnitTemplateNotFoundException(string message): base(message) { }
    }
}
