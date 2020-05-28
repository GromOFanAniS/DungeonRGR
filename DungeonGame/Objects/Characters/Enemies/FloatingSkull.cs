using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    class FloatingSkull : Enemy
    {
        public FloatingSkull()
        {
            Name = "Левитирующий череп";
            _maxHealth = 20 + 10 * Game1.difficulty;
            _health = _maxHealth;
            experience = 15 + 25 * Game1.difficulty;
            Y += 6;
            _bodyParts = new List<AttackSpots>()
            {
                AttackSpots.Head
            };
            _weakSpots = new List<AttackSpots>();
            _weakness = AttackTypes.Physical;
            _resistance = AttackTypes.Magical;
            _attacks = new List<Attack>()
            {
                new Attack(5 + 2 * Game1.difficulty, 75, AttackTypes.Magical, AttackSpots.Body, "Магическая комета"),
                new Attack(7 + 3 * Game1.difficulty, 50, AttackTypes.Magical, AttackSpots.Body, "Страх")
            };

        }

        protected override void AnimationInitialize()
        {
            Animation idle = new Animation();
            for (int i = 0; i < 4; i++)
                idle.AddFrame(new Rectangle(0 + 126 * i, 0, 126, 126), TimeSpan.FromSeconds(0.25));
            _animationPlayer.AddAnimation(Animations.Idle, idle);
            _animationPlayer.SetAnimation(Animations.Idle);
        }
    }
}
