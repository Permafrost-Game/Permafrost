using Engine.TileGrid;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Animals;
using GlobalWarmingGame.Interactions.Interactables.Enemies;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Event.Events
{
    /// <summary>
    /// This event will spawn a merchant at the edge of the map.
    /// The merchant to walk to the closest colonist and then wait for 60s.
    /// After waiting the merchant will leave the map.
    /// </summary>
    public class EventMerchant : IEvent
    {
        public bool Complete { get; private set; } = false;

        private readonly TileMap eventTileMap;
        private Vector2 merchantSpawnLocation;
        private Merchant eventMerchant;

        private float timeToMerchantLeave = 60000f;

        private float timeToRemoveMerchant = 1000f;
        private readonly float timeUntilRemoveMerchant = 1000f;

        private bool merchantLeaving = false;

        public EventMerchant(TileMap tileMap)
        {
            eventTileMap = tileMap;
        }

        public void Trigger()
        {
            //If the map has colonists
            if (GameObjectManager.Filter<Colonist>().Count > 0)
            {
                //Merchant spawn location
                merchantSpawnLocation = EventManager.UtilityRandomEdgeSpawnLocation();

                //If Merchant spawns in water end the event
                if (!eventTileMap.GetTileAtPosition(merchantSpawnLocation).Walkable)
                {
                    Complete = true;
                }
                else
                {
                    //Spawn Merchant
                    eventMerchant = (Merchant)InteractablesFactory.MakeInteractable(Interactable.Merchant, merchantSpawnLocation);
                    GameObjectManager.Add(eventMerchant);

                    //Move merchant to the closest colonist
                    eventMerchant.Goals.Enqueue(EventManager.UtilityFindClosestColonist(eventMerchant.Position).Position);
                }
            }
            else 
            {
                Complete = true;
            }
        }

        public void UpdateTrigger(GameTime gameTime)
        {
            timeToMerchantLeave -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //If the merchant isn't leaving yet but its now time to leave
            if (!merchantLeaving && timeToMerchantLeave < 0)
            {
                eventMerchant.Goals.Clear();
                eventMerchant.Goals.Enqueue(merchantSpawnLocation);

                //Merchant is leaving and no longer trading
                eventMerchant.InstructionTypes.Clear();

                merchantLeaving = true;
            }

            timeToRemoveMerchant -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (merchantLeaving && timeToRemoveMerchant < 0)
            {
                //If merchant is leaving and the merchant is close to their spawn (within two tiles)
                if (EventManager.UtilityDistanceBetweenPosition(eventMerchant.Position, merchantSpawnLocation) < 64f)
                {
                    //Remove merchant and set this event to complete
                    GameObjectManager.Remove(eventMerchant);
                    Complete = true;
                }
                timeToRemoveMerchant = timeUntilRemoveMerchant;
            }
        }
    }
}
