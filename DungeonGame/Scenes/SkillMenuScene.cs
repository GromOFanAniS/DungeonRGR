using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
     class SkillMenuScene : Scene
    {
        private readonly Dictionary<string, Button> _buttons;
        private readonly Label _skillPointsLabel;
        private readonly Label _skillsLabel;
        private readonly Player player = Player.GetPlayer();

        public SkillMenuScene()
        {
            _buttons = new Dictionary<string, Button>();
            _skillPointsLabel = new Label(5, Button.Height + 3, "");
            _skillsLabel = new Label(5, 100, "");
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            int y = Game1.WindowHeight  / 7;
            int i = 0;
            foreach(var skill in player.skills)
            {
                _buttons.Add($"{skill.Name}", new Button(800, y + (Button.Height + 17) * i , "улучшить навык"));
                i++;
            }
            _buttons.Add("Exit", new Button(5, 5, "Назад"));
        }
        private void LabelsUpdate()
        {
            var activeSkills = player.skills.FindAll(x => x.GetSkillType() == typeof(ActiveSkill));
            var passiveSkills = player.skills.FindAll(x => x.GetSkillType() == typeof(PassiveSkill));
            foreach(ActiveSkill skill in activeSkills)
            {
                _skillsLabel.Text += string.Format("{0, 15}: Уровень {1, 2}, Необходимо очков: {2, 2}, Урон: {3, 3},\n "
                                                   + "Время перезарядки: {4, 2}, Тип урона: {5, 5}\n\n",
                                                   skill.Name, skill.level, skill.PointsToLearn, skill.damage,
                                                   skill.CooldownTime, skill.AttackType);
            }
            foreach(PassiveSkill skill in passiveSkills)
            {
                _skillsLabel.Text += string.Format("{0, 15}: Уровень {1, 2}, Необходимо очков: {2, 2},\n "
                                                   + "{3}\n\n",
                                                   skill.Name, skill.level, skill.PointsToLearn, skill.Description);
            }
        }

        public override void Draw(SpriteBatch s)
        {
            foreach (var button in _buttons.Values)
                button.Draw(s);
            _skillPointsLabel.Draw(s);
            _skillsLabel.Draw(s);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var button in _buttons)
            {
                if (button.Key != "Exit" && player.SkillPoints < player.skills.Find(x => x.Name == button.Key).PointsToLearn)
                    button.Value.IsActive = false;
                else
                    button.Value.IsActive = true;
                button.Value.Update();
                if(button.Value.State == StateButton.Press)
                    switch(button.Key)
                    {
                        case "Exit":
                            Game1.gameState = GameState.PlayerMenuScene;
                            DoNewGenerate = true;
                            break;
                        default:
                            player.UpgradeSkill(button.Key);
                            break;
                    }       
            }
            _skillPointsLabel.Text = $"Очков навыков: {player.SkillPoints}";
            LabelsUpdate();
        }
    }
}
