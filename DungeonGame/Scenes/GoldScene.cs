using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    class GoldScene : Scene
    {
        private Gold gold;
        private Door door;


        public GoldScene()
        {
            DoNewGenerate = false;
            gold = new Gold();
            door = new Door(Game1.WindowWidth - Door.closedTexture.Width - 50, Door.closedTexture.Height / 2);
            Game1.actions.Text = "Вам попалось золото";
        }
        public override void Draw(SpriteBatch s)
        {
            gold.Draw(s);
            if (!gold.DoDraw)
                door.Draw(s);
        }

        public override void Update(GameTime gameTime)
        {
            gold.Update(gameTime);
            if (!gold.DoDraw)
                door.Update();
        }
    }
}
