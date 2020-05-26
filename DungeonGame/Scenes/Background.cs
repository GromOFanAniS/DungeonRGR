using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    static class Background
    {
        private static ContentManager _content;
        private static SpriteBatch _spriteBatch;
        
        public static void Init(ContentManager content, SpriteBatch spriteBatch)
        {
            _content = content;
            _spriteBatch = spriteBatch;
        }

        public static Texture2D Load(string name)
        {
            return _content.Load<Texture2D>("Backgrounds/"+name);
        }

        public static void Draw(Texture2D texture)
        {
            _spriteBatch.Draw(texture, new Vector2(0, 0), Color.White);
        }
    }
}
