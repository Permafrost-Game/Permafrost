using Microsoft.Xna.Framework;

namespace GlobalWarmingGame.UI
{
    interface IUpdatableUI
    {
        bool IsActive { get; }
        void Update(GameTime gameTime);
    }
}
