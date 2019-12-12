using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.PathFinding
{
    public interface IPathFindable
    {
        Queue<Vector2> Goals { get; set; }
        Queue<Vector2> Path { get; set; }
        float Speed { get; set; }

        void OnGoalComplete(Vector2 completedGoal);
    }
}
