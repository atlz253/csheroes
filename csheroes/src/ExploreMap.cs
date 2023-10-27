using System.Drawing;

namespace csheroes.src
{
    public class ExploreMap
    {
        private readonly int width;
        private readonly int height;
        
        public IGameObj[,] action;
        public Rectangle[,] background;

        public Hero hero;
        public Point heroCords;

        public Point winCell;

        public ExploreMap(int width, int height)
        {
            this.width = width;
            this.height = height;

            background = new Rectangle[width, height];
        }

        public int Width => width;
        public int Height => height;
    }
}
