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
    public static class ResourceTypeFactory
    {
        private static Dictionary<String, Texture2D> resources;

        public static void LoadContent(ContentManager contentManger)
        {
            resources = new Dictionary<string, Texture2D>();
            resources.Add("stone", contentManger.Load<Texture2D>(@"textures/icons/stone"));
            resources.Add("wood", contentManger.Load<Texture2D>(@"textures/icons/wood"));
            resources.Add("fibers", contentManger.Load<Texture2D>(@"textures/icons/fibers"));
            resources.Add("apple", contentManger.Load<Texture2D>(@"textures/icons/apple"));
        }

        public static ResourceType MakeResource(Resource resource)
        {
            switch (resource) {
                case Resource.stone:
                    ResourceType stone = new ResourceType("stone", "Stone", "A piece of rock", 1f, resources["stone"]);
                    return stone;

                case Resource.wood:
                    ResourceType wood = new ResourceType("wood", "Wood", "A wooden log", 1f, resources["wood"]);
                    return wood;

                case Resource.fibers:
                    ResourceType fibers = new ResourceType("fibers", "Fibers", "A bundle of fibers", 1f, resources["fibers"]);
                    return fibers;

                case Resource.food:
                    ResourceType food = new ResourceType("food", "Food", "Food", 1f, resources["apple"]);
                    return food;

                case Resource.coal:
                    ResourceType coal = new ResourceType("coal", "Coal", "A lump of coal", 1f, null); //Needs texture
                    return coal;

                case Resource.leather:
                    ResourceType leather = new ResourceType("leather", "Leather", "Tanned leather", 1f, null); //Needs texture
                    return leather;

                case Resource.machineParts:
                    ResourceType machineParts = new ResourceType("machineParts", "Machine Parts", "Machine parts", 1f, null); //Needs texture
                    return machineParts;

                default: return null;
            }
        }        
    }

    public enum Resource
    {
        stone, 
        wood,
        fibers,
        food,
        coal,
        leather,
        machineParts
    }

}
