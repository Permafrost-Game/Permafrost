using Engine;
using GlobalWarmingGame.Action;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    class Building : Sprite, IUpdatable
    {

        private List<InstructionType> InstructionTypes { get; }


        public Building(Vector2 position, Texture2D texture, List<InstructionType> instructionTypes) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: position,
            tag: "Building",
            depth: 1f,
            texture: texture
        )
        {
            InstructionTypes = instructionTypes;
        }

        private void AddAction(InstructionType action)
        {
            InstructionTypes.Add(action);
        }

        public void Update()
        {
            
        }
    }
}
