
using Engine;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class Oven : InteractableGameObject, IUpdatable
    {
        private InstructionType cook;
        private InstructionType retrieve;
        private Inventory inventory;

        private bool cooking;
        private float timeUntilCooked;
        private float cockTime = 10000f;

        public Oven(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "Oven",
            depth: 0.7f,
            texture: texture,
            instructionTypes: new List<InstructionType>() { }
        )
        {
            cook = new InstructionType("cook", "Cook", "Cook food", CookFood);
            retrieve = new InstructionType("retrieve", "Retrieve", "Retrieve food", RetrieveFood);
            InstructionTypes.Add(cook);
        }

        public void CookFood(PathFindable findable)
        {
            //Take raw food from colonist and place in oven
            //Once cooking don't let the colonist add more food
        }

        public void RetrieveFood(PathFindable findable) { 
            //Take out cooked food from oven

        }

        public void Update(GameTime gameTime)
        {
            //Use game time to cook the food.
            throw new System.NotImplementedException();
        }
    }
}
