using GlobalWarmingGame.Action;
using GlobalWarmingGame.Resources;

namespace GlobalWarmingGame.Interactions
{
    public interface IInstructionFollower : IStorage
    {
        void AddInstruction(Instruction instruction, int priority);

       
    } 
}
