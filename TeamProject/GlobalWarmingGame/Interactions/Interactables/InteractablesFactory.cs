﻿using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.Interactions.Interactables.Animals;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.Interactions.Interactables.Enemies;
using GlobalWarmingGame.Interactions.Interactables.Environment;
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
            textures = new Dictionary<string, Texture2D>();
            textureSet = new Dictionary<string, Texture2D[][]>(); 
            textureSet.Add("colonist", new Texture2D[][]{
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
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/attackingColonist5")
                    },
                    new Texture2D[]{
                       contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/sprite1"),
                       contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/sprite0")
                    }
               });

            textureSet.Add("campFire", new Texture2D[][]    {
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_1")
                    },
                     new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_1"),
                        contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_2"),
                        contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_3"),
                        contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_4"),
                        contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_5")
                    }

            });

            textures.Add("farm", contentManager.Load<Texture2D>(@"textures/interactables/buildings/farm/sprite0"));
            textures.Add("bushH", contentManager.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite0"));
            textures.Add("bushN", contentManager.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite1"));

            textureSet.Add("bear", new Texture2D[][] {
                new Texture2D[]
                {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite4")


                },
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite2"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite3")
                    },
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite5"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/sprite6")

                    },
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bear/attackingBear")
                    }
            });

            textureSet.Add("robot", new Texture2D[][]  {
                    new Texture2D[]
                    {
                       contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite0")

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
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite9")


                    },
                      new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/sprite0")

                    },
                     new Texture2D[]
                    {

                        contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/attackingRobot1"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/robot/attackingRobot2")

                    }
                });

            textureSet.Add("rabbit", new Texture2D[][] {
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite0"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite1"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite2")
                    },
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite3"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite4")
                    },
                    new Texture2D[]
                    {
                       contentManager.Load<Texture2D>(@"textures/interactables/animals/rabbit2/sprite7"),
                    }
            });

            textures.Add("tree", contentManager.Load<Texture2D>(@"textures/interactables/environment/tree/sprite0"));
            textures.Add("treeStump", contentManager.Load<Texture2D>(@"textures/interactables/environment/tree/sprite2"));
            textures.Add("workBench", contentManager.Load<Texture2D>(@"textures/interactables/buildings/workbench"));
            textures.Add("stoneNode", contentManager.Load<Texture2D>(@"textures/interactables/environment/stone/stone_0"));
            textures.Add("tallGrass", contentManager.Load<Texture2D>(@"textures/interactables/environment/grass/tallgrass"));
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
                    return new Colonist(position, textureSet["colonist"]);
                case Interactable.Farm:
                    return new Farm(position, textures["farm"]);
                case Interactable.Rabbit:
                    return new Rabbit(position, textureSet["rabbit"]);
                case Interactable.Bush:
                    return new Bush(position, textures["bushH"], textures["bushN"]);
                case Interactable.Tree:
                    return new Tree(position, textures["tree"], textures["treeStump"]);
                case Interactable.WorkBench:
                    return new WorkBench(position, textures["workBench"]);
                case Interactable.StoneNode:
                    return new StoneNode(position, textures["stoneNode"]);
                case Interactable.TallGrass:
                    return new TallGrass(position, textures["tallGrass"]);
                case Interactable.CampFire:
                    return new CampFire(position, textureSet["campFire"]);
                case Interactable.Robot:
                    return new Robot(position, textureSet["robot"]);
                case Interactable.Bear:
                    return new Bear(position, textureSet["bear"]);
                default:
                    throw new NotImplementedException(interactable + " has not been implemented");
            }
        }


        [Obsolete]
        public static Colonist MakeColonist(Vector2 position)
        {
            return new Colonist(position, textureSet["colonist"]);
        }

        [Obsolete]
        public static Farm MakeFarm(Vector2 position)
        {
            return new Farm(position, textures["farm"]);
        }

        [Obsolete]
        public static Rabbit MakeRabbit(Vector2 position)
        {
            return new Rabbit(position, textureSet["rabbit"]);
        }

        [Obsolete]
        public static Bush MakeBush(Vector2 position)
        {
            return new Bush(position, textures["bushH"], textures["bushN"]);
        }

        [Obsolete]
        public static Tree MakeTree(Vector2 position)
        {
            return new Tree(position, textures["tree"], textures["treeStump"]);
        }

        [Obsolete]
        public static WorkBench MakeWorkBench(Vector2 position)
        {
            return new WorkBench(position, textures["workBench"]);
        }

        [Obsolete]
        public static StoneNode MakeStoneNode(Vector2 position)
        {
            return new StoneNode(position, textures["stoneNode"]);
        }

        [Obsolete]
        public static TallGrass MakeTallGrass(Vector2 position)
        {
            return new TallGrass(position, textures["tallGrass"]); 
        }

        [Obsolete]
        public static CampFire MakeCampfire(Vector2 position)
        {
            return new CampFire(position, textureSet["campFire"]); 
        }

        [Obsolete]
        public static Robot MakeRobot(Vector2 position)
        {
            return new Robot(position, textureSet["robot"]);
        }

        [Obsolete]
        public static Bear MakeBear(Vector2 position)
        {
            return new Bear(position, textureSet["bear"]);
        }
       
    }
}

public enum Interactable
{
    Bear,
    Robot,
    CampFire,
    TallGrass,
    StoneNode,
    WorkBench,
    Tree,
    Bush,
    Rabbit,
    Farm,
    Colonist,
}