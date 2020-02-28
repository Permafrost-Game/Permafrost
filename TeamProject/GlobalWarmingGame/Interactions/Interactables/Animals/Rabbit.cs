﻿
using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Animals
{
    public class Rabbit : PassiveAnimal, IReconstructable
    {
        private static readonly RandomAI RabbitAI = new RandomAI(63f, 64f);

        [PFSerializable]
        public new Vector2 Position { get; set; }

        [PFSerializable]
        public readonly int textureSetID;

        public Rabbit() : this(Vector2.Zero, TextureSetTypes.rabbit)
        {

        }

        public Rabbit(Vector2 position, TextureSetTypes textureSetType) : base
        (
            position, "Rabbit", Textures.MapSet[textureSetType], 0.05f, RabbitAI, RabbitAI.MoveDistance * 3
        )
        {
            Position = base.Position;
            textureSetID = (int)textureSetType;

            this.InstructionTypes.Add(
                new InstructionType("hunt", "Hunt", "Hunt the Rabbit", onStart: Hunt)
            );
        }

        public void Hunt(IInstructionFollower follower)
        {
            follower.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Food), 2));
            GameObjectManager.Remove(this);
        }

        public object Reconstruct()
        {
            return new Rabbit(Position, (TextureSetTypes)textureSetID);
        }
    }
}
