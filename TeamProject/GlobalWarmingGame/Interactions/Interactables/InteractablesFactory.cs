using GlobalWarmingGame.Interactions.Interactables.Animals;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.Interactions.Interactables.Enemies;
using GlobalWarmingGame.Interactions.Interactables.Environment;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    /// <summary>
    /// A nice and easy way to create interactables
    /// </summary>
    public static class InteractablesFactory
    {

        /// <summary>
        /// Create an Interable given a Enum and a position
        /// </summary>
        /// <param name="interactable"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static IInteractable MakeInteractable(Interactable interactable, Vector2 position)
        {
            switch(interactable)
            {
                case Interactable.Colonist:
                    return new Colonist(position);
                case Interactable.Merchant:
                    return new Merchant(position);
                case Interactable.Farm:
                    return new Farm(position);
                case Interactable.Rabbit:
                    return new Rabbit(position);
                case Interactable.Bush:
                    return new Bush(position);
                case Interactable.Tree:
                    return new Tree(position);
                case Interactable.WorkBench:
                    return new WorkBench(position);
                case Interactable.Forge:
                    return new Forge(position);
                case Interactable.StoneNodeSmall:
                    return new SmallStoneNode(position);
                case Interactable.StoneNodeBig:
                    return new BigStoneNode(position);
                case Interactable.loot:
                    return new Loot(new List<ResourceItem> { new ResourceItem(Resource.Shotgun, 1) }, position);
                case Interactable.TallGrass:
                    return new TallGrass(position);
                case Interactable.CampFire:
                    return new CampFire(position);
                case Interactable.Robot:
                    return new Robot(position);
                case Interactable.Bear:
                    return new Bear(position);
                case Interactable.Bandit:
                    return new Bandit(position);
                case Interactable.SmallRobot:
                    return new SmallRobot(position);
                case Interactable.Tower:
                    return new Tower(position);
                case Interactable.Storage:
                    return new StorageUnit(position);
                case Interactable.BridgeTile:
                    return new BridgeTile(position);
                default:
                    throw new NotImplementedException(interactable + " has not been implemented");
            }
        }
    }
}

public enum Interactable
{
    Bear,
    Robot,
    CampFire,
    TallGrass,
    StoneNodeSmall,
    StoneNodeBig,
    WorkBench,
    Forge,
    Tree,
    Bush,
    Rabbit,
    Farm,
    Colonist,
    Merchant,
    Tower,
    Storage,
    loot,
    Bandit,
    SmallRobot,
    BridgeTile
}
