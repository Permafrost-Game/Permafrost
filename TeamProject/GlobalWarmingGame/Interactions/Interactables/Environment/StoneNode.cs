
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Environment
{
    public class StoneNode : Sprite, IInteractable
    {
        public List<InstructionType> InstructionTypes { get; }

        public StoneNode(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            origin: new Vector2(texture.Width / 2f, texture.Height / 2f),
            tag: "StoneNode",
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType("mine", "Mine", "Mine stone", onStart: Mine)
            };
        }

        private void Mine(IInstructionFollower follower)
        {
            SoundFactory.PlaySoundEffect("stone_pickup");
            follower.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Stone), 4));
            //Maybe destory the node or allow 3 more mine operations
            GameObjectManager.Remove(this);
        }
    }
}
