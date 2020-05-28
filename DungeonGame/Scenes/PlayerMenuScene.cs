using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DungeonGame
{
    class PlayerMenuScene : Scene
    {
        private Dictionary<string, Button> _buttons;
        private readonly Label statPointsLabel;
        private readonly Label statsLabel;
        private readonly Player player = Player.GetPlayer();

        public PlayerMenuScene()
        {
            statPointsLabel = new Label(5, 50, "");
            statsLabel = new Label(GameClass.WindowWidth - 250, 100, "");
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            _buttons = new Dictionary<string, Button>()
            {
                {"Health", new Button(GameClass.WindowWidth / 2  - Button.Width * 3 + 20, GameClass.WindowHeight - Button.Height - 10 , "Здоровье +25") },
                {"Strength", new Button(GameClass.WindowWidth / 2  - Button.Width * 2 + 30, GameClass.WindowHeight - Button.Height - 10, "Сила +1")},
                {"Agility", new Button(GameClass.WindowWidth / 2 - Button.Width + 40, GameClass.WindowHeight - Button.Height - 10, "Ловкость +1") },
                {"Intellect", new Button(GameClass.WindowWidth / 2 + 50, GameClass.WindowHeight - Button.Height - 10, "Интеллект +1") },
                {"Skills", new Button(GameClass.WindowWidth / 2 + Button.Width*2 - 20, GameClass.WindowHeight - Button.Height * 2 - 20, "Навыки") },
                {"Exit", new Button(GameClass.WindowWidth / 2 + Button.Width*2 - 20, GameClass.WindowHeight - Button.Height - 10, "Назад") }
            };
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            foreach (var b in _buttons.Values)
                b.Draw(s);
            statPointsLabel.Draw(s);
            statsLabel.Draw(s);
        }

        public override void Update(GameTime gameTime)
        {
            statPointsLabel.Text = $"Уровень: {player.Level}  Очков характеристик: {player.StatPoints}";
            statsLabel.Text = $" Запас здоровья: {player.MaxHealth} \n Сила: {player.Strength} \n Ловкость {player.Agility} \n Интеллект {player.Intelligence} ";
            foreach (var button in _buttons)
            {
                if (player.StatPoints == 0 && button.Key != "Exit" && button.Key != "Skills")
                    button.Value.IsActive = false;
                else
                    button.Value.IsActive = true;
                button.Value.Update();
                if(button.Value.State == StateButton.Press && button.Key == "Skills")
                {
                    GameClass.gameState = GameState.SkillMenuScene;
                    DoNewGenerate = true;
                    break;
                }
                if (button.Value.State == StateButton.Press)
                    player.ChangeStats(button.Key);
            }
        }
    }
}
