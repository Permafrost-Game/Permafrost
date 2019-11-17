
using Engine;
using GlobalWarmingGame.Action;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class Rabbit : PassiveMovingGameObject
    {


        public Rabbit(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "Rabbit",
            depth: 1f,
            texture: texture,
            instructionTypes: new List<InstructionType>(),
            speed: 1f
        )
        {
            InstructionTypes.Add(new InstructionType("hunt", "Hunt", "Hunt the Rabbit", Hunt));
        }

        public void Hunt()
        {
            //This is tempory and should be replaced by the resource system
            ((DisplayLabel)GameObjectManager.GetObjectsByTag("lblFood")[0]).Value += 1;

            GameObjectManager.Remove(this);
        }
    }
}
