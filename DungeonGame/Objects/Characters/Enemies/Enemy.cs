using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonGame
{
    abstract class Enemy : Character
    {
        private HealthBar healthBar;
        private static bool isDioDead = false;
        protected List<AttackSpots> _bodyParts;
        protected bool isDead = false;

        public bool IsDead => isDead;
        public List<AttackSpots> BodyParts => _bodyParts;
        public int experience;

        protected Enemy()
        {
            _texture = _content.Load<Texture2D>("Enemies/" + GetType().Name.ToString());
            AnimationInitialize();
            Position(GameClass.WindowWidth / 2 + 100, (GameClass.WindowHeight - Height) / 2);
            healthBar = new HealthBar(GameClass.WindowWidth / 2 + 80, GameClass.WindowHeight / 2  + 99);
        }
        public static Enemy Generate()
        {
            if (Player.GetPlayer().Level > 15 && !isDioDead)
            {
                GameClass.actions.Text += "Ты думал, что тут будет финальный босс\n но это я - Дио!";
                return new Dio();
            }
            switch (GameClass.random.Next(0, 9))
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
                default: return new Slime();
            }
        }

        public void AttackPlayer()
        {
            Attack usedAttack = _attacks[GameClass.random.Next(_attacks.Count)];
            DoAttack(Player.GetPlayer(), usedAttack);
        }

        public override void Update(GameTime gameTime)
        {
            if (_health <= 0)
            {
                if (GetType() == typeof(Dio))
                    isDioDead = true;
                isDead = true;
            }
            _animationPlayer.Update(gameTime);
        }

        public void DrawHealthBar(SpriteBatch s)
        {
            healthBar.Draw(s, _health, _maxHealth, 0.5f);
        }
    }
}
