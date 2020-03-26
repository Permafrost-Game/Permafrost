﻿using GlobalWarmingGame.Interactions.Event.Events;
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
                case Event.RobotSiege:
                    return new EventRobotSiege(GameObjectManager.ZoneMap);
                case Event.BearAttack:
                    return new EventBearAttack(GameObjectManager.ZoneMap);
                case Event.Rabbit:
                    return new EventRabbit(GameObjectManager.ZoneMap);
                case Event.ColonistJoin:
                    return new EventColonistJoin(GameObjectManager.ZoneMap);
                case Event.Merchant:
                    return new EventMerchant(GameObjectManager.ZoneMap);
                default:
                    throw new NotImplementedException(eventEnum + " has not been implemented");
            }
        }
    }

    public enum Event
    {
        RobotSiege,
        BearAttack,
        ColonistJoin,
        Merchant,
        Rabbit
    }
}
