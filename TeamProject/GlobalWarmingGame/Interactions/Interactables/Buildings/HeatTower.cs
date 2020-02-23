using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    class HeatTower: Sprite, IInteractable, IHeatable
    {
        public Temperature Temperature { get; set; } = new Temperature(100);
        public bool Heating { get; private set; }

        public List<InstructionType> InstructionTypes { get; }

        public HeatTower(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            origin: new Vector2(texture.Width / 2f, texture.Height / 2f),
            tag: "HeatTower",
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>();
            Heating = true;
            //InstructionTypes.Add(new InstructionType("fuel", "Fuel", "Fuel tower", Fuel));
        }

        private void Fuel(Colonist colonist)
        {
        }
    }
}
