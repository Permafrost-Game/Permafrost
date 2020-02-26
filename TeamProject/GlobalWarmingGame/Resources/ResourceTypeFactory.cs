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
        private static Dictionary<Craftable, CraftableType> craftables;

        public static void Init()
        {
            resources  = new Dictionary<Resource, ResourceType>();
            craftables = new Dictionary<Craftable, CraftableType>();

            //Resource Type definitions
            ResourceType stone        = new ResourceType("Stone",           "A piece of rock",      TextureIconTypes.stone);
            ResourceType wood         = new ResourceType("Wood",            "A wooden log",         TextureIconTypes.wood);
            ResourceType fibers       = new ResourceType("Fibers",          "A bundle of fibers",   TextureIconTypes.fibers);
            ResourceType food         = new ResourceType("Food",            "Food",                 TextureIconTypes.apple);
            ResourceType coal         = new ResourceType("Coal",            "A lump of coal",       TextureIconTypes.coal); //TODO coal Needs texture
            ResourceType leather      = new ResourceType("Leather",         "Tanned leather",       TextureIconTypes.leather); //TODO leather Needs texture
            ResourceType machineParts = new ResourceType("Machine Parts",   "Machine parts",        TextureIconTypes.machineParts); //TODO machineParts Needs texture

            //Populate resources dictionary
            resources.Add(Resource.Stone,   stone);
            resources.Add(Resource.Wood,    wood);
            resources.Add(Resource.Fibers,  fibers);
            resources.Add(Resource.Food,    food);
            resources.Add(Resource.Coal,    coal);
            resources.Add(Resource.Leather, leather);
            resources.Add(Resource.MachineParts, machineParts);

            //Craftable Type definitions and populate craftables dictionary

            craftables.Add(Craftable.Axe,        new CraftableType("Axe", "An axe",        TextureIconTypes.axe));

            craftables.Add(Craftable.Hoe,        new CraftableType("Hoe", "A hoe",         TextureIconTypes.hoe));

            craftables.Add(Craftable.Pickaxe,    new CraftableType("Pickaxe", "A Pickaxe", TextureIconTypes.pickaxe));

            craftables.Add(Craftable.Backpack,   new CraftableType("Backpack", "A backpack",      TextureIconTypes.backpack)); //TODO Needs texture

            craftables.Add(Craftable.BasicRifle, new CraftableType("BasicRifle", "A basic rifle", TextureIconTypes.basicRifle)); //TODO Needs texture

            craftables.Add(Craftable.Bow,        new CraftableType("bow", "A bow with arrows",    TextureIconTypes.bow)); //TODONeeds texture

            craftables.Add(Craftable.Cloth,      new CraftableType("Cloth", "A piece of cloth",   TextureIconTypes.cloth)); //TODONeeds texture

            craftables.Add(Craftable.Coat,       new CraftableType("Coat", "A basic coat",        TextureIconTypes.coat)); //TODO Needs texture


            //The crafting costs need to be set after all the resources and craftables types have been created.
            #region Teir 1 crafting costs
            //Axe
            craftables[Craftable.Axe].CraftingCosts = new List<ResourceItem>() {
                new ResourceItem(GetResource(Resource.Wood), 1),
                new ResourceItem(GetResource(Resource.Fibers), 2),
                new ResourceItem(GetResource(Resource.Stone), 1),
            };

            //Hoe
            craftables[Craftable.Hoe].CraftingCosts = new List<ResourceItem>() {
                new ResourceItem(GetResource(Resource.Wood), 1),
                new ResourceItem(GetResource(Resource.Fibers), 2),
            };

            //Pickaxe
            craftables[Craftable.Pickaxe].CraftingCosts = new List<ResourceItem>() {
                new ResourceItem(GetResource(Resource.Wood), 1),
                new ResourceItem(GetResource(Resource.Fibers), 2),
                new ResourceItem(GetResource(Resource.Stone), 2),
            };

            //Backpack
            craftables[Craftable.Backpack].CraftingCosts = new List<ResourceItem>() {
                new ResourceItem(GetCraftable(Craftable.Cloth), 2),
                new ResourceItem(GetResource(Resource.Leather), 5),
            };

            //BasicRifle
            craftables[Craftable.BasicRifle].CraftingCosts = new List<ResourceItem>() {
                new ResourceItem(GetResource(Resource.Wood), 8),
                new ResourceItem(GetResource(Resource.Leather), 2),
                new ResourceItem(GetResource(Resource.MachineParts), 4),
            };

            //Bow
            craftables[Craftable.Bow].CraftingCosts = new List<ResourceItem>() {
                new ResourceItem(GetResource(Resource.Wood), 4),
                new ResourceItem(GetResource(Resource.Fibers), 6),
                new ResourceItem(GetResource(Resource.Stone), 1),
            };

            //Cloth
            craftables[Craftable.Cloth].CraftingCosts = new List<ResourceItem>() {
                new ResourceItem(GetResource(Resource.Fibers), 4),
            };

            //Coat
            craftables[Craftable.Coat].CraftingCosts =  new List<ResourceItem>() {
                new ResourceItem(GetCraftable(Craftable.Cloth), 4),
                new ResourceItem(GetResource(Resource.Leather), 2),
            };
            #endregion
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

        /// <summary>
        /// Make a Craftable Type that extends Resource Type
        /// </summary>
        /// <param name="craftable"></param>
        /// <returns></returns>
        public static CraftableType GetCraftable(Craftable craftable)
        {
            return craftables[craftable];
        }
    }
}

public enum Craftable
{
    Axe,
    Backpack,
    BasicRifle,
    Bow,
    Cloth,
    Coat,
    Hoe,
    Pickaxe
}

public enum Resource
{
    Stone,
    Wood,
    Fibers,
    Food,
    Coal,
    Leather,
    MachineParts
}
