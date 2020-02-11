using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.Interactions.Interactables.Animals;
using GlobalWarmingGame.Interactions.Interactables.Environment;
using GlobalWarmingGame.Interactions.Enemies;

namespace GlobalWarmingGame
{
    public static class InteractablesFactory
    {
        public static Dictionary<String, Texture2D> textures;
        public static Dictionary<String, Texture2D[][]> textureSet;

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

        public static Colonist MakeColonist(Vector2 position)
        {
            return new Colonist(position, textureSet["colonist"]);
        }

        public static Farm MakeFarm(Vector2 position)
        {
            return new Farm(position, textures["farm"]);
        }
        public static Rabbit MakeRabbit(Vector2 position)
        {
            return new Rabbit(position, textureSet["rabbit"]);
        }
        public static Bush MakeBush(Vector2 position)
        {
            return new Bush(position, textures["BushH"], textures["BushN"]);
        }
        public static Tree MakeTree(Vector2 position)
        {
            return new Tree(position, textures["tree"], textures["treeStump"]);
        }
        public static WorkBench MakeWorkBench(Vector2 position)
        {
            return new WorkBench(position, textures["workBench"]);
        }
        public static StoneNode MakeStoneNode(Vector2 position)
        {
            return new StoneNode(position, textures["stoneNode"]);
        }
        public static TallGrass MakeTallGrass(Vector2 position)
        {
            return new TallGrass(position, textures["tallGrass"]); 
        }
        public static CampFire MakeCampfire(Vector2 position)
        {
            return new CampFire(position, textureSet["campfire"]); 
        }
        public static Enemy MakeRobot(Vector2 position)
        {
            return new Enemy("Robot", 5000, 60, 0, 500,position, textureSet["robot"]);
        }
        public static Enemy MakeBear(Vector2 position)
        {
            return new Enemy("Bear", 1000, 60, 10, 300, position, textureSet["bear"]);
        }
       
    }
}
