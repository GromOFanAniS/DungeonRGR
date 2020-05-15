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

        public DoorScene()
        {
            DoNewGenerate = false;
            GenerateDoors();
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            int x = Game1.gameWindow.ClientBounds.Width;
            int y = Game1.gameWindow.ClientBounds.Height;
            _buttons = new Dictionary<string, Button>()
            {
                {"Save", new Button( x/2 + 10 , y - Button.Height - 10, "Сохранить") },
                {"Heal", new Button( x/2 - Button.Width - 10, y - Button.Height - 10, "Выпить зелье") }
            };
        }

        private void GenerateDoors()
        {
            _doors = new List<Door>()
            {
                new Door(50, Door.closedTexture.Height / 2),
                new Door(Game1.gameWindow.ClientBounds.Width - Door.closedTexture.Width - 50, Door.closedTexture.Height / 2)
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
                if (Game1.player.Potions <= 0)
                    _buttons["Heal"].isActive = false;
                button.Value.Update();
                if (button.Value.state == StateButton.Press)
                {
                    switch (button.Key)
                    {
                        case "Save":
                            SaveLoadSystem.SaveGame(Game1.player);
                            break;
                        case "Heal":
                            Game1.player.UsePotion();
                            break;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {
            _doors[0]?.Draw(s, SpriteEffects.FlipHorizontally);
            _doors[1]?.Draw(s);

            foreach (var button in _buttons.Values)
                button.Draw(s);
        }
    }
}
