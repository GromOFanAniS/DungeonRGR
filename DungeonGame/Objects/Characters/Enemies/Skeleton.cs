using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    class Skeleton : Enemy
    {
        public Skeleton()
        {
            Name = "Скелет";
            _maxHealth = 30 + 25 * Game1.difficulty;
            _health = _maxHealth;
            experience = 20 + 25 * Game1.difficulty;
            Y += 6;
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
                AttackSpots.Hands,
                AttackSpots.Legs
            };
            _weakness = AttackTypes.Physical;
            _resistance = AttackTypes.Ranged;
            _attacks = new List<Attack>()
            {
                new Attack(6 + 2 * Game1.difficulty, 55, AttackTypes.Physical, AttackSpots.Body, "Удар"),
                new Attack(9 + 3 * Game1.difficulty, 40, AttackTypes.Physical, AttackSpots.Body, "Удар"),
                new Attack(5 + 2 * Game1.difficulty, 65, AttackTypes.Physical, AttackSpots.Body, "Удар")

            };

        }

        protected override void AnimationInitialize()
        {
            Animation idle = new Animation();
            for (int i = 0; i < 11; i++)
                idle.AddFrame(new Rectangle(0 + 96*i, 0, 96, 128), TimeSpan.FromSeconds(0.25));
            _animationPlayer.AddAnimation(Animations.Idle, idle);
            _animationPlayer.SetAnimation(Animations.Idle);
        }
    }
}
