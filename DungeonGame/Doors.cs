﻿using System.Collections.Generic;
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
        public StateDoor state { get { return _state; } }
        private Dictionary<StateDoor, Texture2D> _textures;
        public Vector2 Position;
        Rectangle Hitbox;

        Texture2D closedDoorTexture;
        Texture2D openedDoorTexture;
        public Door(int x, int y, Texture2D closedTexture, Texture2D openedTexture)
        {
            _textures = new Dictionary<StateDoor, Texture2D>
            {
                { StateDoor.Closed, closedTexture },
                { StateDoor.Opened, openedTexture }
            };
            Hitbox = new Rectangle(x, y, closedTexture.Width, closedTexture.Height);
            this.Position = new Vector2(x, y);
        }

        public void Update(MouseState mouseState)
        {
            if (Hitbox.Contains(mouseState.X, mouseState.Y))
            {
                _state = StateDoor.Opened;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    
                }
            }
            else
            {
                _state = StateDoor.Opened;
            }
        }

        public void Load(ContentManager content)
        {
            closedDoorTexture = content.Load<Texture2D>("Door/DoorClosed");
            openedDoorTexture = content.Load<Texture2D>("Door/DoorOpened");

        }

        public void Draw(SpriteBatch s)
        {
            s.Begin();
            s.Draw(_textures[_state], Position);
            s.End();
        }

    }
}
