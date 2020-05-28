using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    class Dio : Enemy
    {
        public Dio()
        {
            Name = "Дио";
            _maxHealth = 100 + 35 * Game1.difficulty;
            _health = _maxHealth;
            experience = 100 + 200 * Game1.difficulty;
            X -= 100;
            Y -= 62;
            _bodyParts = new List<AttackSpots>()
            {
                AttackSpots.Head,
                AttackSpots.Body,
                AttackSpots.Hands,
                AttackSpots.Legs
            };
            _weakSpots = new List<AttackSpots>();
            _weakness = AttackTypes.Magical;
            _resistance = AttackTypes.Physical;
            _attacks = new List<Attack>()
            {
                new Attack(20 + 5 * Game1.difficulty, 55, AttackTypes.Magical, AttackSpots.Body, "Удар the World"),
                new Attack(50 + 8 * Game1.difficulty, 20, AttackTypes.Special, AttackSpots.Body, "Остановка времени"),
                new Attack(15 + 3 * Game1.difficulty, 80, AttackTypes.Physical, AttackSpots.Body, "Удар вампира")
            };

        }

        protected override void AnimationInitialize()
        {
            Animation spawn = new Animation();
            for (int i = 0; i < 4; i++)
                spawn.AddFrame(new Rectangle(2860 - 260 * i, 0, 260, 280), TimeSpan.FromSeconds(0.25));
            for (int i = 0; i < 2; i++)
            {
                spawn.AddFrame(new Rectangle(2860 - 260 * 5, 0, 260, 282), TimeSpan.FromSeconds(0.25));
                spawn.AddFrame(new Rectangle(2860 - 260 * 6, 0, 260, 282), TimeSpan.FromSeconds(0.25));
            }
            for (int i = 0; i < 4; i++)
                spawn.AddFrame(new Rectangle(2860 - 260 * (i+7), 0, 260, 282), TimeSpan.FromSeconds(0.25));
            Animation idle = new Animation();
            for (int i = 0; i < 12; i++)
            {
                idle.AddFrame(new Rectangle(2860 - 260 * 10, 0, 260, 282), TimeSpan.FromSeconds(0.25));
                idle.AddFrame(new Rectangle(2860 - 260 * 11, 0, 260, 282), TimeSpan.FromSeconds(0.25));
            }
            _animationPlayer.AddAnimation(Animations.Walk, spawn);
            _animationPlayer.AddAnimation(Animations.Idle, idle);
            _animationPlayer.Play(Animations.Walk);
        }
    }
}
