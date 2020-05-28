using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonGame
{
    class GameOverScene : Scene
    {
        private readonly Button button;
        private readonly Label label;

        public GameOverScene()
        {
            MusicPlayer.ChangeSong(MusicState.GameOver);
            button = new Button((GameClass.WindowWidth - Button.Width) / 2, (GameClass.WindowHeight * 2 / 3), "В главное меню");
            label = new Label(GameClass.WindowWidth / 2, (GameClass.WindowHeight / 3), $"Ваш счет {Player.GetPlayer().gold}", AlignmentPosition.center);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            label.Draw(s, Color.DarkRed);
            button.Draw(s);
        }

        public override void Update(GameTime gameTime)
        {
            button.Update();
            if(button.State == StateButton.Press)
            {
                Player.Kill();
                GameClass.gameState = GameState.MenuScene;
            }
        }
    }
}
