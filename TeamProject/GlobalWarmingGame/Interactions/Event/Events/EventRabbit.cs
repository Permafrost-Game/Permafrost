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
        private readonly TileMap eventTileMap;

        public bool Complete { get; private set; } = false;

        public EventRabbit(TileMap tileMap)
        {
            eventTileMap = tileMap;
        }

        public void Trigger()
        {
            int numRabbits = EventManager.rand.Next(4, 6);

            //Rabbit group spawn location
            Vector2 eventSpawnLocation = EventManager.RandomEdgeSpawnLocation();

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
            Complete = true;
        }

        public void UpdateTrigger(GameTime gameTime)
        {
            //Not used
        }
    }
}
