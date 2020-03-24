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
    /// This event will spawn robots that will try to capture a tower in the map if present
    /// </summary>
    public class EventRobotAttack : IEvent
    {
        private readonly List<Colonist> eventColonists;
        private readonly TileMap eventTileMap;

        public bool Complete { get; private set; } = false;

        public EventRobotAttack(TileMap tileMap, List<Colonist> colonists)
        {
            eventColonists = colonists;
            eventTileMap = tileMap;
        }

        public void Trigger()
        {
            int numRobots = EventManager.rand.Next(3, 6);

            for (int i = 0; i < numRobots; i++)
            {
                //Robot spawn location
                Vector2 eventSpawnLocation = EventManager.RandomEdgeSpawnLocation();

                //Skip robots who spawn in water
                if (!eventTileMap.GetTileAtPosition(eventSpawnLocation).Walkable)
                {
                    continue;
                }

                //Spawn robot
                Robot robot = (Robot)InteractablesFactory.MakeInteractable(Interactable.Robot, eventSpawnLocation);
                GameObjectManager.Add(robot);

                //Have the robot move towards a random colonist
                Colonist[] colonists = eventColonists.ToArray();

                if (colonists.Length > 0)
                {
                    Colonist randomColonist = (Colonist)colonists.GetValue(EventManager.rand.Next(0, colonists.Length));
                    robot.Goals.Enqueue(new Vector2(randomColonist.Position.X, randomColonist.Position.Y));
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
