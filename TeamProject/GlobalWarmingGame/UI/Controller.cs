using Engine;
using Engine.TileGrid;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections; 
using System.Linq;

namespace GlobalWarmingGame.UI
{
    class Controller : IUpdatable
    {
        
        private View view;

        private Colonist selectedColonist;

        private Camera camera;

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private static readonly InstructionType WALK_INSTRUCTION_TYPE = new InstructionType("walk", "Walk", "Walk here");

        public Controller(Camera camera)
        {
            this.camera = camera;
        }

        private void OnClick()
        {
            Vector2 positionClicked = Vector2.Transform(currentMouseState.Position.ToVector2(), camera.InverseTransform);
            GameObject objectClicked = ObjectClicked(positionClicked.ToPoint());

            if(objectClicked != null)
            {
                List<Instruction> options = GerateOptions(objectClicked, selectedColonist);
                if (options != null)
                {
                    view.CreateInstructionMenu(currentMouseState.Position, options);
                }
            }
            
        }

        /// <summary>
        /// Takes an input GameObject 
        /// </summary>
        /// <param name="objectClicked">the object that was </param>
        /// <returns></returns>
        private static List<Instruction> GerateOptions(GameObject objectClicked, Colonist colonist)
        {
            List<Instruction> options = new List<Instruction>();
            if (objectClicked is IInteractable)
            {
                foreach( InstructionType type in ((IInteractable)objectClicked).InstructionTypes)
                {
                    options.Add(new Instruction(type, colonist, objectClicked));
                }
                    
            }
            options.Add(new Instruction(WALK_INSTRUCTION_TYPE, colonist, objectClicked));

            return options;
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
