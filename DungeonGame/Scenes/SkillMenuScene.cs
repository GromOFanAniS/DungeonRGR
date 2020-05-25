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
            _buttons = player.SkillHandler.GenerateUpgradeSkillButtons();
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
                if (button.Key != "Exit" && !player.SkillHandler.IsEnoughPoints(button.Key))
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
                            player.SkillHandler.UpgradeSkill(button.Key);
                            break;
                    }       
            }
            _skillPointsLabel.Text = $"Очков навыков: {player.SkillHandler.SkillPoints}";
            player.SkillHandler.LabelsUpdate(_skillsLabel);
        }
    }
}
