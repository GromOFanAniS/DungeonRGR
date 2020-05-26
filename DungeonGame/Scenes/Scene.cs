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
        protected Texture2D _backgroundTexture;

        protected Scene()
        {
            _backgroundTexture = Background.Load(GetType().Name.ToString());
        }

        abstract public void Update(GameTime gameTime);
        virtual public void Draw(SpriteBatch s)
        {
            Background.Draw(_backgroundTexture);
        }
    }
}
