using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Camera
    {
        protected Viewport _viewport;
        protected Matrix _transform;
        protected Matrix _inverseTransorm;
        protected Vector2 _position;
        protected float _zoom;

        protected MouseState _mouseState;
        protected KeyboardState _keyboardState;
        protected int _scroll;

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
            _position = Vector2.Zero;
            _zoom = 2.0f;
            _scroll = 1;
        }


        public Matrix Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }

        public Matrix InverseTransform
        {
            get { return _inverseTransorm; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }

        /// <summary>
        /// Fetches input values and updates transforms accordingly
        /// </summary>
        public void UpdateCamera()
        {
            GetInput();

            _transform = Matrix.CreateScale(new Vector3(_zoom, _zoom, 1)) * 
                Matrix.CreateTranslation(_position.X, _position.Y, 0);

            _inverseTransorm = Matrix.Invert(_transform);
        }

        /// <summary>
        /// Fetches mouse and keyboard input
        /// </summary>
        private void GetInput()
        {
            GetMouseInput();
            GetKeyboardInput();
        }

        /// <summary>
        /// Captures mouse state, sets zoom value according to the current mouse wheel value then updates said value
        /// </summary>
        private void GetMouseInput()
        {
            _mouseState = Mouse.GetState();

            if (_mouseState.ScrollWheelValue > _scroll)
            {
                _zoom += 0.1f;
                _scroll = _mouseState.ScrollWheelValue;
            }

            if (_mouseState.ScrollWheelValue < _scroll)
            {
                _zoom -= 0.1f;
                _scroll = _mouseState.ScrollWheelValue;
            }
        }

        /// <summary>
        /// Captures keyboard state then updates the viewport's position based on fetched input
        /// </summary>
        private void GetKeyboardInput()
        {
            _keyboardState = Keyboard.GetState();

            if (_keyboardState.IsKeyDown(Keys.W) || _keyboardState.IsKeyDown(Keys.Up))
            {
                _position.Y += 10.0f;
            }

            if (_keyboardState.IsKeyDown(Keys.A) || _keyboardState.IsKeyDown(Keys.Left))
            {
                _position.X += 10.0f;
            }

            if (_keyboardState.IsKeyDown(Keys.S) || _keyboardState.IsKeyDown(Keys.Down))
            {
                _position.Y -= 10.0f;
            }

            if (_keyboardState.IsKeyDown(Keys.D) || _keyboardState.IsKeyDown(Keys.Right))
            {
                _position.X -= 10.0f;
            }
        }
    }
}