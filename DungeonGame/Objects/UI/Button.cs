using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public enum StateButton
    {
        None,
        Hover,
        Press
    }
    public class Button
    {
        public Vector2 Position;
        private StateButton _state;
        private static Dictionary<StateButton, Texture2D> _textures;
        private Rectangle Hitbox;
        private static SpriteFont _font;
        private string _label;
        private static MouseState oldState;

        public StateButton State => _state; 
        public bool IsActive { get; set; }
        public static int Width => _textures[0].Width;
        public static int Height => _textures[0].Height;

        public Button(int x, int y, string label, bool isActive = true)
        {
            this.IsActive = isActive;
            _label = label;
            Hitbox = new Rectangle(x, y, _textures[0].Width, _textures[0].Height);
            Position = new Vector2(x, y);
        }

        public static void Load(ContentManager content)
        {
            _textures = new Dictionary<StateButton, Texture2D>
            {
                { StateButton.None, content.Load<Texture2D>("Buttons/Button") },
                { StateButton.Hover, content.Load<Texture2D>("Buttons/ButtonHover") }
            };
            _font = content.Load<SpriteFont>("Fonts/ButtonFont");
        }

        public void Update()
        {
            if (!IsActive)
            {
                _state = StateButton.Hover;
                return;
            }
            if (Hitbox.Contains(GameClass.mouseState.X, GameClass.mouseState.Y))
            {
                MouseState newState = Mouse.GetState();
                if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                {
                    _state = StateButton.Press;
                }
                else
                {
                    _state = StateButton.Hover;
                }
                oldState = newState;
            }
            else
            {
                _state = StateButton.None;
            }
        }

        public void Draw(SpriteBatch s)
        {
            if (_state == StateButton.Press)
                s.Draw(_textures[StateButton.Hover], Position);
            else s.Draw(_textures[_state], Position);
            s.DrawString(_font, _label, new Vector2((int)(Position.X + (_textures[0].Width - _font.MeasureString(_label).X )/2), (int)(Position.Y + (_textures[0].Height - _font.MeasureString(_label).Y) / 2)  + 2), Color.White);
        }
    }
}
