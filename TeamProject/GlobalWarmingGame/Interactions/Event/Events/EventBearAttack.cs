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
    /// randomly attack colonists in the map.
    /// </summary>
    public class EventBearAttack : IEvent
    {
        private readonly List<Colonist> eventColonists;
        private readonly TileMap eventTileMap;

        public bool Complete { get; private set; } = false;

        public EventBearAttack(TileMap tileMap, List<Colonist> colonists)
        {
            eventColonists = colonists;
            eventTileMap = tileMap;
        }

        public void Trigger()
        {
            int numBears = EventManager.rand.Next(3, 6);

            //Bear group spawn location
            Vector2 eventSpawnLocation = EventManager.RandomEdgeSpawnLocation();

            for (int i = 0; i < numBears; i++)
            {

                //Skip bears who spawn in water
                if (!eventTileMap.GetTileAtPosition(eventSpawnLocation).Walkable)
                {
                    continue;
                }

                //Spawn bear
                Bear bear = (Bear)InteractablesFactory.MakeInteractable(Interactable.Bear, eventSpawnLocation);
                GameObjectManager.Add(bear);

                //Have the bear move towards a random colonist
                Colonist[] colonists = eventColonists.ToArray();

                if (colonists.Length > 0)
                {
                    Colonist randomColonist = (Colonist)colonists.GetValue(EventManager.rand.Next(0, colonists.Length));
                    bear.Goals.Enqueue(new Vector2(randomColonist.Position.X, randomColonist.Position.Y));
                }
            }
            Complete = true;
        }

        public void UpdateTrigger(GameTime gameTime)
        {
            //Not used
        }
    }
}
