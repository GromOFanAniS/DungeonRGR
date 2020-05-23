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
        public AttackTurn attackTurn = AttackTurn.Player;

        private Player player = Player.GetPlayer();
        private bool enemyOldState;
        private Enemy enemy;
        private Door door;
        private Dictionary<string, Button> _buttons = new Dictionary<string, Button>();

        public EnemyScene()
        {
            enemyOldState = false;
            DoNewGenerate = false;
            door = new Door(Game1.WindowWidth - Door.closedTexture.Width - 50, Door.closedTexture.Height / 2);
            enemy = Enemy.Generate();
            Game1.actions.Text = $"Вам встретился {enemy.Name}";
            SetButtons();
        }

        public override void Draw(SpriteBatch s)
        {
            
            if(enemy.IsDead)
            {
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
            if (player.Potions <= 0 || player.Health == player.MaxHealth)
                _buttons["Heal"].isActive = false;
            else _buttons["Heal"].isActive = true;
            enemy.Update(gameTime);
            switch (attackTurn)
            {
                case AttackTurn.Enemy:
                    if (enemy.IsDead)
                    {
                        door.Update();
                        if (!enemyOldState)
                        {
                            Game1.actions.Text += $"Вы победили {enemy.Name}\n";
                            player.experience += enemy.experience;
                            player.canWalk = true;
                            enemyOldState = true;
                        }
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
                        if (button.Value.State == StateButton.Press)
                        {
                            Game1.actions.Text = "";
                            player.AttackAction(button.Key, enemy);
                            attackTurn = AttackTurn.Enemy;
                        }
                    }
                    break;
            }
        }

        private void SetButtons()
        {
            int x = Game1.WindowWidth / 5 * 4;
            int y = Game1.WindowHeight / 8;
            
            _buttons.Add("Head", new Button(x, y, "Удар по голове"));
            _buttons.Add("Body", new Button(x, y * 2, "Удар по торсу"));
            _buttons.Add("Hands", new Button(x, y * 3, "Удар по рукам"));
            _buttons.Add("Legs", new Button(x, y * 4, "Удар по ногам"));
            _buttons.Add("Heal", new Button(x, y * 5, "Выпить зелье"));
            _buttons.Add("Flee", new Button(x, y * 6, "Сбежать"));
            _buttons.Add("Skills", new Button(x, y * 7, "Использовать навык"));
        }
    }
}
