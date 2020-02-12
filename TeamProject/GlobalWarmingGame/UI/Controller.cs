using Engine;
using Engine.TileGrid;
using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GlobalWarmingGame.UI
{
    /// <summary>
    /// The controller class controlls the logic behind the UI<br>
    /// Communicates with the <see cref="View"/> to display menus<br>
    /// eg.<br>
    /// When the user clicks, this class will tell View to create a new menu, view is in charge of the GUI specific logic.<br>
    /// This class defines what UI needs to exist.
    /// </summary>
    class Controller : IUpdatable
    {
 
        private View view;

        /// <summary>The currently selected colonist that instructions will be given</summary>
        public static Colonist SelectedColonist { get; set; }
        /// <summary>Reference to the camera for inverse transforms</summary>
        private Camera camera;

        /// <summary>Whether the mouse is hovering over a menu</summary>
        private bool hovering = false;

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private static readonly InstructionType WALK_INSTRUCTION_TYPE = new InstructionType("walk", "Walk", "Walk here");
        private static readonly InstructionType COLONIST_INSTRUCTION_TYPE = new InstructionType("selectColonist", "Select", "Selects the colonist");


        public Controller(Camera camera)
        {
            this.camera = camera;
            view = new View();
            UserInterface.Active.WhileMouseHoverOrDown = (Entity e) => { hovering = true; };
        }

        /// <summary>
        /// Called on a mouse click
        /// </summary>
        private void OnClick()
        {
            if (!hovering)
            {
                Vector2 positionClicked = Vector2.Transform(currentMouseState.Position.ToVector2(), camera.InverseTransform);
                GameObject objectClicked = ObjectClicked(positionClicked.ToPoint());

                if (objectClicked != null)
                {
                    List<InstructionHandler> options = GerateOptions(objectClicked, SelectedColonist);
                    if (options != null)
                    {
                        view.CreateInstructionMenu(currentMouseState.Position, options);
                    }
                }
            }

        }

        /// <summary>
        /// Takes an input GameObject 
        /// </summary>
        /// <param name="objectClicked">the object that was </param>
        /// <returns></returns>
        private static List<InstructionHandler> GerateOptions(GameObject objectClicked, Colonist colonist)
        {
            List<InstructionHandler> options = new List<InstructionHandler>();
            options.Add(new InstructionHandler(new Instruction(WALK_INSTRUCTION_TYPE, colonist, objectClicked), IssueInstruction));

            if(objectClicked is IInteractable)
            {
                foreach (InstructionType type in ((IInteractable)objectClicked).InstructionTypes)
                {
                    options.Add(new InstructionHandler(new Instruction(type, colonist, objectClicked), IssueInstruction));
                }
            }

            if (objectClicked is Colonist)
            {
                options.Add(new InstructionHandler(new Instruction(COLONIST_INSTRUCTION_TYPE, colonist, objectClicked), SelectColonist));
            }


            return options;
        }

        /// <summary>
        /// Adds the instruction to the active member of the instruction.
        /// </summary>
        /// <param name="instruction">the instruction to be issued</param>
        private static void IssueInstruction(Instruction instruction)
        {
            instruction.ActiveMember.AddInstruction(instruction, 0);
        }

        /// <summary>
        /// Takes the passive member of the instruction and sets that as the active colonist<br>
        /// This is meant to be used to select a colonist
        /// </summary>
        /// <param name="instruction">The instruction that has been selected</param>
        private static void SelectColonist(Instruction instruction)
        {
            SelectedColonist = (Colonist) instruction.PassiveMember;
        }


        public void Update(GameTime gameTime)
        {
            currentMouseState = Mouse.GetState();

            if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                OnClick();

            previousMouseState = currentMouseState;
            hovering = false;
        }


        /// <summary>
        /// Checks to see if a <see cref="Interactions.IInteractable"/> <see cref="GameObject"/> was clicked
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
            return ZoneManager.CurrentZone.TileMap.GetTileAtPosition(position.ToVector2());
        }

    }
}
