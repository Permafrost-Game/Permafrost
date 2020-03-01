using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Resources
{
    /// <summary>
    /// An nice and easy way to create Resource types
    /// </summary>
    public static class ResourceTypeFactory
    {
        private static Dictionary<Resource, ResourceType> resources;

        public static void LoadContent(ContentManager contentManger)
        {
            resources = new Dictionary<Resource, ResourceType>
            {

                //Populate resources dictionary

                { Resource.Stone,        new ResourceType("Stone", "A piece of rock", contentManger.Load<Texture2D>(@"textures/icons/stone")) },

                { Resource.Wood,         new ResourceType("Wood", "A wooden log", contentManger.Load<Texture2D>(@"textures/icons/wood")) },

                { Resource.Fibers,       new ResourceType("Fibers", "A bundle of fibers", contentManger.Load<Texture2D>(@"textures/icons/fibers")) },

                { Resource.Food,         new ResourceType("Food", "Food", contentManger.Load<Texture2D>(@"textures/icons/apple")) },

                { Resource.Coal,         new ResourceType("Coal", "A lump of coal", null) }, //TODO coal Needs texture

                { Resource.Leather,      new ResourceType("Leather", "Tanned leather", null) }, //TODO leather Needs texture

                { Resource.MachineParts, new ResourceType("Machine Parts", "Machine parts", null) }, //TODO machineParts Needs texture

                //Teir 1 Craftable Resource Type definitions

                { Resource.Axe,          new ResourceType("Axe", "An axe", contentManger.Load<Texture2D>(@"textures/icons/axe")) },

                { Resource.Hoe,          new ResourceType("Hoe", "A hoe", contentManger.Load<Texture2D>(@"textures/icons/hoe")) },

                { Resource.Pickaxe,      new ResourceType("Pickaxe", "A Pickaxe", contentManger.Load<Texture2D>(@"textures/icons/pickaxe")) },

                { Resource.Backpack,     new ResourceType("Backpack", "A backpack", null) }, //TODO Needs texture

                { Resource.BasicRifle,   new ResourceType("BasicRifle", "A basic rifle", null) }, //TODO Needs texture

                { Resource.Bow,          new ResourceType("bow", "A bow with arrows", null) }, //TODONeeds texture

                { Resource.Cloth,        new ResourceType("Cloth", "A piece of cloth", null) }, //TODONeeds texture

                { Resource.Coat,         new ResourceType("Coat", "A basic coat", null) } //TODO Needs texture
            };

        }

        /// <summary>
        /// Make a Resource Type
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static ResourceType GetResource(Resource resource)
        {
            return resources[resource];
        }
    }
}

public enum Resource
{
    Stone,
    Wood,
    Fibers,
    Food,
    Coal,
    Leather,
    MachineParts,
    Axe,
    Backpack,
    BasicRifle,
    Bow,
    Cloth,
    Coat,
    Hoe,
    Pickaxe
}
