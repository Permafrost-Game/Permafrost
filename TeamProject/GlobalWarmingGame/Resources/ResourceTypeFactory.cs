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

        private static void addResource(ResourceType resource)
        {
            resources.Add(resource.ResourceID, resource);
        }

        public static void Init()
        {
            resources  = new Dictionary<Resource, ResourceType>();

            //Populate resources dictionary
            addResource(new ResourceType(Resource.Stone, "Stone", "A piece of rock", TextureIconTypes.Stone));
            addResource(new ResourceType(Resource.Wood, "Wood", "A wooden log", TextureIconTypes.Wood));
            addResource(new ResourceType(Resource.Fibers, "Fibers", "A bundle of fibers", TextureIconTypes.Fibers));
            addResource(new ResourceType(Resource.Food, "Food", "Food", TextureIconTypes.Apple));
            addResource(new ResourceType(Resource.Leather, "Leather", "Tanned leather", TextureIconTypes.Leather)); //TODO Needs texture
            addResource(new ResourceType(Resource.MachineParts, "Machine Parts", "Machine parts", TextureIconTypes.MachineParts));
            //Add a wheat resource to replace food from farm

            //Teir 1 Craftable Resource Type definitions

            addResource(new ResourceType(Resource.Axe, "Axe", "An axe", TextureIconTypes.Axe));
            addResource(new ResourceType(Resource.Hoe, "Hoe", "A hoe", TextureIconTypes.Hoe));
            addResource(new ResourceType(Resource.Pickaxe, "Pickaxe", "A Pickaxe", TextureIconTypes.Pickaxe));
            addResource(new ResourceType(Resource.Cloth, "Cloth", "A piece of cloth", TextureIconTypes.Cloth)); //TODO Needs texture
            addResource(new ResourceType(Resource.Coat, "Coat", "A basic coat", TextureIconTypes.Coat)); //TODO Needs texture
            addResource(new ResourceType(Resource.Shotgun, "Shotgun", "A Shotgun", TextureIconTypes.Shotgun));

            addResource(new ResourceType(Resource.RobotCore, "Robot Core", "A Core", TextureIconTypes.RobotCore));


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
    Cloth,
    Coat,
    Hoe,
    Pickaxe,
    Shotgun,
    RobotCore
}
