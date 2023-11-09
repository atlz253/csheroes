using System;

namespace csheroes.src
{
    public class GameObject
    {
        private Vector position = new();

        public event Action<Vector> OnPositionChange;

        public Vector Position
        {
            get => new(position);
            
            set 
            {
                position = value;

                OnPositionChange?.Invoke(Position);
            }
        }

    }
}
