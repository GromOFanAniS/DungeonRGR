﻿using System;
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
    public enum AttackSpots
    {
        Head,
        Body,
        Hands,
        Legs
    }
    public abstract class Character
    {
        public class Attack
        {
            private int _damage;
            private int _baseDamage;
            private int _baseChance;
            private int _successChance;
            private AttackTypes _type;
            private AttackSpots _spot;

            public int Damage { get => _damage; }
            public int BaseDamage { get => _baseDamage; }
            public int BaseChance { get => _baseChance; }
            public int SuccessChance { get => _successChance; }
            public AttackTypes Type { get => _type; }
            public AttackSpots Spot { get => _spot; }

            public Attack(int damage = 0, int baseDamage = 0, int baseChance = 0, int successChance = 0, AttackTypes type = AttackTypes.None, AttackSpots spot = AttackSpots.Body)
            {
                _damage = damage;
                _baseDamage = baseDamage;
                _baseChance = baseChance;
                _successChance = successChance;
                _type = type;
                _spot = spot;
            }
        }

        protected int _health;
        protected int _maxHealth;
        protected static Texture2D _texture;
        protected AttackTypes _weakness;
        protected AttackTypes _resistance;
        protected Animation _animation;

        protected List<Attack> _attacks;
        protected List<AttackSpots> _weakSpots;

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
            if (Game1.random.Next(0, 100) <= attack.SuccessChance)
            {
                int dmg = attack.Damage;
                if (attack.Type != AttackTypes.Special)
                {
                    if (target._weakSpots.Contains(attack.Spot))
                    {
                        dmg = (int)(dmg * 1.5);
                    }
                    if (attack.Type == target._weakness)
                    {
                        dmg = (int)(dmg * 1.5);
                    }
                    else if (attack.Type == target._resistance)
                    {
                        dmg = (int)(dmg / 1.5);
                    }
                    if (Game1.random.Next(0, 100) < 10)
                    {
                        dmg *= 2;
                    }
                }
                target.Health -= dmg;
            }
            else
            {
                //промазал
            }
        }
    }
}
