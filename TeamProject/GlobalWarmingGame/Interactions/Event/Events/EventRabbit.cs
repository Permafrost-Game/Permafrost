using Engine.TileGrid;
using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Animals;
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
    /// This event will spawn rabbits at the one edge of the map 
    /// </summary>
    public class EventRabbit : IEvent
    {
        public bool IsComplete { get; private set; }

        private readonly TileMap eventTileMap;

        public EventRabbit(TileMap tileMap)
        {
            eventTileMap = tileMap;
        }

        public void TriggerEvent()
        {
            int numRabbits = EventManager.rand.Next(4, 6);

            //Rabbit group spawn location
            Vector2 eventSpawnLocation = EventManager.UtilityRandomEdgeSpawnLocation();

            for (int i = 0; i < numRabbits; i++)
            {

                //Skip rabbits who spawn in water
                if (!eventTileMap.GetTileAtPosition(eventSpawnLocation).Walkable)
                {
                    continue;
                }

                //Spawn rabbit
                Rabbit rabbit = (Rabbit)InteractablesFactory.MakeInteractable(Interactable.Rabbit, eventSpawnLocation);
                GameObjectManager.Add(rabbit);
            }
            IsComplete = true;
        }

        public void UpdateEvent(GameTime gameTime)
        {
            //Not used
        }
    }
}
