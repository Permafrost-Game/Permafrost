using Engine;
using Microsoft.Xna.Framework;
using System.Linq;

namespace GlobalWarmingGame.Action
{
    abstract class SelectionInputMethod
    {
        protected GameObject ObjectClicked(Point position)
        {
            foreach (GameObject o in GameObjectManager.Interactable)
            {
                if (new Rectangle(o.Position.ToPoint(), o.Size.ToPoint()).Contains(position))
                {
                    return o;
                }
            }
            return null;
        }
    }
}
