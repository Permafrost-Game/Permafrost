using Engine.PathFinding;
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
    /// This event will spawn robots that will rush the tower on the map and try to recapture the players tower.
    /// If timeToTowerCapture < 0 and some robots survived the tower is captured by the robots.
    /// </summary>
    public class EventRobotSiege : IEvent
    {
        public bool IsComplete { get; private set; }
        public string Description { get; }

        private readonly TileMap eventTileMap;
        private readonly List<Enemy> activeRobots;
        private Tower eventTower;

        private float timeToTowerCapture = 80000f;

        private float timeToActivateRobotAI = 1000f;
        private readonly float timeUntilActivateRobotAI = 1000f;

        public EventRobotSiege(string description, TileMap tileMap)
        {
            Description = description;
            eventTileMap = tileMap;
            activeRobots = new List<Enemy>();
        }

        public bool TriggerEvent()
        {
            bool triggered = false;

            //If the map has a tower and it is captured by the player
            if (GameObjectManager.Filter<Tower>().Count() != 0 && GameObjectManager.Filter<Tower>().First()._isCaptured)
            {
                //Number of robots is equal to the number of colonists / 1.5, this equation can be rebalanced
                int numRobots = (int)(GameObjectManager.Filter<Colonist>().Count() / 1.5);
                eventTower = GameObjectManager.Filter<Tower>().First();

                for (int i = 0; i < numRobots; i++)
                {
                    //Robot spawn location
                    Vector2 eventSpawnLocation = EventManager.UtilityRandomEdgeSpawnLocation();

                    //Skip robots who spawn in water
                    if (!eventTileMap.GetTileAtPosition(eventSpawnLocation).Walkable)
                    {
                        continue;
                    }

                    //Spawn a robot of random type ie (small or large)
                    Enemy robot;

                    int robotTypeNums = EventManager.rand.Next(0, 3);
                    switch (robotTypeNums)
                    {
                        case 0:
                        case 1:
                            robot = (Robot)InteractablesFactory.MakeInteractable(Interactable.Robot, eventSpawnLocation);
                            break;
                        case 2:
                            robot = (SmallRobot)InteractablesFactory.MakeInteractable(Interactable.SmallRobot, eventSpawnLocation);
                            break;
                        default:
                            robot = (Robot)InteractablesFactory.MakeInteractable(Interactable.Robot, eventSpawnLocation);
                            break;
                    }

                    GameObjectManager.Add(robot);
                    activeRobots.Add(robot);

                    //Event robots with 50% increased aggro range
                    robot.AggroRange *= 1.5;

                    //Disable Random Robot AI
                    robot.AI = null;

                    //Move robot towards tower
                    robot.Goals.Enqueue(new Vector2(eventTower.Position.X, eventTower.Position.Y));

                    //A robot has spawned and now the event counts as triggered
                    triggered = true;
                }
            }
            else 
            {
                IsComplete = true;
            }

            return triggered;
        }

        public void UpdateEvent(GameTime gameTime)
        {
            //Turn on the robots random AI when they are by the tower
            timeToActivateRobotAI -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeToActivateRobotAI < 0)
            {
                foreach (Enemy enemy in activeRobots)
                {
                    //If the robot is within 10 tiles of the tower
                    if (Vector2.Distance(enemy.Position, eventTower.Position) <= 320)
                    {
                        enemy.AI = new RandomAI(70, 0);
                    }
                }
                timeToActivateRobotAI = timeUntilActivateRobotAI;
            }

            //If any of the event robots are alive at this point they have won and the tower is captured.
            timeToTowerCapture -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeToTowerCapture < 0)
            {
                foreach (Enemy enemy in activeRobots)
                {
                    //If the robot is alive and within 10 tiles of the tower
                    if (enemy.Health > 0 && Vector2.Distance(enemy.Position, eventTower.Position) <= 320)
                    {
                        eventTower.ResetCapture();
                    }
                }

                IsComplete = true;
            }
        }
    }
}
