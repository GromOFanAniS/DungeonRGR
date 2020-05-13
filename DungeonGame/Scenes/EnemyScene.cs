using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonGame
{
    enum AttackTurn
    {
        Player,
        Enemy
    }
    class EnemyScene : Scene
    {
        private Enemy enemy;
        private Door door;
        public AttackTurn attackTurn = AttackTurn.Player;
        private static Dictionary<AttackSpots, Button> _buttons = new Dictionary<AttackSpots, Button>();

        public EnemyScene()
        {
            DoNewGenerate = false;
            enemy = Enemy.Generate();
            SetButtons();
        }

        public override void Draw(SpriteBatch s)
        {
            enemy.Draw(s);
            enemy.DrawHealthBar(s);
            foreach (var button in _buttons)
                button.Value.Draw(s);
        }

        public override void Update(GameTime gameTime)
        {
            enemy.Update(gameTime);

            if (attackTurn == AttackTurn.Enemy) return;
            foreach (var button in _buttons)
            {
                button.Value.Update();
                if (button.Value.state == StateButton.Press)
                {
                    switch (button.Key)
                    {
                        case AttackSpots.Head:
                            Game1.player.Health -= 10;
                            break;

                        case AttackSpots.Body:
                            Game1.player.Health += 10;
                            break;

                        case AttackSpots.Hands:
                            enemy.Health -= 10;
                            break;

                        case AttackSpots.Legs:
                            enemy.Health += 10;
                            break;
                    }
                }
            }
        }

        private static void SetButtons()
        {
            int x = Game1.gameWindow.ClientBounds.Width / 5 * 4;
            int y = Game1.gameWindow.ClientBounds.Height / 8;
            
            _buttons.Add(AttackSpots.Head, new Button(x, y, "Hit Head"));
            _buttons.Add(AttackSpots.Body, new Button(x, y * 2, "Hit Body"));
            _buttons.Add(AttackSpots.Hands, new Button(x, y * 3, "Hit Hands"));
            _buttons.Add(AttackSpots.Legs, new Button(x, y * 4, "Hit Legs"));
        }
    }
}
