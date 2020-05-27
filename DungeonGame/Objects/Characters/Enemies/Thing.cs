using Microsoft.Xna.Framework;
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
            _maxHealth = 40 + 20 * Game1.difficulty;//TODO: ребаланс
            _health = _maxHealth;
            experience = 10 + 20 * Game1.difficulty;//TODO: ребаланс
            X += 12;
            Y -= 30;
            _bodyParts = new List<AttackSpots>()
            {
                AttackSpots.Head,
                AttackSpots.Body,
                AttackSpots.Hands
            };
            _weakSpots = new List<AttackSpots>()//TODO: ребаланс
            {
                AttackSpots.Body
            };
            _weakness = AttackTypes.Physical;//TODO: ребаланс
            _attacks = new List<Attack>()//TODO: ребаланс
            {
                new Attack(3 + 1 * Game1.difficulty, 75, AttackTypes.Poison, AttackSpots.Body, "Наскок"),
                new Attack(6 + 2 * Game1.difficulty, 50, AttackTypes.Poison, AttackSpots.Body, "Поедание")
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
