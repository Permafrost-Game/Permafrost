﻿using Engine.TileGrid;
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
    /// This event will spawn add a colonist to the players colony.
    /// </summary>
    public class EventColonist : IEvent
    {
        public bool IsComplete { get; private set; } = false;

        private readonly TileMap eventTileMap;

        public EventColonist(TileMap tileMap)
        {
            eventTileMap = tileMap;
        }

        public void TriggerEvent()
        {
            //Create a new colonist at the edge of the map
            Vector2 colonistSpawnLocation = EventManager.UtilityRandomEdgeSpawnLocation();

            if (eventTileMap.GetTileAtPosition(colonistSpawnLocation).Walkable)
            {
                Colonist newColonist = (Colonist)InteractablesFactory.MakeInteractable(Interactable.Colonist, colonistSpawnLocation);
                GameObjectManager.Add(newColonist);
            }

            IsComplete = true;
        }

        public void UpdateEvent(GameTime gameTime)
        {
            //Not used
        }
    }
}