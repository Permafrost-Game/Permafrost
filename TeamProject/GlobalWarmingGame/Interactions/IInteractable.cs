using GlobalWarmingGame.Action;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions
{
    public interface IInteractable
    {
        List<InstructionType> InstructionTypes { get; }
    }
}
