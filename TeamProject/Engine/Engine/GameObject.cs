using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class GameObject
    {

        protected Vector2 position = new Vector2();
        protected Vector2 rotation = new Vector2();
        protected Vector2 velocity = new Vector2();

        public GameObject()
        {

        }

        public GameObject(Vector2 position) : this()
        {
            this.position = position;

        }

        public GameObject(Vector2 position, Vector2 rotation) : this(position)
        {
            this.rotation = rotation;
        }

        public GameObject(Vector2 position, Vector2 rotation, Vector2 velocity) : this(position, rotation)
        {
            this.velocity = velocity;
        }
    }
}
