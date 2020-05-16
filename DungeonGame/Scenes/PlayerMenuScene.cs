using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    class PlayerMenuScene : Scene
    {
        private Dictionary<string, Button> _buttons;
        private Label statPointsLabel;
        private Label statsLabel;

        public PlayerMenuScene()
        {
            statPointsLabel = new Label(5, 50, "");
            statsLabel = new Label(Game1.WindowWidth - 250, 100, "");
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            _buttons = new Dictionary<string, Button>()
            {
                {"Health", new Button(Game1.WindowWidth / 2  - Button.Width * 2 - 45, Game1.WindowHeight - Button.Height - 10 , "Здоровье +25") },
                {"Strength", new Button(Game1.WindowWidth / 2  - Button.Width - 35, Game1.WindowHeight - Button.Height - 10, "Сила +1")},
                {"Agility", new Button(Game1.WindowWidth / 2 - 25, Game1.WindowHeight - Button.Height - 10, "Ловкость +1") },
                {"Intellect", new Button(Game1.WindowWidth / 2 + Button.Width - 15, Game1.WindowHeight - Button.Height - 10, "Интеллект +1") },
                {"Exit", new Button(Game1.WindowWidth / 2 + Button.Width*2 - 5, Game1.WindowHeight - Button.Height - 10, "Назад") }
            };
        }

        public override void Draw(SpriteBatch s)
        {
            foreach (var b in _buttons.Values)
                b.Draw(s);
            statPointsLabel.Draw(s);
            statsLabel.Draw(s);
        }

        public override void Update(GameTime gameTime)
        {
            statPointsLabel.Text = $"Уровень: {Game1.player.Level}  Очков характеристик: {Game1.player.StatPoints}";
            statsLabel.Text = $" Запас здоровья: {Game1.player.MaxHealth} \n Сила: {Game1.player.Strength} \n Ловкость {Game1.player.Agility} \n Интеллект {Game1.player.Intellect} ";
            foreach (var button in _buttons)
            {
                if (Game1.player.StatPoints == 0 && button.Key != "Exit")
                    button.Value.isActive = false;
                else
                    button.Value.isActive = true;
                button.Value.Update();
                if (button.Value.State == StateButton.Press)
                {
                    Game1.player.StatPoints--;
                    switch (button.Key)
                    {
                        case "Health":
                            Game1.player.MaxHealth += 25;
                            break;
                        case "Strength":
                            Game1.player.Strength++;
                            break;
                        case "Agility":
                            Game1.player.Agility++;
                            break;
                        case "Intellect":
                            Game1.player.Intellect++;
                            break;
                        case "Exit":
                            Game1._gameState = GameState.DoorScene;
                            DoNewGenerate = true;
                            Game1.player.canWalk = true;
                            break;
                    }
                }
            }
        }
    }
}
