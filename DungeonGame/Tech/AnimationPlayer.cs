using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    public class AnimationPlayer
    {
        private Animation _animation;
        private bool _playOnce;
        private bool _isPlaying;

        public bool IsPlaying => _isPlaying;

        public bool PlayOnce => _playOnce;
        public Rectangle CurrentRectangle => _animation.CurrentRectangle;

        public void Set(Animation animation)
        {
            _animation = animation;
            _isPlaying = true;
        }

        public void Play(Animation animation)
        {
            Set((Animation)animation.Clone());
            _playOnce = true;
        }
        public void Draw(SpriteBatch s, Texture2D sheetTexture, Vector2 position, SpriteEffects flip)
        {
            _animation.Draw(s, sheetTexture, position, flip);
        }

        public void Update(GameTime gameTime)
        {
            if(_isPlaying)
                _animation.Update(gameTime);

            if(_animation.IsLast && _playOnce)
            {
                _playOnce = false;
                _isPlaying = false;
            }
        }
    }
}
