using GlobalWarmingGame.ResourceItems;
using System.Collections.Generic;

namespace GlobalWarmingGame.Resources
{
    /// <summary>
    /// An nice and easy way to create Resource types
    /// </summary>
    public static class ResourceTypeFactory
    {
        private static Dictionary<Resource, ResourceType> resources;

        private static void AddResource(ResourceType resource)
        {
            resources.Add(resource.ResourceID, resource);
        }

        public static void Init()
        {
            resources  = new Dictionary<Resource, ResourceType>();

            //Populate resources dictionary
            AddResource(new ResourceType(Resource.Stone, "Stone", "A piece of rock", TextureIconTypes.Stone));
            AddResource(new ResourceType(Resource.Wood, "Wood", "A wooden log", TextureIconTypes.Wood));
            AddResource(new ResourceType(Resource.Coal, "Coal", "A lump of coal", TextureIconTypes.Coal));
            AddResource(new ResourceType(Resource.Fibers, "Fibers", "A bundle of fibers", TextureIconTypes.Fibers));
            AddResource(new ResourceType(Resource.Food, "Food", "Food", TextureIconTypes.Apple));
            AddResource(new ResourceType(Resource.Leather, "Leather", "Tanned leather", TextureIconTypes.Leather)); 
            AddResource(new ResourceType(Resource.MachineParts, "Machine Parts", "Machine parts", TextureIconTypes.MachineParts));
            AddResource(new ResourceType(Resource.IronOre, "Iron", "A iron ore", TextureIconTypes.IronOre));
            AddResource(new ResourceType(Resource.RobotCore, "Robot Core", "A Core", TextureIconTypes.RobotCore));

            //Teir 1 Craftable Resource Type definitions

            AddResource(new ResourceType(Resource.Cloth, "Cloth", "A piece of cloth", TextureIconTypes.Cloth)); 
            AddResource(new ResourceType(Resource.Coat, "Coat", "A basic coat", TextureIconTypes.Coat)); 
            AddResource(new ResourceType(Resource.ThickCoat, "ThickCoat", "A thick coat", TextureIconTypes.ThickCoat)); 
            AddResource(new ResourceType(Resource.Shotgun, "Shotgun", "A Shotgun", TextureIconTypes.Shotgun));

            //Teir 2 Craftable Resource Type definitions
            AddResource(new ResourceType(Resource.Axe, "Axe", "An axe", TextureIconTypes.Axe));
            AddResource(new ResourceType(Resource.Hoe, "Hoe", "A hoe", TextureIconTypes.Hoe));
            AddResource(new ResourceType(Resource.Pickaxe, "Pickaxe", "A Pickaxe", TextureIconTypes.Pickaxe));
            AddResource(new ResourceType(Resource.CombatKnife, "CombatKnife", "A CombatKnife", TextureIconTypes.CombatKnife));
            AddResource(new ResourceType(Resource.IronIngot, "Iron", "A iron ingot", TextureIconTypes.IronIngot));
            AddResource(new ResourceType(Resource.MKIIShotgun, "Shotgun MKII", "A MKII Shotgun", TextureIconTypes.MKIIShotgun));


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
    IronOre,
    IronIngot,
    Wood,
    Fibers,
    Food,
    Coal,
    Leather,
    MachineParts,
    Axe,
    Cloth,
    Coat,
    Hoe,
    Pickaxe,
    CombatKnife,
    Shotgun,
    RobotCore,
    MKIIShotgun,
    ThickCoat
}
