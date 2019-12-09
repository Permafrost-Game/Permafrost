﻿
using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class Rabbit : PassiveAnimal
    {
        private static readonly RandomAI RabbitAI = new RandomAI(63f, 64f);

        public Rabbit(Vector2 position, Texture2D[][] textureSet) : base
        (
            position, "Rabbit", textureSet, 0.05f, RabbitAI, RabbitAI.MoveDistance * 3
        )
        {

            this.InstructionTypes.Add(
                new InstructionType("hunt", "Hunt", "Hunt the Rabbit", new ResourceItem(new Food(), 2), Hunt)
            );
        }

        

        public void Hunt()
        {
            //This is tempory and should be replaced by the resource system
            ((DisplayLabel)GameObjectManager.GetObjectsByTag("lblFood")[0]).Value += 2;

            GameObjectManager.Remove(this);
        }

    }
}