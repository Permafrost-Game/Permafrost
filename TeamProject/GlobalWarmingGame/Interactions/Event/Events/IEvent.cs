using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Event.Events
{
    public interface IEvent
    {
        /// <summary>
        /// Set Complete to true when the event has finished.
        /// This can happen as an event is triggered or
        /// after an event has triggered in the UpdateTrigger loop.
        /// </summary>
        bool Complete { get; }

        /// <summary>
        /// Trigger the event and execute a function
        /// </summary>
        void Trigger();

        /// <summary>
        /// Update elementss of a ongoing event
        /// </summary>
        void UpdateTrigger(GameTime gameTime);
    }
}
