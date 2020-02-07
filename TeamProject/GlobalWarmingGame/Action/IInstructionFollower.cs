using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions
{
    interface IInstructionFollower
    {
        Inventory Inventory { get; }
        void AddInstruction(Instruction instruction, int priority);

       
    }
}
