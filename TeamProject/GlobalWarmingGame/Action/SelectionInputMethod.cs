using Engine;
using Microsoft.Xna.Framework;
using System.Linq;

namespace GlobalWarmingGame.Action
{
    abstract class SelectionInputMethod
    {
        /// <summary>
        /// Calculates which <see cref="Interactions.IInteractable"/> <see cref="GameObject"/> was clicked
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
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