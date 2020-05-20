using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    [Serializable]
    public enum AttackTypes
    {
        None,
        Physical,
        Magic,
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
    [Serializable]
    public abstract class Character
    {
        [Serializable]
        public class Attack
        {
            private string _name;
            private int _damage;
            private int _baseDamage;
            private int _baseChance;
            private int _successChance;
            private AttackTypes _type;
            private AttackSpots _spot;

            public int Damage { get => _damage; set => _damage = value; }
            public string Name { get => _name; }
            public int BaseDamage { get => _baseDamage; }
            public int BaseChance { get => _baseChance; }
            public int SuccessChance { get => _successChance; set => _successChance = value; }
            public AttackTypes Type { get => _type; }
            public AttackSpots Spot { get => _spot; }

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

        protected int _health;
        protected int _maxHealth;
        [NonSerialized]
        protected HealthBar _healthBar;
        protected AttackTypes _weakness;
        protected AttackTypes _resistance;
        [NonSerialized]
        protected Animation _animation;

        protected List<Attack> _attacks;
        protected List<AttackSpots> _weakSpots;

        //public List<Attack> Attacks => _attacks;
        public int Health
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
        public int Height => _animation.CurrentRectangle.Height;
        public int Width => _animation.CurrentRectangle.Width;
        public string Name { get; set; }
        

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
                        Game1.actions.Text += "Крит!";
                    }
                }
                Game1.actions.Text += string.Format("{0} использовал {1}! Урон: {2}\n", Name, attack.Name, dmg);
                target.Health -= dmg;
            }
            else
            {
                Game1.actions.Text = $"{Name} промахнулся\n";
            }
        }
    }
}
