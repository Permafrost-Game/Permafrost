using Engine.TileGrid;
using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Animals;
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
    /// This event will spawn a merchant at the edge of the map
    /// </summary>
    public class EventMerchant : IEvent
    {
        private readonly TileMap eventTileMap;

        public bool Complete { get; private set; } = false;

        private Merchant merchant;

        private float timeToMerchantLeave = 60000f;

        public EventMerchant(TileMap tileMap)
        {
            eventTileMap = tileMap;
        }

        public void Trigger()
        {
            //Merchant spawn location
            Vector2 eventSpawnLocation = EventManager.RandomEdgeSpawnLocation();

            //If Merchant doesn't spawn water
            if (!eventTileMap.GetTileAtPosition(eventSpawnLocation).Walkable)
            {
                Complete = true;
            }
            else
            {
                //Spawn Merchant
                merchant = (Merchant)InteractablesFactory.MakeInteractable(Interactable.Merchant, eventSpawnLocation);
                GameObjectManager.Add(merchant);
            }

        }

        /// <summary>
        /// Merchant should leave after 60 second
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateTrigger(GameTime gameTime)
        {
            timeToMerchantLeave -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeToMerchantLeave < 0)
            {
                GameObjectManager.Remove(merchant);
                Complete = true;
            }
        }
    }
}
