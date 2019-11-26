using Engine;
using Microsoft.Xna.Framework;
using System.Linq;

namespace GlobalWarmingGame.Action
{
    abstract class SelectionInputMethod
    {
        /// <summary>
        /// Calculates which <see cref="Interactions.IInteractable"/> <see cref="Colonist"/> was clicked
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected Colonist ObjectClicked(Point position)
        {
            foreach (Colonist o in GameObjectManager.Interactable)
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
