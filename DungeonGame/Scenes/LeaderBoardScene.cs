using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public LeaderBoardScene(Label label)
        {
            _button = new Button((Game1.WindowWidth - Button.Width) / 2,
                                 Game1.WindowHeight - Button.Height, "Назад");
            _label = label;
        }
        public override void Update(GameTime gameTime)
        {
            _button.Update();
            if (_button.State == StateButton.Press)
            {
                DoNewGenerate = true;
                Game1._gameState = GameState.MenuScene;
            }
            //TODO : скролл и ещё что-нибудь
        }

        public override void Draw(SpriteBatch s)
        {
            _button.Draw(s);
            _label.Draw(s);
        }
    }
}
