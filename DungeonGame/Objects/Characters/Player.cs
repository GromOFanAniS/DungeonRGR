using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace DungeonGame
{
    [Serializable]
    public class Player : Character
    {
         public static class SaveLoadSystem
        {
            public static void SaveGame()
            {
                if (!Directory.Exists(@"saves/")) Directory.CreateDirectory(@"saves/");
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(@"saves/save.dat", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, playerInstance);
                }
            }
            public static void LoadGame()
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream fs = new FileStream(@"saves/save.dat", FileMode.OpenOrCreate))
                    {
                        Player player = (Player)formatter.Deserialize(fs);
                        player.Initialize();
                        playerInstance = player;
                    }
                }
                catch (IOException /*e*/)
                {
                    playerInstance = new Player();
                }
                catch (SerializationException)
                {
                    playerInstance = new Player();
                }
            }
        }

        private static Player playerInstance = null;

        public int gold = 0;
        public bool canWalk = true;
        public int experience;

        private const float velocity = 250f;
        [NonSerialized]
        private Animation _idle;
        [NonSerialized]
        private Animation _walk;
        [NonSerialized]
        private static Texture2D _playerSheetTexture;
        [NonSerialized]
        private SpriteEffects _flip = SpriteEffects.None;
        [NonSerialized]
        private Label _weaponLabel;
        [NonSerialized]
        private Label _goldAndPots;
        [NonSerialized]
        private Label _expLabel;

        private Weapon _currentWeapon;
        private int _level;
        private int _experienceToNextLevel;
        private int _potions;
        private int _statPoints;
        private int _strength;
        private int _agility;
        private int _intelligence;
        private bool _drawWeaponString;

        public int Level => _level;
        public int Potions
        { 
            get => _potions; 
            private set
            {
                if (value > 0)
                    _potions = value;
                else
                    _potions = 0;
            }
        }  
        public int StatPoints 
        { 
            get => _statPoints; 
            set
            {
                if (value > 0)
                    _statPoints = value;
                else
                    _statPoints = 0;
            }
        }
        public int Strength => _strength;
        public int Agility => _agility;
        public int Intelligence => _intelligence;
        public int MaxHealth => _maxHealth;

        private Player()
        {
            _level = 1;
            _experienceToNextLevel = 10;
            _maxHealth = 100;
            _health = _maxHealth;
            _potions = 2;
            _strength = 1;
            _agility = 1;
            _intelligence = 1;

            _weakSpots = new List<AttackSpots>();
            _weakness = AttackTypes.None;
            _attacks = new List<Attack>()
            {
                new Attack(30, 45, AttackTypes.Physical, AttackSpots.Head, "удар по голове"),
                new Attack(23, 75, AttackTypes.Physical, AttackSpots.Body, "удар по торсу"),
                new Attack(18, 85, AttackTypes.Physical, AttackSpots.Hands, "удар по рукам"),
                new Attack(15, 95, AttackTypes.Physical, AttackSpots.Legs, "удар по ногам")
            };

            Initialize();
        }

        public static Player GetPlayer()
        {
            if (playerInstance == null)
                playerInstance = new Player();
            return playerInstance;
        }
        public static void Kill()
        {
            playerInstance = null;
        }

        public static void Load(ContentManager content)
        {
            _playerSheetTexture = content.Load<Texture2D>("Player/PlayerSheet");
        }

        public override void Update(GameTime gameTime)
        {
            _animation = _idle;
            CheckLevel();
            CheckDifficulty();
            WalkingUpdate(gameTime);
            if(_health <= 0)
            {
                LeaderBoard.GetLeaderBoard().AddToBoard(Name, gold);
                Game1.gameState = GameState.GameOverScene;
                Scene.DoNewGenerate = true;
            }
            _animation.Update(gameTime);
        }

        public void ButtonAction(string key, Character enemy)
        {
            switch (key)
            {
                case "Head":
                    DoAttack(enemy, _attacks[0]);
                    _currentWeapon?.DamageWeapon();
                    break;
                case "Body":
                    DoAttack(enemy, _attacks[1]);
                    _currentWeapon?.DamageWeapon();
                    break;
                case "Hands":
                    DoAttack(enemy, _attacks[2]);
                    _currentWeapon?.DamageWeapon();
                    break;
                case "Legs":
                    DoAttack(enemy, _attacks[3]);
                    _currentWeapon?.DamageWeapon();
                    break;
                case "Heal":
                    UsePotion();
                    break;
                case "Flee":
                    if(Game1.random.Next(101) > 90)
                    {
                        Game1.actions.Text += "Вам удалось сбежать\n";
                        Game1.gameState = GameState.DoorScene;
                        canWalk = true;
                        Scene.DoNewGenerate = true;
                    }
                    else
                        Game1.actions.Text += "Вам не удалось сбежать\n";
                    break;
            }
        }

        public void Initialize()
        {
            Name = "Игрок";
            _healthBar = new HealthBar(5, 10);
            _weaponLabel = new Label(50, 50, "");
            _goldAndPots = new Label(Game1.WindowWidth / 2, 15);
            _expLabel = new Label(Game1.WindowWidth / 2, 15);
            _drawWeaponString = false;
            _idle = new Animation();
            _idle.AddFrame(new Rectangle(0, 0, 95, 184), TimeSpan.FromSeconds(1));
            _animation = _idle;
            _walk = new Animation();
            for (int i = 1; i < 8; i++)
                _walk.AddFrame(new Rectangle(96 * i, 0, 95, 184), TimeSpan.FromSeconds(.25));
        }

        public override void Draw(SpriteBatch s)
        {
            Vector2 topLeftOfSprite = new Vector2(X, Y);
            Color color = Color.White;
            var sourceRectangle = _animation.CurrentRectangle;
            s.Draw(_playerSheetTexture, topLeftOfSprite, null, sourceRectangle, null, 0, null, color, _flip);
            if(Game1.gameState != GameState.MenuScene)
                _healthBar.Draw(s, _health, _maxHealth);
            if(Game1.gameState != GameState.PlayerMenuScene && Game1.gameState != GameState.MenuScene)
            {
                _goldAndPots.Draw(s);
                _goldAndPots.Text = $"У вас {gold} золота и {Potions} зелий";
            }
            else if(Game1.gameState != GameState.MenuScene)
            {
                _expLabel.Draw(s);
                _expLabel.Text = $"Очков опыта: {experience}. Необходимо: {_experienceToNextLevel}";
            }
            if (_drawWeaponString)
                _weaponLabel.Draw(s);
        }

        public void DrawWeaponLabel(Weapon weaponToTake, bool draw)
        {
            if(draw)
            {
                _drawWeaponString = true;
                _weaponLabel.Text =  $"{"", 4}Оружие:{_currentWeapon?.Name,-15}{weaponToTake.Name}\n"
                                   + $"{"", 8}Урон:{_currentWeapon?.Damage,-15}{weaponToTake.Damage}\n"
                                   + $"Прочность:{_currentWeapon?.Durability, -15}{weaponToTake.Durability}\n"
                                   + $"Тип урона: \n";
            }
            else
            {
                _drawWeaponString = false;
                _weaponLabel.Text = "";
            }            
        }

        public void TakeNewWeapon(Weapon weaponToTake)
        {
            _currentWeapon = weaponToTake;
            RegenerateAttacks();
        }

        public void UsePotion()
        {
            Potions--;
            Health += 15 * Game1.difficulty;
        }
        
        public void TakePotions(int value)
        {
            _potions += value;
        }
        public void ChangeStats(string stat)
        {
            StatPoints--;
            switch (stat)
            {
                case "Health":
                    _maxHealth += 25;
                    Health += 25;
                    break;
                case "Strength":
                    _strength++;
                    break;
                case "Agility":
                    _agility++;
                    break;
                case "Intellect":
                    _intelligence++;
                    break;
                case "Exit":
                    Game1.gameState = GameState.DoorScene;
                    Scene.DoNewGenerate = true;
                    canWalk = true;
                    break;
            }
            RegenerateAttacks();
        }

        private void WalkingUpdate(GameTime gameTime)
        {
            if (canWalk)
            {
                if (Game1.keyboardState.IsKeyDown(Keys.A))
                {
                    _animation = _walk;
                    _flip = SpriteEffects.FlipHorizontally;
                    if ((X - velocity * (float)gameTime.ElapsedGameTime.TotalSeconds) <= 0) return;
                    X -= velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (Game1.keyboardState.IsKeyDown(Keys.D))
                {
                    _animation = _walk;
                    _flip = SpriteEffects.None;
                    if ((X + velocity * (float)gameTime.ElapsedGameTime.TotalSeconds) >= (Game1.gameWindow.ClientBounds.Width - Width)) return;
                    X += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                _flip = SpriteEffects.None;
                Position(Game1.gameWindow.ClientBounds.Width / 2 - 100 - Width, (int)Y);
            }
        }
        
        private void CheckLevel()
        {
            if(experience >= _experienceToNextLevel)
            {
                Game1.actions.Text += "Новый Уровень!\n";
                _statPoints += 3;
                _level++;
                experience -= _experienceToNextLevel;
                _experienceToNextLevel += 15 * _level;
            }
        }
        private void RegenerateAttacks()
        {
            foreach(var attack in _attacks)
            {
                switch(_currentWeapon?.AttackType)
                {
                    case AttackTypes.Physical:
                        attack.Damage = attack.BaseDamage + _currentWeapon.Damage + _strength * 5 ;
                        attack.SuccessChance = attack.BaseChance + _agility * 2;
                        break;
                    case AttackTypes.Ranged:
                        attack.Damage = attack.BaseDamage + _currentWeapon.Damage + _agility * 5;
                        attack.SuccessChance = attack.BaseChance + _agility * 3;
                        break;
                    case AttackTypes.Magic:
                        attack.Damage = attack.BaseDamage + _currentWeapon.Damage + _intelligence * 5;
                        attack.SuccessChance = attack.BaseChance + _agility * 1;
                        break;
                    case AttackTypes.Poison:
                        attack.Damage = attack.BaseDamage + _currentWeapon.Damage;
                        break;
                    case null:
                        attack.Damage = attack.BaseDamage + _strength * 5;
                        attack.SuccessChance = attack.BaseChance + _agility * 2;
                        break;
                }
                attack.Type = _currentWeapon?.AttackType ?? AttackTypes.Physical;
            }
        }
        private void CheckDifficulty()
        {
            if(_level > Game1.difficulty * 3)
            {
                Game1.difficulty++;
                Game1.actions.Text += "Вы нашли дверь, ведущую на более глубокий уровень подземелья.\n"
                                      + "Враги станут сильнее, но и награда больше.\n";
            }
        }
    }
}
