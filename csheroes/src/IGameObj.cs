using csheroes.src.Textures;
using System.Drawing;

namespace csheroes.src
{
    internal interface IGameObj
    {
        Tile Tile { get; }

        ISnapshot MakeSnapshot();
    }
}
