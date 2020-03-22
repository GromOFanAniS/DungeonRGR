using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public class Player
    {
        private Animation _idleAnimation;
        private Animation _walkAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private AnimationPlayer sprite;

        private bool _isAlive;
        public bool IsAlive
        {
            get { return _isAlive; }
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private bool _isOnGround;
        public bool IsOnGround
        {
            get { return _isOnGround; }
        }

        private float _movement;

        private Rectangle _localBounds;

        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + _localBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + _localBounds.Y;

                return new Rectangle(left, top, _localBounds.Width, _localBounds.Height);
            }
        }

        public Player(Vector2 position)
        {
            Reset(position);
        }

        public void Reset(Vector2 position)
        {
            _position = position;
            _isAlive = true;
            sprite.PlayAnimation(_idleAnimation);
        }

        public void Load(ContentManager content)
        {
            _idleAnimation = new Animation(content.Load<Texture2D>("Player/Idle"), 0.1f, true);
            _walkAnimation = new Animation(content.Load<Texture2D>("Player/Walk"), -0.1f, true);

            int width = _idleAnimation.FrameWidth;
            int left = (_idleAnimation.FrameWidth - width) / 2;
            int height = _idleAnimation.FrameHeight;
            int top = _idleAnimation.FrameHeight - height;
            _localBounds = new Rectangle(left, top, width, height);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, DisplayOrientation orientation)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, Position, flip);
        }
    }
}
