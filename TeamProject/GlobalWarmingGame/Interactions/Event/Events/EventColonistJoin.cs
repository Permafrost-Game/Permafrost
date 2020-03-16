using Engine.TileGrid;
using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Event.Events
{
    /// <summary>
    /// This event will spawn add a colonist to the players colony.
    /// </summary>
    public class EventColonistJoin : IEvent
    {
        public bool Complete { get; private set; } = false;
        private readonly TileMap eventTileMap;

        public EventColonistJoin(TileMap tileMap)
        {
            eventTileMap = tileMap;
        }

        public void Trigger()
        {
            //Create a new colonist at the edge of the map
            Vector2 colonistSpawnLocation = EventManager.RandomEdgeSpawnLocation();

            if (!eventTileMap.GetTileAtPosition(colonistSpawnLocation).Walkable)
            {
                Colonist newColonist = (Colonist)InteractablesFactory.MakeInteractable(Interactable.Colonist, colonistSpawnLocation);
                GameObjectManager.Add(newColonist);
            }

            Complete = true;
        }

        public void UpdateTrigger(GameTime gameTime)
        {
        }
    }
}
