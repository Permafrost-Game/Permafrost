using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using GlobalWarmingGame.Action;
using Engine;
using GlobalWarmingGame;
using Engine.TileGrid;
using GlobalWarmingGame.ResourceItems;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class Colonist : PathFindable, IInteractable, IUpdatable
    {

        public List<InstructionType> InstructionTypes { get; }
        public Inventory Inventory { get; }

    private Queue<Instruction> instructions;

        

        public float Health { get; private set; }
        public string Name { get; private set; }

        public Colonist(Vector2 position, Texture2D texture, float inventoryCapacity) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0,0),
            tag: "Colonist",
            depth: 1f,
            texture: texture,
            speed: 100f
        )
        {
            Health = 10f;
            Inventory = new Inventory(inventoryCapacity);
            instructions = new Queue<Instruction>();
            InstructionTypes = new List<InstructionType>();
            InstructionTypes.Add(new InstructionType("select", "Select Colonist", "Selects this colonist"));
        }

        public void AddInstruction(Instruction instruction)
        {
            instructions.Enqueue(instruction);
        }

        protected override void OnGoalComplete(Vector2 completedGoal)
        {
            
            if (instructions.Count > 0 &&
                //Since the instruction is identified by the goal, this may cause problems if two instructions have the same goal position.
                completedGoal == (((GameObject)instructions.Peek().PassiveMember).Position) &&
                instructions.Count != 0)
            {
                instructions.Peek().Type.Act();
                instructions.Dequeue();
            }
        }
            
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(goals.Count == 0 && instructions.Count > 0 )
            {
                AddGoal(((GameObject)instructions.Peek().PassiveMember).Position);
            }
        }
    }
}
