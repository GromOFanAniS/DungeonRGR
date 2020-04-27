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
        private Texture2D startButtonNone;
        private Texture2D startButtonHover;
        private Texture2D startButtonPress;

        private Texture2D leaderboardButtonNone;
        private Texture2D leaderboardButtonHover;
        private Texture2D leaderboardButtonPress;

        private Texture2D exitButtonNone;
        private Texture2D exitButtonHover;
        private Texture2D exitButtonPress;

        private Song _song;

        private TextBox playerName;

        private Dictionary<string, Button> _buttons = new Dictionary<string, Button>();

        public void Load(ContentManager Content)
        {
            playerName = new TextBox(200, 200, 100, 250, "Fonts/MainMenuFont", Content);

            _song = Content.Load<Song>("Music/MainMenu");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.33f;
            MediaPlayer.Play(_song);
            
            startButtonNone = Content.Load<Texture2D>("Buttons/StartButton");
            startButtonHover = Content.Load<Texture2D>("Buttons/StartButtonHover");
            startButtonPress = Content.Load<Texture2D>("Buttons/StartButtonHover");
            _buttons.Add("Start", new Button(20, (int)(startButtonNone.Height * 1.5f), startButtonNone, startButtonHover, startButtonPress, false));

            leaderboardButtonNone = Content.Load<Texture2D>("Buttons/LeaderboardButton");
            leaderboardButtonHover = Content.Load<Texture2D>("Buttons/LeaderboardButtonHover");
            leaderboardButtonPress = Content.Load<Texture2D>("Buttons/LeaderboardButtonHover");
            _buttons.Add("Leaderboard", new Button(20, (int)(startButtonNone.Height * 3), leaderboardButtonNone, leaderboardButtonHover, leaderboardButtonPress));

            exitButtonNone = Content.Load<Texture2D>("Buttons/ExitButton");
            exitButtonHover = Content.Load<Texture2D>("Buttons/ExitButtonHover");
            exitButtonPress = Content.Load<Texture2D>("Buttons/ExitButtonHover");
            _buttons.Add("Exit", new Button(20, (int)(startButtonNone.Height * 4.5f), exitButtonNone, exitButtonHover, exitButtonPress));
        }
        public void Update()
        {
            playerName.CheckClick();
            if (playerName.Text.Length != 0) _buttons["Start"].isActive = true;
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
