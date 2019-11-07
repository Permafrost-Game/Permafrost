using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Action
{
    class SelectionManager
    {
        public Instruction CurrentInstruction { get; set; }
        public List<SelectionInputMethod> InputMethods { get; set; }


        public SelectionManager()
        {
            CurrentInstruction = new Instruction();
            InputMethods = new List<SelectionInputMethod>();
        }
    }
}
