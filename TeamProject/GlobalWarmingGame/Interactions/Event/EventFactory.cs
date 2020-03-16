using GlobalWarmingGame.Interactions.Event.Events;
using GlobalWarmingGame.Interactions.Interactables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Event
{
    /// <summary>
    /// A factory that can create events for clients without them having to know specifics about creation.
    /// </summary>
    public static class EventFactory
    {
        //A enum list of all events
        private static readonly Event[] events = (Event[])Enum.GetValues(typeof(Event));

        /// <summary>
        /// Create an event given a enum.
        /// </summary>
        /// <param name="eventEnum"></param>
        /// <returns>An event related to the given enum.</returns>
        public static IEvent CreateEvent(Event eventEnum) 
        {
           
            if (eventEnum == Event.Random) 
            {
                //Picks a random event from events but length is reduced by 1 to prevent event.random from being picked again
                eventEnum = (Event)events.GetValue(EventManager.rand.Next(0, events.Length - 1 ));
            }

            switch (eventEnum) 
            {
                case Event.BearAttack:
                    return new EventBearAttack(GameObjectManager.ZoneMap, GameObjectManager.Filter<Colonist>());
                case Event.ColonistJoin:
                    return new EventColonistJoin(GameObjectManager.ZoneMap);
                default:
                    throw new NotImplementedException(eventEnum + " has not been implemented");
            }
        }
    }

    public enum Event
    {
        BearAttack,
        ColonistJoin,
        Random
    }
}
