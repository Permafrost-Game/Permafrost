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

        public string Name { get; private set; }
        public float Health { get; private set; }
        public Temperature Temperature { get; set; } = new Temperature(38);
        private readonly float CoreBodyTemperature = 38;
        public int UpperComfortRange { get; private set; } = 40;
        public int LowerComfortRange { get; private set; } = 15;

        private static readonly float Base_Consumption_Rate = 60000f;
        private float timeUntillFoodTick;
        private float timeUntillTemperature = 2000f;
        private float timeToTemperature;
        private float timeUntilTemperatureUpdate = 2000f;
        private float timeToTemperatureUpdate;

        public Colonist(Vector2 position, Texture2D texture, float inventoryCapacity) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0,0),
            tag: "Colonist",
            depth: 1f,
            texture: texture,
            speed: 0.5f
        )
        {
            Health = 10f;
            Inventory = new Inventory(inventoryCapacity);
            Temperature.Value = CoreBodyTemperature;
            timeUntillFoodTick = Base_Consumption_Rate;
            timeToTemperature = timeUntillTemperature;
            timeToTemperatureUpdate = timeUntilTemperatureUpdate;

            instructions = new Queue<Instruction>();
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType("select", "Select Colonist", "Selects this colonist")
            };
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
                Instruction currentInstruction = instructions.Peek();
                currentInstruction.Type.Act();

                if (currentInstruction.Type.ResourceItem != null)
                    Inventory.AddItem(currentInstruction.Type.ResourceItem);

                instructions.Dequeue();
            }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(goals.Count == 0 && instructions.Count > 0 )
                AddGoal(((GameObject)instructions.Peek().PassiveMember).Position);

            //Temperature affecting food
            timeUntillFoodTick -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Double foodFormula = (1 + Temperature.Value / CoreBodyTemperature);

            if (foodFormula <= 0.25) 
            {
                foodFormula = 0.25;
            }

            // foodFormula is a multiplier on the timeUntillFoodTick
            if ((timeUntillFoodTick * foodFormula) < 0) 
            {
                FoodTick();
                timeUntillFoodTick = Base_Consumption_Rate;
            }

            //Temperature affecting colonist's health          
            timeToTemperature -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeToTemperature < 0f)
            {
                if (Temperature.Value < LowerComfortRange || Temperature.Value > UpperComfortRange) 
                {
                    Health -= 1;                                    
                }
                timeToTemperature = timeUntillTemperature;
            }
        }

        public void UpdateTemp(float tileTemp, GameTime gameTime)
        {
            //Adjust the colonist's temperature based on the tile they are over
            timeToTemperatureUpdate -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeToTemperatureUpdate < 0f) 
            {
                if (tileTemp > CoreBodyTemperature)
                {
                    Temperature.Value = Temperature.Value + (tileTemp / 8);
                    Temperature.Value = MathHelper.Clamp(Temperature.Value, -100, tileTemp);
                    Console.Out.WriteLine("Greater"+Temperature.Value + " t:" + tileTemp + " core: " + CoreBodyTemperature);
                }
                else
                {
                    Temperature.Value = Temperature.Value + (tileTemp / 8);
                    Temperature.Value = MathHelper.Clamp(Temperature.Value, tileTemp, 100);
                    Console.Out.WriteLine("Lower"+Temperature.Value + " t:" + tileTemp + " core: " + CoreBodyTemperature);
                }
                timeToTemperatureUpdate = timeUntilTemperatureUpdate;
            }
        }

        private void FoodTick() 
        {
            //If colonist doesn't have food on them, they are starving -1 health
            ResourceItem food = new ResourceItem(new Food(), 1);
            if (!Inventory.RemoveItem(food))
            {
                Health -= 1;
            }
            else 
            {
                ((DisplayLabel)GameObjectManager.GetObjectsByTag("lblFood")[0]).Value -= 1;
            }
        }


    }
}
