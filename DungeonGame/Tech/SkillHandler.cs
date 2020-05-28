using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    [Serializable]
    public class SkillHandler
    {
        private List<Skill> skills;
        private List<ActiveSkill> activeSkills;
        private List<PassiveSkill> passiveSkills;

        private int _skillPoints;

        public int SkillPoints
        {
            get => _skillPoints;
            set
            {
                if (value > 0)
                    _skillPoints = value;
                else
                    _skillPoints = 0;
            }
        }

        public SkillHandler()
        {
            activeSkills = new List<ActiveSkill>();
            passiveSkills = new List<PassiveSkill>();
            SkillsInitialize();
        }

        public bool IsEnoughPoints(string skillName) => _skillPoints >= skills.Find(x => x.Name == skillName).PointsToLearn;

        public bool IsInCooldown(string skillName) => activeSkills.Find(x => x.Name == skillName).Cooldown > 0;

        public void UpgradeSkill(string skillName)
        {
            var skill = skills.Find(x => x.Name == skillName);
            SkillPoints -= skill.PointsToLearn;
            skill.level++;
        }

        public void UseSkill(string skillName, Character target)
        {
            activeSkills.Find(x => x.Name == skillName).Use(target);
        }

        public void RegenerateSkills()
        {
            foreach (ActiveSkill skill in activeSkills)
            {
                if (skill.level > 0) skill.Regenerate();
            }
        }

        public void CooldownTick()
        {
            foreach (ActiveSkill skill in activeSkills)
                skill.Cooldown--;
        }

        public int FindPassiveSkillLevel(string skillName)
        {
            return passiveSkills.Find(x => x.Name == skillName).level;
        }

        public Dictionary<string, Button> GenerateUpgradeSkillButtons()
        {
            Dictionary<string, Button> buttons = new Dictionary<string, Button>();
            int y = Game1.WindowHeight / 7;
            int i = 0;
            foreach (var skill in activeSkills)
            {
                buttons.Add($"{skill.Name}", new Button(800, y + (Button.Height + 17) * i, "Улучшить\nнавык"));
                i++;
            }
            foreach (var skill in passiveSkills)
            {
                buttons.Add($"{skill.Name}", new Button(800, y + (Button.Height + 17) * i, "Улучшить\nнавык"));
                i++;
            }
            buttons.Add("Exit", new Button(5, 5, "Назад"));
            return buttons;
        }

        public Dictionary<string, Button> GenerateUseSkillButtons()
        {
            int x = Game1.WindowWidth / 5 * 4;
            int y = Game1.WindowHeight / 8;
            int i = 1;
            Dictionary<string, Button> buttons = new Dictionary<string, Button>();
            foreach (Skill skill in activeSkills.FindAll(k => k.level > 0))
            {
                buttons.Add(skill.Name, new Button(x, y * i, $"{skill.Name}"));
                i++;
            }
            buttons.Add("Strikes", new Button(x, y * i, "Атаковать"));

            return buttons;
        }

        public void LabelsUpdate(Label skillsLabel)
        {
            foreach (ActiveSkill skill in activeSkills)
            {
                skillsLabel.Text += string.Format("{0, 15}: Уровень {1, 2}, Необходимо очков: {2, 2}, Урон: {3, 3},\n "
                                                   + "Время перезарядки: {4, 2}, Тип урона: {5, 5}\n\n",
                                                   skill.Name, skill.level, skill.PointsToLearn, skill.damage,
                                                   skill.CooldownTime, skill.AttackType);
            }
            foreach (PassiveSkill skill in passiveSkills)
            {
                skillsLabel.Text += string.Format("{0, 15}: Уровень {1, 2}, Необходимо очков: {2, 2},\n "
                                                   + "{3}\n\n",
                                                   skill.Name, skill.level, skill.PointsToLearn, skill.Description);
            }
        }


        private void SkillsInitialize()
        {
            skills = new List<Skill>()
            {
                new CripplingStrike(),
                new FireBall(),
                new ExperienceBuff(),
                new StoneSkin()
            };
            foreach (ActiveSkill skill in skills.FindAll(x => x.GetSkillType() == typeof(ActiveSkill)))
                activeSkills.Add(skill);
            foreach (PassiveSkill skill in skills.FindAll(x => x.GetSkillType() == typeof(PassiveSkill)))
                passiveSkills.Add(skill);
        }
    }
}
