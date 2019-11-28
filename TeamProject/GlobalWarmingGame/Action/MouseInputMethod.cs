using Engine;
using GlobalWarmingGame;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Action
{

    /// <summary>
    /// This class manages the selection and instruction of interactable <see cref="GameObject"/>s using mouse input
    /// </summary>
    class MouseInputMethod : SelectionInputMethod
    {
        private readonly Camera camera;
        private Instruction currentInstruction;

        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        /// <param name="camera">The current camera view, required for translating MouseState point into game world Vector2s</param>
        /// <param name="desktop">Myra UI desktop, required for mouse event handling</param>
        /// <param name="currentInstruction">The current instruction</param>
        public MouseInputMethod(Camera camera, Instruction currentInstruction)
        {
            this.camera = camera;
            this.currentInstruction = currentInstruction;
        }
    }
}