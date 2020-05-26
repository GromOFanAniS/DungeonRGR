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
        enum MenuState
        {
            strike,
            skill
        }
        public AttackTurn attackTurn = AttackTurn.Player;
        private MenuState _menuState;
        private bool enemyOldState;
        private Dictionary<string, Button> _skillButtons;
        private readonly Enemy enemy;
        private readonly Door door;
        private readonly Dictionary<string, Button> _attackButtons;
        private readonly Player player = Player.GetPlayer();

        public EnemyScene()
        {
            _attackButtons = new Dictionary<string, Button>();
            _skillButtons = new Dictionary<string, Button>();
            enemyOldState = false;
            DoNewGenerate = false;
            _menuState = MenuState.strike;
            door = new Door(Game1.gameWindow.ClientBounds.Width / 2 + 147, Door.closedTexture.Height / 2 + 4);
            enemy = Enemy.Generate();
            Game1.actions.Text = $"Вам встретился {enemy.Name}\n";
            player.AnimationPlayer.SetAnimation(Animations.Idle);
            SetButtons();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            door.Draw(s);
            if (!enemy.IsDead)
            {
                enemy.Draw(s);
                enemy.DrawHealthBar(s);
                switch (_menuState)
                {
                    case MenuState.strike:
                        foreach (var button in _attackButtons)
                            button.Value.Draw(s);
                        break;
                    case MenuState.skill:
                        foreach (var button in _skillButtons)
                            button.Value.Draw(s);
                        break;
                }

            }
        }
        public override void Update(GameTime gameTime)
        {
            if (player.AnimationPlayer.PlayOnce || enemy.AnimationPlayer.PlayOnce) return;
            enemy.Update(gameTime);

            for(int i = 0; i < 4; i++)
            {
                var button = _attackButtons.ElementAt(i);
                if (enemy.BodyParts.Contains((AttackSpots)Enum.Parse(typeof(AttackSpots), button.Key)))
                    button.Value.IsActive = true;
                else
                    button.Value.IsActive = false;
            }


            if (player.Potions <= 0 || player.Health == player.MaxHealth)
                _attackButtons["Heal"].IsActive = false;
            else _attackButtons["Heal"].IsActive = true;

            switch (attackTurn)
            {
                case AttackTurn.Enemy:
                    if (enemy.IsDead)
                    {
                        door.Update();
                        if (!enemyOldState)
                        {
                            Game1.actions.Text += $"Вы победили {enemy.Name}\n";
                            player.Experience += enemy.experience;
                            player.canWalk = true;
                            enemyOldState = true;
                        }
                        return;
                    }
                    else
                    {
                        enemy.AttackPlayer();
                        player.AnimationPlayer.SetAnimation(Animations.Idle);
                    }
                    attackTurn = AttackTurn.Player;
                    break;
                case AttackTurn.Player:
                    switch(_menuState)
                    {
                        case MenuState.strike:
                            foreach (var button in _attackButtons)
                            {
                                button.Value.Update();
                                if (button.Value.State == StateButton.Press)
                                {
                                    switch (button.Key)
                                    {
                                        case "Skills":
                                            _menuState = MenuState.skill;
                                            break;
                                        default:
                                            Game1.actions.Text = "";
                                            player.AttackAction(button.Key, enemy);
                                            attackTurn = AttackTurn.Enemy;
                                            break;
                                    }

                                }
                            }
                            break;
                        case MenuState.skill:
                            foreach (var button in _skillButtons)
                            {
                                if (button.Key != "Strikes" && player.SkillHandler.IsInCooldown(button.Key))
                                    button.Value.IsActive = false;
                                else
                                    button.Value.IsActive = true;
                                button.Value.Update();
                                if(button.Key == "Strikes" && button.Value.State == StateButton.Press)
                                    _menuState = MenuState.strike;
                                else if (button.Value.State == StateButton.Press)
                                {
                                    player.SkillHandler.UseSkill(button.Key, enemy);
                                    _menuState = MenuState.strike;
                                    attackTurn = AttackTurn.Enemy;
                                    break;
                                }
                            }

                            break;
                    }
                    break;
            }
        }

        private void SetButtons()
        {
            int x = Game1.WindowWidth / 5 * 4;
            int y = Game1.WindowHeight / 8;
            
            _attackButtons.Add("Head", new Button(x, y, "Удар по голове"));
            _attackButtons.Add("Body", new Button(x, y * 2, "Удар по торсу"));
            _attackButtons.Add("Hands", new Button(x, y * 3, "Удар по рукам"));
            _attackButtons.Add("Legs", new Button(x, y * 4, "Удар по ногам"));
            _attackButtons.Add("Heal", new Button(x, y * 5, "Выпить зелье"));
            _attackButtons.Add("Flee", new Button(x, y * 6, "Сбежать"));
            _attackButtons.Add("Skills", new Button(x, y * 7, "Использовать\n    навык"));

            _skillButtons = player.SkillHandler.GenerateUseSkillButtons();
        }
    }
}
