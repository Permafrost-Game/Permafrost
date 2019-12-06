using GlobalWarmingGame.Action;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    class Building
    {
        public int ID { get; }
        public Texture2D Texture { get; }
        public InstructionType InstructionType { get; }

        public Building(int id, Texture2D tex, InstructionType instructionType)
        {
            ID = id;
            Texture = tex;
            InstructionType = instructionType;
        }
    }
}
