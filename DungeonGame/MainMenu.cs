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

        private Dictionary<string, Button> _buttons = new Dictionary<string, Button>();

        public void Load(ContentManager Content)
        {
            _song = Content.Load<Song>("Music/MainMenu");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.33f;
            MediaPlayer.Play(_song);
            
            startButtonNone = Content.Load<Texture2D>("Buttons/StartButton");
            startButtonHover = Content.Load<Texture2D>("Buttons/StartButtonHover");
            startButtonPress = Content.Load<Texture2D>("Buttons/StartButtonHover");
            _buttons.Add("Start", new Button(20, (int)(startButtonNone.Height * 1.5f), startButtonNone, startButtonHover, startButtonPress));

            leaderboardButtonNone = Content.Load<Texture2D>("Buttons/LeaderboardButton");
            leaderboardButtonHover = Content.Load<Texture2D>("Buttons/LeaderboardButtonHover");
            leaderboardButtonPress = Content.Load<Texture2D>("Buttons/LeaderboardButtonHover");
            _buttons.Add("Leaderboard", new Button(20, (int)(startButtonNone.Height * 3), leaderboardButtonNone, leaderboardButtonHover, leaderboardButtonPress));

            exitButtonNone = Content.Load<Texture2D>("Buttons/ExitButton");
            exitButtonHover = Content.Load<Texture2D>("Buttons/ExitButtonHover");
            exitButtonPress = Content.Load<Texture2D>("Buttons/ExitButtonHover");
            _buttons.Add("Exit", new Button(20, (int)(startButtonNone.Height * 4.5f), exitButtonNone, exitButtonHover, exitButtonPress));
        }
        public void Update(MouseState mouseState)
        {
            foreach (var button in _buttons)
            {
                button.Value.Update(mouseState);
                if (button.Value.state == StateButton.Press)
                {
                    switch (button.Key)
                    {
                        case "Start":
                            Game1._gameState = GameState.DoorScene;
                            Game1.player.Reset(new Vector2(0, 0));
                            break;

                        case "Leaderboard":
                            Game1._gameState = GameState.LeaderboardScene;
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
            foreach (var button in _buttons)
                button.Value.Draw(s);
        }
    }
}
