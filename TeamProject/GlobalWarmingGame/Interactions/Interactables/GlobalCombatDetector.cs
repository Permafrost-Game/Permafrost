
using GlobalWarmingGame.Interactions.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables
{
   public class GlobalCombatDetector
    {
       public static IEnumerable<Colonist> colonists;
        public static IEnumerable<Enemy> enemies;

        public static void Initialize() {
            
            colonists =  GameObjectManager.Filter<Colonist>();
            enemies = GameObjectManager.Filter<Enemy>();
        }

        public static Enemy FindColonistThreat (Colonist col) { 
                foreach (Enemy enemy in enemies) {
                if (col.attackRange>DistanceBetweenCombatants(col.Position,enemy.Position)) {
                }
                return enemy;
                }
            return null;
        }
        public static Colonist FindEnemyThreat(Enemy enemy)
        {


            foreach (Colonist col in colonists)
            {
                if (enemy.attackRange > DistanceBetweenCombatants(enemy.Position, col.Position))
                {
                    return col;
                }
                
            }
            return null;
        }

        private static double DistanceBetweenCombatants(Vector2 myPos, Vector2 threatPos)
        {
            return Math.Sqrt((threatPos.X - myPos.X) * (threatPos.X - myPos.X) + (threatPos.Y - myPos.Y) * (threatPos.Y - myPos.Y));
        }

    }

       

    }
