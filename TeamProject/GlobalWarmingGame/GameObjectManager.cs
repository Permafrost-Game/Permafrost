using Engine;

using System.Collections.Generic;
using System.Linq;

namespace GlobalWarmingGame
{
    static class GameObjectManager
    {
        private static readonly List<GameObject> _objects = new List<GameObject>();
        private static readonly List<IUpdatable> _updatable = new List<IUpdatable>();
        private static readonly List<IDrawable> _drawable = new List<IDrawable>();

        public static List<GameObject> Objects { get => _objects.ToList(); }
        public static List<IUpdatable> Updatable { get => _updatable.ToList(); }
        public static List<IDrawable> Drawable { get => _drawable.ToList(); }

        public static void Add(GameObject gameObject)
        {
            _objects.Add(gameObject);

            if (gameObject is IDrawable drawable)
                _drawable.Add(drawable);

            if (gameObject is IUpdatable updatable)
                _updatable.Add(updatable);
        }

        public static void Remove(GameObject gameObject)
        {
            _objects.Remove(gameObject);

            if (gameObject is IDrawable drawable)
                _drawable.Remove(drawable);

            if (gameObject is IUpdatable updatable)
                _updatable.Remove(updatable);
        }

        public static IEnumerable<T> Filter<T>() where T : GameObject
        {
            return _objects.OfType<T>().ToList();
        }

        public static List<GameObject> GetObjectsByTag(string tag)
        {
            List<GameObject> go = new List<GameObject>();

            foreach(GameObject o in _objects)
            {
                if(o.Tag == tag)
                {
                    go.Add(o);
                }
            }

            return go;
        }
    }
}
