using System.Collections.Generic;

namespace csheroes.src
{
    public class Scene
    {
        private readonly List<GameObject> gameObjects = new();

        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
        }
    }
}
