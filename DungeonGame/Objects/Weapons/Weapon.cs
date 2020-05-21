using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    [Serializable]
    public abstract class Weapon
    {

        [NonSerialized]
        protected Texture2D _texture;
        protected string _name;
        protected int _damage;
        protected int _durability;
        [NonSerialized]
        protected Rectangle _hitbox;
        [NonSerialized]
        protected int x;
        [NonSerialized]
        protected int y;
        protected bool _enabled;
        protected AttackTypes _attackType;

        public AttackTypes AttackType => _attackType;
        public string Name => _name;
        public int Damage => _damage;
        public int Durability
        {
            get => _durability;
            private set
            {
                if (value > 0)
                    _durability = value;
                else
                    _durability = 0;
            }
        }

        protected int Width => _texture.Width;
        protected int Height => _texture.Height;

        public Weapon()
        {
            _enabled = true;
            y = 300;
        }

        public void Draw(SpriteBatch s)
        {
            if (_enabled)
            {
                Vector2 topLeftOfSprite = new Vector2(x, y);
                s.Draw(_texture, topLeftOfSprite);
            }

        }
        public void DamageWeapon()
        {
            Durability--;
        }
        public void Update()
        {
            if (_enabled)
            {
                Rectangle playerPosition = new Rectangle((int)Game1.player.X, (int)Game1.player.Y - 1, Game1.player.Width, Game1.player.Height);
                if (_hitbox.Intersects(playerPosition))
                {
                    Game1.player.DrawWeaponLabel(this, true);
                    if (Game1.keyboardState.IsKeyDown(Keys.Space))
                    {
                        Game1.player.DrawWeaponLabel(this, false);
                        Game1.player.TakeNewWeapon(this);
                        Game1.actions.Text += $"Вы взяли {_name}";
                        _enabled = false;
                    }
                }
                else
                {
                    Game1.player.DrawWeaponLabel(this, false);
                }
            }
        }
    }
    
}
