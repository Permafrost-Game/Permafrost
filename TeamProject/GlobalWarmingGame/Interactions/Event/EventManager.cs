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

        //A enum list of all events
        private static readonly Event[] eventEnums = (Event[])Enum.GetValues(typeof(Event));

        //Random number generator based off the seed
        private static readonly List<IEvent> activeEvents = new List<IEvent>();

        private static float timeToEvent = 60000f;
        private static readonly float timeUntilEvent = 120000f;

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
                //Picks a random event enum from events
                Event randomEventEnum = (Event)eventEnums.GetValue(rand.Next(0, eventEnums.Length));

                CreateGameEvent(randomEventEnum);

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
        /// Create an event, trigger it and add it to the active events if the event has a duration or condition.
        /// </summary>
        /// <param name="eventEnum"></param>
        public static void CreateGameEvent(Event eventEnum) 
        {
            IEvent randomEvent = EventFactory.CreateEvent(eventEnum);

            randomEvent.Trigger();
            if (!randomEvent.Complete)
            {
                activeEvents.Add(randomEvent);
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
                //Decreased max y (3200) by two tiles to 3136 so that when entities spawn at the bottom their sprites are fully in the map
                case 0:
                    //north edge
                    location = new Vector2(rand.Next(0, 3136), 0);
                    break;
                case 1:
                    //west edge
                    location = new Vector2(0, rand.Next(0, 3136));
                    break;
                case 2:
                    //south edge
                    location = new Vector2(rand.Next(0, 3136), 3136);
                    break;
                case 3:
                    //east edge
                    location = new Vector2(3168, rand.Next(0, 3136));
                    break;
            }
            return location;
        }
    }
}
