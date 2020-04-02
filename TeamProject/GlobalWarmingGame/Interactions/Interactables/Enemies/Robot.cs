using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Enemies
{
    public class Robot : Enemy, IReconstructable
    {
        [PFSerializable]
        public float PFSHealth
        {
            get { return Health; }
            set { Health = value; }
        }

        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        private readonly List<ResourceItem> loot = new List<ResourceItem>
            {
                new ResourceItem(Resource.MachineParts, 2)
            };

        public Robot() : base("", 0, 0, 0, 0, Vector2.Zero, TextureSetTypes.Robot)
        {

        }

        public Robot(Vector2 position, int hp = 500) : base("Robot",5000, 70, 0, hp, position, TextureSetTypes.Robot)
        {
        
        }

        public override void AnimateAttack()
        {
                isAnimated = true;
                this.TextureGroupIndex = 3;    
 
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
           
        }

        protected override void ChaseColonist(Colonist colonist)
        {
            Vector2 fakeLeftXcoordinate = new Vector2(colonist.Position.X - 40, colonist.Position.Y);
            Vector2 fakeRightXcoordinate = new Vector2(colonist.Position.X + 40, colonist.Position.Y);
            if (this.Position.X < colonist.Position.X)
            {
                Goals.Enqueue(fakeLeftXcoordinate);
            }
            else
            {
                Goals.Enqueue(fakeRightXcoordinate);
            }
        }

        internal override void AttackingSound()
        {
            SoundFactory.PlaySoundEffect(Sound.robotShock);
        }

        public override void EnemyAttack(GameTime gameTime) {
            Random dmg = new Random();
            AttackPower = dmg.Next(12, 30);
            base.EnemyAttack(gameTime);
        }

        internal override void DeathSound()
        {
            SoundFactory.PlaySoundEffect(Sound.robotBreak);
        }

        protected override void SetDead()
        {
            //remove the enemy from the game 
            this.DeathSound();
            notDefeated = false;
            GameObjectManager.Add(new Loot(loot, this.Position));
            GameObjectManager.Remove(this);
        }

        public object Reconstruct()
        {
            return new Robot(PFSPosition, (int)PFSHealth);
        }
    }
}
