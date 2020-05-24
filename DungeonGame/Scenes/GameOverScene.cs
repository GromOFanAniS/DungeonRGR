using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    class GameOverScene : Scene
    {
        private readonly Button button;
        private readonly Label label;

        public GameOverScene()
        {
            MusicPlayer.ChangeSong(MusicState.GameOver);
            button = new Button((Game1.WindowWidth - Button.Width) / 2, (Game1.WindowHeight * 2 / 3), "В главное меню");
            label = new Label(Game1.WindowWidth / 2, (Game1.WindowHeight / 3), $"Вы умерли, ваш счет {Player.GetPlayer().gold}", AlignmentPosition.center);
        }

        public override void Draw(SpriteBatch s)
        {
            label.Draw(s);
            button.Draw(s);
        }

        public override void Update(GameTime gameTime)
        {
            button.Update();
            if(button.State == StateButton.Press)
            {
                Game1.gameState = GameState.MenuScene;
                Player.Kill();
            }
        }
    }
}
