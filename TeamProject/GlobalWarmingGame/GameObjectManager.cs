using Engine;
using Engine.TileGrid;
using Engine.Drawing;
using GlobalWarmingGame.Interactions;
using Microsoft.Xna.Framework;
using System.Collections;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IDrawable = Engine.Drawing.IDrawable;
using Engine.PathFinding;
using GlobalWarmingGame.Interactions.Interactables;
using System;

namespace GlobalWarmingGame
{
    /// <summary>
    /// This class is used for managing GameObjects<br/>
    /// This class allows for controlled accessing of any game object by Type or Tag
    /// </summary>
    static class GameObjectManager
    {
        private static int seed = 255;
        private static TileSet tileSet;

        static Vector2 zonePos;
        readonly static IDictionary<Vector2, Zone> zoneTable;

        public static Zone CurrentZone { get => zoneTable[zonePos]; }

        public static Camera Camera { get; set; }

        public static TileMap ZoneMap { get; set; }

        static GameObjectManager()
        {
            zoneTable = new Dictionary<Vector2, Zone>
            {
                { zonePos, new Zone() }
            };
        }

        [Obsolete]
        public static string MapPath(Vector2 pos)
        {
            return string.Format(@"Content/maps/map1/{0}{1}.csv", pos.X, pos.Y);
        }

        static TileMap LoadMap(Vector2 pos)
        {
            //return TileMapParser.parseTileMap(MapPath(pos), tileSet);
            return TileMapGenrator.GenerateTileMap(seed: seed, scale: 0.005f, xOffset: (int)pos.X * 100, yOffset: (int)pos.Y * 100, width: 100, height: 100, tileSet);
        }

        public static void Init(TileSet ts)
        {
            tileSet = ts;
            ZoneMap = LoadMap(zonePos);
            PathFinder.TileMap = ZoneMap;
        }

        public static bool isZone(Vector2 direction)
        {
            Vector2 newZonePos = zonePos + direction;

            return zoneTable.ContainsKey(newZonePos);
        }

        public static void MoveZone(Vector2 direction)
        {
            //if (isZone(direction))
            {
                List<Colonist> colonists = GameObjectManager.Filter<Colonist>().ToList();

                foreach (Colonist colonist in colonists)
                    GameObjectManager.Remove(colonist);

                Vector2 newZonePos = zonePos + direction;

                if (zoneTable.ContainsKey(newZonePos))
                {
                    zonePos = newZonePos;
                    ZoneMap = LoadMap(newZonePos);
                }
                else //if (File.Exists(MapPath(newZonePos)))
                {
                    zoneTable.Add(newZonePos, new Zone());
                    ZoneMap = LoadMap(newZonePos);

                    zonePos = newZonePos;
                }

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
                PathFinder.TileMap = ZoneMap;
            }
        }

        public static List<GameObject> Objects { get => CurrentZone.GameObjects.ToList(); }
        public static List<IUpdatable> Updatable { get => CurrentZone.Updatables.ToList(); }
        public static List<IDrawable> Drawable { get => CurrentZone.Drawables.ToList(); }
        public static List<IInteractable> Interactable { get => CurrentZone.Interactables.ToList(); }

        /// <summary>
        /// Adds a GameObject
        /// </summary>
        /// <param name="gameObject">The GameObject to be Added</param>
        public static void Add(GameObject gameObject)
        {
            CurrentZone.GameObjects.Add(gameObject);

            if (gameObject is IDrawable d)
                CurrentZone.Drawables.Add(d);

            if (gameObject is IUpdatable u)
                CurrentZone.Updatables.Add(u);

            if (gameObject is IInteractable i)
                CurrentZone.Interactables.Add(i);
        }

        /// <summary>
        /// Removes a GameObject
        /// </summary>
        /// <param name="gameObject">The GameObject to be removed</param>
        public static void Remove(GameObject gameObject)
        {
            CurrentZone.GameObjects.Remove(gameObject);

            if (gameObject is IDrawable d)
                CurrentZone.Drawables.Remove(d);

            if (gameObject is IUpdatable u)
                CurrentZone.Updatables.Remove(u);

            if (gameObject is IInteractable i)
                CurrentZone.Interactables.Remove(i);
        }

        /// <summary>
        /// Returns all GameObjects of a specified Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Filter<T>()
        {
            return CurrentZone.GameObjects.OfType<T>().ToList();
        }

        /// <summary>
        /// Retrieves a list of GameObjects with a given tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>GameObjects with the specified tag</returns>
        public static List<GameObject> GetObjectsByTag(string tag)
        {
            List<GameObject> go = new List<GameObject>();

            foreach(GameObject o in CurrentZone.GameObjects)
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
