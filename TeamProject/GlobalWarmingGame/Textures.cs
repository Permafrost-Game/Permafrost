using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    static class Textures
    {
        public static IDictionary<TextureTypes, Texture2D> Map { get; }
        public static IDictionary<TextureSetTypes, Texture2D[][]> MapSet { get; }

        static Textures()
        {
            Map = new Dictionary<TextureTypes, Texture2D>();
            MapSet = new Dictionary<TextureSetTypes, Texture2D[][]>();
        }

        public static void LoadContent(ContentManager contentManager)
        {
            Map.Add(TextureTypes.farm, contentManager.Load<Texture2D>(@"textures/interactables/buildings/farm/sprite0"));
            Map.Add(TextureTypes.bushH, contentManager.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite0"));
            Map.Add(TextureTypes.bushN, contentManager.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite1"));
            Map.Add(TextureTypes.tree, contentManager.Load<Texture2D>(@"textures/interactables/environment/tree/sprite0"));
            Map.Add(TextureTypes.treeStump, contentManager.Load<Texture2D>(@"textures/interactables/environment/tree/sprite2"));
            Map.Add(TextureTypes.workBench, contentManager.Load<Texture2D>(@"textures/interactables/buildings/workbench"));
            Map.Add(TextureTypes.stoneNode, contentManager.Load<Texture2D>(@"textures/interactables/environment/stone/stone_0"));
            Map.Add(TextureTypes.tallGrass, contentManager.Load<Texture2D>(@"textures/interactables/environment/grass/tallgrass"));

            MapSet.Add(TextureSetTypes.colonist, new Texture2D[][]{
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

            MapSet.Add(TextureSetTypes.campFire, new Texture2D[][]    {
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

            MapSet.Add(TextureSetTypes.bear, new Texture2D[][] {
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

            MapSet.Add(TextureSetTypes.robot, new Texture2D[][]  {
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

            MapSet.Add(TextureSetTypes.rabbit, new Texture2D[][] {
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
        }
    }

    public enum TextureTypes
    {
        farm,
        bushH,
        bushN,
        tree,
        treeStump,
        workBench,
        stoneNode,
        tallGrass
    }

    public enum TextureSetTypes
    {
        colonist,
        campFire,
        bear,
        robot,
        rabbit
    }
}