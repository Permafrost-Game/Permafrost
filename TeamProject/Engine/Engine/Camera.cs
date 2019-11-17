using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        /// Fetches Input values and updates Transforms accordingly
        /// </summary>
        public void UpdateCamera()
        {
            GetInput();

            _zoom = MathHelper.Clamp(_zoom, 0.5f, 4.0f); // Clamps Zoom value
            _position.X = MathHelper.Clamp(_position.X, -400f, 400f); // Clamps camera position on X
            _position.Y = MathHelper.Clamp(_position.Y, -400f, 400f); // Clamps camera position on Y

            _transform = Matrix.CreateTranslation(_position.X, _position.Y, 0) * // Main Translation Matrix
                Matrix.CreateTranslation(-400, -400, 0) * // Tilemap Offset Matrix (Assumes the Tilemap is 50x50 @ 16p per tile, to be changed later)
                Matrix.CreateScale(new Vector3(_zoom, _zoom, 1)) * // Scale Matrix
                Matrix.CreateTranslation(new Vector3(_viewport.Width / 2, _viewport.Height / 2, 0)); // Origin Offset Matrix

            _inverseTransorm = Matrix.Invert(_transform); // Inverse Transform Matrix
        }

        /// <summary>
        /// Fetches Mouse and Keyboard Input
        /// </summary>
        private void GetInput()
        {
            GetMouseInput();
            GetKeyboardInput();
        }

        /// <summary>
        /// Captures Mouse State, sets Zoom value according to the current Mouse Wheel value then updates said value
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
        /// Captures Keyboard State then updates the Viewport's Position based on fetched Input
        /// </summary>
        private void GetKeyboardInput()
        {
            _keyboardState = Keyboard.GetState();

            if (_keyboardState.IsKeyDown(Keys.W) || _keyboardState.IsKeyDown(Keys.Up))
            {
                _position.Y += 5.0f;
            }

            if (_keyboardState.IsKeyDown(Keys.A) || _keyboardState.IsKeyDown(Keys.Left))
            {
                _position.X += 5.0f;
            }

            if (_keyboardState.IsKeyDown(Keys.S) || _keyboardState.IsKeyDown(Keys.Down))
            {
                _position.Y -= 5.0f;
            }

            if (_keyboardState.IsKeyDown(Keys.D) || _keyboardState.IsKeyDown(Keys.Right))
            {
                _position.X -= 5.0f;
            }
        }
    }
}
