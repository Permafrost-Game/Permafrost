using Engine;
using Engine.PathFinding;
using Engine.TileGrid;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Environment;
using Microsoft.Xna.Framework;
using SimplexNoise;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IDrawable = Engine.Drawing.IDrawable;

namespace GlobalWarmingGame
{
    /// <summary>
    /// This class is used for managing GameObjects<br/>
    /// This class allows for controlled accessing of any game object by Type or Tag
    /// </summary>
    static class GameObjectManager
    {
        private static readonly int seed = 255;
        private static TileSet tileSet;

        static Vector2 zonePos;

        private static readonly List<GameObject> gameObjects;

        public static event EventHandler<GameObject> ObjectAdded = delegate { };
        public static event EventHandler<GameObject> ObjectRemoved = delegate { };

        public static Camera Camera { get; set; }

        public static TileMap ZoneMap { get; set; }

        static GameObjectManager()
        {
            gameObjects = new List<GameObject>();
            Updatables = new List<IUpdatable>();
            Drawables = new List<IDrawable>();
            Interactables = new List<IInteractable>();
        }

        static TileMap GenerateMap(Vector2 pos)
        {
            //return TileMapParser.parseTileMap(MapPath(pos), tileSet);
            return TileMapGenrator.GenerateTileMap(seed: seed, scale: 0.005f, xOffset: (int)pos.X * 100, yOffset: (int)pos.Y * 100, width: 100, height: 100, tileSet);
        }

        public static void Init(TileSet ts)
        {
            tileSet = ts;

            SetZone(Vector2.Zero);
        }

        public static string ZoneFileName()
        {
            return String.Format("{0},{1}", zonePos.X, zonePos.Y);
        }

        public static string ZoneFilePath()
        {
            return String.Format(@"{0}/{1}.json", @"Content/zones", ZoneFileName());
        }

        public static void SaveZone()
        {
            Console.WriteLine("Saving to " + ZoneFilePath());
            Serializer.Serialize(ZoneFilePath(), gameObjects);
        }

        private static void SetZone(Vector2 position)
        {
            zonePos = position;
            ZoneMap = GenerateMap(position);
            PathFinder.TileMap = ZoneMap;

            gameObjects.Clear();
            Updatables.Clear();
            Drawables.Clear();
            Interactables.Clear();

            try
            {
                IDictionary<Type, IEnumerable<object>> objs = Serializer.Deserialize(ZoneFilePath());

                Console.WriteLine("Loading from " + ZoneFilePath());

                foreach (IEnumerable<object> objList in objs.Values)
                    foreach (GameObject gameObject in objList)
                        Add(gameObject);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Creating " + ZoneFilePath());

                ZoneGenerator.SpawnGameObjects(seed);
                SaveZone();
            }
        }

        public static void MoveZone(Vector2 direction)
        {
            List<Colonist> colonists = GameObjectManager.Filter<Colonist>().ToList();

            foreach (Colonist colonist in colonists)
                GameObjectManager.Remove(colonist);

            SaveZone();
            SetZone(zonePos + direction);

            for (int i = 0; i < colonists.Count(); i++)
            {
                Colonist colonist = (Colonist)colonists[i];
                colonist.Goals.Clear();
                colonist.Path.Clear();

                if (direction.X == 1 || direction.X == -1)
                {
                    float x = direction.X == 1 ? 0 : (ZoneMap.Size.X - 1) * (tileSet.textureSize.X);
                    float y = (ZoneMap.Size.Y / 2) * (tileSet.textureSize.Y)
                        + (i * colonist.Size.Y) + (i * tileSet.textureSize.Y)
                        - ((colonists.Count / 2) * colonist.Size.Y);

                    colonist.Position = new Vector2(x, y);
                }
                else if (direction.Y == -1 || direction.Y == 1)
                {
                    float x = (ZoneMap.Size.X / 2) * (tileSet.textureSize.X)
                        + (i * colonist.Size.X) + (i * tileSet.textureSize.X)
                        - ((colonists.Count / 2) * colonist.Size.X);
                    float y = direction.Y == -1 ? (ZoneMap.Size.Y - 2) * (tileSet.textureSize.Y) : 0;

                    colonist.Position = new Vector2(x, y);
                }

                GameObjectManager.Add(colonist);
            }

            if (direction.X == 1)
            {
                Camera.Position = new Vector2(ZoneMap.Size.X * tileSet.textureSize.X, (ZoneMap.Size.Y * tileSet.textureSize.Y) / 2);
            }

            else if (direction.X == -1)
            {
                Camera.Position = new Vector2(0, (ZoneMap.Size.Y * tileSet.textureSize.Y) / 2);
            }

            else if (direction.Y == -1)
            {
                Camera.Position = new Vector2((ZoneMap.Size.Y * tileSet.textureSize.Y) / 2, 0);
            }

            else if (direction.Y == 1)
            {
                Camera.Position = new Vector2((ZoneMap.Size.Y * tileSet.textureSize.Y) / 2, ZoneMap.Size.Y * tileSet.textureSize.Y);
            }
        }

        public static List<GameObject> Objects { get => gameObjects.ToList(); }
        public static List<IUpdatable> Updatables { get; private set; } = new List<IUpdatable>();
        public static List<IDrawable> Drawables { get; private set; } = new List<IDrawable>();
        public static List<IInteractable> Interactables { get; private set; } = new List<IInteractable>();

        /// <summary>
        /// Adds a GameObject
        /// </summary>
        /// <param name="gameObject">The GameObject to be Added</param>
        public static void Add(GameObject gameObject)
        {
            gameObjects.Add(gameObject);

            if (gameObject is IDrawable d)
                Drawables.Add(d);

            if (gameObject is IUpdatable u)
                Updatables.Add(u);

            if (gameObject is IInteractable i)
                Interactables.Add(i);

            ObjectAdded.Invoke(null, gameObject);
        }

        /// <summary>
        /// Removes a GameObject
        /// </summary>
        /// <param name="gameObject">The GameObject to be removed</param>
        public static void Remove(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);

            if (gameObject is IDrawable d)
                Drawables.Remove(d);

            if (gameObject is IUpdatable u)
                Updatables.Remove(u);

            if (gameObject is IInteractable i)
                Interactables.Remove(i);

            ObjectRemoved.Invoke(null, gameObject);
        }

        /// <summary>
        /// Returns all GameObjects of a specified Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Filter<T>()
        {
            return gameObjects.OfType<T>().ToList();
        }

        /// <summary>
        /// Retrieves a list of GameObjects with a given tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>GameObjects with the specified tag</returns>
        public static List<GameObject> GetObjectsByTag(string tag)
        {
            List<GameObject> go = new List<GameObject>();

            foreach(GameObject o in gameObjects)
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
