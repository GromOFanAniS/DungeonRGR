using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
                    playerInstance._difficulty = GameClass.difficulty;
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
                        GameClass.difficulty = player._difficulty;
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
        #region static fields
        private static Player playerInstance = null;

        [NonSerialized]
        private static Texture2D _playerSheetTexture;
        #endregion

        #region public fields
        public int gold = 0;
        public bool canWalk = true;
        #endregion

        #region private fields
        private const float velocity = 350f;

        [NonSerialized]
        private SpriteEffects _flip = SpriteEffects.None;
        [NonSerialized]
        private Label _weaponLabel;
        [NonSerialized]
        private Label _goldAndPots;
        [NonSerialized]
        private Label _expLabel;
        [NonSerialized]
        private Rectangle _playerPosition;

        private SkillHandler _skillHandler;
        private Weapon _currentWeapon;
        private int _difficulty;
        private int _experience;
        private int _level;
        private int _experienceToNextLevel;
        private int _potions;
        private int _statPoints;
        private int _strength;
        private int _agility;
        private int _intelligence;
        private bool _drawWeaponString;
        #endregion

        #region public properties
        public override int Health 
        { 
            get => base.Health; 
            set
            {
                value +=  (int)Math.Round((_health - value) * (5.0/100) * _skillHandler.FindPassiveSkillLevel("Каменная кожа"));
                base.Health = value;
            }
        }
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
            private set
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
        public Rectangle PlayerPosition => _playerPosition;
        public SkillHandler SkillHandler => _skillHandler;
        public int Experience
        {
            get => _experience; 
            set
            {
                _experience = value + value * _skillHandler.FindPassiveSkillLevel("Ученик") / 3;
            }
        }
        #endregion

        #region constructors
        private Player()
        {
            StatsInitialize();

            _skillHandler = new SkillHandler();

            _weakSpots = new List<AttackSpots>();
            _weakness = AttackTypes.None;
            AttackInitialize();

            Initialize();
        }

        #endregion

        #region static methods
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
        public static void Load()
        {
            _playerSheetTexture = _content.Load<Texture2D>("Player/PlayerSheet");
        }
        #endregion

        #region public methods
        public override void Update(GameTime gameTime)
        {
            _playerPosition = new Rectangle((int)X + 50, (int)Y - 1, Width - 100, Height);
            CheckLevel();
            WeaponUpdate();
            WalkingUpdate(gameTime);
            CheckDead();
            _animationPlayer.Update(gameTime);
        }

        public void AttackAction(string key, Character enemy)
        {
            switch (key)
            {
                case "Head":
                    DoAttack(enemy, _attacks[0]);
                    goto default;
                case "Body":
                    DoAttack(enemy, _attacks[1]);
                    goto default;
                case "Hands":
                    DoAttack(enemy, _attacks[2]);
                    goto default;
                case "Legs":
                    DoAttack(enemy, _attacks[3]);
                    goto default;
                case "Heal":
                    UsePotion();
                    break;
                case "Flee":
                    if(GameClass.random.Next(101) > 90)
                    {
                        GameClass.actions.Text += "Вам удалось сбежать\n";
                        MusicPlayer.ChangeSong(MusicState.Peaceful);
                        GameClass.gameState = GameState.DoorScene;
                        canWalk = true;
                        Scene.DoNewGenerate = true;
                    }
                    else
                        GameClass.actions.Text += "Вам не удалось сбежать\n";
                    break;
                default:
                    _currentWeapon?.DamageWeapon();
                    _animationPlayer.Play(Animations.Attack);
                    break;
            }
            _skillHandler.CooldownTick();
        }
        public override void Draw(SpriteBatch s)
        {
            _animationPlayer.Draw(s, _playerSheetTexture, new Vector2(X, Y), _flip);
            if(GameClass.gameState != GameState.MenuScene)
                _healthBar.Draw(s, _health, _maxHealth);
            if(GameClass.gameState != GameState.PlayerMenuScene && GameClass.gameState != GameState.MenuScene)
            {
                _goldAndPots.Draw(s);
                _goldAndPots.Text = $"У вас {gold} золота и {Potions} зелий";
            }
            else if(GameClass.gameState != GameState.MenuScene)
            {
                _expLabel.Draw(s);
                _expLabel.Text = $"Очков опыта: {Experience}. Необходимо: {_experienceToNextLevel}";
            }
            if (_drawWeaponString)
                _weaponLabel.Draw(s);
        }
        public void DrawWeaponLabel(Weapon weaponToTake, bool draw)
        {
            if(draw)
            {
                _drawWeaponString = true;
                _weaponLabel.Text =  $"{"", 4}Оружие:{_currentWeapon?.Name,-25}{weaponToTake.Name}\n"
                                   + $"{"", 8}Урон:{_currentWeapon?.Damage,-25}{weaponToTake.Damage}\n"
                                   + $"Прочность:{_currentWeapon?.Durability, -25}{weaponToTake.Durability}\n"
                                   + $"Тип урона:{_currentWeapon?.AttackType, -25}{weaponToTake.AttackType}\n";
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
            Health += 25 * GameClass.difficulty;
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
                    RegenerateAttacks();
                    _skillHandler.RegenerateSkills();
                    GameClass.gameState = GameState.DoorScene;
                    Scene.DoNewGenerate = true;
                    canWalk = true;
                    break;
            }
        }
        #endregion

        #region private methods
        private void Initialize()
        {
            _animationPlayer = new AnimationPlayer();
            _healthBar = new HealthBar(5, 10);
            _weaponLabel = new Label(50, 50, "");
            _goldAndPots = new Label(GameClass.WindowWidth / 2, 15);
            _expLabel = new Label(GameClass.WindowWidth / 2, 15);
            _drawWeaponString = false;

            AnimationInitialize();
            Position((GameClass.WindowWidth - Width) / 2, 180);
            RegenerateAttacks();
            _skillHandler.RegenerateSkills();
        }
        private void StatsInitialize()
        {
            Name = "Игрок";
            _level = 1;
            _experienceToNextLevel = 10;
            _maxHealth = 100;
            _health = _maxHealth;
            _potions = 2;
            _strength = 0;
            _agility = 0;
            _intelligence = 0;
            _difficulty = GameClass.difficulty;
        }
        private void AttackInitialize()
        {
            _attacks = new List<Attack>()
            {
                new Attack(15, 45, AttackTypes.Physical, AttackSpots.Head, "удар по голове"),
                new Attack(10, 75, AttackTypes.Physical, AttackSpots.Body, "удар по торсу"),
                new Attack(7, 85, AttackTypes.Physical, AttackSpots.Hands, "удар по рукам"),
                new Attack(5, 95, AttackTypes.Physical, AttackSpots.Legs, "удар по ногам")
            };
        }
        protected override void AnimationInitialize()
        {
            Animation idleAnimation = new Animation();
            Animation walkAnimation = new Animation();
            Animation attackAnimation = new Animation();

            for (int i = 0; i < 4; i++)
                idleAnimation.AddFrame(new Rectangle(0 + 200 * i, 0, 200, 148), TimeSpan.FromSeconds(0.25));
            for (int i = 0; i < 6; i++)
                walkAnimation.AddFrame(new Rectangle(0 + 200 * i, 148, 200, 148), TimeSpan.FromSeconds(0.15));
            for (int i = 0; i < 7; i++)
                attackAnimation.AddFrame(new Rectangle(0 + 200 * i, 296, 200, 148), TimeSpan.FromSeconds(0.14));

            _animationPlayer.AddAnimation(Animations.Idle, idleAnimation);
            _animationPlayer.AddAnimation(Animations.Walk, walkAnimation);
            _animationPlayer.AddAnimation(Animations.Attack, attackAnimation);

            _animationPlayer.SetAnimation(Animations.Idle);
        }
        private void WalkingUpdate(GameTime gameTime)
        {
            if (canWalk)
            {
                float speed = velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (KeyboardHandler.IsPressed(Keys.A))
                {
                    _animationPlayer.SetAnimation(Animations.Walk);
                    _flip = SpriteEffects.FlipHorizontally;
                    if (X - speed <= 0) return;
                    X -= speed;
                }
                else if (KeyboardHandler.IsPressed(Keys.D))
                {
                    _animationPlayer.SetAnimation(Animations.Walk);
                    _flip = SpriteEffects.None;
                    if ((X + speed) >= (GameClass.WindowWidth - Width)) return;
                    X += speed;
                }
                else _animationPlayer.SetAnimation(Animations.Idle);
            }
            else
            {
                _flip = SpriteEffects.None;
                Position(GameClass.WindowWidth / 2 - 100 - Width, (int)Y);
            }
        }
        private void CheckDead()
        {
            if (_health <= 0)
            {
                LeaderBoard.GetLeaderBoard().AddToBoard(Name, gold);
                GameClass.gameState = GameState.GameOverScene;
                Position((GameClass.WindowWidth - Width) / 2, (GameClass.WindowHeight - Height) / 2 + 24);
                Scene.DoNewGenerate = true;
            }
        }
        private void CheckLevel()
        {
            if(Experience >= _experienceToNextLevel)
            {
                GameClass.actions.Text += "Новый Уровень!\n";
                StatPoints += 3;
                _skillHandler.SkillPoints += 2;
                _level++;
                Experience -= _experienceToNextLevel;
                _experienceToNextLevel += 15 * _level;
                CheckDifficulty();
            }
        }
        private void RegenerateAttacks()
        {
            foreach(var attack in _attacks)
            {
                switch(_currentWeapon?.AttackType)
                {
                    case AttackTypes.Physical:
                        attack.Damage = attack.BaseDamage + _currentWeapon.Damage + _strength * 2;
                        attack.SuccessChance = attack.BaseChance + _agility * 1;
                        break;
                    case AttackTypes.Ranged:
                        attack.Damage = attack.BaseDamage + _currentWeapon.Damage + _agility * 1;
                        attack.SuccessChance = attack.BaseChance + _agility * 2;
                        break;
                    case AttackTypes.Magical:
                        attack.Damage = attack.BaseDamage + _currentWeapon.Damage + _intelligence * 3;
                        attack.SuccessChance = attack.BaseChance;
                        break;
                    case AttackTypes.Poison:
                        attack.Damage = attack.BaseDamage + _currentWeapon.Damage;
                        break;
                    case null:
                        attack.Damage = attack.BaseDamage + _strength * 2;
                        attack.SuccessChance = attack.BaseChance + _agility * 1;
                        break;
                }
                attack.Type = _currentWeapon?.AttackType ?? AttackTypes.Physical;
            }
        }
        private void CheckDifficulty()
        {
            if(_level > GameClass.difficulty * 3)
            {
                GameClass.difficulty++;
                GameClass.actions.Text += "Вы нашли дверь, ведущую на более глубокий уровень подземелья.\n"
                                      + "Враги станут сильнее, но и награда больше.\n";
            }
        }
        private void WeaponUpdate()
        {
            if(_currentWeapon?.Durability <= 0)
            {
                _currentWeapon = null;
            }
        }
        #endregion
    }
}
