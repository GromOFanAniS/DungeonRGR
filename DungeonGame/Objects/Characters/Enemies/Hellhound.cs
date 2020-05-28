using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace DungeonGame
{
    class Hellhound : Enemy
    {
        public Hellhound()
        {
            Name = "Адская Гончая";
            _maxHealth = 30 + 10 * GameClass.difficulty;
            _health = _maxHealth;
            experience = 20 + 30 * GameClass.difficulty;
            X -= 12;
            Y +=6;
            _bodyParts = new List<AttackSpots>()
            {
                AttackSpots.Head,
                AttackSpots.Body,
                AttackSpots.Hands,
                AttackSpots.Legs
            };
            _weakSpots = new List<AttackSpots>()
            {
                AttackSpots.Body
            };
            _weakness = AttackTypes.Magical;
            _resistance = AttackTypes.Physical;
            _attacks = new List<Attack>()
            {
                new Attack(7 + 3 * GameClass.difficulty, 55, AttackTypes.Physical, AttackSpots.Body, "Наскок"),
                new Attack(15 + 7 * GameClass.difficulty, 40, AttackTypes.Physical, AttackSpots.Body, "Хват за шею"),
                new Attack(5 + 2 * GameClass.difficulty, 60, AttackTypes.Physical, AttackSpots.Body, "Укус")
            };

        }

        protected override void AnimationInitialize()
        {
            Animation idle = new Animation();
            idle.AddFrame(new Rectangle(0, 0, 168, 128), TimeSpan.FromSeconds(1));
            for (int i = 1; i < 6; i++)
                idle.AddFrame(new Rectangle(0 + 168 * i, 0, 168, 128), TimeSpan.FromSeconds(0.25));
            for (int i = 0; i < 4; i++)
                idle.AddFrame(new Rectangle(672 - 168 * i, 0, 168, 128), TimeSpan.FromSeconds(0.25));
            _animationPlayer.AddAnimation(Animations.Idle, idle);
            _animationPlayer.SetAnimation(Animations.Idle);
        }
    }
}
