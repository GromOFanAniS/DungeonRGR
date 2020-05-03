using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        public StateButton state { get { return _state; } }
        private Dictionary<StateButton, Texture2D> _textures;
        public Vector2 Position;
        Rectangle Hitbox;
        public bool isActive { get; set; }


        public Button(int x, int y, Texture2D noneTexture, Texture2D hoverTexture, bool isActive = true)
        {
            this.isActive = isActive;
            _textures = new Dictionary<StateButton, Texture2D>
            {
                { StateButton.None, noneTexture },
                { StateButton.Hover, hoverTexture }
            };
            Hitbox = new Rectangle(x, y, noneTexture.Width, noneTexture.Height);
            this.Position = new Vector2(x, y);
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
                if (Game1.mouseState.LeftButton == ButtonState.Pressed)
                {
                    _state = StateButton.Press;
                }
                else
                {
                    _state = StateButton.Hover;
                }
            }
            else
            {
                _state = StateButton.None;
            }
        }

        public void Draw(SpriteBatch s)
        {
            if (_state == StateButton.Press)
                s.Draw(_textures[_state], new Vector2(Position.X, ++Position.Y));
            else s.Draw(_textures[_state], Position);
        }
    }
}
