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
            _maxHealth = 100;
            _health = _maxHealth;
            _animation = _idle;
            Name = "Слизень";
            Y += Height / 2;
            _attacks = new List<Attack>()
            {
                new Attack(10, 100, AttackTypes.Poison)
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
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            var sourceRectangle = _animation.CurrentRectangle;
            s.Draw(_texture, topLeftOfSprite, null, sourceRectangle);
        }
    }
}
