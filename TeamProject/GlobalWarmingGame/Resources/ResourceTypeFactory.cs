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
        private static Dictionary<string, Texture2D> resources;
        private static Dictionary<string, Texture2D> craftables;

        public static void LoadContent(ContentManager contentManger)
        {
            resources = new Dictionary<string, Texture2D>();
            craftables = new Dictionary<string, Texture2D>();

            resources.Add("stone", contentManger.Load<Texture2D>(@"textures/icons/stone"));
            resources.Add("wood", contentManger.Load<Texture2D>(@"textures/icons/wood"));
            resources.Add("fibers", contentManger.Load<Texture2D>(@"textures/icons/fibers"));
            resources.Add("apple", contentManger.Load<Texture2D>(@"textures/icons/apple"));

            craftables.Add("axe", contentManger.Load<Texture2D>(@"textures/icons/axe"));
            craftables.Add("pickaxe", contentManger.Load<Texture2D>(@"textures/icons/pickaxe"));
            craftables.Add("hoe", contentManger.Load<Texture2D>(@"textures/icons/hoe"));
        }

        public static ResourceType MakeResource(Resource resource)
        {
            switch (resource)
            {
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

        public static CraftableType MakeCraftable(Craft craft)
        {
            switch (craft)
            {
                case Craft.axe:

                    List<ResourceItem> axeCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeResource(Resource.wood), 1), 
                                                                                     new ResourceItem(MakeResource(Resource.fibers), 2),
                                                                                     new ResourceItem(MakeResource(Resource.stone), 1) };

                    return CreationHelper("axe", "Axe", "An Axe", 5f, craftables["axe"], axeCraftingCosts);

                case Craft.backpack:

                    List<ResourceItem> backpackCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeCraftable(Craft.cloth), 2), 
                                                                                          new ResourceItem(MakeResource(Resource.leather), 5) };

                    return CreationHelper("backpack", "Backpack", "A backpack", 2f, null, backpackCraftingCosts); //Needs texture

                case Craft.basicRifle:

                    List<ResourceItem> basicRifleCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeResource(Resource.wood), 8), 
                                                                                            new ResourceItem(MakeResource(Resource.leather), 2),
                                                                                            new ResourceItem(MakeResource(Resource.machineParts), 4) };

                    return CreationHelper("basicRifle", "BasicRifle", "A basic rifle", 10f, null, basicRifleCraftingCosts); //Needs texture

                case Craft.bow:

                    List<ResourceItem> bowCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeResource(Resource.wood), 4), 
                                                                                     new ResourceItem(MakeResource(Resource.fibers), 6),
                                                                                     new ResourceItem(MakeResource(Resource.stone), 1) };

                    return CreationHelper("bow", "bow", "A bow with arrows", 5f, null, bowCraftingCosts); //Needs texture

                case Craft.cloth:

                    List<ResourceItem> clothCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeResource(Resource.fibers), 4) };

                    return CreationHelper("cloth", "Cloth", "A piece of cloth", 1f, null, clothCraftingCosts); //Needs texture

                case Craft.coat:

                    List<ResourceItem> coatCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeCraftable(Craft.cloth), 4), 
                                                                                      new ResourceItem(MakeResource(Resource.leather), 2) };

                    return CreationHelper("coat", "Coat", "A basic coat", 5f, null, coatCraftingCosts); //Needs texture

                case Craft.hoe:

                    List<ResourceItem> hoeCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeResource(Resource.wood), 1), 
                                                                                     new ResourceItem(MakeResource(Resource.fibers), 2),
                                                                                     new ResourceItem(MakeResource(Resource.stone), 1) };

                    return CreationHelper("hoe", "Hoe", "A hoe", 5f, craftables["hoe"], hoeCraftingCosts); 

                case Craft.pickaxe:

                    List<ResourceItem> pickaxeCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeResource(Resource.wood), 1), 
                                                                                         new ResourceItem(MakeResource(Resource.fibers), 2),
                                                                                         new ResourceItem(MakeResource(Resource.stone), 2) };


                    return CreationHelper("pickaxe", "Pickaxe", "A Pickaxe", 10f, craftables["pickaxe"], pickaxeCraftingCosts); 

                default: return null;
            }
        }

        public static CraftableType CreationHelper(string id, string displayName, string description, float weight, Texture2D texture, List<ResourceItem> craftingCosts) 
        {
            CraftableType craftable = new CraftableType(id, displayName, description, weight, texture);
            craftable.CraftingCosts = craftingCosts;

            return craftable;
        }

    }

    public enum Craft
    {
        axe,
        backpack,
        basicRifle,
        bow,
        cloth,
        coat,
        hoe,
        pickaxe
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
