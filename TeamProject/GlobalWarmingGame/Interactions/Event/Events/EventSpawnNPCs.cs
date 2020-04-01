using Engine;
using Engine.TileGrid;
using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.Interactions.Interactables;
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
    /// This event will spawn NPCs at the edges of the current map and that are either hostile and attack
    /// colonists or are passive.
    /// </summary>
    public class EventSpawnNPCs : IEvent
    {
        public bool IsComplete { get; private set; }
        public string Description { get; }

        private readonly Interactable npcEnum;
        private readonly bool peaceful;
        private readonly bool groupedUp;

        private readonly int minimumNumNPCs = 1;
        private readonly int maximumNumNPCs;

        private readonly int requiredNumColonists;

        //Multiply aggro range for (not)peaceful NPCs
        private readonly int aggroMultiplier;

        private readonly TileMap eventTileMap;

        public EventSpawnNPCs(string description, TileMap tileMap, Interactable npc, int maximumNumNPCs, int requiredNumColonists, bool peaceful = false, bool groupedUp = false, int aggroMultiplier = 8)
        {
            Description = description;
            npcEnum = npc;
            eventTileMap = tileMap;
            this.maximumNumNPCs = maximumNumNPCs;
            this.aggroMultiplier = aggroMultiplier;
            this.requiredNumColonists = requiredNumColonists;
            this.peaceful = peaceful;
            this.groupedUp = groupedUp;
        }

        public bool TriggerEvent()
        {
            bool triggered = false;
            int numNPCs = EventManager.rand.Next(minimumNumNPCs, maximumNumNPCs);

            if (requiredNumColonists <= GameObjectManager.Filter<Colonist>().Count())
            {
                //NPC spawn location
                Vector2 eventSpawnLocation = EventManager.UtilityRandomEdgeSpawnLocation();

                for (int i = 0; i < numNPCs; i++)
                {
                    //If they aren't grouped up set a new event spawn location
                    if (!groupedUp)
                    {
                        eventSpawnLocation = EventManager.UtilityRandomEdgeSpawnLocation();
                    }

                    //Skip NPCs who spawn in water
                    if (!eventTileMap.GetTileAtPosition(eventSpawnLocation).Walkable)
                    {
                        continue;
                    }

                    //Spawn NPC
                    GameObject npc = (GameObject)InteractablesFactory.MakeInteractable(npcEnum, eventSpawnLocation);
                    GameObjectManager.Add(npc);

                    if (!peaceful)
                    {
                        //Event NPC with a 900% increase in aggro range
                        ((Enemy)npc).AggroRange *= aggroMultiplier;
                    }

                    //A NPC has spawned and now the event counts as triggered
                    triggered = true;
                }
            }
            IsComplete = true;

            return triggered;
        }

        public void UpdateEvent(GameTime gameTime)
        {
            //Not used
        }
    }
}
