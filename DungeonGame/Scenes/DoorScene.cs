using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace DungeonGame
{
    class DoorScene : Scene
    {
        private List<Door> _doors;
        private Dictionary<string, Button> _buttons;
        private readonly Player player = Player.GetPlayer();

        public DoorScene()
        {
            DoNewGenerate = false;
            GenerateDoors();
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            int x = Game1.WindowWidth;
            int y = Game1.WindowHeight;
            _buttons = new Dictionary<string, Button>()
            {
                {"Save", new Button( x/2 + 5 , y - Button.Height - 10, "Сохранить") },
                {"Heal", new Button( x/2 - Button.Width - 5, y - Button.Height - 10, "Выпить зелье") },
                {"PlayerMenu", new Button( x/2 - Button.Width*2 - 15, y - Button.Height - 10, "Меню игрока") },
                {"Exit", new Button( x/2  + Button.Width + 15, y - Button.Height - 10, "Выйти в меню")}
            };
        }

        private void GenerateDoors()
        {
            _doors = new List<Door>()
            {
                new Door(182, Door.closedTexture.Height / 2 + 4),
                new Door(Game1.gameWindow.ClientBounds.Width / 2 + 147, Door.closedTexture.Height / 2 + 4)
            };
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var door in _doors)
            {
                door.Update();
            }
            foreach (var button in _buttons)
            {
                if (player.Potions <= 0 || player.Health == player.MaxHealth)
                    _buttons["Heal"].IsActive = false;
                button.Value.Update();
                if (button.Value.State == StateButton.Press)
                {
                    switch (button.Key)
                    {
                        case "Save":
                            Player.SaveLoadSystem.SaveGame();
                            break;
                        case "Heal":
                            player.UsePotion();
                            break;
                        case "PlayerMenu":
                            DoNewGenerate = true;
                            player.AnimationPlayer.Play(Animations.Idle);
                            Game1.gameState = GameState.PlayerMenuScene;
                            break;
                        case "Exit":
                            DoNewGenerate = true;
                            Game1.gameState = GameState.MenuScene;
                            break;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            _doors[0]?.Draw(s, SpriteEffects.FlipHorizontally);
            _doors[1]?.Draw(s);

            foreach (var button in _buttons.Values)
                button.Draw(s);
        }
    }
}
