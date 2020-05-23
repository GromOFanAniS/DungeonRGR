using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public class Animation
    {
        private List<AnimationFrame> _frames = new List<AnimationFrame>();
        private TimeSpan _timeIntoAnimation;
        private List<AnimationFrame> _oldFrames = new List<AnimationFrame>();
        private bool _needToChange = false;
        private AnimationFrame _currentFrame;

        public bool IsPlaying => _needToChange;


        TimeSpan Duration
        {
            get
            {
                double totalSeconds = 0;
                foreach (var frame in _frames)
                {
                    totalSeconds += frame.Duration.TotalSeconds;
                }
                return TimeSpan.FromSeconds(totalSeconds);
            }
        }

        public void AddFrame(Rectangle rectangle, TimeSpan duration)
        {
            AnimationFrame newFrame = new AnimationFrame()
            {
                SourceRectangle = rectangle,
                Duration = duration
            };

            _frames.Add(newFrame);
        }

        public Rectangle CurrentRectangle
        {
            get
            {
                AnimationFrame currentFrame = null;

                TimeSpan accumulatedTime = new TimeSpan();

                foreach (var frame in _frames)
                {
                    if (accumulatedTime + frame.Duration >= _timeIntoAnimation)
                    {
                        currentFrame = frame;
                        break;
                    }
                    else
                    {
                        accumulatedTime += frame.Duration;
                    }
                }

                if (currentFrame == null)
                {
                    currentFrame = _frames.LastOrDefault();
                }
                if (currentFrame != null)
                {
                    _currentFrame = currentFrame;
                    return currentFrame.SourceRectangle;
                }
                else
                {
                    return Rectangle.Empty;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            double secondsIntoAnimation = _timeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;
            double remainder = secondsIntoAnimation % Duration.TotalSeconds;

            _timeIntoAnimation = TimeSpan.FromSeconds(remainder);

            if (_needToChange && _currentFrame == _frames.LastOrDefault() && (int)remainder == 0)
            {
                _frames = _oldFrames;
                _needToChange = false;
            }
        }

        public void Draw(SpriteBatch s, Texture2D sheetTexture, Vector2 topLeftOfSprite, SpriteEffects _flip)
        {
            Color color = Color.White;
            var sourceRectangle = CurrentRectangle;
            s.Draw(sheetTexture, topLeftOfSprite, null, sourceRectangle, null, 0, null, color, _flip);
        }

        public void Play(Animation nextAnimation)
        {
            _oldFrames = _frames;
            _frames = nextAnimation._frames;
            _needToChange = true;
        }
    }
}
