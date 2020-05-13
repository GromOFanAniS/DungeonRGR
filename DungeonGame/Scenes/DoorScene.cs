using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    class DoorScene : Scene
    {
        private List<Door> _doors = new List<Door>() { null, null };

        public DoorScene()
        {
            DoNewGenerate = false;
            _doors[0] = new Door(50, Door.closedTexture.Height / 2);
            _doors[1] = new Door(Game1.gameWindow.ClientBounds.Width - Door.closedTexture.Width - 50, Door.closedTexture.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var door in _doors)
            {
                door.Update();
            }
        }

        public override void Draw(SpriteBatch s)
        {
            _doors[0]?.Draw(s, SpriteEffects.FlipHorizontally);
            _doors[1]?.Draw(s);
        }
    }
}
