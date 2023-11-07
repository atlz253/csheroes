using System;

namespace csheroes.src
{
    public class ScenePosition : Script
    {
        private Vector position = new();

        public event Action<IReadOnlyVector> OnPositionChange;

        public Vector Position
        {
            get => position;

            set
            {
                position = value;

                OnPositionChange?.Invoke(position);
            }
        }
    }
}
