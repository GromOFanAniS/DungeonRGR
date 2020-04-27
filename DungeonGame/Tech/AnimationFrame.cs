using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public class AnimationFrame
    {
        public Rectangle SourceRectangle { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
