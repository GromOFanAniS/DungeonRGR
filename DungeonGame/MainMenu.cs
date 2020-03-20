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
        Texture2D startButtonNone;
        Texture2D startButtonHover;
        Texture2D startButtonPress;

        Texture2D leaderboardButtonNone;
        Texture2D leaderboardButtonHover;
        Texture2D leaderboardButtonPress;

        Texture2D exitButtonNone;
        Texture2D exitButtonHover;
        Texture2D exitButtonPress;
        
        Song song;

        private Dictionary<string, Button> _buttons = new Dictionary<string, Button>();

        public void Load(ContentManager Content)
        {
            song = Content.Load<Song>("Music/MainMenu");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.Play(song);

            startButtonNone = Content.Load<Texture2D>("Buttons/StartButton");
            startButtonHover = Content.Load<Texture2D>("Buttons/StartButtonHover");
            startButtonPress = Content.Load<Texture2D>("Buttons/StartButtonHover");
            _buttons.Add("Start", new Button(20, 100, startButtonNone, startButtonHover, startButtonPress));

            leaderboardButtonNone = Content.Load<Texture2D>("Buttons/LeaderboardButton");
            leaderboardButtonHover = Content.Load<Texture2D>("Buttons/LeaderboardButtonHover");
            leaderboardButtonPress = Content.Load<Texture2D>("Buttons/LeaderboardButtonHover");
            _buttons.Add("Leaderboard", new Button(20, 200, leaderboardButtonNone, leaderboardButtonHover, leaderboardButtonPress));

            exitButtonNone = Content.Load<Texture2D>("Buttons/ExitButton");
            exitButtonHover = Content.Load<Texture2D>("Buttons/ExitButtonHover");
            exitButtonPress = Content.Load<Texture2D>("Buttons/ExitButtonHover");
            _buttons.Add("Exit", new Button(20, 300, exitButtonNone, exitButtonHover, exitButtonPress));
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
                            Game1._gameState = GameState.Doors;
                            break;

                        case "Leaderboard":
                            Game1._gameState = GameState.Leaderboard;
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
