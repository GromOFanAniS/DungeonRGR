using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    class Goblin : Enemy
    {
        public Goblin()
        {
            Name = "Гоблин";
            _maxHealth = 50 + 23 * Game1.difficulty;
            _health = _maxHealth;
            experience = 20 + 30 * Game1.difficulty;
            X += 12;
            Y += 25;
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
            _resistance = AttackTypes.Poison;
            _attacks = new List<Attack>()
            {
                new Attack(7 + 3 * Game1.difficulty, 65, AttackTypes.Physical, AttackSpots.Body, "Удар"),
                new Attack(10 + 5 * Game1.difficulty, 50, AttackTypes.Physical, AttackSpots.Body, "Грабёж"),
                new Attack(2 + 1 * Game1.difficulty, 40, AttackTypes.Physical, AttackSpots.Body, "Слабый приём")
            };

        }

        protected override void AnimationInitialize()
        {
            Animation idle = new Animation();
            for (int i = 0; i < 4; i++)
                idle.AddFrame(new Rectangle(0 + 85 * i, 0, 85, 90), TimeSpan.FromSeconds(0.25));
            _animationPlayer.AddAnimation(Animations.Idle, idle);
            _animationPlayer.SetAnimation(Animations.Idle);
        }
    }
}
