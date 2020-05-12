using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    class Gold
    {
        private int _amount;
        private static Texture2D _sheetTexture;
        private static Animation _animation;
        private Rectangle _hitbox;

        private bool _doDraw;
        public bool DoDraw => _doDraw;

        private int _width => _animation.CurrentRectangle.Width;
        private int _height => _animation.CurrentRectangle.Height;
        private int X;
        private int Y;

        public Gold()
        {
            _doDraw = true;
            _amount = Game1.random.Next(0, 101);

            X = (Game1.gameWindow.ClientBounds.Width - _width) / 2;
            Y = 240;

            _hitbox = new Rectangle(X, Y, _width, _height);
        }

        public static void Load(ContentManager content)
        {
            _sheetTexture = content.Load<Texture2D>("Objects/GoldPile");
            _animation = new Animation();
            _animation.AddFrame(new Rectangle(0, 0, 250, 95), TimeSpan.FromSeconds(0.25));
            _animation.AddFrame(new Rectangle(0, 96, 250, 95), TimeSpan.FromSeconds(0.25));
        }

        public void Update(GameTime gameTime)
        {
            Rectangle playerPosition = new Rectangle((int)Game1.player.X, (int)Game1.player.Y - 1, Game1.player.Width, Game1.player.Height);
            if (_hitbox.Intersects(playerPosition) && Game1.keyboardState.IsKeyDown(Keys.Space) && _doDraw)
            {
                _doDraw = false;
                Game1.player.gold += _amount;
                Game1.actions.Text = "Подобрано " + _amount + " золота";
            }
            _animation.Update(gameTime);
        }

        public void Draw(SpriteBatch s)
        {
            if (_doDraw)
            {
                Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
                var sourceRectangle = _animation.CurrentRectangle;
                s.Draw(_sheetTexture, topLeftOfSprite, null, sourceRectangle);
            }
        }
    }
}
