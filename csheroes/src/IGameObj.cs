using csheroes.src.Textures;

namespace csheroes.src
{
    public interface IGameObj
    {
        Tile Tile { get; }

        ISnapshot MakeSnapshot();
    }
}
