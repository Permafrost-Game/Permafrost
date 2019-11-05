using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine.UI
{
    public class DropdownMenu : Sprite
    {
        public List<Button> Buttons { get; set; }

        public DropdownMenu(Vector2 position, Vector2 size, string tag, Texture2D texture) : base 
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
            Buttons = new List<Button>();
        }





    }
}
