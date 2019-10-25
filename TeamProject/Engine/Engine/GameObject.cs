using Microsoft.Xna.Framework;

namespace Engine
{
    public class GameObject
    {

        protected Vector2 position = new Vector2();
        protected int rotation = 0;
        protected Vector2 velocity = new Vector2();

        public GameObject()
        {

        }

        public GameObject(Vector2 position) : this()
        {
            this.position = position;
        }

        public GameObject(Vector2 position, int rotation) : this(position)
        {
            this.rotation = rotation;
        }

        public GameObject(Vector2 position, int rotation, Vector2 velocity) : this(position, rotation)
        {
            this.velocity = velocity;
        }
    }
}
