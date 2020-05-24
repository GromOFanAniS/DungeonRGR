using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    [Serializable]
    public class ShortSword : Weapon
    {
        private static Texture2D _shortSwordTexture;
        public ShortSword()
        {
            _name = "Короткий меч";
            _damage = 4 + 1 * Game1.difficulty;
            _durability = 20;
            _attackType = AttackTypes.Physical;
            _texture = _shortSwordTexture;
            x = (Game1.WindowWidth - Width) / 2 + Width * 5;
            _hitbox = new Rectangle(x, y, Width, Height);
        }

        public static void Load(ContentManager content)
        {
            _shortSwordTexture = content.Load<Texture2D>("Objects/Sword1");
        }
    }
    [Serializable]
    public class Sword : Weapon
    {
        private static Texture2D _swordTexture;
        public Sword()
        {
            _name = "Меч";
            _damage = 7 + 3 * Game1.difficulty;
            _durability = 40;
            _texture = _swordTexture;
            _attackType = AttackTypes.Physical;
            x = (Game1.WindowWidth - Width) / 2 + Width * 5;
            _hitbox = new Rectangle(x, y, Width, Height);
        }
        public static void Load(ContentManager content)
        {
            _swordTexture = content.Load<Texture2D>("Objects/Sword2");
        }
    }
    [Serializable]
    public class Bow : Weapon
    {
        private static Texture2D _bowTexture;

        public Bow()
        {
            _name = "Лук";
            _damage = 5 + 2 * Game1.difficulty;
            _durability = 25;
            _texture = _bowTexture;
            _attackType = AttackTypes.Ranged;
            x = (Game1.WindowWidth - Width) / 2 + Width * 10;
            _hitbox = new Rectangle(x, y, Width, Height);
        }
        public static void Load(ContentManager content)
        {
            _bowTexture = content.Load<Texture2D>("Objects/Bow");
        }
    }
    [Serializable]
    public class SkyFracture : Weapon
    {
        private static Texture2D _mSwordTexture;

        public SkyFracture()
        {
            _name = "Магический меч";
            _damage = 8 + 4 * Game1.difficulty;
            _durability = 15;
            _texture = _mSwordTexture;
            _attackType = AttackTypes.Magical;
            x = (Game1.WindowWidth - Width) / 2 + Width * 10;
            _hitbox = new Rectangle(x, y, Width, Height);
        }
        public static void Load(ContentManager content)
        {
            _mSwordTexture = content.Load<Texture2D>("Objects/MagicSword");
        }
    }
}
