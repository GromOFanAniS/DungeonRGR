using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.Tech
{
    [Serializable]
    abstract class Skill
    {
        public int level;

        protected string _name;
        protected int _pointsToLearn;

        public string Name => _name;
        public int PointsToLearn => _pointsToLearn;
    }
    [Serializable]
    abstract class ActiveSkill
    {
        public int damage;

        protected int _baseDamage;
        protected int _cooldownTime;
        protected int _cooldown;
        protected AttackTypes _attackType;

        public int Cooldown
        {
            get => _cooldown;
            set
            {
                if (value <= 0) _cooldown = 0;
                else _cooldown = value;
            }
        }
        public int CooldownTime => _cooldownTime;
        public int BaseDamage => _baseDamage;
        public AttackTypes AttackType => _attackType;
    }
}
