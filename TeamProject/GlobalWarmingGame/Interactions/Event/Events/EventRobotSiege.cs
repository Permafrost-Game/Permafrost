using Engine.TileGrid;
using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
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
    public class EventRobotSiege : IEvent
    {
        private readonly TileMap eventTileMap;

        public bool Complete { get; private set; } = false;

        public EventRobotSiege(TileMap tileMap)
        {
            eventTileMap = tileMap;
        }

        public void Trigger()
        {
            //If the map has a tower 
            if (GameObjectManager.Filter<Tower>() != null) 
            {
                int numRobots = EventManager.rand.Next(3, 6);

                for (int i = 0; i < numRobots; i++)
                {
                    //Robot spawn location
                    Vector2 eventSpawnLocation = EventManager.UtilityRandomEdgeSpawnLocation();

                    //Skip robots who spawn in water
                    if (!eventTileMap.GetTileAtPosition(eventSpawnLocation).Walkable)
                    {
                        continue;
                    }

                    //Spawn robot
                    Robot robot = (Robot)InteractablesFactory.MakeInteractable(Interactable.Robot, eventSpawnLocation);
                    GameObjectManager.Add(robot);

                    //Event robot with half map as aggro range
                    robot.AggroRange = 1600;

                    //robot.Goals.Enqueue(new Vector2(randomColonist.Position.X, randomColonist.Position.Y));                    
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
