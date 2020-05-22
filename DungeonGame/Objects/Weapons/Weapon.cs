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
                Rectangle playerPosition = new Rectangle((int)Player.GetPlayer().X, (int)Player.GetPlayer().Y - 1, Player.GetPlayer().Width, Player.GetPlayer().Height);
                if (_hitbox.Intersects(playerPosition))
                {
                    Player.GetPlayer().DrawWeaponLabel(this, true);
                    if (Game1.keyboardState.IsKeyDown(Keys.Space))
                    {
                        Player.GetPlayer().DrawWeaponLabel(this, false);
                        Player.GetPlayer().TakeNewWeapon(this);
                        Game1.actions.Text += $"Вы взяли {_name}";
                        _enabled = false;
                    }
                }
                else
                {
                    Player.GetPlayer().DrawWeaponLabel(this, false);
                }
            }
        }
    }
    
}
