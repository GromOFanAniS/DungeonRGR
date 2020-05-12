using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public class HealthBar
    {
        private static Texture2D _texture;
        private Vector2 _position;
        private Rectangle _overlayRectangle = new Rectangle(0, 56, 320, 36);

        public HealthBar(int x, int y)
        {
            _position = new Vector2(x, y);
        }

        public static void Load(ContentManager content)
        {
            _texture = content.Load<Texture2D>("UI/HealthBar");
        }

        public void Draw(SpriteBatch s, int value, int maxValue)
        {
            Rectangle hpRectangle = new Rectangle(32, 8, 256 * value / maxValue, 20);
            s.Draw(_texture, _position, _overlayRectangle, Color.White);
            s.Draw(_texture, _position + new Vector2(32, 8), hpRectangle, Color.White);
        }
    }
}
