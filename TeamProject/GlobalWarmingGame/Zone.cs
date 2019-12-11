using Engine;
using Engine.TileGrid;
using Engine.Drawing;
using GlobalWarmingGame.Interactions;
using System.Collections.Generic;

namespace PermaFrost
{
    class Zone
    {
        public List<GameObject> GameObjects { get; }
        public List<IUpdatable> Updatables { get; }
        public List<IDrawable> Drawables { get; }
        public List<IClickable> Clickables { get; }
        public List<IInteractable> Interactables { get; }

        public TileMap tileMap;

        public Zone()
        {
            GameObjects = new List<GameObject>();
            Updatables = new List<IUpdatable>();
            Drawables = new List<IDrawable>();
            Clickables = new List<IClickable>();
            Interactables = new List<IInteractable>();
        }
    }
}