using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DungeonGame
{
    abstract class Enemy : Character
    {
        public static Enemy Generate()
        {
            switch (Game1.random.Next(0, 10))
            {
                case 0: return new Slime();
                default: return new Slime();
            }
        }
    }
}
