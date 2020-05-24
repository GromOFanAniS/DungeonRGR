using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public class Animation : ICloneable
    {
        private List<AnimationFrame> _frames;
        public TimeSpan _timeIntoAnimation;
        private AnimationFrame _currentFrame;

        public bool IsLast => _currentFrame == _frames.LastOrDefault();

        public Animation()
        {
            _frames = new List<AnimationFrame>();
        }


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
                _currentFrame = null;

                TimeSpan accumulatedTime = new TimeSpan();

                foreach (var frame in _frames)
                {
                    if (accumulatedTime + frame.Duration >= _timeIntoAnimation)
                    {
                        _currentFrame = frame;
                        break;
                    }
                    else
                    {
                        accumulatedTime += frame.Duration;
                    }
                }

                if (_currentFrame == null)
                {
                    _currentFrame = _frames.LastOrDefault();
                }
                if (_currentFrame != null)
                {
                    return _currentFrame.SourceRectangle;
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
        }

        public void Draw(SpriteBatch s, Texture2D sheetTexture, Vector2 topLeftOfSprite, SpriteEffects _flip)
        {
            Color color = Color.White;
            var sourceRectangle = CurrentRectangle;
            s.Draw(sheetTexture, topLeftOfSprite, null, sourceRectangle, null, 0, null, color, _flip);
        }

        public object Clone()
        {
            var frames = new List<AnimationFrame>();
            foreach (var frame in _frames)
                frames.Add(frame);
            frames.Add(new AnimationFrame { SourceRectangle = _frames.First().SourceRectangle, Duration = TimeSpan.FromSeconds(1) });
            return new Animation()
            {
                _frames = frames,
                _currentFrame = _currentFrame,
                _timeIntoAnimation = _timeIntoAnimation
            };
        }
    }
}
