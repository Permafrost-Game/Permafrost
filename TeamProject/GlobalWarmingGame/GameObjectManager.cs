﻿using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using Engine.TileGrid;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Environment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
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
        private static bool serialization;
        private static IDictionary<Vector2, List<GameObject>> zoneMap;

        public static int seed;
        public static TileSet TileSet { get; set; }

        static Vector2 zonePos;

        private static /* readonly */ List<GameObject> gameObjects;

        public static event EventHandler<GameObject> ObjectAdded = delegate { };
        public static event EventHandler<GameObject> ObjectRemoved = delegate { };

        public static GraphicsDevice GraphicsDevice { get; set; }
        public static Camera Camera { get; private set; }

        public static TileMap ZoneMap { get; set; }

        private static int saveID;

        public static SpriteBatch SpriteBatch { get; set; }

        public static Vector2 GreyTilesSize { get; private set; }
        public static RenderTarget2D GreyTiles { get; private set; }

        static GameObjectManager()
        {
            gameObjects = new List<GameObject>();
            Updatables = new List<Engine.IUpdatable>();
            Drawables = new List<IDrawable>();
            Interactables = new List<IInteractable>();
        }

        public static void Init(int currentSaveID, int worldSeed, Vector2 currentZone, bool isSerialized = true)
        {
            saveID = currentSaveID;

            if (!(serialization = isSerialized))
            {
                currentZone = Vector2.Zero;
                zoneMap = new Dictionary<Vector2, List<GameObject>>();
            }

            seed = worldSeed;
            zonePos = currentZone;

            SetZone(zonePos);

            Camera = new Camera(GraphicsDevice.Viewport, GameObjectManager.ZoneMap.Size * GameObjectManager.ZoneMap.Tiles[0, 0].Size);
        }

        public static string ZoneFileName()
        {
            return String.Format("{0},{1}", zonePos.X, zonePos.Y);
        }

        public static string ZoneFilePath()
        {
            return String.Format(@"Content/saves/{0}/zones/{1}.json", saveID, ZoneFileName());
        }

        public static void SaveZone()
        {
            if (!serialization)
            {
                zoneMap[zonePos] = gameObjects.ToList();
            }
            else
            {
                Console.WriteLine("Saving to " + ZoneFilePath());
                Serializer.Serialize(ZoneFilePath(), gameObjects);
            }
        }

        public static TileMap GenerateMap(Vector2 pos)
        {
            return TileMapGenrator.GenerateTileMap(seed: seed, scale: 0.005f, xOffset: (int)pos.X * 99, yOffset: (int)pos.Y * 99, width: 100, height: 100, TileSet, TemperatureManager.GlobalTemperature.Value);
        }

        private static void SetZone(Vector2 position, List<Colonist> colonists = null)
        {

            zonePos = position;
            ZoneMap = GenerateMap(position);
            PathFinder.TileMap = ZoneMap;

            gameObjects.Clear();
            Updatables.Clear();
            Drawables.Clear();
            Interactables.Clear();

            if (colonists != null)
                foreach (Colonist colonist in colonists)
                    Add(colonist);
                    

            if (!serialization)
            {
                if (zoneMap.ContainsKey(zonePos))
                {
                    gameObjects = zoneMap[zonePos];
                    if (colonists != null)
                        foreach (Colonist colonist in colonists)
                            Add(colonist);
                    Updatables = Filter<Engine.IUpdatable>();
                    Drawables = Filter<IDrawable>();
                    Interactables = Filter<IInteractable>();
                }
                else
                {

                    ZoneGenerator.SpawnGameObjects(seed, zonePos);
                    zoneMap.Add(zonePos, gameObjects);

                    if (position == Vector2.Zero)
                        Add((Colonist)InteractablesFactory.MakeInteractable(Interactable.Colonist, position: ZoneMap.Size * ZoneMap.Tiles[0, 0].Size / 2));
                }
            }

            else
            {
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

                    ZoneGenerator.SpawnGameObjects(seed, zonePos);

                    if (position == Vector2.Zero)
                        Add((Colonist)InteractablesFactory.MakeInteractable(Interactable.Colonist, position: ZoneMap.Size * ZoneMap.Tiles[0, 0].Size / 2));

                    SaveZone();
                }
            }

            GreyTilesSize = ZoneMap.Size * ZoneMap.TileSize * 3;

            GreyTiles = new RenderTarget2D(
                GraphicsDevice,
                (int)GreyTilesSize.X,
                (int)GreyTilesSize.Y,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            GraphicsDevice.SetRenderTarget(GreyTiles);
            GraphicsDevice.Clear(Color.Transparent);

            Vector2[] greyZonePositions =
            {
                new Vector2(-1,  0), // left
                new Vector2( 1,  0), // right
                new Vector2( 0, -1), // up
                new Vector2( 0,  1), // down
                new Vector2( 1, -1), // top right
                new Vector2(-1, -1), // top left
                new Vector2( 1,  1), // top right
                new Vector2(-1,  1) // top left
            };

            SpriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.Opaque,
                samplerState: SamplerState.PointClamp,
                depthStencilState: null,
                rasterizerState: null,
                effect: null
            );

            foreach (Vector2 greyZonePos in greyZonePositions)
            {
                TileMap greyTileMap = GenerateMap(new Vector2(greyZonePos.X + zonePos.X, greyZonePos.Y + zonePos.Y));

                foreach (Tile tile in greyTileMap.Tiles)
                {
                    Vector2 offset = new Vector2(greyZonePos.X + 1, greyZonePos.Y + 1);
                    Vector2 newPosition = new Vector2(tile.Position.X + (greyTileMap.Size.X * tile.Size.X * offset.X), tile.Position.Y + (greyTileMap.Size.Y * tile.Size.Y * offset.Y));

                    Tile offsettedTile = new Tile(tile.texture, newPosition, tile.Size, false, 0f);
                    offsettedTile.Draw(SpriteBatch);
                }
            }

            SpriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
        }

        public static void MoveZone(Vector2 direction)
        {
            List<Colonist> colonists = GameObjectManager.Filter<Colonist>().ToList();

            foreach (Colonist colonist in colonists)
                Remove(colonist);

            SaveZone();

            for (int i = 0; i < colonists.Count(); i++)
            {
                Colonist colonist = colonists[i];
                colonist.ClearInstructions();

                if (direction.X == 1 || direction.X == -1)
                {
                    float x = direction.X == 1 ? 0 : (ZoneMap.Size.X - 1) * (TileSet.textureSize.X);
                    float y = (ZoneMap.Size.Y / 2) * (TileSet.textureSize.Y)
                        + (i * colonist.Size.Y) + (i * TileSet.textureSize.Y)
                        - ((colonists.Count / 2) * colonist.Size.Y);

                    colonist.Position = new Vector2(x, y);
                }
                else if (direction.Y == -1 || direction.Y == 1)
                {
                    float x = (ZoneMap.Size.X / 2) * (TileSet.textureSize.X)
                        + (i * colonist.Size.X) + (i * TileSet.textureSize.X)
                        - ((colonists.Count / 2) * colonist.Size.X);
                    float y = direction.Y == -1 ? (ZoneMap.Size.Y - 2) * (TileSet.textureSize.Y) : 0;

                    colonist.Position = new Vector2(x, y);
                }
            }

            SetZone(zonePos + direction, colonists);

            if (direction.X == 1)
                Camera.Position = new Vector2(ZoneMap.Size.X * TileSet.textureSize.X, (ZoneMap.Size.Y * TileSet.textureSize.Y) / 2);

            else if (direction.X == -1)
                Camera.Position = new Vector2(0, (ZoneMap.Size.Y * TileSet.textureSize.Y) / 2);

            else if (direction.Y == -1)
                Camera.Position = new Vector2((ZoneMap.Size.Y * TileSet.textureSize.Y) / 2, 0);

            else if (direction.Y == 1)
                Camera.Position = new Vector2((ZoneMap.Size.Y * TileSet.textureSize.Y) / 2, ZoneMap.Size.Y * TileSet.textureSize.Y);
        }

        public static List<GameObject> Objects { get => gameObjects.ToList(); }
        public static List<Engine.IUpdatable> Updatables { get; private set; } = new List<Engine.IUpdatable>();
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

            if (gameObject is Engine.IUpdatable u)
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

            if (gameObject is Engine.IUpdatable u)
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
        [Obsolete]
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
