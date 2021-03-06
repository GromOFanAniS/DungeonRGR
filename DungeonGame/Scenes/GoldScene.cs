﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonGame
{
    class GoldScene : Scene
    {
        private readonly Gold gold;
        private readonly Door door;
        private readonly Weapon weapon;

        public GoldScene()
        {
            DoNewGenerate = false;
            gold = new Gold();
            door = new Door(GameClass.gameWindow.ClientBounds.Width / 2 + 147, Door.closedTexture.Height / 2 + 4);
            GameClass.actions.Text += "Вы нашли золото!\n";
            if (GameClass.random.Next(101) < 20)
            {
                GameClass.actions.Text += "Вы нашли оружие!\n";
                switch (GameClass.random.Next(12))
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        weapon = new ShortSword();
                        break;
                    case 5:
                    case 6:
                    case 7:
                        weapon = new Sword();
                        break;
                    case 8:
                    case 9:
                        weapon = new Bow();
                        break;
                    case 10:
                    case 11:
                        weapon = new SkyFracture();
                        break;
                }
            }
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            door.Draw(s);
            gold.Draw(s);
            weapon?.Draw(s);
        }

        public override void Update(GameTime gameTime)
        {
            if (!gold.DoDraw)
                door.Update();
            weapon?.Update();
            gold.Update(gameTime);
        }
    }
}
