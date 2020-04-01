using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables.Enemies
{
    class SmallRobot : Enemy, IReconstructable
    {
       
        private bool robotExploded=false;

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

        public SmallRobot() : base("", 0, 0, 0, 0, Vector2.Zero, TextureSetTypes.smallRobot)
        {

        }

        public SmallRobot(Vector2 position, int hp = 500) : base("SmallRobot", 1000, 70, 0, hp, position, TextureSetTypes.smallRobot)
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
                if (robotExploded) {
                    GameObjectManager.Remove(this);
                }

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
            SoundFactory.PlaySoundEffect(Sound.robotFire);
        }

        public override void EnemyAttack(GameTime gameTime)
        {
            Random dmg = new Random();
            AttackPower = dmg.Next(1, 10);
            base.EnemyAttack(gameTime);
        }

        internal override void DeathSound()
        {
            SoundFactory.PlaySoundEffect(Sound.robotBreak);
        }
        internal override List<ResourceItem> Loot()
        {
            List<ResourceItem> loot = new List<ResourceItem>();
            loot.Add(new ResourceItem(Resource.robotCore, 1));
            return loot;
        }

        public override void SetEnemyDead()
        {
                //remove the enemy from the game 
                if (notDefeated)
                {
                    isInCombat = false;
                    Goals.Clear();
                    notDefeated = false;
                    TextureGroupIndex = 4;
                    this.DeathSound();
                    InstructionTypes.Clear();
                    InstructionTypes.Add(new InstructionType("Extract Core", $"Extract core (chance of explosion!)", onComplete: SelfDestruct));
                }
        }

        private void SelfDestruct(Instruction instruction)
        {
            Random rd = new Random();
            int chance = rd.Next(1, 100);
            if (chance > 50)
            {
                GameObjectManager.Add(new Loot(this.Loot(), this.Position));
                robotExploded = true;
            }
            else
            {
                SoundFactory.PlaySoundEffect(Sound.Explosion);
                TextureGroupIndex = 5;
                Colonist colonist = (Colonist) instruction.ActiveMember;
                colonist.Health = colonist.Health - 50;
                Task.Delay(new TimeSpan(0, 0, 2)).ContinueWith(o =>
                {
                    robotExploded = true;
                });
               
            }
        }

        public object Reconstruct()
        {
            return new SmallRobot(PFSPosition, (int)PFSHealth);
        }
    }
}
