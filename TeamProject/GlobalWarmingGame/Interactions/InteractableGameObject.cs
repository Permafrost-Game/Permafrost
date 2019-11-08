using Engine;
using GlobalWarmingGame.Action;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions
{
    /// <summary>
    /// This class describes an interactable object
    /// </summary>
    class InteractableGameObject : Sprite, IInteractable
    {
        public List<InstructionType> InstructionTypes { get; }
        
        public InteractableGameObject(Vector2 position, Texture2D texture, List<InstructionType> instructionTypes) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0),
            tag: "Building", //TODO rename tag
            depth: 0.5f,
            texture: texture
        )
        {
            InstructionTypes = instructionTypes;
        }


    }
}
