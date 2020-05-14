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
        public int gold = 0;
        public bool canWalk = true;

        private const float velocity = 250f;

        private Animation _idle;
        private Animation _walk;
        private static Texture2D _playerSheetTexture;
        private SpriteEffects _flip = SpriteEffects.None;

        public Player()
        {
            _healthBar = new HealthBar(5, 10);
            _maxHealth = 100;
            _health = _maxHealth;
            _idle = new Animation();
                _idle.AddFrame(new Rectangle(0, 0, 95, 184), TimeSpan.FromSeconds(1));
            _animation = _idle;
            _weakSpots = new List<AttackSpots>();
            _weakness = AttackTypes.None;

            _walk = new Animation();
            for (int i = 1; i < 8; i++)
                _walk.AddFrame(new Rectangle(96*i, 0, 95, 184), TimeSpan.FromSeconds(.25));
            
        }

        public static void Load(ContentManager content)
        {
            _playerSheetTexture = content.Load<Texture2D>("Player/PlayerSheet");
        }

        public override void Update(GameTime gameTime)
        {
            _animation = _idle;
            if (canWalk)
            {
                if (Game1.keyboardState.IsKeyDown(Keys.A))
                {
                    _animation = _walk;
                    _flip = SpriteEffects.FlipHorizontally;
                    if ((this.X - velocity * (float)gameTime.ElapsedGameTime.TotalSeconds) <= 0) return;
                    this.X -= velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (Game1.keyboardState.IsKeyDown(Keys.D))
                {
                    _animation = _walk;
                    _flip = SpriteEffects.None;
                    if ((this.X + velocity * (float)gameTime.ElapsedGameTime.TotalSeconds) >= (Game1.gameWindow.ClientBounds.Width - Width)) return;
                    this.X += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                _flip = SpriteEffects.None;
                Position(Game1.gameWindow.ClientBounds.Width / 2 - 100 - Width, (int)Y);
            }
            if(_health <= 0)
            {
                Game1._gameState = GameState.GameOverScene;
                Scene.DoNewGenerate = true;
            }
            _animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch s)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            Color color = Color.White;
            var sourceRectangle = _animation.CurrentRectangle;
            s.Draw(_playerSheetTexture, topLeftOfSprite, null, sourceRectangle, null, 0, null, color, _flip);
            _healthBar.Draw(s, _health, _maxHealth);
        }
    }
}
