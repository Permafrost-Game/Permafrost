﻿using Engine.TileGrid;
using GlobalWarmingGame.Interactions.Event.Events;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Event
{
    /// <summary>
    /// A class that creates, updates and helps events.
    /// </summary>
    public static class EventManager
    {
        //Utility random number generator for all events
        public static readonly Random rand = new Random(GameObjectManager.seed);

        //Turn random events on and off
        public static bool RandomEvents { get; set; } = true;

        //A enum list of all events
        private static readonly Event[] eventEnums = (Event[])Enum.GetValues(typeof(Event));

        //Random number generator based off the seed
        private static readonly List<IEvent> activeEvents = new List<IEvent>();

        private static float timeToRandomEvent = 80000f;
        private static readonly float timeUntilRandomEvent = 120000f;

        /// <summary>
        /// A method in the game's update loop that is called every frame
        /// and triggers an event after a fixed period of time.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void UpdateEventTime(GameTime gameTime)
        {
            if (RandomEvents)
            {
                timeToRandomEvent -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                //If its time for a new event create a new random event, trigger it and add it to the active events list
                if (timeToRandomEvent < 0f)
                {
                    //Picks a random event enum from events
                    Event randomEventEnum = (Event)eventEnums.GetValue(rand.Next(0, eventEnums.Length));

                    CreateGameEvent(randomEventEnum);

                    timeToRandomEvent = timeUntilRandomEvent;
                }
            }

            //Loop through all active events and call their update trigger
            foreach (IEvent evnt in activeEvents.ToArray())
            {
                evnt.UpdateEvent(gameTime);
                if (evnt.IsComplete)
                {
                    activeEvents.Remove(evnt);
                }
            }
        }

        /// <summary>
        /// Create an event, trigger it and add it to the active events if the event is not complete yet.
        /// </summary>
        /// <param name="eventEnum"></param>
        public static void CreateGameEvent(Event eventEnum)
        {
            IEvent randomEvent = EventFactory.CreateEvent(eventEnum);

            randomEvent.TriggerEvent();
            if (!randomEvent.IsComplete)
            {
                activeEvents.Add(randomEvent);
            }
        }

        #region Event Utility Methods
        /// <summary>
        /// Utility method for events to call that will give them a random position at a edge
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Vector2 UtilityRandomEdgeSpawnLocation()
        {
            Vector2 location = new Vector2();
            switch (rand.Next(0, 4))
            {
                //Decreased max range (3200) by two tiles to 3136
                //Increased minimum range (0) by two tiles 64
                //Assures that textures spawn fully in map
                case 0:
                    //north edge
                    location = new Vector2(rand.Next(64, 3136), 64);
                    break;
                case 1:
                    //west edge
                    location = new Vector2(64, rand.Next(64, 3136));
                    break;
                case 2:
                    //south edge
                    location = new Vector2(rand.Next(64, 3136), 3136);
                    break;
                case 3:
                    //east edge
                    location = new Vector2(3136, rand.Next(64, 3136));
                    break;
            }
            return location;
        }
        #endregion
    }
}