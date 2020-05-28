using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DungeonGame
{
    public class AnimationPlayer
    {
        private Dictionary<Animations, Animation> _animationList = new Dictionary<Animations, Animation>();
        private Animation _animation;
        private bool _playOnce;
        private bool _isPlaying;

        public bool IsPlaying => _isPlaying;
        public bool PlayOnce => _playOnce;
        public Rectangle CurrentRectangle => _animation.CurrentRectangle;

        public void AddAnimation(Animations key, Animation animation)
        {
            _animationList.Add(key, animation);
        }

        public void SetAnimation(Animations animation)
        {
            Set(_animationList[animation]);
        }

        private void Set(Animation animation)
        {
            _animation = animation;
            _isPlaying = true;
        }

        public void Play(Animations animation)
        {
            Set((Animation)_animationList[animation].Clone());
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
                SetAnimation(Animations.Idle);
            }
        }
    }
}
