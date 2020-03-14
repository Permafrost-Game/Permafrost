using GlobalWarmingGame.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.UI
{
    class ButtonHandler<T>
    {
        public T Tag { get; set; }

        public string DisplayName { get; set; }

        public delegate void Action(T tag);

        public Action action;

        public ButtonHandler(T tag, Action action) : this(tag, tag.ToString(), action) { }

        public ButtonHandler(T tag, string displayName, Action action)
        {
            this.Tag = tag;
            this.DisplayName = displayName;
            this.action = action;
        }

        public override string ToString()
        {
            return DisplayName;
        }

    }
}
