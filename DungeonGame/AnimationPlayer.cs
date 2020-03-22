using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    struct AnimationPlayer
    {
        private Animation _animation;
        public Animation Animation
        {
            get { return _animation; }
        }

        private int _frameIndex;
        public int FrameIndex
        {
            get { return _frameIndex; }
        }

        private float _time;

        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2f, Animation.FrameHeight); }
        }

        public void PlayAnimation(Animation animation)
        {
            if (Animation == animation) return;

            _animation = animation;
            _frameIndex = 0;
            _time = 0f;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (_time > Animation.FrameTime)
            {
                _time -= Animation.FrameTime;

                if (Animation.IsLooping)
                {
                    _frameIndex = (_frameIndex + 1) % Animation.FrameCount;
                }
                else
                {
                    _frameIndex = Math.Min(_frameIndex + 1, Animation.FrameCount - 1);
                }
            }

            Rectangle source = new Rectangle(FrameIndex * Animation.Texture.Height, 0, Animation.Texture.Height, Animation.Texture.Height);

            spriteBatch.Draw(Animation.Texture, position, source, Color.White, 0f, Origin, 1f, spriteEffects, 0f);
        }
    }
}
