﻿
using Engine;
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
public static List<Colonist> colonists=GameObjectManager.Filter<Colonist>().ToList();
public static List<Enemy> enemies = GameObjectManager.Filter<Enemy>().ToList();

public static Enemy FindColonistThreat (Colonist col) {
        foreach (Enemy enemy in enemies) {
        if (col.AttackRange>DistanceBetweenCombatants(enemy.Position,col.Position)) {
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

internal static void updateParticipants()
{
    GameObjectManager.ObjectAdded += ObjectAddedEventHandler;
    GameObjectManager.ObjectRemoved += ObjectRemovedEventHandler;
}

private static void ObjectRemovedEventHandler(object sender, GameObject GameObject)
{
    if (GameObject is Colonist)
    {
        colonists.Remove((Colonist)GameObject);
    }

    if (GameObject is Enemy enemy)
    {
        enemies.Remove((Enemy) GameObject);
    }
}

public static void ObjectAddedEventHandler(object sender, GameObject GameObject)
{
    if (GameObject is Colonist)
    {
        colonists.Add((Colonist)GameObject);
    }
    if (GameObject is Enemy enemy)
    {
        enemies.Add((Enemy)GameObject);
    }
}

public static Colonist ColonistInAggroRange(Enemy enemy)
{
    foreach (Colonist col in colonists)
    {
        if (enemy.aggroRange > DistanceBetweenCombatants(enemy.Position, col.Position))
        {           
            return col;
        }
    }
    return null;
}
}
}
