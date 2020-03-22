using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    class Animation
    {
        private Texture2D _texture;
        public Texture2D Texture
        {
            get { return _texture; }
        }

        private float _frameTime;
        public float FrameTime
        {
            get { return _frameTime; }
        }

        private bool _isLooping;
        public bool IsLooping
        {
            get { return _isLooping; }
        }

        public int FrameCount
        {
            get { return Texture.Width / FrameWidth; }
        }

        public int FrameWidth
        {
            get { return Texture.Width; }
        }

        public int FrameHeight
        {
            get { return Texture.Height; }
        }

        public Animation(Texture2D texture, float frameTime, bool isLooping)
        {
            _texture = texture;
            _frameTime = frameTime;
            _isLooping = isLooping;
        }
    }
}
