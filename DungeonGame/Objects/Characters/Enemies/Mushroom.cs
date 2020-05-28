using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace DungeonGame
{
    class Mushroom : Enemy
    {
        public Mushroom()
        {
            Name = "Гриб";
            _maxHealth = 60 + 15 * GameClass.difficulty;
            _health = _maxHealth;
            experience = 25 + 20 * GameClass.difficulty;
            X += 12;
            Y -= 7;
            _bodyParts = new List<AttackSpots>()
            {
                AttackSpots.Head,
                AttackSpots.Body,
                AttackSpots.Hands,
                AttackSpots.Legs
            };
            _weakSpots = new List<AttackSpots>()
            {
                AttackSpots.Body,
                AttackSpots.Head
            };
            _weakness = AttackTypes.Physical;
            _resistance = AttackTypes.Poison;
            _attacks = new List<Attack>()
            {
                new Attack(5 + 2 * GameClass.difficulty, 65, AttackTypes.Physical, AttackSpots.Body, "Удар"),
                new Attack(10 + 3 * GameClass.difficulty, 50, AttackTypes.Poison, AttackSpots.Body, "Облако спор"),
                new Attack(8 + 3 * GameClass.difficulty, 55, AttackTypes.Magical, AttackSpots.Body, "Проклятие"),
            };

        }

        protected override void AnimationInitialize()
        {
            Animation idle = new Animation();
            for (int i = 0; i < 4; i++)
                idle.AddFrame(new Rectangle(0 + 100 * i, 0, 100, 154), TimeSpan.FromSeconds(0.25));
            _animationPlayer.AddAnimation(Animations.Idle, idle);
            _animationPlayer.SetAnimation(Animations.Idle);
        }
    }
}
