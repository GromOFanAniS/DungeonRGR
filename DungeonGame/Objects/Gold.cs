﻿using System;
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
        private int _potionAmount;
        private static Texture2D _sheetTexture;
        private static Animation _animation;
        private Rectangle _hitbox;
        private Player player = Player.GetPlayer();

        private bool _doDraw;
        public bool DoDraw => _doDraw;

        private int Width => _animation.CurrentRectangle.Width;
        private int Height => _animation.CurrentRectangle.Height;
        private int x;
        private int y;
        private static KeyboardState oldState;

        public Gold()
        {
            _doDraw = true;
            _amount = Game1.random.Next(101);
            _potionAmount = Game1.random.Next(1, 5);

            x = (Game1.WindowWidth - Width) / 2;
            y = 240;

            _hitbox = new Rectangle(x, y, Width, Height);
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
            KeyboardState newState = Keyboard.GetState();
            if (_hitbox.Intersects(player.PlayerPosition) && _doDraw && newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                _doDraw = false;
                player.gold += _amount;
                player.TakePotions(_potionAmount);
                Game1.actions.Text = $"Подобрано {_amount} золота и {_potionAmount} зелий\n";
            }
            oldState = newState;
            _animation.Update(gameTime);
        }

        public void Draw(SpriteBatch s)
        {
            if (_doDraw)
            {
                Vector2 topLeftOfSprite = new Vector2(this.x, this.y);
                var sourceRectangle = _animation.CurrentRectangle;
                s.Draw(_sheetTexture, topLeftOfSprite, null, sourceRectangle);
            }
        }
    }
}
