using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.UI.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalWarmingGame.UI
{
    /// <summary>
    /// This class is for implementation specific UI, in this case <see cref="GeonBit.UI"/> <br/>
    /// This class is for creating buttons and panels based on the information provided by <see cref="Controller"/>.<br/>
    /// This class should not reference any <see cref="GlobalWarmingGame"/> specific classes, and should be the only class referencing <see cref="GeonBit.UI"/> specific classes.<br/>
    /// </summary>
    static class View
    {
        static MainMenu MainMenu;
        static PauseMenu PauseMenu;

        private static Panel topPanel;
        private static Panel bottomPanel;
        private static Panel menu;
        private static Dictionary<int, Panel> inventories;
        private static Dictionary<int, Icon> inventoryButtons;

        /// <summary>True if the current mouse position is over a UI entity</summary>
        internal static bool Hovering { get; set; }

        static View()
        {
            inventories = new Dictionary<int, Panel>();
            inventoryButtons = new Dictionary<int, Icon>();
            UserInterface.Active.WhileMouseHoverOrDown = (Entity e) => { Hovering = true; };
        }

        /// <summary>
        /// Resets currently active UI elements
        /// </summary>
        internal static void Reset()
        {
            UserInterface.Active.Clear();
            inventories = new Dictionary<int, Panel>();
            inventoryButtons = new Dictionary<int, Icon>();
        }

        internal static void CreateMainMenuUI(Texture2D MainMenuLogo)
        {
            MainMenu = new MainMenu(MainMenuLogo);

            MainMenu.MainToGame.OnClick = (Entity button) => {
                Game1.GameState = GameState.Intro;
            };
            MainMenu.MainToQuit.OnClick = (Entity button) => Game1.GameState = GameState.Exiting;
        }

        internal static void CreateGameUI()
        {
            PauseMenu = new PauseMenu();

            PauseMenu.PauseToGame.OnClick = (Entity button) => { Game1.GameState = GameState.Playing; };
            PauseMenu.PauseToMain.OnClick = (Entity button) => { Game1.GameState = GameState.MainMenu; };
            PauseMenu.PauseToQuit.OnClick = (Entity button) => Game1.GameState = GameState.Exiting;

            UserInterface.Active.AddEntity(PauseMenu);

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
            UserInterface.Active.AddEntity(bottomPanel);
            #endregion
        }

        internal static void SetUIScale(float scale)
        {
            UserInterface.Active.GlobalScale = scale;
        }

        internal static void Update(GameTime gameTime)
        {
            Hovering = false;
            UserInterface.Active.Update(gameTime);
        }

        internal static void SetMainMenuVisiblity(bool show)
        {
            MainMenu.Visible = show;
        }

        internal static void SetPauseMenuVisiblity(bool show)
        {
            PauseMenu.Visible = show;
        }

        internal static void Draw(SpriteBatch spriteBatch)
        {
            UserInterface.Active.Draw(spriteBatch);
        }

        /// <summary>
        /// Creates a button list menu.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ButtonHandler{T}"/></typeparam>
        /// <param name="text">The label of the menu</param>
        /// <param name="location">the screenspace location of the menu</param>
        /// <param name="options">the elements of the menu</param>
        internal static void CreateMenu<T>(string text, Point location, List<ButtonHandler<T>> options)
        {
            if (menu != null) UserInterface.Active.RemoveEntity(menu);

            menu = new Panel(new Vector2(190, 80f + (options.Count * 40f)), PanelSkin.Simple, Anchor.TopLeft, location.ToVector2() / UserInterface.Active.GlobalScale)
            {
                Opacity = 200
            };
            UserInterface.Active.AddEntity(menu);

            Label label = new Label(text, Anchor.TopCenter, new Vector2(190f, 20f))
            {
                Scale = 0.8f
            };

            menu.AddChild(label);
            int counter = 0;
            foreach (ButtonHandler<T> option in options)
            {
                Button newButton = new Button(option.Tag.ToString(), ButtonSkin.Default, Anchor.TopCenter, new Vector2(175f, 30f), new Vector2(0f, (counter + 1f) * 40f));
                newButton.ButtonParagraph.Scale = 0.85f;
                //newButton.Scale = 2f;
                newButton.Padding = Vector2.Zero;
                menu.AddChild(newButton);

                newButton.OnClick = (Entity btn) =>
                {
                    option.action(option.Tag);
                    menu.Dispose();
                    UserInterface.Active.RemoveEntity(menu);
                    menu = null;
                };
                counter++;
            }

        }

        /// <summary>
        /// Creates a drop down menu in the top panel.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ButtonHandler{T}"/></typeparam>
        /// <param name="text">The label of the dropdown button</param>
        /// <param name="options">The elements of the drop down</param>
        internal static void CreateDropDown<T>(string text, List<ButtonHandler<T>> options)
        {
            DropDown menu = new DropDown(new Vector2(225f, 75f), Anchor.CenterLeft, new Vector2(250f * topPanel.Children.Count, 4f), PanelSkin.ListBackground, PanelSkin.ListBackground, true)
            {
                DefaultText = text,
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

        /// <summary>
        /// Creates a notification for the user
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text">Common notification text</param>
        /// <param name="list">List of objects of type T that will be appended to the notification text</param>
        internal static void Notification<T>(string text, List<T> list = null) 
        {
            string notificatonText = text;

            Panel Notification = new Panel(new Vector2(225f, 100f), PanelSkin.Default, Anchor.TopCenter, new Vector2(0, 100f))
            {
                Padding = Vector2.Zero,
                Visible = false
            };

            UserInterface.Active.AddEntity(Notification);

            if (list != null && list.Count > 0) 
            {
                foreach(T item in list) 
                {
                    Notification.Size += new Vector2(0f, 10f);
                    notificatonText += "\n " + item.ToString();
                }
            }

            Notification.AddChild(new Label(notificatonText, Anchor.Center));
            Notification.Visible = true;

            Task.Delay(new TimeSpan(0, 0, 2)).ContinueWith(o =>
            {
                Notification.Dispose();
                UserInterface.Active.RemoveEntity(Notification);
            });
        }


        /// <summary>
        /// Adds an inventory button and menu to the bottom panel.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="ButtonHandler{T}"/></typeparam>
        /// <param name="buttonHandler">The <see cref="ButtonHandler{T}"/> that handles the OnClick event</param>
        /// <param name="visible">whether the inventory menu should be visible on creation</param>
        /// <param name="icon">A custom <see cref="Texture2D"/> that is to be used, if null then <see cref="IconType.Sack"/> will be used</param>
        internal static void AddInventory<T>(ButtonHandler<T> buttonHandler, bool visible = false, Texture2D icon = default)
        {
            bool customIcon = icon != default;
            Icon inventoryButton = new Icon(customIcon ? IconType.None : IconType.Sack, Anchor.BottomLeft, 1f, true, new Vector2(64f * inventories.Count, 0f)) ;
            if(customIcon) inventoryButton.Texture = icon;

            inventoryButtons.Add(buttonHandler.Tag.GetHashCode(), inventoryButton);
            bottomPanel.AddChild(inventoryButton);

            inventoryButton.OnClick = (Entity btn) => {
                buttonHandler.action(buttonHandler.Tag);
            };

            Panel inventory = new Panel(new Vector2(282f, 400f), PanelSkin.Simple, Anchor.BottomLeft, new Vector2(-26f, 75f))
            {
                Opacity = 192,
                Visible = visible,
            };

            inventories.Add(buttonHandler.Tag.GetHashCode(), inventory);

            bottomPanel.AddChild(inventory);
        }

        /// <summary>
        /// Sets the elements of the inventory menu.
        /// </summary>
        /// <param name="id">The unique ID of the inventory menu (eg hashcode)</param>
        /// <param name="items">Items that are to be added to the inventory menu</param>
        /// <example><c>View.UpdateInventoryMenu(inventory.GetHashCode(), ItemElements);</c></example>
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

            
            slot.AddChild(new Label(i.Label, Anchor.TopLeft, null, new Vector2(7.9f, -20f)));
            return slot;
        }

        /// <summary>
        /// Sets the given inventory as the currently selected inventory
        /// </summary>
        /// <param name="id">The unique id (eg hashcode) of the inventory  that is to be made visible</param>
        /// <example><c>View.SetInventoryVisible(inventory.GetHashCode());</c></example>
        internal static void SetInventoryVisiblity(int id)
        {
            foreach (Entity panel in inventories.Values)
            {
                panel.Visible = false;
            }
            inventories[id].Visible = true;
        }

        /// <summary>
        /// Removes an inventory menu
        /// </summary>
        /// <param name="id">The unique id (eg hashcode) of the inventory  that is to be removed</param>
        /// <example><c>View.RemoveInventory(inventory.GetHashCode());</c></example>
        internal static void RemoveInventory(int id)
        {
            bottomPanel.RemoveChild(inventoryButtons[id]);
            inventoryButtons.Remove(id);
            bottomPanel.RemoveChild(inventories[id]);
            inventories[id].Dispose();
            inventories.Remove(id);
            
        }

        
    }
}
