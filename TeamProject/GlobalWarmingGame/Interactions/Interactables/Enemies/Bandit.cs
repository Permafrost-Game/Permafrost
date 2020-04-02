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
    class Bandit : Enemy, IReconstructable
    {
        public bool killed=false;

        [PFSerializable]
        public bool dying = false;

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
                new ResourceItem(Resource.Food, 2),
                new ResourceItem(Resource.Coat, 1),
                new ResourceItem(Resource.Axe, 1)
            };

        public Bandit() : base("", 0, 0, 0, 0, Vector2.Zero, TextureSetTypes.bandit)
        {



        public Bandit(Vector2 position, Texture2D[][] textureSet)
        : base("Bandit", 1500, 70, 6, 300, position, textureSet)
        { }

        public Bandit(Vector2 position, int hp = 300, bool dying = false)
        : base("Bandit", 1500, 70, 10, hp, position, TextureSetTypes.bandit)
        {
            this.killed = dying;
        }

        public override void AnimateAttack()
        {
            isAnimated = true;
            this.TextureGroupIndex = 3;

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (killed)
            {
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
            SoundFactory.PlaySoundEffect(Sound.banditCut);
        }

        internal override void DeathSound()
        {
            SoundFactory.PlaySoundEffect(Sound.banditDying);
        }

        protected override void SetDead() {
            
            if (notDefeated)
            {
             
                Goals.Clear();
                TextureGroupIndex = 4;
                notDefeated=false;
                isInCombat = false;
                SoundFactory.PlaySoundEffect(Sound.banditGiveUp);
                InstructionTypes.Clear();
                InstructionTypes.Add(new InstructionType("Kill", $"Kill Bandit", onComplete:Dying));
                InstructionTypes.Add(new InstructionType("Spare", $"Spare Bandit", onComplete:join));     
            }
        }

        private void join(Instruction instruction)
        {
            SoundFactory.PlaySoundEffect(Sound.banditJoins);
            killed = true;
            Vector2 spawnplace = new Vector2(this.Position.X + 20, this.Position.Y);
            GameObjectManager.Add(new Colonist(spawnplace));
        }

        private void Dying(Instruction instruction) {
            dying = true;

            this.Rotation = 1.5f;
            isAnimated = false;
            SoundFactory.PlaySoundEffect(Sound.banditDying);
            Task.Delay(new TimeSpan(0, 0, 2)).ContinueWith(o =>
            {
                GameObjectManager.Add(new Loot(loot, this.Position));
                killed = true;
            });
        }

        public object Reconstruct()
        {
            return new Bandit(PFSPosition, (int)PFSHealth, dying);
        }
    }
    
}
