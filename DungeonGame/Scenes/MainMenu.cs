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
    class MainMenu
    {
        private Song _song;

        private TextBox playerName;

        private Dictionary<string, Button> _buttons = new Dictionary<string, Button>();

        public void Load(ContentManager Content)
        {
            playerName = new TextBox(Game1.gameWindow.ClientBounds.Width / 2 - 11, Game1.player.Height - 30, "Персонажа зовут", "Fonts/MainMenuFont", Content);

            _song = Content.Load<Song>("Music/MainMenu");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.33f;
            MediaPlayer.Play(_song);

            Button.Load(Content);

            _buttons.Add("Start", new Button(20, 100, "Start", false));
            _buttons.Add("Leaderboard", new Button(20, 200, "Leaderboard"));
            _buttons.Add("Exit", new Button(20, 300, "Exit"));
        }
        public void Update()
        {
            //playerName.CheckClick();
            if (playerName.Text.Length != 0) _buttons["Start"].isActive = true;
            else _buttons["Start"].isActive = false;
            foreach (var button in _buttons)
            {
                button.Value.Update();
                if (button.Value.state == StateButton.Press)
                {
                    switch (button.Key)
                    {
                        case "Start":
                            Game1._gameState = GameState.DoorScene;
                            Game1.player.Name = playerName.Text;
                            playerName.isFocused = false;
                            break;

                        case "Leaderboard":
                            Game1._gameState = GameState.LeaderboardScene;
                            playerName.isFocused = false;
                            Console.WriteLine("Leaderboard");
                            break;

                        case "Exit":
                            Game1._gameState = GameState.Exit;
                            break;
                    }
                }
            }
        }
        public void Draw(SpriteBatch s)
        {
            playerName.Draw(s);
            foreach (var button in _buttons)
                button.Value.Draw(s);
        }
    }
}
