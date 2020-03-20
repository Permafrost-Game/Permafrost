using Engine.TileGrid;
using GlobalWarmingGame.Interactions.Event.Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Event
{
    public static class EventManager
    {
        //Utility random number generator for all events
        public static readonly Random rand = new Random(GameObjectManager.seed);

        //Random number generator based off the seed
        private static readonly List<IEvent> activeEvents = new List<IEvent>();

        private static float timeToEvent = 60000f;
        private static readonly float timeUntilEvent = 60000f;

        /// <summary>
        /// A method in the game's update loop that is called every frame
        /// and triggers an event after a fixed period of time.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void UpdateEventTime(GameTime gameTime)
        {
            timeToEvent -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //If its time for a new event create a new random event, trigger it and add it to the active events list
            if (timeToEvent < 0f)
            {
                IEvent randomEvent = EventFactory.CreateEvent(Event.Random);
                randomEvent.Trigger();
                if (!randomEvent.Complete)
                {
                    activeEvents.Add(randomEvent);
                }
                timeToEvent = timeUntilEvent;
            }

            //Loop through all active events and call their update trigger
            foreach (IEvent evnt in activeEvents.ToArray()) 
            {
                evnt.UpdateTrigger(gameTime);
                if (evnt.Complete) 
                {
                    activeEvents.Remove(evnt);
                }
            }
        }

        /// <summary>
        /// Utility method for events to call that will give them a random position at a edge
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Vector2 RandomEdgeSpawnLocation()
        {
            Vector2 location = new Vector2();
            switch (rand.Next(0, 4))
            {
                case 0:
                    //north edge (adjusted by 32, 1 tile)
                    location = new Vector2(1568, 32);
                    break;
                case 1:
                    //west edge (adjusted by 32, 1 tile)
                    location = new Vector2(32, 1568);
                    break;
                case 2:
                    //south edge (adjusted by 32, 1 tile)
                    location = new Vector2(1568, 3168);
                    break;
                case 3:
                    //east edge (adjusted by 32, 1 tile)
                    location = new Vector2(3168, 1568);
                    break;
            }
            return location;
        }
    }
}
