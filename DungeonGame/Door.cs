﻿using System;
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

        public void Update()
        {
            Rectangle playerPosition = new Rectangle((int)Game1.player.X, (int)Game1.player.Y - 1, Game1.player.Width, Game1.player.Height);
            if (_hitbox.Intersects(playerPosition))
            {
                _state = StateDoor.Opened;
                if (Game1.keyboardState.IsKeyDown(Keys.Space)) OpenDoor();
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


        private static void OpenDoor()
        {
            Random random = new Random();
            if (random.Next(100) <= 50)
            {
                Game1._gameState = GameState.EnemyScene;
                Console.WriteLine("Enemy");
            }
            else
            {
                Game1._gameState = GameState.GoldScene;
                Console.WriteLine("Gold");
            }
        }
    }
}