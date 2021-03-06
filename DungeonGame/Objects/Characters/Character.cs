﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    [Serializable]
    public enum AttackTypes
    {
        None,
        Physical,
        Magical,
        Poison,
        Ranged,
        Special
    }
    [Serializable]
    public enum AttackSpots
    {
        Head,
        Body,
        Hands,
        Legs
    }
    public enum Animations
    {
        Idle,
        Walk,
        Attack
    }
    [Serializable]
    public abstract class Character
    {
        [Serializable]
        public class Attack
        {
            private int _damage;
            private int _successChance;
            private AttackTypes _type;

            private readonly string _name;
            private readonly int _baseDamage;
            private readonly int _baseChance;
            private readonly AttackSpots _spot;

            public string Name => _name;
            public int BaseDamage => _baseDamage;
            public int BaseChance => _baseChance;
            public AttackSpots Spot => _spot;
            public int Damage { get => _damage; set => _damage = value; }
            public int SuccessChance { get => _successChance; set => _successChance = value; }
            public AttackTypes Type { get => _type; set => _type = value; }

            public Attack(int baseDamage = 0, int baseChance = 0, AttackTypes type = AttackTypes.None, AttackSpots spot = AttackSpots.Body, string name = "")
            {
                _damage = baseDamage;
                _baseDamage = baseDamage;
                _baseChance = baseChance;
                _successChance = baseChance;
                _type = type;
                _spot = spot;
                _name = name;
            }
        }

        [NonSerialized]
        protected HealthBar _healthBar;
        [NonSerialized]
        protected AnimationPlayer _animationPlayer = new AnimationPlayer();
        [NonSerialized]
        protected Texture2D _texture;
        [NonSerialized]
        protected static ContentManager _content;
        protected int _health;
        protected int _maxHealth;
        protected AttackTypes _weakness;
        protected AttackTypes _resistance;

        protected List<Attack> _attacks;
        protected List<AttackSpots> _weakSpots;

        //public List<Attack> Attacks => _attacks;
        public virtual int Health
        {
            get => _health;
            set
            {
                if (value <= _maxHealth && value >= 0)
                    _health = value;
                else if (value < 0)
                    _health = 0;
                else
                    _health = _maxHealth;
            }
        }

        public float X { get; protected set; } = -100;

        public float Y { get; protected set; }
        public int Height => _animationPlayer.CurrentRectangle.Height;
        public int Width => _animationPlayer.CurrentRectangle.Width;
        public string Name { get; set; }

        public AnimationPlayer AnimationPlayer => _animationPlayer;

        public AttackTypes Weakness => _weakness;
        public AttackTypes Resistance => _resistance;

        protected abstract void AnimationInitialize();

        public abstract void Update(GameTime gameTime);
        public virtual void Draw(SpriteBatch s)
        {
            Vector2 topLeftOfSprite = new Vector2(X, Y);
            _animationPlayer.Draw(s, _texture, topLeftOfSprite, SpriteEffects.None);
        }

        public void Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static void LoadCharacters(ContentManager content)
        {
            _content = content;
            Player.Load();
        }

        public void DoAttack(Character target, Attack attack)
        {
            if (GameClass.random.Next(0, 100) <= attack.SuccessChance)
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
                        GameClass.actions.Text += $"{target.Name} уязвим к данному типу урона\n";
                    }
                    else if (attack.Type == target._resistance)
                    {
                        dmg = (int)(dmg / 1.5);
                        GameClass.actions.Text += $"{target.Name} устойчив к данному типу урона\n";
                    }
                    if (GameClass.random.Next(0, 100) < 10)
                    {
                        dmg *= 2;
                        GameClass.actions.Text += "Крит! ";
                    }
                }
                GameClass.actions.Text += string.Format("{0} использовал {1}! Урон: {2}\n", Name, attack.Name, dmg);
                target.Health -= dmg;
            }
            else
            {
                GameClass.actions.Text += $"{Name} промахнулся\n";
            }
        }
    }
}
