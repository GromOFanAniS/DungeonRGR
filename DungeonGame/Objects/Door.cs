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
        public static Texture2D closedTexture;
        public static Texture2D openedTexture;
        private StateDoor _state;
        private Dictionary<StateDoor, Texture2D> _textures;
        private Vector2 _position;
        private Rectangle _hitbox;
        private readonly Player player = Player.GetPlayer();

        public StateDoor State { get { return _state; } }

        public Door(int x, int y)
        {
            _textures = new Dictionary<StateDoor, Texture2D>
            {
                { StateDoor.Closed, closedTexture },
                { StateDoor.Opened, openedTexture }
            };
            _hitbox = new Rectangle(x, y, closedTexture.Width, closedTexture.Height);
            _position = new Vector2(x, y);
        }

        public static void Load(ContentManager content)
        {
            closedTexture = content.Load<Texture2D>("Door/DoorClosed");
            openedTexture = content.Load<Texture2D>("Door/DoorOpened");
        }

        public void Update()
        {
            if (_hitbox.Intersects(Player.GetPlayer().PlayerPosition))
            {
                _state = StateDoor.Opened;
                if (KeyboardHandler.HasBeenPressed(Keys.Space)) OpenDoor();
            }
            else
            {
                _state = StateDoor.Closed;
            }
        }

        public void Draw(SpriteBatch s, SpriteEffects spriteEffect = SpriteEffects.None)
        {
            s.Draw(_textures[_state], _position, null, null, null, 0, null, null, spriteEffect);
        }


        private void OpenDoor()
        {
            if (GameClass.gameState == GameState.DoorScene)
            {
                if (GameClass.random.Next(100) <= 50)
                {
                    GameClass.gameState = GameState.EnemyScene;
                    MusicPlayer.ChangeSong(MusicState.Battle);
                }
                else
                {
                    if (_position.X > GameClass.WindowWidth / 2)
                        player.Position((GameClass.WindowWidth - player.Width) / 2 - 230, (GameClass.WindowHeight - player.Height) / 2);
                    GameClass.gameState = GameState.GoldScene;
                }
                Scene.DoNewGenerate = true;
            }
            else if (GameClass.gameState == GameState.EnemyScene || GameClass.gameState == GameState.GoldScene)
            {
                if (GameClass.gameState == GameState.EnemyScene) 
                    MusicPlayer.ChangeSong(MusicState.Peaceful);
                GameClass.gameState = GameState.DoorScene;
                Scene.DoNewGenerate = true;
            }
        }
    }
}
