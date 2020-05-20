using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    class LeaderBoardScene : Scene
    {
        private Button _button;
        private Label _label;

        private int _oldScroll;

        public LeaderBoardScene(Label label)
        {
            _button = new Button((Game1.WindowWidth - Button.Width) / 2,
                                 Game1.WindowHeight - Button.Height, "Назад");
            _label = label;
            _oldScroll = Game1.mouseState.ScrollWheelValue;
            DoNewGenerate = false;
        }
        public override void Update(GameTime gameTime)
        {
            _button.Update();
            if (_button.State == StateButton.Press)
            {
                DoNewGenerate = true;
                _label.Y = 20;
                Game1.gameState = GameState.MenuScene;
            }

            Console.WriteLine(Game1.mouseState.ScrollWheelValue + " " + _oldScroll);
            if (Game1.mouseState.ScrollWheelValue > _oldScroll) //вверх
                _label.Y += _label.Y + 20 <= 20 ? 20 : 0;
            else if (Game1.mouseState.ScrollWheelValue < _oldScroll) //вниз
                _label.Y -= _label.Y - 20 >= Game1.WindowHeight -_label.FontSize.Y ? 20 : 0;
            _oldScroll = Game1.mouseState.ScrollWheelValue;
        }

        public override void Draw(SpriteBatch s)
        {
            _button.Draw(s);
            _label.Draw(s);
        }
    }
}
