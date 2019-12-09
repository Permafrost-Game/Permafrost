using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions
{
    /// <summary>
    /// This class describes an interactable 
    /// This class is now obsolete See <see cref="IInteractable"/>
    /// </summary>
    [Obsolete]
    class InteractableGameObject : Sprite, IInteractable
    {
        public List<InstructionType> InstructionTypes { get; }

        public InteractableGameObject(Vector2 position, Texture2D texture, string tag, List<InstructionType> instructionTypes) : this
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0),
            tag: tag,
            depth: 0.5f,
            texture: texture,
            instructionTypes: instructionTypes
        ) 
        {
        }

        public InteractableGameObject(Vector2 position, Vector2 size, float rotation, Vector2 rotationOrigin, string tag, float depth, Texture2D texture, List<InstructionType> instructionTypes) : base
        (
            position: position,
            size: size,
            rotation: rotation,
            rotationOrigin: rotationOrigin,
            tag: tag,
            depth: depth,
            texture: texture
        )
        {
            InstructionTypes = instructionTypes;
        }
    }
}
