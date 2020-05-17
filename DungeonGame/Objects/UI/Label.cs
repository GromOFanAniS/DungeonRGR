using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public enum AlignmentPosition
    {
        left,
        center,
        right
    }
    public class Label
    {
        private static SpriteFont _font;
        private string _text;
        private Vector2 _position;
        private AlignmentPosition _alignment;

        public Vector2 FontSize => _font.MeasureString(_text);

        private const float _delay = 2;
        private float _remainingDelay = _delay;

        public float Y
        {
            get => _position.Y;
            set => _position.Y = value;
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                _remainingDelay = _delay;
            }
        }

        public Label(int x, int y, string text = "", AlignmentPosition alignment = AlignmentPosition.left)
        {
            _text = text;
            _position = new Vector2(x, y);
            _alignment = alignment;
        }

        public static void Load(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/LabelFont");
        }

        public void Update(GameTime gameTime)
        {
            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _remainingDelay -= timer;

            if (_remainingDelay <= 0)
            {
                _text = "";
                _remainingDelay = _delay;
            }
        }

        public void Draw(SpriteBatch s)
        {
            if (_alignment == AlignmentPosition.center)
            {
                s.DrawString(_font, _text, new Vector2(_position.X - _font.MeasureString(_text).X / 2 + 0.5f, _position.Y), Color.Black);
                
            }
            else
            {
                s.DrawString(_font, _text, _position, Color.Black);
            }
        }
    }
}
