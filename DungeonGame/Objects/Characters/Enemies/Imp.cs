﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    class Imp : Enemy
    {
        public Imp()
        {
            Name = "Имп";
            _maxHealth = 40 + 15 * Game1.difficulty;
            _health = _maxHealth;
            experience = 15 + 25 * Game1.difficulty;
            X += 12;
            Y += 10;
            _bodyParts = new List<AttackSpots>()
            {
                AttackSpots.Head,
                AttackSpots.Body,
                AttackSpots.Hands,
                AttackSpots.Legs
            };
            _weakSpots = new List<AttackSpots>()
            {
                AttackSpots.Head,
                AttackSpots.Hands
            };
            _weakness = AttackTypes.Ranged;
            _resistance = AttackTypes.Magical;
            _attacks = new List<Attack>()
            {
                new Attack(5 + 2 * Game1.difficulty, 60, AttackTypes.Physical, AttackSpots.Body, "Удар"),
                new Attack(7 + 3 * Game1.difficulty, 40, AttackTypes.Magical, AttackSpots.Body, "Сгусток аргента"),
                new Attack(6 + 1 * Game1.difficulty, 55, AttackTypes.Physical, AttackSpots.Body, "Прыжок")
            };

        }

        protected override void AnimationInitialize()
        {
            Animation idle = new Animation();
            for (int i = 0; i < 6; i++)
                idle.AddFrame(new Rectangle(0 + 80 * i, 0, 80, 120), TimeSpan.FromSeconds(0.25));
            _animationPlayer.AddAnimation(Animations.Idle, idle);
            _animationPlayer.SetAnimation(Animations.Idle);
        }
    }
}