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
        /// <summary>
        /// Create an event given a enum.
        /// </summary>
        /// <param name="eventEnum"></param>
        /// <returns>An event related to the given enum.</returns>
        public static IEvent CreateEvent(Event eventEnum) 
        {
            switch (eventEnum)
            {
                case Event.RobotAttack:
                    return new EventRobotAttack(GameObjectManager.ZoneMap, GameObjectManager.Filter<Colonist>());
                case Event.BearAttack:
                    return new EventBearAttack(GameObjectManager.ZoneMap);
                case Event.RabbitJoin:
                    return new EventRabbitJoin(GameObjectManager.ZoneMap);
                case Event.ColonistJoin:
                    return new EventColonistJoin(GameObjectManager.ZoneMap);
                default:
                    throw new NotImplementedException(eventEnum + " has not been implemented");
            }
        }
    }

    public enum Event
    {
        RobotAttack,
        BearAttack,
        ColonistJoin,
        RabbitJoin
    }
}
