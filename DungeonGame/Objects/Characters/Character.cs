using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public enum AttackTypes
    {
        None,
        Physical,
        Magic,
        Poison,
        Ranged,
        Special
    }
    public abstract class Character
    {
        public class Attack
        {
            private int _damage;
            private int _baseDamage;
            private int _baseChance;
            private int _successChance;
            private AttackTypes _attackType;

            public int Damage { get => _damage; set => _damage = value; }
            public int BaseDamage { get => _baseDamage; }
            public int BaseChance { get => _baseChance; }
            public int SuccessChance { get => _successChance; }
            public AttackTypes AttackType { get => _attackType; }
        }

        protected int _health;
        protected int _maxHealth;
        protected static Texture2D _texture;
        protected AttackTypes _weakness;
        protected AttackTypes _resistance;
        protected Animation _animation;

        protected List<Attack> _attacks;

        protected int Health
        {
            get => _health;
            set
            {
                if (value <= _maxHealth)
                    _health = value;
                else
                    _health = _maxHealth;
            }
        }

        public float X { get; protected set; } = -100;

        public float Y { get; protected set; }
        public int Height => _animation.CurrentRectangle.Height;
        public int Width => _animation.CurrentRectangle.Width;

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch s);

        public void Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static void LoadCharacters(ContentManager content)
        {
            Player.Load(content);
            Slime.Load(content);
        }

        public void DoAttack(Character target, Attack attack)
        {
            target.Health -= attack.Damage;
        }
    }
}
