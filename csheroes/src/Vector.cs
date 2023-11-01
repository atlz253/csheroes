namespace csheroes.src
{
    public class Vector: IReadOnlyVector
    {
        private int x;
        private int y;

        public Vector()
        {

        }

        public Vector(Vector vector)
        {
            x = vector.x;

            y = vector.y;
        }

        public int X
        {
            get => x;
            set => x = value;
        }

        public int Y
        {
            get => y;
            set => y = value;
        }
    }

    public interface IReadOnlyVector
    {
        int X { get; }
        int Y { get; }
    }
}
