﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    class Thing : Enemy
    {
        public Thing()
        {
            Name = "Нечто";
            _maxHealth = 50 + 25 * Game1.difficulty;
            _health = _maxHealth;
            experience = 30 + 20 * Game1.difficulty;
            X += 12;
            Y -= 30;
            _bodyParts = new List<AttackSpots>()
            {
                AttackSpots.Head,
                AttackSpots.Body,
                AttackSpots.Hands
            };
            _weakSpots = new List<AttackSpots>();
            _weakness = AttackTypes.Magical;
            _resistance = AttackTypes.Physical;
            _attacks = new List<Attack>()
            {
                new Attack(2 + 3 * Game1.difficulty, 65, AttackTypes.Magical, AttackSpots.Body, "Нечто"),
                new Attack(2 + 5 * Game1.difficulty, 50, AttackTypes.Magical, AttackSpots.Body, "Нечто"),
                new Attack(5 + 7 * Game1.difficulty, 40, AttackTypes.Magical, AttackSpots.Body, "Нечто")
            };

        }

        protected override void AnimationInitialize()
        {
            Animation idle = new Animation();
            for (int i = 0; i < 4; i++)
                idle.AddFrame(new Rectangle(0 + 100 * i, 0, 100, 192), TimeSpan.FromSeconds(0.25));
            _animationPlayer.AddAnimation(Animations.Idle, idle);
            _animationPlayer.SetAnimation(Animations.Idle);
        }
    }
}
