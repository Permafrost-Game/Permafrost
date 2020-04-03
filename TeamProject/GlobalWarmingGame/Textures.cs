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
            Map.Add(TextureTypes.TowerF,            contentManager.Load<Texture2D>(@"textures/interactables/buildings/tower/final_tower"));
            Map.Add(TextureTypes.StorageUnit,       contentManager.Load<Texture2D>(@"textures/interactables/buildings/storage/sprite0"));
            Map.Add(TextureTypes.Loot,              contentManager.Load<Texture2D>(@"textures/interactables/environment/loot/loot-bag"));
            Map.Add(TextureTypes.Forge,             contentManager.Load<Texture2D>(@"textures/interactables/buildings/forge/forge"));

            #endregion

            #region Icon
            MapIcon.Add(TextureIconTypes.Stone,        contentManager.Load<Texture2D>(@"textures/icons/stone"));
            MapIcon.Add(TextureIconTypes.Wood,         contentManager.Load<Texture2D>(@"textures/icons/wood"));
            MapIcon.Add(TextureIconTypes.Coal,         contentManager.Load<Texture2D>(@"textures/icons/coal"));
            MapIcon.Add(TextureIconTypes.Fibers,       contentManager.Load<Texture2D>(@"textures/icons/fibers"));
            MapIcon.Add(TextureIconTypes.Apple,        contentManager.Load<Texture2D>(@"textures/icons/apple"));
            MapIcon.Add(TextureIconTypes.IronOre,      contentManager.Load<Texture2D>(@"textures/icons/ironOre"));
            MapIcon.Add(TextureIconTypes.IronIngot,    contentManager.Load<Texture2D>(@"textures/icons/ironIngot"));

            MapIcon.Add(TextureIconTypes.Leather,      contentManager.Load<Texture2D>(@"textures/icons/leather"));
            MapIcon.Add(TextureIconTypes.RobotCore,    contentManager.Load<Texture2D>(@"textures/icons/robotCore"));
            MapIcon.Add(TextureIconTypes.MachineParts, contentManager.Load<Texture2D>(@"textures/icons/machineparts"));

            MapIcon.Add(TextureIconTypes.Axe,          contentManager.Load<Texture2D>(@"textures/icons/axe"));
            MapIcon.Add(TextureIconTypes.Hoe,          contentManager.Load<Texture2D>(@"textures/icons/hoe"));
            MapIcon.Add(TextureIconTypes.Pickaxe,      contentManager.Load<Texture2D>(@"textures/icons/pickaxe"));

            MapIcon.Add(TextureIconTypes.CombatKnife,  contentManager.Load<Texture2D>(@"textures/icons/combatKnife"));
            MapIcon.Add(TextureIconTypes.Shotgun,      contentManager.Load<Texture2D>(@"textures/icons/shotgun"));
            MapIcon.Add(TextureIconTypes.MKIIShotgun,  contentManager.Load<Texture2D>(@"textures/icons/mkIIShotgun"));

            MapIcon.Add(TextureIconTypes.Cloth,        null);
            MapIcon.Add(TextureIconTypes.Coat,         contentManager.Load<Texture2D>(@"textures/icons/coat"));
            MapIcon.Add(TextureIconTypes.ThickCoat,    contentManager.Load<Texture2D>(@"textures/icons/thickCoat"));

            #endregion

            #region TextureSet
            MapSet.Add(TextureSetTypes.Colonist, new Texture2D[][]{
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
                    new Texture2D[]{
                       contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/sprite1"),
                       contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/sprite0"),
                    },
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/shotgunColonist1"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/shotgunColonist2"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/shotgunColonist3"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/shotgunColonist4"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/colonist/shotgunColonist5"),
                    }
               });

            MapSet.Add(TextureSetTypes.CampFire, new Texture2D[][]    {
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
                        contentManager.Load<Texture2D>(@"textures/interactables/buildings/campfire/sprite_5"),
                    }

            });

            MapSet.Add(TextureSetTypes.Bear, new Texture2D[][] {
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
            });

            MapSet.Add(TextureSetTypes.Robot, new Texture2D[][]  {
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
                });

            MapSet.Add(TextureSetTypes.Merchent, new Texture2D[][] {
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/merchent/sprite0"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/merchent/sprite1"),
                    },
                     new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/merchent/sprite0"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/merchent/sprite1"),

                    },
                      new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/merchent/sprite0"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/merchent/sprite1"),
                    },
            });

            MapSet.Add(TextureSetTypes.Rabbit, new Texture2D[][] {
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
            });

            MapSet.Add(TextureSetTypes.Fox, new Texture2D[][] {
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/fox/Wolf4"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/fox/Wolf5"),
                    },
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/fox/Wolf6"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/fox/Wolf7"),
                    },
                    new Texture2D[]
                    {
                       contentManager.Load<Texture2D>(@"textures/interactables/animals/fox/Wolf0"),
                       contentManager.Load<Texture2D>(@"textures/interactables/animals/fox/Wolf1"),
                    }
            });
            MapSet.Add(TextureSetTypes.Goat, new Texture2D[][] {
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/goat/Goat/sprite0"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/goat/Goat/sprite1"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/goat/Goat/sprite2"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/goat/Goat/sprite3"),
                    },
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/goat/Goat/sprite8"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/goat/Goat/sprite9"),
                    },
                    new Texture2D[]
                    {
                       contentManager.Load<Texture2D>(@"textures/interactables/animals/goat/Goat/sprite4"),
                       contentManager.Load<Texture2D>(@"textures/interactables/animals/goat/Goat/sprite5"),
                       contentManager.Load<Texture2D>(@"textures/interactables/animals/goat/Goat/sprite6"),
                       contentManager.Load<Texture2D>(@"textures/interactables/animals/goat/Goat/sprite7"),
                    }
            });
            MapSet.Add(TextureSetTypes.Bandit, new Texture2D[][]
                {
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/idlebandit"),
                    },
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/walkingbandit1"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/walkingbandit2"),
                    },
                    new Texture2D[]
                    {
                            contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/walkingbandit1"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/walkingbandit2"),
                    },
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/banditAttack1"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/banditAttack2"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/banditAttack3"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/banditAttack4"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/banditAttack5"),
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/banditAttack6"),
                    },
                    new Texture2D[]
                    {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/bandit/downedBandit"),

                }
            });

            MapSet.Add(TextureSetTypes.SmallRobot, new Texture2D[][]
            {
                new Texture2D[]
                {
                    contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/idleSmallRobot"),
                },
                new Texture2D[]
                {
                    contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/idleSmallRobot"),
                },
                new Texture2D[]
                {
                        contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotWalkUp"),

                },
                new Texture2D[]
                {
                    contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotFight1"),
                    contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotFight2"),
                    contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotFight3"),
                    contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotFight4"),
                    contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotFight5"),
                    contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/smallRobotFight6"),
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
                    contentManager.Load<Texture2D>(@"textures/interactables/animals/smallrobot/explosionRobot4"),

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
        TowerF,
        StorageUnit,
        Loot,
        Forge
    }

    public enum TextureIconTypes
    {
        Stone,
        IronOre,
        IronIngot,
        Wood,
        Coal,
        Fibers,
        Apple,
        Leather,
        MachineParts,
        Axe,
        Hoe,
        Pickaxe,
        Cloth,
        Coat,
        CombatKnife,
        Shotgun,
        RobotCore,
        MKIIShotgun,
        ThickCoat
    }

    public enum TextureSetTypes
    {
        Merchent,
        Colonist,
        CampFire,
        Bear,
        Robot,
        Rabbit,
        Bandit,
        SmallRobot,
        Fox,
        Goat
    }
}
