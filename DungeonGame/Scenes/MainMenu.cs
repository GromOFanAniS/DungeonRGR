using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace DungeonGame
{
    class MainMenu : Scene
    {
        private static Song _song;

        private static TextBox playerName;

        private Dictionary<string, Button> _buttons = new Dictionary<string, Button>();

        public MainMenu()
        {
            _buttons.Add("Start", new Button(20, 100, "Новая игра", false));
            _buttons.Add("Load", new Button(20, 200, "Загрузить"));
            _buttons.Add("Leaderboard", new Button(20, 300, "Доска почёта"));
            _buttons.Add("Exit", new Button(20, 400, "Выйти"));
            playerName.isFocused = true;
            playerName.Text = "";
            DoNewGenerate = false;
        }

        public static void Load(ContentManager Content)
        {
            playerName = new TextBox(Game1.WindowWidth / 2 - 11, Game1.player.Height - 30, "Персонажа зовут", "Fonts/MainMenuFont", Content);
            Button.Load(Content);
        }
        public override void Update(GameTime gameTime)
        {
            //playerName.CheckClick();
            if (playerName.Text.Length != 0) _buttons["Start"].isActive = true;
            else _buttons["Start"].isActive = false;
            foreach (var button in _buttons)
            {
                button.Value.Update();
                if (button.Value.State == StateButton.Press)
                {
                    switch (button.Key)
                    {
                        case "Start":
                            Game1.gameState = GameState.DoorScene;
                            Game1.player.Name = playerName.Text;
                            DoNewGenerate = true;
                            break;
                        case "Load":
                            Game1.gameState = GameState.DoorScene;
                            Game1.player = SaveLoadSystem.LoadGame();
                            DoNewGenerate = true;
                            break;
                        case "Leaderboard":
                            Game1.gameState = GameState.LeaderboardScene;
                            DoNewGenerate = true;
                            break;
                        case "Exit":
                            Game1.gameState = GameState.Exit;
                            break;
                    }
                    playerName.isFocused = false;
                }
            }
        }
        public override void Draw(SpriteBatch s)
        {
            playerName.Draw(s);
            foreach (var button in _buttons)
                button.Value.Draw(s);
        }
    }
}
