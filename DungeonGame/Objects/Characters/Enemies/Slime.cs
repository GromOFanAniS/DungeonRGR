using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    class Slime : Enemy
    {
        private static Animation _idle;
        private static Texture2D _texture;

        public Slime()
        {
            Name = "Слизень";
            _maxHealth = 20 + 10 * Game1.difficulty;
            _health = _maxHealth;
            _animation.Set(_idle);
            experience = 10 + 20 * Game1.difficulty;
            Y += Height / 2;
            _weakSpots = new List<AttackSpots>()
            {
                AttackSpots.Body
            };
            _weakness = AttackTypes.Physical;
            _attacks = new List<Attack>()
            {
                new Attack(10 + 5 * Game1.difficulty, 75, AttackTypes.Poison, AttackSpots.Body, "Наскок"),
                new Attack(20 + 10 * Game1.difficulty, 50, AttackTypes.Poison, AttackSpots.Body, "Поедание")
            };
        }

        public static void Load(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Enemies/Slime");
            _idle = new Animation();
            _idle.AddFrame(new Rectangle(0, 4, 120, 64), TimeSpan.FromSeconds(0.25));
            _idle.AddFrame(new Rectangle(0, 76, 120, 64), TimeSpan.FromSeconds(0.25));
        }

        public override void Draw(SpriteBatch s)
        {
            Vector2 topLeftOfSprite = new Vector2(X, Y);
            _animation.Draw(s, _texture, topLeftOfSprite, SpriteEffects.None);
        }
    }
}
