using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DungeonGame
{
    abstract class Enemy : Character
    {
        private HealthBar healthBar;
        
        protected List<AttackSpots> _bodyParts;
        protected bool isDead = false;

        public bool IsDead => isDead;
        public List<AttackSpots> BodyParts => _bodyParts;
        public int experience;

        protected Enemy()
        {
            _texture = _content.Load<Texture2D>("Enemies/" + GetType().Name.ToString());
            AnimationInitialize();
            Position(Game1.WindowWidth / 2 + 100, (Game1.WindowHeight - Height) / 2);
            healthBar = new HealthBar(Game1.WindowWidth / 2 + 80, Game1.WindowHeight / 2  + 99);
        }
        public static Enemy Generate()
        {
            switch (Game1.random.Next(0, 10))
            {
                case 0: return new Slime();
                case 1: return new Skeleton();
                case 2: return new FloatingSkull();
                case 3: return new Imp();
                case 4: return new Mushroom();
                case 5: return new Demon();
                case 6: return new Goblin();
                case 7: return new Thing();
                case 8: return new Hellhound();
                //case 9: return new Dio();
                default: return new Slime();
            }
        }

        public void AttackPlayer()
        {
            Attack usedAttack = _attacks[Game1.random.Next(_attacks.Count)];
            DoAttack(Player.GetPlayer(), usedAttack);
        }

        public override void Update(GameTime gameTime)
        {
            if (_health <= 0) isDead = true;
            _animationPlayer.Update(gameTime);
        }

        public void DrawHealthBar(SpriteBatch s)
        {
            healthBar.Draw(s, _health, _maxHealth, 0.5f);
        }
    }
}
