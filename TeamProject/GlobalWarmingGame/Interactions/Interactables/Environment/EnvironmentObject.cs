using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Environment
{
    public class EnvironmentObject : Sprite, IInteractable
    {
        public List<InstructionType> InstructionTypes { get; set; }

        public EnvironmentObject(Vector2 position, TextureTypes defaultTextureType) : base
        (
            position: position,
            size: new Vector2(Textures.Map[defaultTextureType].Width, Textures.Map[defaultTextureType].Height),
            rotation: 0f,
            origin: new Vector2(Textures.Map[defaultTextureType].Width / 2f, Textures.Map[defaultTextureType].Height / 2f),
            tag: "Environmental",
            texture: Textures.Map[defaultTextureType]
        )
        {
            InstructionTypes = new List<InstructionType>();
        }
    }
}
