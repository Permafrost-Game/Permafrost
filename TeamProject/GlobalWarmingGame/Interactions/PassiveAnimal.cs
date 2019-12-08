
using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    abstract class PassiveAnimal: AnimatedSprite, IInteractable, IPathFindable
    {
        public Queue<Vector2> Goals { get; set; }
        public Queue<Vector2> Path { get; set; }
        public float Speed { get; set; }

        public List<InstructionType> InstructionTypes { get; }

        protected RandomAI ai;

        public PassiveAnimal(Vector2 position, string tag, Texture2D[][] textureSet, float speed, RandomAI ai, float frameTime = 10f) : base
        (
            position: position,
            size: new Vector2(textureSet[0][0].Width, textureSet[0][0].Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: tag,
            depth: 0.9f,
            textureSet: textureSet,
            frameTime: frameTime
        )
        {
            this.Speed = speed;
            this.ai = ai;
            this.InstructionTypes = new List<InstructionType>();

        }

        public void OnGoalComplete(Vector2 completedGoal)
        {
            Goals.Enqueue(this.Position + ai.RandomTranslation());
        }
    }
}
