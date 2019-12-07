using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Interactions;
using System.Collections.Generic;
using System.Linq;

namespace GlobalWarmingGame
{
    /// <summary>
    /// This class is used for managing GameObjects<br/>
    /// This class allows for controlled accessing of any game object by Type or Tag
    /// </summary>
    static class GameObjectManager
    {
        private static readonly List<GameObject> _objects = new List<GameObject>();


        //FIXME having seperate list for interfaces is a terrible design, need to think of a better solution

        private static readonly List<IUpdatable> _updatable = new List<IUpdatable>();
        private static readonly List<IDrawable> _drawable = new List<IDrawable>();
        private static readonly List<IClickable> _clickable = new List<IClickable>();
        private static readonly List<IInteractable> _interactable = new List<IInteractable>();

        public static List<GameObject> Objects { get => _objects.ToList(); }
        public static List<IUpdatable> Updatable { get => _updatable.ToList(); }
        public static List<IDrawable> Drawable { get => _drawable.ToList(); }
        public static List<IClickable> Clickable { get => _clickable.ToList(); }
        public static List<IInteractable> Interactable { get => _interactable.ToList(); }

        /// <summary>
        /// Adds a GameObject
        /// </summary>
        /// <param name="gameObject">The GameObject to be Added</param>
        public static void Add(GameObject gameObject)
        {
            _objects.Add(gameObject);

            if (gameObject is IDrawable d)
                _drawable.Add(d);

            if (gameObject is IUpdatable u)
                _updatable.Add(u);

            if (gameObject is IClickable c)
                _clickable.Add(c);

            if (gameObject is IInteractable i)
                _interactable.Add(i);
        }

        /// <summary>
        /// Removes a GameObject
        /// </summary>
        /// <param name="gameObject">The GameObject to be removed</param>
        public static void Remove(GameObject gameObject)
        {
            _objects.Remove(gameObject);

            if (gameObject is IDrawable d)
                _drawable.Remove(d);

            if (gameObject is IUpdatable u)
                _updatable.Remove(u);

            if (gameObject is IClickable c)
                _clickable.Remove(c);

            if (gameObject is IInteractable i)
                _interactable.Remove(i);
        }

        /// <summary>
        /// Returns all GameObjects of a specified Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Filter<T>()
        {
            return _objects.OfType<T>().ToList();
        }

        /// <summary>
        /// Retrieves a list of GameObjects with a given tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>GameObjects with the specified tag</returns>
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
