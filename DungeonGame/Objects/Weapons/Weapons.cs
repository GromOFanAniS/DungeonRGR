using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DungeonGame
{
    [Serializable]
    public class ShortSword : Weapon
    {
        private static Texture2D _shortSwordTexture;
        public ShortSword()
        {
            _name = "Короткий меч";
            _damage = 4 + 1 * GameClass.difficulty;
            _durability = 20;
            _attackType = AttackTypes.Physical;
            _texture = _shortSwordTexture;
            x = (GameClass.WindowWidth - Width) / 2 + 100;
            y += 12;
            _hitbox = new Rectangle(x + 75, y, Width - 125, Height);
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
            _damage = 7 + 3 * GameClass.difficulty;
            _durability = 40;
            _texture = _swordTexture;
            _attackType = AttackTypes.Physical;
            x = (GameClass.WindowWidth - Width) / 2 + 100;
            _hitbox = new Rectangle(x + 75, y, Width - 125, Height);
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
            _damage = 5 + 2 * GameClass.difficulty;
            _durability = 25;
            _texture = _bowTexture;
            _attackType = AttackTypes.Ranged;
            x = (GameClass.WindowWidth - Width) / 2 + 50;
            _hitbox = new Rectangle(x + 75, y, Width - 125, Height);
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
            _damage = 8 + 4 * GameClass.difficulty;
            _durability = 15;
            _texture = _mSwordTexture;
            _attackType = AttackTypes.Magical;
            x = (GameClass.WindowWidth - Width) / 2 + 100;
            y -= 28;
            _hitbox = new Rectangle(x + 75, y, Width - 125, Height);
        }
        public static void Load(ContentManager content)
        {
            _mSwordTexture = content.Load<Texture2D>("Objects/MagicSword");
        }
    }
}
