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
        private static Dictionary<Resource, ResourceType> resources;
        private static Dictionary<Craftable, CraftableType> craftables;

        public static void LoadContent(ContentManager contentManger)
        {
            resources = new Dictionary<Resource, ResourceType>();
            craftables = new Dictionary<Craftable, CraftableType>();

            //Resource Type definitions
            ResourceType stone = new ResourceType("stone", "Stone", "A piece of rock", 1f, contentManger.Load<Texture2D>(@"textures/icons/stone"));
            ResourceType wood = new ResourceType("wood", "Wood", "A wooden log", 1f, contentManger.Load<Texture2D>(@"textures/icons/wood"));
            ResourceType fibers = new ResourceType("fibers", "Fibers", "A bundle of fibers", 1f, contentManger.Load<Texture2D>(@"textures/icons/fibers"));
            ResourceType food = new ResourceType("food", "Food", "Food", 1f, contentManger.Load<Texture2D>(@"textures/icons/apple"));
            ResourceType coal = new ResourceType("coal", "Coal", "A lump of coal", 1f, null); //Needs texture
            ResourceType leather = new ResourceType("leather", "Leather", "Tanned leather", 1f, null); //Needs texture
            ResourceType machineParts = new ResourceType("machineParts", "Machine Parts", "Machine parts", 1f, null); //Needs texture

            //Populate resources dictionary
            resources.Add(Resource.stone, stone);
            resources.Add(Resource.wood, wood);
            resources.Add(Resource.fibers, fibers);
            resources.Add(Resource.food, food);
            resources.Add(Resource.coal, coal);
            resources.Add(Resource.leather, leather);
            resources.Add(Resource.machineParts, machineParts);

            //Craftable Type definitions and populate craftables dictionary
            //Axe
            Texture2D axe = contentManger.Load<Texture2D>(@"textures/icons/axe");
            craftables.Add(Craftable.axe, new CraftableType("axe", "Axe", "An Axe", 5f, axe));
            //Hoe
            Texture2D hoe = contentManger.Load<Texture2D>(@"textures/icons/hoe");
            craftables.Add(Craftable.hoe, new CraftableType("hoe", "Hoe", "A hoe", 5f, hoe));
            //Pickaxe
            Texture2D pickaxe = contentManger.Load<Texture2D>(@"textures/icons/pickaxe");
            craftables.Add(Craftable.pickaxe, new CraftableType("pickaxe", "Pickaxe", "A Pickaxe", 10f, pickaxe));
            //Backpack
            craftables.Add(Craftable.backpack, new CraftableType("backpack", "Backpack", "A backpack", 2f, null)); //Needs texture
            //BasicRifle
            craftables.Add(Craftable.basicRifle, new CraftableType("basicRifle", "BasicRifle", "A basic rifle", 10f, null)); //Needs texture
            //Bow
            craftables.Add(Craftable.bow, new CraftableType("bow", "bow", "A bow with arrows", 5f, null)); //Needs texture
            //Cloth
            craftables.Add(Craftable.cloth, new CraftableType("cloth", "Cloth", "A piece of cloth", 1f, null)); //Needs texture
            //Coat
            craftables.Add(Craftable.coat, new CraftableType("coat", "Coat", "A basic coat", 5f, null)); //Needs texture


            //The crafting costs need to be set after all the resources and craftables types have been created.
            #region Teir 1 crafting costs
            //Axe
            List<ResourceItem> axeCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeResource(Resource.wood), 1),
                                                                                     new ResourceItem(MakeResource(Resource.fibers), 2),
                                                                                     new ResourceItem(MakeResource(Resource.stone), 1) };
            craftables[Craftable.axe].CraftingCosts = axeCraftingCosts;

            //Hoe
            List<ResourceItem> hoeCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeResource(Resource.wood), 1),
                                                                                     new ResourceItem(MakeResource(Resource.fibers), 2),
                                                                                     new ResourceItem(MakeResource(Resource.stone), 1) };
            craftables[Craftable.hoe].CraftingCosts = hoeCraftingCosts;

            //Pickaxe
            List<ResourceItem> pickaxeCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeResource(Resource.wood), 1),
                                                                                         new ResourceItem(MakeResource(Resource.fibers), 2),
                                                                                         new ResourceItem(MakeResource(Resource.stone), 2) };
            craftables[Craftable.pickaxe].CraftingCosts = pickaxeCraftingCosts;

            //Backpack
            List<ResourceItem> backpackCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeCraftable(Craftable.cloth), 2),
                                                                                          new ResourceItem(MakeResource(Resource.leather), 5) };
            craftables[Craftable.backpack].CraftingCosts = backpackCraftingCosts;

            //BasicRifle
            List<ResourceItem> basicRifleCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeResource(Resource.wood), 8),
                                                                                            new ResourceItem(MakeResource(Resource.leather), 2),
                                                                                            new ResourceItem(MakeResource(Resource.machineParts), 4) };
            craftables[Craftable.basicRifle].CraftingCosts = basicRifleCraftingCosts;

            //Bow
            List<ResourceItem> bowCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeResource(Resource.wood), 4),
                                                                                     new ResourceItem(MakeResource(Resource.fibers), 6),
                                                                                     new ResourceItem(MakeResource(Resource.stone), 1) };
            craftables[Craftable.bow].CraftingCosts = bowCraftingCosts;

            //Cloth
            List<ResourceItem> clothCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeResource(Resource.fibers), 4) };
            craftables[Craftable.cloth].CraftingCosts = clothCraftingCosts;

            //Coat
            List<ResourceItem> coatCraftingCosts = new List<ResourceItem>() { new ResourceItem(MakeCraftable(Craftable.cloth), 4),
                                                                                      new ResourceItem(MakeResource(Resource.leather), 2) };
            craftables[Craftable.coat].CraftingCosts = coatCraftingCosts;
            #endregion
        }

        public static ResourceType MakeResource(Resource resource)
        {
            return resources[resource];
        }

        public static CraftableType MakeCraftable(Craftable craftable)
        {
            return craftables[craftable];
        }
    }
}

public enum Craftable
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
