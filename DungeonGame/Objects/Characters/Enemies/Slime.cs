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
        static Animation idle;
        public Slime()
        {
            _maxHealth = 100;
            _health = _maxHealth;
            _animation = idle;
            Y += Height / 2;
            Game1.actions.Text += "Слизень";
            _attacks = new List<Attack>()
            {
                new Attack()
            };
        }

        public static void Load(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Enemies/Slime");
            idle = new Animation();
            idle.AddFrame(new Rectangle(0, 4, 120, 64), TimeSpan.FromSeconds(0.25));
            idle.AddFrame(new Rectangle(0, 76, 120, 64), TimeSpan.FromSeconds(0.25));
        }

        public override void Draw(SpriteBatch s)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            var sourceRectangle = _animation.CurrentRectangle;
            s.Draw(_texture, topLeftOfSprite, null, sourceRectangle);
        }



        public override void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
        }
    }
}
