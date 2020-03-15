using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    static class Textures
    {
        public static IDictionary<TextureTypes, Texture2D> Map { get; }

        public static IDictionary<TextureIconTypes, Texture2D> MapIcon { get; }

        public static IDictionary<TextureSetTypes, Texture2D[][]> MapSet { get; }

        static Textures()
        {
            Map = new Dictionary<TextureTypes, Texture2D>();
            MapIcon = new Dictionary<TextureIconTypes, Texture2D>();
            MapSet = new Dictionary<TextureSetTypes, Texture2D[][]>();
        }

        public static void LoadContent(ContentManager contentManager)
        {
            #region Textures
            Map.Add(TextureTypes.Farm,              contentManager.Load<Texture2D>(@"textures/interactables/buildings/farm/sprite0"));
            Map.Add(TextureTypes.BushH,             contentManager.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite0"));
            Map.Add(TextureTypes.BushN,             contentManager.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite1"));
            Map.Add(TextureTypes.Tree,              contentManager.Load<Texture2D>(@"textures/interactables/environment/tree/sprite0"));
            Map.Add(TextureTypes.TreeStump,         contentManager.Load<Texture2D>(@"textures/interactables/environment/tree/sprite2"));
            Map.Add(TextureTypes.WorkBench,         contentManager.Load<Texture2D>(@"textures/interactables/buildings/workbench"));
            Map.Add(TextureTypes.SmallStoneNode,    contentManager.Load<Texture2D>(@"textures/interactables/environment/stone/stone_0"));
            Map.Add(TextureTypes.BigStoneNode,      contentManager.Load<Texture2D>(@"textures/interactables/environment/stone/stone_1"));
            Map.Add(TextureTypes.TallGrass,         contentManager.Load<Texture2D>(@"textures/interactables/environment/grass/tallgrass"));
            Map.Add(TextureTypes.TowerC,            contentManager.Load<Texture2D>(@"textures/interactables/buildings/tower/captured_tower"));
            Map.Add(TextureTypes.TowerH,            contentManager.Load<Texture2D>(@"textures/interactables/buildings/tower/hostile_tower"));
            Map.Add(TextureTypes.StorageUnit,       contentManager.Load<Texture2D>(@"textures/interactables/buildings/storage/sprite0"));

            #endregion

            #region Icon
            MapIcon.Add(TextureIconTypes.stone, contentManager.Load<Texture2D>(@"textures/icons/stone"));
            MapIcon.Add(TextureIconTypes.wood, contentManager.Load<Texture2D>(@"textures/icons/wood"));
            MapIcon.Add(TextureIconTypes.fibers, contentManager.Load<Texture2D>(@"textures/icons/fibers"));
            MapIcon.Add(TextureIconTypes.apple, contentManager.Load<Texture2D>(@"textures/icons/apple"));

            /* todo */
            MapIcon.Add(TextureIconTypes.coal, null);
            MapIcon.Add(TextureIconTypes.leather, null);
            MapIcon.Add(TextureIconTypes.machineParts, null);

            MapIcon.Add(TextureIconTypes.axe, contentManager.Load<Texture2D>(@"textures/icons/axe"));
            MapIcon.Add(TextureIconTypes.hoe, contentManager.Load<Texture2D>(@"textures/icons/hoe"));
            MapIcon.Add(TextureIconTypes.pickaxe, contentManager.Load<Texture2D>(@"textures/icons/pickaxe"));

            /* todo */
            MapIcon.Add(TextureIconTypes.backpack, null);
            MapIcon.Add(TextureIconTypes.basicRifle, null);
            MapIcon.Add(TextureIconTypes.bow, null);
            MapIcon.Add(TextureIconTypes.cloth, null);
            MapIcon.Add(TextureIconTypes.coat, null);

            #endregion

            #region TextureSet
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

            #endregion
        }
    }

    public enum TextureTypes
    {
        Farm,
        BushH,
        BushN,
        Tree,
        TreeStump,
        WorkBench,
        SmallStoneNode,
        BigStoneNode,
        TallGrass,
        TowerC,
        TowerH,
        StorageUnit,
    }

    public enum TextureIconTypes
    {
        stone,
        wood,
        fibers,
        apple,
        coal,
        leather,
        machineParts,
        axe,
        hoe,
        pickaxe,
        backpack,
        basicRifle,
        bow,
        cloth,
        coat
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