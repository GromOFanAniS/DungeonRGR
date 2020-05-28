using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonGame
{
    class LeaderBoardScene : Scene
    {
        private Button _button;
        private Label _label;

        private int _oldScroll;

        public LeaderBoardScene(Label label)
        {
            _button = new Button((GameClass.WindowWidth - Button.Width) / 2,
                                 GameClass.WindowHeight - Button.Height, "Назад");
            _label = label;
            _oldScroll = GameClass.mouseState.ScrollWheelValue;
            DoNewGenerate = false;
        }
        public override void Update(GameTime gameTime)
        {
            _button.Update();
            if (_button.State == StateButton.Press)
            {
                DoNewGenerate = true;
                _label.Y = 20;
                GameClass.gameState = GameState.MenuScene;
            }

            if (GameClass.mouseState.ScrollWheelValue > _oldScroll) //вверх
                _label.Y += _label.Y + 20 <= 20 ? 20 : 0;
            else if (GameClass.mouseState.ScrollWheelValue < _oldScroll) //вниз
                _label.Y -= _label.Y - 20 >= GameClass.WindowHeight -_label.FontSize.Y ? 20 : 0;
            _oldScroll = GameClass.mouseState.ScrollWheelValue;
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            _button.Draw(s);
            _label.Draw(s);
        }
    }
}
