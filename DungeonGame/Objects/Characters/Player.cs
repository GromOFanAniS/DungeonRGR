using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public class Player : Character
    {
        public StringBuilder Name { get; set; }
        public int gold = 0;
        Animation idle;
        Animation walk;
        static Texture2D playerSheetTexture;
        SpriteEffects flip = SpriteEffects.None;
        public bool canWalk = true;

        public Player()
        {
            Name = new StringBuilder();
            _healthBar = new HealthBar(5, 10);
            _maxHealth = 100;
            _health = 100;
            idle = new Animation();
                idle.AddFrame(new Rectangle(0, 0, 95, 184), TimeSpan.FromSeconds(1));
            _animation = idle;

            walk = new Animation();
            for (int i = 1; i < 8; i++)
                walk.AddFrame(new Rectangle(96*i, 0, 95, 184), TimeSpan.FromSeconds(.25));
            
        }

        public static void Load(ContentManager content)
        {
            playerSheetTexture = content.Load<Texture2D>("Player/PlayerSheet");
        }

        private float velocity = 250f;
        public override void Update(GameTime gameTime)
        {
            _animation = idle;
            if (canWalk)
            {
                if (Game1.keyboardState.IsKeyDown(Keys.A))
                {
                    _animation = walk;
                    flip = SpriteEffects.FlipHorizontally;
                    if ((this.X - velocity * (float)gameTime.ElapsedGameTime.TotalSeconds) <= 0) return;
                    this.X -= velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (Game1.keyboardState.IsKeyDown(Keys.D))
                {
                    _animation = walk;
                    flip = SpriteEffects.None;
                    if ((this.X + velocity * (float)gameTime.ElapsedGameTime.TotalSeconds) >= (Game1.gameWindow.ClientBounds.Width - Width)) return;
                    this.X += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                flip = SpriteEffects.None;
                Position(Game1.gameWindow.ClientBounds.Width / 2 - 100 - Width, (int)Y);
            }
            _animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch s)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            Color color = Color.White;
            var sourceRectangle = _animation.CurrentRectangle;
            s.Draw(playerSheetTexture, topLeftOfSprite, null, sourceRectangle, null, 0, null, color, flip);
            _healthBar.Draw(s, _health, _maxHealth);
        }
    }
}
