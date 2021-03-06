﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace DungeonGame
{
    class Demon : Enemy
    {
        public Demon()
        {
            Name = "Демон";
            _maxHealth = 80 + 40 * GameClass.difficulty;
            _health = _maxHealth;
            experience = 80 + 20 * GameClass.difficulty;
            X += 12;
            Y -= 4;
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
            _weakness = AttackTypes.Physical;
            _resistance = AttackTypes.Magical;
            _attacks = new List<Attack>()
            {
                new Attack(10 + 3 * GameClass.difficulty, 70, AttackTypes.Physical, AttackSpots.Body, "Удар"),
                new Attack(15 + 6 * GameClass.difficulty, 50, AttackTypes.Magical, AttackSpots.Body, "Проклятие")
            };

        }

        protected override void AnimationInitialize()
        {
            Animation idle = new Animation();
            for (int i = 0; i < 6; i++)
                idle.AddFrame(new Rectangle(0 + 100 * i, 0, 100, 156), TimeSpan.FromSeconds(0.25));
            _animationPlayer.AddAnimation(Animations.Idle, idle);
            _animationPlayer.SetAnimation(Animations.Idle);
        }
    }
}
