using Engine;
using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.UI
{
    /// <summary>
    /// This class is for implementation specific UI, in this case GeoBit UI<br>
    /// This class is for creating buttons and panels based on the information provided by controller.<br>
    /// </summary>
    internal static class View
    {

        private static readonly Panel topPanel;
        private static readonly Panel bottomPanel;
        private static Panel menu;
        private static readonly Dictionary<int, Panel> inventories;

        internal static bool Hovering { get; set; } = false;

        static View()
        {
            UserInterface.Active.WhileMouseHoverOrDown = (Entity e) => { Hovering = true; };
            UserInterface.Active.         OnMouseLeave = (Entity e) => { Hovering = false; };

            inventories = new Dictionary<int, Panel>();

            #region topPanel
            topPanel = new Panel(new Vector2(0, 100), PanelSkin.Simple, Anchor.TopCenter)
            {
                Opacity = 192,
                Visible = true,
            };
            UserInterface.Active.AddEntity(topPanel);
            #endregion

            #region bottomPanel
            bottomPanel = new Panel(new Vector2(0, 100), PanelSkin.Simple, Anchor.BottomCenter)
            {
                Opacity = 192,
                Visible = true,
            };

            //inventories = PrepareInventoryMenu();

            UserInterface.Active.AddEntity(bottomPanel);
            #endregion


        }


        internal static void CreateMenu<T>(Point location, List<ButtonHandler<T>> options)
        {
            if (menu != null) UserInterface.Active.RemoveEntity(menu);

            menu = new Panel(new Vector2(150, 75 + (options.Count * 30)), PanelSkin.Default, Anchor.TopLeft, location.ToVector2());
            UserInterface.Active.AddEntity(menu);

            Label label = new Label("Choose Action", Anchor.TopCenter, new Vector2(500, 50))
            {
                Scale = 0.7f
            };

            menu.AddChild(label);
            int counter = 0;
            foreach (ButtonHandler<T> option in options)
            {
                Button newButton = new Button(option.Tag.ToString(), ButtonSkin.Default, Anchor.TopCenter, new Vector2(125, 25), new Vector2(0, (counter + 1) * 30));
                newButton.ButtonParagraph.Scale = 0.5f;
                newButton.Padding = Vector2.Zero;
                menu.AddChild(newButton);

                newButton.OnClick = (Entity btn) =>
                {
                    option.action(option.Tag);
                    menu.Visible = false;
                    Hovering = false;
                };
                counter++;
            }

        }

        internal static void CreateDropDown<T>(string label, List<ButtonHandler<T>> options)
        {
            DropDown menu = new DropDown(new Vector2(225, 75), Anchor.CenterLeft, new Vector2(250 * topPanel.Children.Count, 4), PanelSkin.ListBackground, PanelSkin.ListBackground, true)
            {
                DefaultText = label,
                AutoSetListHeight = true,
                DontKeepSelection = true,
            };

            foreach (ButtonHandler<T> option in options)
            {
                menu.AddItem(option.Tag.ToString());
            }

            topPanel.AddChild(menu);

            menu.OnValueChange = (Entity e) =>
            {
                foreach (ButtonHandler<T> option in options)
                {
                    if (option.Tag.ToString().Equals(menu.SelectedValue))
                    {
                        option.action(option.Tag);
                        break;
                    }

                }
            };

        }


        internal static void AddInventory<T>(ButtonHandler<T> buttonHandler, bool visible = false, Texture2D icon = default)
        {
            bool customIcon = icon != default;
            Icon inventoryButton = new Icon(customIcon ? IconType.None : IconType.Sack, Anchor.BottomLeft, 1f, true, new Vector2(64f * inventories.Count, 0f)) ;
            if(customIcon) inventoryButton.Texture = icon;

            bottomPanel.AddChild(inventoryButton);

            inventoryButton.OnClick = (Entity btn) => {
                buttonHandler.action(buttonHandler.Tag);
            };

            Panel inventory = new Panel(new Vector2(282, 400), PanelSkin.Simple, Anchor.BottomLeft, new Vector2(-26, 75))
            {
                Opacity = 192,
                Visible = visible,
            };

            inventories.Add(buttonHandler.Tag.GetHashCode(), inventory);

            bottomPanel.AddChild(inventory);
        }

        internal static void UpdateInventoryMenu(int id, IEnumerable<ItemElement> items)
        {
            inventories[id].ClearChildren();
            foreach (ItemElement i in items)
            {
                inventories[id].AddChild(CreateInventoryElement(i));
            }

            for (int i = inventories[id].Children.Count; i < 24; i++)
            {
                inventories[id].AddChild(CreateInventoryElement(new ItemElement(null, "0")));
            }
        }

        private static Entity CreateInventoryElement(ItemElement i)
        {
            Icon slot = new Icon(IconType.None, Anchor.AutoInline, 0.75f, true);
            if (i.Texture != null) slot.Texture = i.Texture;

            
            slot.AddChild(new Label(i.Label, Anchor.TopLeft, null, new Vector2(7.9f, -20)));
            return slot;
        }


        internal static void ToggleInventoryMenuVisibility(int id)
        {
            bool oldState = inventories[id].Visible;
            foreach (Entity panel in inventories.Values)
            {
                panel.Visible = false;
            }
            inventories[id].Visible = !oldState;
        }

        internal static void RemoveInventory(int id)
        {
            inventories[id].Dispose();
        }
    }
}
