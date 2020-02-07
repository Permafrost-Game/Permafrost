using Engine;
using Engine.TileGrid;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace GlobalWarmingGame.UI
{
    class Controller : IUpdatable
    {
        View view;

        Instruction currentInstruction;

        private Camera camera;
        private TileMap tileMap;

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        public Controller(Camera camera, TileMap tileMap)
        {
            this.camera = camera;
            this.tileMap = tileMap;
        }

        private void OnClick()
        {
            Vector2 positionClicked = Vector2.Transform(currentMouseState.Position.ToVector2(), camera.InverseTransform);
            GameObject objectClicked = ObjectClicked(positionClicked.ToPoint());
            Tile tileClicked = tileMap.GetTileAtPosition(positionClicked);

            if (tileClicked != null)
            {
                //TODO view

            }
        }

        


        public void Update(GameTime gameTime)
        {
            currentMouseState = Mouse.GetState();

            if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                OnClick();

            previousMouseState = currentMouseState;
        }


        /// <summary>
        /// Calculates which <see cref="Interactions.IInteractable"/> <see cref="GameObject"/> was clicked
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private static GameObject ObjectClicked(Point position)
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
