using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    class Slime : Enemy
    {
        public Slime()
        {
            Name = "Слизень";
            _maxHealth = 10 + 20 * Game1.difficulty;
            _health = _maxHealth;
            experience = 10 + 15 * Game1.difficulty;
            Y += 38;
            _bodyParts = new List<AttackSpots>()
            {
                AttackSpots.Body
            };
            _weakSpots = new List<AttackSpots>()
            {
                AttackSpots.Body
            };
            _weakness = AttackTypes.Physical;
            _resistance = AttackTypes.Poison;
            _attacks = new List<Attack>()
            {
                new Attack(3 + 1 * Game1.difficulty, 65, AttackTypes.Poison, AttackSpots.Body, "Наскок"),
                new Attack(5 + 2 * Game1.difficulty, 50, AttackTypes.Poison, AttackSpots.Body, "Поедание")
            };

        }

        protected override void AnimationInitialize()
        {
            Animation idle = new Animation();
            idle.AddFrame(new Rectangle(0, 4, 120, 64), TimeSpan.FromSeconds(0.25));
            idle.AddFrame(new Rectangle(0, 76, 120, 64), TimeSpan.FromSeconds(0.25));
            _animationPlayer.AddAnimation(Animations.Idle, idle);
            _animationPlayer.SetAnimation(Animations.Idle);
        }
    }
}
