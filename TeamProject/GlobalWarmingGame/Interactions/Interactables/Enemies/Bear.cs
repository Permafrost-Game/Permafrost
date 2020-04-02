using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Enemies
{
    public class Bear : Enemy, IReconstructable
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
                new ResourceItem(Resource.Food, 8),
                new ResourceItem(Resource.Leather, 2)
            };

        public Bear() : base("", 0, 0, 0, 0, Vector2.Zero, TextureSetTypes.Bear)
        {

        }

        public Bear ( Vector2 position, int hp = 300)
        : base ("Bear",2000, 70, 10, hp, position, TextureSetTypes.Bear)
        { }



        public override void AnimateAttack()
        {
            isAnimated = true;
            this.TextureGroupIndex = 3;

        }

        protected override void SetDead()
        {
            //remove the enemy from the game
            this.DeathSound();
            notDefeated = false;
            GameObjectManager.Add(new Loot(loot, this.Position));
            GameObjectManager.Remove(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


        }

        protected override void ChaseColonist(Colonist colonist)
        {
            Vector2 fakeLeftXcoordinate = new Vector2(colonist.Position.X - 60, colonist.Position.Y);
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
            SoundFactory.PlaySoundEffect(Sound.BearAttack);
        }

        internal override void DeathSound()
        {
            SoundFactory.PlaySoundEffect(Sound.BearDeath);
        }
        public object Reconstruct()
        {
            return new Bear(PFSPosition, (int)PFSHealth);
        }
    }
}
