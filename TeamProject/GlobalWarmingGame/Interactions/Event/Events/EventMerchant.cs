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
        public bool IsComplete { get; private set; }
        public string Description { get; }

        private readonly TileMap eventTileMap;
        private Vector2 merchantSpawnLocation;
        private Vector2 closestColonistLocation;
        private Merchant eventMerchant;

        private float timeToMerchantLeave = 60000f;

        //Time until: Stop merchant by closest colonist
        private float timeToStopMerchant = 500f;
        private readonly float timeUntilStopMerchant = 500f;

        private float timeToRemoveMerchant = 1000f;
        private readonly float timeUntilRemoveMerchant = 1000f;

        private bool isLeaving = false;

        public EventMerchant(string description, TileMap tileMap)
        {
            Description = description;
            eventTileMap = tileMap;
        }

        public bool TriggerEvent()
        {
            bool triggered = false;

            //Merchant spawn location
            merchantSpawnLocation = EventManager.UtilityRandomEdgeSpawnLocation();

            //If the map has colonists and If Merchant doesn't spawn in water
            if (GameObjectManager.Filter<Colonist>().Count() > 0 && eventTileMap.GetTileAtPosition(merchantSpawnLocation).Walkable)
            {
                //Spawn Merchant
                eventMerchant = (Merchant)InteractablesFactory.MakeInteractable(Interactable.Merchant, merchantSpawnLocation);
                GameObjectManager.Add(eventMerchant);

                //Move merchant to the closest colonist
                closestColonistLocation = GlobalCombatDetector.GetClosestColonist(eventMerchant.Position).Position;
                eventMerchant.Goals.Enqueue(closestColonistLocation);

                //A merchant has spawned and now the event counts as triggered
                triggered = true;
            }
            else
            {
                IsComplete = true;
            }

            return triggered;
        }

        public void UpdateEvent(GameTime gameTime)
        {
            timeToStopMerchant -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeToStopMerchant < 0)
            {
                //If the merchant is close to their spawn (roughly within two tiles)
                if (Vector2.Distance(eventMerchant.Position, closestColonistLocation) < eventTileMap.TileSize.X * 2) 
                {
                    eventMerchant.Goals.Clear();
                }
                timeToStopMerchant = timeUntilStopMerchant;
            }

            timeToMerchantLeave -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //If the merchant isn't leaving yet but its now time to leave
            if (!isLeaving && timeToMerchantLeave < 0)
            {
                eventMerchant.Goals.Clear();
                eventMerchant.Goals.Enqueue(merchantSpawnLocation);

                //Merchant is leaving and no longer trading
                eventMerchant.InstructionTypes.Clear();

                isLeaving = true;
            }

            //If merchant is leaving
            if (isLeaving)
            {
                timeToRemoveMerchant -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                //If its time to try remove the merchant
                if (timeToRemoveMerchant < 0) 
                {
                    //If the merchant is close to their spawn (roughly within two tiles)
                    if (Vector2.Distance(eventMerchant.Position, merchantSpawnLocation) < eventTileMap.TileSize.X * 2)
                    {
                        //Remove merchant and set this event to complete
                        GameObjectManager.Remove(eventMerchant);
                        IsComplete = true;
                    }
                    timeToRemoveMerchant = timeUntilRemoveMerchant;
                }
            }
        }
    }
}
