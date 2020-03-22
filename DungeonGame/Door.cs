using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public enum StateDoor
    {
        Closed,
        Opened
    }
    class Door
    {
        private StateDoor _state;
        private Dictionary<StateDoor, Texture2D> _textures;
        private Vector2 _position;
        private Rectangle _hitbox;

        public StateDoor state { get { return _state; } }

        public Door(int x, int y, Texture2D closedTexture, Texture2D openedTexture)
        {
            _textures = new Dictionary<StateDoor, Texture2D>
            {
                { StateDoor.Closed, closedTexture },
                { StateDoor.Opened, openedTexture }
            };
            _hitbox = new Rectangle(x, y, closedTexture.Width, closedTexture.Height);
            this._position = new Vector2(x, y);
        }

        public void Update(MouseState mouseState, GameTime gameTime)
        {
            if (_hitbox.Contains(mouseState.X, mouseState.Y))
            {
                _state = StateDoor.Opened;
            }
            else
            {
                _state = StateDoor.Closed;
            }
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(_textures[_state], _position);
        }

    }
}
