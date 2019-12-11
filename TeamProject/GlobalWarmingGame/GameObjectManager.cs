using Engine;
using Engine.TileGrid;
using Engine.Drawing;
using GlobalWarmingGame.Interactions;
using Microsoft.Xna.Framework;
using PermaFrost;
using System.Collections;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IDrawable = Engine.IDrawable;
using Zone = PermaFrost.Zone;

namespace GlobalWarmingGame
{
    /// <summary>
    /// This class is used for managing GameObjects<br/>
    /// This class allows for controlled accessing of any game object by Type or Tag
    /// </summary>
    static class GameObjectManager
    {
        static Vector2 zonePos;
        readonly static IDictionary<Vector2, Zone> zoneTable;
        static Zone zone;
        static TileSet tileSet;

        public static TileMap ZoneMap { get => zone.tileMap; }

        static GameObjectManager()
        {
            zonePos = new Vector2(0, 0);

            zoneTable = new Dictionary<Vector2, Zone>();
            zoneTable.Add(zonePos, new Zone());

            zone = zoneTable[zonePos];
        }

        public static string MapPath(Vector2 pos)
        {
            return string.Format(@"Content/maps/map1/{0},{1}.csv", pos.X, pos.Y);
        }

        static TileMap LoadMap(Vector2 pos)
        {
            return TileMapParser.parseTileMap(MapPath(pos), tileSet);
        }

        public static void Init(TileSet ts)
        {
            tileSet = ts;
            zone.tileMap = LoadMap(zonePos);
        }

        public static bool isZone(Vector2 direction)
        {
            Vector2 newZonePos = zonePos + direction;

            return zoneTable.ContainsKey(newZonePos) || File.Exists(MapPath(newZonePos));
        }

        public static void MoveZone(Vector2 direction)
        {
            if (isZone(direction))
            {
                List<GameObject> colonists = GameObjectManager.GetObjectsByTag("Colonist");

                foreach (GameObject colonist in colonists)
                    GameObjectManager.Remove(colonist);

                Vector2 newZonePos = zonePos + direction;

                if (zoneTable.ContainsKey(newZonePos))
                {
                    zonePos = newZonePos;
                    zone = zoneTable[zonePos];
                }

                else if (File.Exists(MapPath(newZonePos)))
                {
                    zoneTable.Add(newZonePos, new Zone());
                    zoneTable[newZonePos].tileMap = LoadMap(newZonePos);

                    zonePos = newZonePos;
                    zone = zoneTable[zonePos];
                }

                for (int i = 0; i < colonists.Count; i++)
                {
                    if (direction.X == 1 || direction.X == -1)
                    {
                        float x = direction.X == 1 ? 0 : (ZoneMap.Size.X - 1) * (tileSet.textureSize.X);
                        float y = (ZoneMap.Size.Y / 2) * (tileSet.textureSize.Y)
                            + (i * colonists[i].Size.Y) + (i * tileSet.textureSize.Y)
                            - ((colonists.Count / 2) * colonists[i].Size.Y);

                        colonists[i].Position = new Vector2(x, y);
                    }
                    else if (direction.Y == -1 || direction.Y == 1)
                    {
                        float x = (ZoneMap.Size.X / 2) * (tileSet.textureSize.X)
                            + (i * colonists[i].Size.X) + (i * tileSet.textureSize.X)
                            - ((colonists.Count / 2) * colonists[i].Size.X);
                        float y = direction.Y == -1 ? (ZoneMap.Size.Y - 2) * (tileSet.textureSize.Y) : 0;

                        colonists[i].Position = new Vector2(x, y);
                    }

                    GameObjectManager.Add(colonists[i]);
                }

                ZoneManager.CurrentZone.TileMap = ZoneMap;
            }
        }

        public static List<GameObject> Objects { get => zone.GameObjects.ToList(); }
        public static List<IUpdatable> Updatable { get => zone.Updatables.ToList(); }
        public static List<IDrawable> Drawable { get => zone.Drawables.ToList(); }
        public static List<IClickable> Clickable { get => zone.Clickables.ToList(); }
        public static List<IInteractable> Interactable { get => zone.Interactables.ToList(); }
        public static List<IHeatable> Buildable { get => _buildable.ToList(); }

        /// <summary>
        /// Adds a GameObject
        /// </summary>
        /// <param name="gameObject">The GameObject to be Added</param>
        public static void Add(GameObject gameObject)
        {
            zone.GameObjects.Add(gameObject);

            if (gameObject is IDrawable d)
                zone.Drawables.Add(d);

            if (gameObject is IUpdatable u)
                zone.Updatables.Add(u);

            if (gameObject is IClickable c)
                zone.Clickables.Add(c);

            if (gameObject is IInteractable i)
                zone.Interactables.Add(i);
                zone.Interactables.Add(i);
        }

        /// <summary>
        /// Removes a GameObject
        /// </summary>
        /// <param name="gameObject">The GameObject to be removed</param>
        public static void Remove(GameObject gameObject)
        {
            zone.GameObjects.Remove(gameObject);

            if (gameObject is IDrawable d)
                zone.Drawables.Remove(d);

            if (gameObject is IUpdatable u)
                zone.Updatables.Remove(u);

            if (gameObject is IClickable c)
                zone.Clickables.Remove(c);

            if (gameObject is IInteractable i)
                zone.Interactables.Remove(i);
                zone.Interactables.Remove(i);
        }

        /// <summary>
        /// Returns all GameObjects of a specified Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Filter<T>()
        {
            return zone.GameObjects.OfType<T>().ToList();
        }

        /// <summary>
        /// Retrieves a list of GameObjects with a given tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>GameObjects with the specified tag</returns>
        public static List<GameObject> GetObjectsByTag(string tag)
        {
            List<GameObject> go = new List<GameObject>();

            foreach(GameObject o in zone.GameObjects)
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
