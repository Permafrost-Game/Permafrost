using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.PathFinding
{
    /// <summary>
    /// Modles a simiple random moving AI
    /// </summary>
    public class RandomAI
    {
        private static readonly Random Rand = new Random();

        
        /// <summary>base distance each move should make</summary>
        public float MoveDistance { get; set; }
        /// <summary>± applied to distance</summary>
        public float MoveDistanceVariation { get; set; }

        /// <summary>
        /// Creates a new <see cref="RandomAI"/> for random movement
        /// </summary>
        /// <param name="moveDistance">base distance each move should make</param>
        /// <param name="moveDistanceVariation">± distance each move should make</param>
        public RandomAI(float moveDistance, float moveDistanceVariation = 0f)
        {
            MoveDistance = moveDistance;
            MoveDistanceVariation = moveDistanceVariation;
        }

        /// <summary>
        /// Generates a random <see cref="Vector2"/> with a distance of <see cref="RandomAI.MoveDistance"/> ± <see cref="RandomAI.MoveDistanceVariation"/> from origin
        /// </summary>
        /// <returns>A <see cref="Vector2"/> translation</returns>
        /// <example>
        /// In an <see cref="IPathFindable"/> class
        /// <c>this.Goals.Enqueue(this.Position + ai.RandomTranslation());</c>
        /// </example>
        public Vector2 RandomTranslation()
        {
            int angle = Rand.Next(0, 361);
            float distance = MoveDistance + (MoveDistanceVariation * Rand.Next(-1, 2));

            return new Vector2
            {
                X = (float)Math.Cos(angle) * distance,
                Y = (float)Math.Sin(angle) * distance
            };
        }

    }
}
