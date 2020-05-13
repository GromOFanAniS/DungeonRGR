using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

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
        private StateButton _state;
        public StateButton state => _state; 
        private static Dictionary<StateButton, Texture2D> _textures;
        public Vector2 Position;
        Rectangle Hitbox;
        public bool isActive { get; set; }
        private static SpriteFont _font;
        private string _label;
        private MouseState oldState;


        public Button(int x, int y, string label, bool isActive = true)
        {
            this.isActive = isActive;
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
            if (!isActive)
            {
                _state = StateButton.Hover;
                return;
            }
            if (Hitbox.Contains(Game1.mouseState.X, Game1.mouseState.Y))
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
            s.DrawString(_font, _label, new Vector2(Position.X + _textures[0].Width / 2 - _font.MeasureString(_label).X / 2 + 0.5f, Position.Y + _textures[0].Height / 2 - _font.MeasureString(_label).Y / 2  + 2), Color.White);
        }
    }
}
