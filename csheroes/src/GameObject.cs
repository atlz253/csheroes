using System.Collections.Generic;

namespace csheroes.src
{
    public class GameObject
    {
        private readonly List<Script> scripts = new(); // TODO: HashTable?

        public T AddScript<T>() where T : Script, new()
        {
            T script = new();

            return AddScript(script);
        }

        public T AddScript<T>(T script) where T : Script
        {
            if (HaveScriptOnScene<T>())
            {
                throw new GameObjectException($"Script {nameof(T)} already added");
            }
            else
            {
                scripts.Add(script);
            }

            return script;
        }

        private bool HaveScriptOnScene<T>() where T : Script
        {
            Script script = GetScriptOrNullIfNotInScene<T>();
            return script != null;
        }

        public T GetScript<T>() where T : Script
        {
            T script = GetScriptOrNullIfNotInScene<T>();

            if (script == null)
            {
                throw new GameObjectException($"Script {nameof(T)} not found in GameObject");
            }
            else
            {
                return script;
            }
        }

        private T GetScriptOrNullIfNotInScene<T>() where T : Script
        {
            foreach (Script script in scripts)
            {
                if (script.GetType() == typeof(T))
                {
                    return (T)script;
                }
            }

            return null;
        }
    }
}
