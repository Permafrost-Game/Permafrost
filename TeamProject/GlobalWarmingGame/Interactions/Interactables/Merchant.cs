
using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Animals
{
    public class Merchant : PassiveAnimal
    {
        private static readonly RandomAI MerchantAI = new RandomAI(63f, 64f);

        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        public readonly int textureSetID;
        
        public Merchant(Vector2 position, TextureSetTypes textureSetType = TextureSetTypes.colonist) : base
        (
            position, "Merchant", Textures.MapSet[textureSetType], 0.05f, MerchantAI, MerchantAI.MoveDistance * 2
        )
        {
            textureSetID = (int)textureSetType;

            InstructionTypes.Add(
                new InstructionType("TradeStone", "TradeStone", "Trade food for Stone", 0, new List<ResourceItem>() { new ResourceItem(Resource.Food, 1) }, onComplete: Trade)
            );
        }

        public void Trade(Instruction instruction)
        {
            Resource resource = (Resource)Enum.Parse(typeof(Resource), instruction.Type.ID);
            Colonist colonist = (Colonist)instruction.ActiveMember;

            if (colonist.Inventory.ContainsAll(instruction.Type.RequiredResources))
            {
                colonist.Inventory.AddItem(new ResourceItem(resource, 1));
                //colonist.Inventory.RemoveItem(item);
            }
        }
    }
}
