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

        private static void addResource(ResourceType resource)
        {
            resources.Add(resource.ResourceID, resource);
        }

        public static void Init()
        {
            resources  = new Dictionary<Resource, ResourceType>();

            //Populate resources dictionary
            addResource(new ResourceType(Resource.Stone, "Stone", "A piece of rock", TextureIconTypes.stone));
            addResource(new ResourceType(Resource.Wood, "Wood", "A wooden log", TextureIconTypes.wood));
            addResource(new ResourceType(Resource.Fibers, "Fibers", "A bundle of fibers", TextureIconTypes.fibers));
            addResource(new ResourceType(Resource.Food, "Food", "Food", TextureIconTypes.apple));
            addResource(new ResourceType(Resource.Coal, "Coal", "A lump of coal", TextureIconTypes.coal)); //TODO coal Needs texture
            addResource(new ResourceType(Resource.Leather, "Leather", "Tanned leather", TextureIconTypes.leather)); //TODO leather Needs texture
            addResource(new ResourceType(Resource.MachineParts, "Machine Parts", "Machine parts", TextureIconTypes.machineParts)); //TODO machineParts Needs texture


            //Teir 1 Craftable Resource Type definitions

            addResource(new ResourceType(Resource.Axe, "Axe", "An axe", TextureIconTypes.axe));
            addResource(new ResourceType(Resource.Hoe, "Hoe", "A hoe", TextureIconTypes.hoe));
            addResource(new ResourceType(Resource.Pickaxe, "Pickaxe", "A Pickaxe", TextureIconTypes.pickaxe));
            addResource(new ResourceType(Resource.Backpack, "Backpack", "A backpack", TextureIconTypes.backpack));
            addResource(new ResourceType(Resource.BasicRifle, "BasicRifle", "A basic rifle", TextureIconTypes.basicRifle));
            addResource(new ResourceType(Resource.Bow, "bow", "A bow with arrows", TextureIconTypes.bow));
            addResource(new ResourceType(Resource.Cloth, "Cloth", "A piece of cloth", TextureIconTypes.cloth));
            addResource(new ResourceType(Resource.Coat, "Coat", "A basic coat", TextureIconTypes.coat));
            

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
