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
    }

    [Serializable]
    public abstract class PassiveSkill : Skill
    {
        protected string _description;

        public string Description => _description;

        public override Type GetSkillType()
        {
            return typeof(PassiveSkill);
        }
    }
    [Serializable]
    public class ExperienceBuff : PassiveSkill
    {
        public ExperienceBuff()
        {
            _name = "Ученик";
            _pointsToLearn = 6;
            _description = "Увеличивает количество получаемого опыта";
            level = 0;
        }
    }
    [Serializable]
    public class StoneSkin : PassiveSkill
    {
        public StoneSkin()
        {
            _name = "Каменная кожа";
            _pointsToLearn = 3;
            _description = "Уменьшает получаемый урон";
            level = 0;
        }
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

        public abstract void Regenerate();

        public override Type GetSkillType()
        {
            return typeof(ActiveSkill);
        }

        public void Use(Character target)
        {
            Player.GetPlayer().AnimationPlayer.Play(Animations.Attack);
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
    public class CripplingStrike : ActiveSkill
    {
        public CripplingStrike()
        {
            _name = "Критический удар";
            _baseDamage = 10;
            _cooldownTime = 3;
            _cooldown = 0;
            _pointsToLearn = 2;
            damage = _baseDamage;
            _attackType = AttackTypes.Physical;
            level = 0;
        }
        public override void Regenerate()
        {
            damage = BaseDamage + level * 2 + Player.GetPlayer().Strength * 2;
        }
    }
    [Serializable]
    public class FireBall : ActiveSkill
    {
        public FireBall()
        {
            _name = "Огненный шар";
            _baseDamage = 20;
            _cooldownTime = 5;
            _cooldown = 0;
            _pointsToLearn = 3;
            _attackType = AttackTypes.Magical;
            damage = _baseDamage;
            level = 0;
        }
        public override void Regenerate()
        {
            damage = BaseDamage + level * 3 + Player.GetPlayer().Intelligence * 4;
        }
    }
}
