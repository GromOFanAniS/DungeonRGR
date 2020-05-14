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
        private Dictionary<AttackSpots, Button> _buttons = new Dictionary<AttackSpots, Button>();

        public EnemyScene()
        {
            DoNewGenerate = false;
            door = new Door(Game1.gameWindow.ClientBounds.Width - Door.closedTexture.Width - 50, Door.closedTexture.Height / 2);
            enemy = Enemy.Generate();
            Game1.actions.Text = $"Вам встретился {enemy.Name}";
            SetButtons();
        }

        public override void Draw(SpriteBatch s)
        {
            
            if(enemy.IsDead)
            {
                Game1.actions.Text = $"Вы победили {enemy.Name}";
                door.Draw(s);
            }
            else
            {
                enemy.Draw(s);
                enemy.DrawHealthBar(s);
                foreach (var button in _buttons)
                    button.Value.Draw(s);
            }
        }

        public override void Update(GameTime gameTime)
        {
            enemy.Update(gameTime);
            switch (attackTurn)
            {
                case AttackTurn.Enemy:
                    if (enemy.IsDead)
                    {
                        door.Update();
                        Game1.player.canWalk = true;
                        return;
                    }
                    else
                        enemy.AttackPlayer();
                    attackTurn = AttackTurn.Player;
                    break;
                case AttackTurn.Player:
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
                            Game1.actions.Text = "";
                            attackTurn = AttackTurn.Enemy;
                        }
                    }
                    
                    break;
            }
        }

        private void SetButtons()
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
