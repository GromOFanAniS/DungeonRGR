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
        private Gold gold = null;
        private Door door = null;

        //public static bool DoNewGenerate = true;

        public GoldScene()
        {
            DoNewGenerate = false;
            gold = new Gold();
            door = new Door(Game1.gameWindow.ClientBounds.Width - Door.closedTexture.Width - 50, Door.closedTexture.Height / 2);
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
