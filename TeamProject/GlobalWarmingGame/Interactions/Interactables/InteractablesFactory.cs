using GlobalWarmingGame.Interactions.Enemies;
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
        public static Dictionary<string, Texture2D> textures;
        public static Dictionary<string, Texture2D[][]> textureSet;
        
         

        public static void LoadContent(ContentManager contentManager)
        {
            
            textures = new Dictionary<string, Texture2D>
            {
                { "farm", contentManager.Load<Texture2D>(@"textures/interactables/buildings/farm/sprite0") },
                { "bushH", contentManager.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite0") },
                { "bushN", contentManager.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite1") },
                { "tree", contentManager.Load<Texture2D>(@"textures/interactables/environment/tree/sprite0") },
                { "treeStump", contentManager.Load<Texture2D>(@"textures/interactables/environment/tree/sprite2") },
                { "workBench", contentManager.Load<Texture2D>(@"textures/interactables/buildings/workbench") },
                { "stoneNodeSmall", contentManager.Load<Texture2D>(@"textures/interactables/environment/stone/stone_0") },
                { "stoneNodeBig", contentManager.Load<Texture2D>(@"textures/interactables/environment/stone/stone_1") },
                { "tallGrass", contentManager.Load<Texture2D>(@"textures/interactables/environment/grass/tallgrass") },
                { "towerH", contentManager.Load<Texture2D>(@"textures/interactables/buildings/tower/hostile_tower") },
                { "towerC", contentManager.Load<Texture2D>(@"textures/interactables/buildings/tower/captured_tower") },
                { "loot", contentManager.Load<Texture2D>(@"textures/interactables/environment/loot/loot-bag") }
            };

            textureSet = new Dictionary<string, Texture2D[][]>
            {
                {
                    "SmallRobot",
                    new Texture2D[][]
                    {
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/idleSmallRobot"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/idleSmallRobot")                        
                        },
                        new Texture2D[]
                        {
                              contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotWalkUp")
                            
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotFight1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotFight2"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotFight3"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotFight4"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotFight5"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotFight6")
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotDies"),

                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/explosionRobot1"),
                             contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/explosionRobot2"),
                              contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/explosionRobot3"),
                               contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/explosionRobot4")

                        }
                    }
                },
                {
                    "colonist",
                    new Texture2D[][]
                    {
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/sprite0"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/attackingColonist1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/attackingColonist2"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/attackingColonist3"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/attackingColonist4"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/attackingColonist5"),
                        },
                        new Texture2D[]
                        {
                           contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/sprite1"),
                           contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/sprite0"),
                        }
                   }
                },
                {
                    "campFire",
                    new Texture2D[][]
                    {
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_1"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_2"),
                            contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_3"),
                            contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_4"),
                            contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_5"),
                        }
                    }
                },
                {
                    "bear",
                    new Texture2D[][]
                    {
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite4"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite2"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite3"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite5"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite6"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/attackingBear"),
                        }
                    }
                },
                {
                    "robot",
                    new Texture2D[][]
                    {
                        new Texture2D[]
                        {
                           contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite0"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite2"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite3"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite4"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite5"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite6"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite7"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite8"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite9"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite0"),
                        },
                        new Texture2D[]
                        {

                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/attackingRobot1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/attackingRobot2"),

                        }
                    }
                },{
                    "Bandit",
                    new Texture2D[][]
                    {
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/idlebandit"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/walkingbandit1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/walkingbandit2")
                        },
                        new Texture2D[]
                        {
                              contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/walkingbandit1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/walkingbandit2")
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/banditAttack1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/banditAttack2"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/banditAttack3"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/banditAttack4"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/banditAttack5"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/banditAttack6")
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/downedBandit"),

                        }
                    }
                },
                {
                    "rabbit",
                    new Texture2D[][]
                    {
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite0"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite1"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite2"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite3"),
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite4"),
                        },
                        new Texture2D[]
                        {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite7"),
                        }
                    }
                }
            };

        }

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
                case Interactable.StoneNodeSmall:
                    return new SmallStoneNode(position);
                case Interactable.StoneNodeBig:
                    return new BigStoneNode(position);
                case Interactable.loot:
                    List<ResourceItem> loot = new List<ResourceItem>();
                    loot.Add(new ResourceItem(Resource.Shotgun, 1));
                    return new Loot(loot, position);
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
    Tree,
    Bush,
    Rabbit,
    Farm,
    Colonist,
    Tower,
    Storage,
    loot,
    Bandit,
    SmallRobot,
}