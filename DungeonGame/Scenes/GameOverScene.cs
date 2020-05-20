﻿using Microsoft.Xna.Framework;
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
        Button button;
        Label label;

        public GameOverScene()
        {
            button = new Button((Game1.WindowWidth - Button.Width) / 2, (Game1.WindowHeight * 2 / 3), "В главное меню");
            label = new Label(Game1.WindowWidth / 2, (Game1.WindowHeight / 3), $"Вы умерли, ваш счет {Game1.player.gold}", AlignmentPosition.center);
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
                Game1.player = new Player();
            }
        }
    }
}
