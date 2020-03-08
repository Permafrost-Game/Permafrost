using GlobalWarmingGame.Action;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    [Obsolete]
    class Building
    {
        public int ID { get; }
        public string StringID { get; }
        public Texture2D Texture { get; }
        public InstructionType InstructionType { get; }

        public Building(int id, String stringID, InstructionType instructionType, Texture2D texture = null)
        {
            ID = id;
            StringID = stringID;
            Texture = texture;
            InstructionType = instructionType;
        }
    }
}
