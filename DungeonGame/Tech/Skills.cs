using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    [Serializable]
    public abstract class Skill
    {
        public int level;

        protected string _name;
        protected int _pointsToLearn;

        public string Name => _name;
        public int PointsToLearn => _pointsToLearn;

        public abstract Type GetSkillType();
        public abstract void Regenerate();

    }
    [Serializable]
    public abstract class ActiveSkill : Skill
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
                if (value > 0) _cooldown = value;
                else _cooldown = 0;
            }
        }
        public int CooldownTime => _cooldownTime;
        public int BaseDamage => _baseDamage;
        public AttackTypes AttackType => _attackType;

        public override Type GetSkillType()
        {
            return typeof(ActiveSkill);
        }

        public void Use(Character target)
        {
            int dmg = damage;
            if (_attackType == target.Weakness)
            {
                dmg = (int)(dmg * 1.5);
            }
            else if (_attackType == target.Resistance)
            {
                dmg = (int)(dmg / 1.5);
            }
            target.Health -= dmg;
            Game1.actions.Text += $"Вы нанесли {dmg} урона с помощью {_name}\n";
            Player.GetPlayer().CooldownTick();
            Cooldown = CooldownTime;
        }
    }
    [Serializable]
    public class Punch : ActiveSkill
    {
        public Punch()
        {
            _name = "Удар";
            _baseDamage = 30;
            _cooldownTime = 3;
            _cooldown = 0;
            _pointsToLearn = 2;
            damage = _baseDamage;
            _attackType = AttackTypes.Physical;
            level = 0;
        }
        public override void Regenerate()
        {
            damage = BaseDamage + level * 2 + Player.GetPlayer().Strength * 3;
        }
    }
}
