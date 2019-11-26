using Microsoft.Xna.Framework;

namespace Engine
{
    /// <summary>
    /// A GameObject is an object that exists in the game world this requires a position and a size
    /// </summary>
    public class GameObject
    {
        public Vector2 Position { get; protected set;}
        public Vector2 Size { get; protected set; }
        public float Rotation { get; protected set; }
        public Vector2 RotationOrigin { get; protected set; }
        public string Tag { get; protected set; }

        public GameObject(Vector2 position, Vector2 size, float rotation, Vector2 rotationOrigin, string tag)
        {
            this.Position = position;
            this.Size = size;
            this.Rotation = rotation;
            this.RotationOrigin = rotationOrigin;
            this.Tag = tag;
        }

        public GameObject(Vector2 position, Vector2 size, string tag) : this(position, size, 0, new Vector2(0), tag)
        {

        }

        public GameObject(Vector2 position, Vector2 size) : this(position, size, null)
        {

        }
    }
}