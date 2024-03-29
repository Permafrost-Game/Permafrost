﻿using Microsoft.Xna.Framework;

namespace GlobalWarmingGame.Interactions.Event.Events
{
    public interface IEvent
    {
        /// <summary>
        /// Description for the event.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Set Complete to true when the event has finished.
        /// This can happen as an event is triggered or
        /// after an event has triggered in the UpdateEvent loop.
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// Trigger the event and execute a function
        /// </summary>
        bool TriggerEvent();

        /// <summary>
        /// Update elements of a ongoing event.
        /// Becareful as it updates on every frame.
        /// </summary>
        void UpdateEvent(GameTime gameTime);
    }
}
