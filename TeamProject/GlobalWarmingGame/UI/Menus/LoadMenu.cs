using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.UI.Menus
{
    class LoadMenu<T> : Entity
    {

        private class LoadSaveEntity : Entity
        {
            public LoadSaveEntity(ButtonHandler<T> onClick, ButtonHandler<T> onDelete, string saveName, TimeSpan playTime, int numberOfTowersCaptured, Vector2 size = default, Anchor anchor = Anchor.Auto, Vector2 offset = default) :
                base(size, anchor, offset)
            {

                string text = $"{saveName} - Play Time: {playTime.Hours}h {playTime.Minutes % 60}m - Towers Captured: {numberOfTowersCaptured}";
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
        private readonly Dictionary<T, LoadSaveEntity> saveEntries;

        public LoadMenu()
        {
            saveEntries = new Dictionary<T, LoadSaveEntity>();

            menu = new Panel(Vector2.Zero, PanelSkin.Simple, Anchor.Center);
            this.AddChild(menu);

            Paragraph title = new Paragraph("Load a save game", anchor: Anchor.TopCenter)
            {
                Scale = 2f
            };
            menu.AddChild(title);




        }

        /// <summary>
        /// Adds a UI element for the save
        /// </summary>
        /// <param name="name">The display name of the save</param>
        /// <param name="playTime">The time span of the save</param>
        /// <param name="numberOfTowersCaptured">The number of towers that have been captured</param>
        /// <param name="onClick">When the load button is clicked</param>
        /// <param name="onDelete">When the delete save button is clicked</param>
        public void AddSave(string name, TimeSpan playTime, int numberOfTowersCaptured, ButtonHandler<T> onClick, ButtonHandler<T> onDelete)
        {
            LoadSaveEntity LoadSave = new LoadSaveEntity(onClick, onDelete, name, playTime, numberOfTowersCaptured, new Vector2(0, 200));
            saveEntries.Add(onClick.Tag, LoadSave);
            menu.AddChild(LoadSave);
        }

        /// <summary>
        /// Removes a save from the UI
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>false if the tag is not found</returns>
        public bool RemoveSave(T tag)
        {
            if(saveEntries.ContainsKey(tag))
            {
                LoadSaveEntity e = saveEntries[tag];
                saveEntries.Remove(tag);
                menu.RemoveChild(e);
                return true;
            }
            return false;
        }
    }
}
