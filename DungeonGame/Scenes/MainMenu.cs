using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    class MainMenu : Scene
    {
        private static TextBox playerName;
        private static readonly int playerHeight = Player.GetPlayer().Height;
        private readonly Dictionary<string, Button> _buttons = new Dictionary<string, Button>();
        private readonly Player player;


        public MainMenu()
        {
            Player.Kill();
            player = Player.GetPlayer();
            player.canWalk = false;
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
            playerName = new TextBox(GameClass.WindowWidth / 2 - 11, GameClass.WindowHeight / 2 - playerHeight + 80, "Персонажа зовут", "Fonts/MainMenuFont", Content);
            Button.Load(Content);
        }
        public override void Update(GameTime gameTime)
        {
            //playerName.CheckClick();
            if (playerName.Text.Length != 0) _buttons["Start"].IsActive = true;
            else _buttons["Start"].IsActive = false;
            foreach (var button in _buttons)
            {
                button.Value.Update();
                if (button.Value.State == StateButton.Press)
                {
                    switch (button.Key)
                    {
                        case "Start":
                            GameClass.gameState = GameState.DoorScene;
                            player.Name = playerName.Text;
                            if (player.Name == "Doom Slayer")
                            {
                                MusicPlayer.DoomMode = true;
                                MusicPlayer.ChangeSong(MusicState.Peaceful);
                            }
                            else if(MusicPlayer.DoomMode)
                            {
                                MusicPlayer.DoomMode = false;
                                MusicPlayer.ChangeSong(MusicState.Peaceful);
                            }
                            GameClass.difficulty = 1;
                            player.canWalk = true;
                            DoNewGenerate = true;
                            break;
                        case "Load":
                            GameClass.gameState = GameState.DoorScene;
                            Player.SaveLoadSystem.LoadGame();
                            player.canWalk = true;
                            DoNewGenerate = true;
                            break;
                        case "Leaderboard":
                            GameClass.gameState = GameState.LeaderboardScene;
                            DoNewGenerate = true;
                            break;
                        case "Exit":
                            GameClass.gameState = GameState.Exit;
                            break;
                    }
                    playerName.isFocused = false;
                }
            }
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            playerName.Draw(s);
            foreach (var button in _buttons)
                button.Value.Draw(s);
        }
    }
}
