using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DungeonGame
{
    abstract class Enemy : Character
    {
        private HealthBar healthBar;

        protected bool isDead = false;

        public bool IsDead => isDead;
        public int experience;

        protected Enemy()
        {
            Position(Game1.WindowWidth / 2 + 100, Game1.WindowHeight / 2);
            healthBar = new HealthBar(Game1.WindowWidth / 2 + 80, Game1.WindowHeight / 2  + 125);
        }
        public static Enemy Generate()
        {
            switch (Game1.random.Next(0, 10))
            {
                case 0: return new Slime();
                default: return new Slime();
            }
        }

        public override void SetAnimation(Animations animation)
        {
            throw new NotImplementedException();
        }

        public void AttackPlayer()
        {
            Attack usedAttack = _attacks[Game1.random.Next(_attacks.Count)];
            DoAttack(Player.GetPlayer(), usedAttack);
        }

        public override void Update(GameTime gameTime)
        {
            if (_health <= 0) isDead = true;
            _animation.Update(gameTime);
        }

        public void DrawHealthBar(SpriteBatch s)
        {
            healthBar.Draw(s, _health, _maxHealth, 0.5f);
        }
    }
}
