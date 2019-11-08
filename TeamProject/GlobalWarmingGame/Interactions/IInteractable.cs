using GlobalWarmingGame.Action;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions
{
    interface IInteractable
    {
        List<InstructionType> InstructionTypes { get; }
    }
}
