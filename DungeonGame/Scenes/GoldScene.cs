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
        private readonly Gold gold;
        private readonly Door door;
        private readonly Weapon weapon;

        public GoldScene()
        {
            DoNewGenerate = false;
            gold = new Gold();
            door = new Door(Game1.WindowWidth - Door.closedTexture.Width - 50, Door.closedTexture.Height / 2);
            Game1.actions.Text += "Вы нашли золото!\n";
            if (Game1.random.Next(101) < 20)
            {
                Game1.actions.Text += "Вы нашли оружие!\n";
                switch (Game1.random.Next(12))
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
            gold.Draw(s);
            weapon?.Draw(s);
            if (!gold.DoDraw)
                door.Draw(s);
        }

        public override void Update(GameTime gameTime)
        {
            gold.Update(gameTime);
            weapon?.Update();
            if (!gold.DoDraw)
                door.Update();
        }
    }
}
