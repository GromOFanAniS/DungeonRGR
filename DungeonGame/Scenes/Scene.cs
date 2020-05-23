using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DungeonGame
{
    abstract class Scene
    {
        public static bool DoNewGenerate = true;

        abstract public void Update(GameTime gameTime);
        abstract public void Draw(SpriteBatch s);
    }
}
