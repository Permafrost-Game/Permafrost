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
    /// This event will spawn bears at the one edge of the map and have them
    /// randomly attack the closest colonists in the map.
    /// </summary>
    public class EventBearAttack : IEvent
    {
        public bool Complete { get; private set; } = false;

        private readonly TileMap eventTileMap;

        public EventBearAttack(TileMap tileMap)
        {
            eventTileMap = tileMap;
        }

        public void Trigger()
        {
            int numBears = EventManager.rand.Next(2, 4);

            for (int i = 0; i < numBears; i++)
            {
                //Bear spawn location
                Vector2 eventSpawnLocation = EventManager.UtilityRandomEdgeSpawnLocation();

                //Skip bears who spawn in water
                if (!eventTileMap.GetTileAtPosition(eventSpawnLocation).Walkable)
                {
                    continue;
                }

                //Spawn bear
                Bear bear = (Bear)InteractablesFactory.MakeInteractable(Interactable.Bear, eventSpawnLocation);
                GameObjectManager.Add(bear);

                //Event bear with half the map as aggro range
                bear.AggroRange = 1600;
            }
            Complete = true;
        }

        public void UpdateTrigger(GameTime gameTime)
        {
            //Not used
        }
    }
}
