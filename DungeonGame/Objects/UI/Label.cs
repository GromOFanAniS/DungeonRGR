using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public class Label
    {
        private static SpriteFont _font;
        private string _text;
        private Vector2 _position;

        private const float _delay = 2;
        private float _remainingDelay = _delay;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                _remainingDelay = _delay;
            }
        }

        public Label(int x, int y, string text = "")
        {
            _text = text;
            _position = new Vector2(x, y);
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
            s.DrawString(_font, _text, _position, Color.Black);
        }
    }
}
