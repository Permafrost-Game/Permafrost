using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using GlobalWarmingGame.Action;
using Engine;
using GlobalWarmingGame;
using Engine.TileGrid;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class Colonist : PathFindable, IInteractable
    {

        public List<InstructionType> InstructionTypes { get; }

        private Queue<Instruction> instructions; 

        public float Health { get; private set; }
        public string Name { get; private set; }
         

        public Colonist(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0,0),
            tag: "Colonist",
            depth: 1f,
            texture: texture,
            speed: 10f
        )
        {
            Health = 10f;
            instructions = new Queue<Instruction>();
            InstructionTypes = new List<InstructionType>();
            InstructionTypes.Add(new InstructionType("select", "Select Colonist", "Selects this colonist"));
        }

        public void AddInstruction(Instruction instruction)
        {
            instructions.Enqueue(instruction);
        }

        protected override void PathComplete()
        {
            if (instructions.Count != 0)
            {
                if (instructions.Count != 0)
                {
                    AddGoal(((GameObject)instructions.Peek().PassiveMember).Position);
                }
                instructions.Peek().Type.Act();

                instructions.Dequeue();
            }
        }




    }
}
