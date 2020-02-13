using Engine;
using Engine.TileGrid;
using Engine.Drawing;
using GlobalWarmingGame.Interactions;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    class Zone
    {
        public List<GameObject> GameObjects { get; }
        public List<IUpdatable> Updatables { get; }
        public List<IDrawable> Drawables { get; }
        public List<IInteractable> Interactables { get; }

        public Zone()
        {
            GameObjects = new List<GameObject>();
            Updatables = new List<IUpdatable>();
            Drawables = new List<IDrawable>();
            Interactables = new List<IInteractable>();
        }
    }
}