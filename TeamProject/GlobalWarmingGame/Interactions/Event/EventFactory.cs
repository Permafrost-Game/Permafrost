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
                    return new EventRobotSiege("A force of robots are trying to take the tower.", GameObjectManager.ZoneMap);
                case Event.BearAttack:
                    return new EventBearAttack("Some angry bears are nearby.", GameObjectManager.ZoneMap);
                case Event.Rabbit:
                    return new EventRabbit("Some rabbits are wondering nearby.", GameObjectManager.ZoneMap);
                case Event.Colonist:
                    return new EventColonist("A colonist has joined.", GameObjectManager.ZoneMap);
                case Event.Merchant:
                    return new EventMerchant("A merchant has arrived.", GameObjectManager.ZoneMap);
                default:
                    throw new NotImplementedException(eventEnum + " has not been implemented");
            }
        }
    }

    public enum Event
    {
        RobotSiege,
        BearAttack,
        Colonist,
        Merchant,
        Rabbit
    }
}