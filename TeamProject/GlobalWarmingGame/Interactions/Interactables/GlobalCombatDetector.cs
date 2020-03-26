using Engine;
using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.Interactions.Interactables.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalWarmingGame.Interactions.Interactables
{
    public class GlobalCombatDetector
    {
        public static List<Colonist> colonists=GameObjectManager.Filter<Colonist>().ToList();
        public static List<Enemy> enemies = GameObjectManager.Filter<Enemy>().ToList();
        public static Enemy FindColonistThreat (Colonist col)
        {
           
            foreach (Enemy enemy in enemies) {
               
                if (col.AttackRange>DistanceBetweenCombatants(enemy.Position,col.Position) && enemy.notDefeated)
                {
                    return enemy;
                }    
            } 
            return null;
        }
        /*public static Colonist FindEnemyThreat(Enemy enemy)
        {
            foreach (Colonist col in colonists)
            {
                if (enemy.AttackRange > DistanceBetweenCombatants(enemy.Position, col.Position))
                {
                    return col;
                }          
            }
            return null;
        }*/
        public static double DistanceBetweenCombatants(Vector2 myPos, Vector2 threatPos)
        {
            return Math.Sqrt((threatPos.X - myPos.X) * (threatPos.X - myPos.X) + (threatPos.Y - myPos.Y) * (threatPos.Y - myPos.Y));
        }

        internal static void UpdateParticipants()
        {
            GameObjectManager.ObjectAdded += ObjectAddedEventHandler;
            GameObjectManager.ObjectRemoved += ObjectRemovedEventHandler;
        }

        private static void ObjectRemovedEventHandler(object sender, GameObject GameObject)
        {
            if (GameObject is Colonist colonist)
            {
                colonists.Remove(colonist);
            }

            if (GameObject is Enemy enemy)
            {
                enemies.Remove(enemy);
            }
        }

        public static void ObjectAddedEventHandler(object sender, GameObject GameObject)
        {
            if (GameObject is Colonist colonist)
            {
                colonists.Add(colonist);
            }
            if (GameObject is Enemy enemy)
            {
                enemies.Add(enemy);
            }
        }

        public static Colonist ColonistInAggroRange(Enemy enemy)
        {
            Colonist closestColonist=null;
            double shortestDistance=Double.MaxValue;
            double distance;
            foreach (Colonist col in colonists)
            {
                distance = DistanceBetweenCombatants(enemy.Position, col.Position);
                
                    if (shortestDistance > distance) { 
                        shortestDistance = distance;
                        closestColonist = col;
                    }
            }
            return closestColonist;
        }
    }
}
