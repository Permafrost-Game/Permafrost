using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.UI.Menus
{
    class DeleteConfirmationDialogue : Entity
    {
        public Button Cancel { get; private set; }
        public Button Delete { get; private set; }

        public DeleteConfirmationDialogue(string text, Vector2 size = default, Anchor anchor = Anchor.Auto, Vector2 offset = default) :
            base(size, anchor, offset)
        {
            Panel panel = new Panel(Vector2.Zero, skin: PanelSkin.Golden);
            this.AddChild(panel);
            Paragraph p = new Paragraph($"Are you sure you want to delete {text}");
            panel.AddChild(p);

            Delete = new Button("Delete", anchor: Anchor.BottomRight, size: new Vector2(size.X / 3, 75))
            {
                FillColor = Color.Red
            };
            panel.AddChild(Delete);

            Cancel = new Button("Cancel", anchor: Anchor.BottomLeft, size: new Vector2(size.X / 3, 75));
            panel.AddChild(Cancel);
        }   

    }
}
