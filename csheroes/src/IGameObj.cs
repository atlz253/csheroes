using System.Drawing;

namespace csheroes.src
{
    internal interface IGameObj
    {
        Rectangle GetTile(); // TODO: свойство

        ISnapshot MakeSnapshot();
    }
}
