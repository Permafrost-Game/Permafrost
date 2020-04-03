using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GlobalWarmingGame.UI.Menus
{
    /// <summary>
    /// UI element for the SaveGame load menu
    /// </summary>
    /// <typeparam name="S">The type of the onLoad</typeparam>
    /// <typeparam name="D">The type of the onDelete</typeparam>
    class LoadMenu<S,D> : Entity
    {

        private class SaveGameEntity : Entity
        {
            public SaveGameEntity(ButtonHandler<S> onClick, ButtonHandler<D> onDelete, string text, Vector2 size = default, Anchor anchor = Anchor.Auto, Vector2 offset = default) :
                base(size, anchor, offset)
            {
                Button p = new Button(
                    text:text,
                    skin: ButtonSkin.Default,
                    anchor: Anchor.CenterLeft,
                    size: new Vector2(0, 120), 
                    offset: new Vector2(0))
                {
                    Scale = 3f,
                    OnClick = (Entity e) => { onClick.action(onClick.Tag); }
                };
                
                this.AddChild(p);


                Button delete = new Button("Delete Save", anchor: Anchor.CenterRight, size: new Vector2(250,75))
                {
                    Scale = 3f,
                    OnClick = (Entity e) => { onDelete.action(onDelete.Tag); },
                    FillColor = Color.Red
                };
                p.AddChild(delete);

            }

        }


        private readonly Panel menu;
        private readonly Dictionary<S, SaveGameEntity> saveEntries;
        private readonly Paragraph noSave;

        public Button LoadToMain { get; private set; }

        public LoadMenu()
        {
            saveEntries = new Dictionary<S, SaveGameEntity>();

            menu = new Panel(Vector2.Zero, PanelSkin.Simple, Anchor.Center);
            menu.PanelOverflowBehavior = PanelOverflowBehavior.VerticalScroll;
            this.AddChild(menu);

            Paragraph title = new Paragraph("Load a save game", anchor: Anchor.TopCenter)
            {
                Scale = 2f
            };
            menu.AddChild(title);

            noSave = new Paragraph("No Saves Found", anchor: Anchor.Center)
            {
                Scale = 2f,
                FillColor = Color.Gray
            };
            menu.AddChild(noSave);


            LoadToMain = new Button(
                text: "Main Menu",
                size: new Vector2(300,75),
                offset: new Vector2(50),
                anchor: Anchor.BottomLeft
                );
           this.AddChild(LoadToMain);



        }

        /// <summary>
        /// Adds a UI element for a given SaveGame option
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="onClick">When the load button is clicked</param>
        /// <param name="onDelete">When the delete save button is clicked</param>
        public void AddLoadSaveGame(string text, ButtonHandler<S> onClick, ButtonHandler<D> onDelete)
        {
            SaveGameEntity LoadSave = new SaveGameEntity(onClick, onDelete, text, new Vector2(0, 200));
            saveEntries.Add(onClick.Tag, LoadSave);
            menu.AddChild(LoadSave);
            CheckNoSave();
        }

        private void CheckNoSave()
        {
            noSave.Visible = saveEntries.Count <= 0;
        }

        /// <summary>
        /// Removes a save from the UI
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>false if the tag is not found</returns>
        public bool RemoveSave(S tag)
        {
            if(saveEntries.ContainsKey(tag))
            {
                SaveGameEntity e = saveEntries[tag];
                saveEntries.Remove(tag);
                menu.RemoveChild(e);
                CheckNoSave();
                return true;
            }
            return false;
        }
    }
}
