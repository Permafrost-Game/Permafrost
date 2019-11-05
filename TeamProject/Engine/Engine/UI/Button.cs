using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.UI
{
    public class Button : Sprite, IClickable
    {

        private string text;

        public Button(Vector2 position, Vector2 size, string tag, Texture2D texture) : base
        (
            position: position,
            size: size,
            rotation: 0f,
            rotationOrigin: new Vector2(0),
            tag: tag,
            depth: 3f,
            texture: texture
        )
        {

        }

        public void OnClick(MouseState mouseState)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(font, text, new Vector2(100, 100), Color.Black);
        }
    }
}
