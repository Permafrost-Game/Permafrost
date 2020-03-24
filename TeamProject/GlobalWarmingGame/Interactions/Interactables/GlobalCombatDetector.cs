using Engine;
using GlobalWarmingGame.Interactions.Enemies;
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
                if (col.AttackRange>DistanceBetweenCombatants(enemy.Position,col.Position))
                {
                    return enemy;
                }    
            } 
            return null;
        }
        public static Colonist FindEnemyThreat(Enemy enemy)
        {
            foreach (Colonist col in colonists)
            {
                if (enemy.AttackRange > DistanceBetweenCombatants(enemy.Position, col.Position))
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

        /// <summary>
        /// Finds the closest colonist to the bear 
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns>Closest colonist in range</returns>
        public static Colonist ColonistInAggroRange(Enemy enemy)
        {
            Colonist closestColonist = null; 

            //Set to a large distance that could be between two combatants
            double closestDistanceBetweenCombatants = 3200;

            foreach (Colonist col in colonists)
            {
                double distanceBetweenCombatants = DistanceBetweenCombatants(enemy.Position, col.Position);

                //If colonist is in aggro range
                if (enemy.AggroRange > distanceBetweenCombatants)
                {
                    //First colonist in aggro range 
                    if (closestColonist == null)
                    {
                        closestColonist = col;
                        closestDistanceBetweenCombatants = distanceBetweenCombatants;
                    }
                    //Closest colonist in aggro range so far compared with next colonist in aggro range
                    else if (distanceBetweenCombatants < closestDistanceBetweenCombatants)
                    {
                        closestColonist = col;
                        closestDistanceBetweenCombatants = distanceBetweenCombatants;
                    }
                }
            }

            //Returns null if no colonists are in range
            return closestColonist;
        }
    }
}
