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
        private Player player = Player.GetPlayer();
        private Dictionary<string, Button> _buttons;
        private Label _skillPointsLabel;
        private Label _skillsLabel;

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
                _buttons.Add($"{skill.Name}", new Button(700, y + (Button.Height + 17) * i , "улучшить навык"));
                i++;
            }
            _buttons.Add("Exit", new Button(0, 0, "Назад"));
        }
        private void LabelsUpdate()
        {
            var ActiveSkills = player.skills.FindAll(x => x.GetSkillType() == typeof(ActiveSkill));
            //var PassiveSkills = player.skills.FindAll(x => x.GetSkillType() == typeof(PassiveSkill));
            foreach(ActiveSkill skill in ActiveSkills)
            {
                _skillsLabel.Text += string.Format("{0, 10}: Уровень {1, 2}, Необходимо очков: {2, 2}, Урон: {3, 3},\n "
                                                   + "Время перезарядки: {4, 2}, Тип урона: {5, 5}\n\n",
                                                   skill.Name, skill.level, skill.PointsToLearn, skill.damage,
                                                   skill.CooldownTime, skill.AttackType);
            }
            //foreach(PassiveSkill skill in PassiveSkill)
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
                if (player.SkillPoints == 0 && button.Key != "Exit")
                    button.Value.isActive = false;
                else
                    button.Value.isActive = true;
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
