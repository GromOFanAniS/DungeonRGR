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
        #region static fields
        private static Player playerInstance = null;

        [NonSerialized]
        private static Texture2D _playerSheetTexture;
        #endregion

        #region public fields
        public int gold = 0;
        public bool canWalk = true;
        public List<Skill> skills;
        #endregion

        #region private fields
        private const float velocity = 350f;
        [NonSerialized]
        private Animation _idleAnimation;
        [NonSerialized]
        private Animation _walkAnimation;
        [NonSerialized]
        private Animation _attackAnimation;

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

        private Weapon _currentWeapon;
        private int _experience;
        private int _level;
        private int _experienceToNextLevel;
        private int _potions;
        private int _statPoints;
        private int _skillPoints;
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
                value +=  (int)Math.Round((_health - value) * (5.0/100) * skills.Find(x => x.Name == "Каменная кожа").level);
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
        public int SkillPoints
        {
            get => _skillPoints;
            private set
            {
                if (value > 0)
                    _skillPoints = value;
                else
                    _skillPoints = 0;
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
        public int Experience
        {
            get => _experience; 
            set
            {
                _experience = value + value * skills.Find(x => x.Name == "Ученик").level / 3;
            }
        }
        #endregion

        #region constructors
        private Player()
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

            skills = new List<Skill>()
            {
                new CripplingStrike(),
                new FireBall(),
                new ExperienceBuff(),
                new StoneSkin()
            };
            _weakSpots = new List<AttackSpots>();
            _weakness = AttackTypes.None;
            _attacks = new List<Attack>()
            {
                new Attack(10, 45, AttackTypes.Physical, AttackSpots.Head, "удар по голове"),
                new Attack(7, 75, AttackTypes.Physical, AttackSpots.Body, "удар по торсу"),
                new Attack(4, 85, AttackTypes.Physical, AttackSpots.Hands, "удар по рукам"),
                new Attack(2, 95, AttackTypes.Physical, AttackSpots.Legs, "удар по ногам")
            };

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
        public static void Load(ContentManager content)
        {
            _playerSheetTexture = content.Load<Texture2D>("Player/PlayerSheet");
        }
        #endregion

        #region public methods
        public override void Update(GameTime gameTime)
        {
            _playerPosition = new Rectangle((int)X + 50, (int)Y - 1, Width - 100, Height);
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
        public override void SetAnimation(Animations animation)
        {
            switch (animation)
            {
                case Animations.Idle:
                    _animation.Set(_idleAnimation);
                    break;
                case Animations.Walk:
                    _animation.Set(_walkAnimation);
                    break;
                case Animations.Attack:
                    _animation.Set(_attackAnimation);
                    break;
            }
        }
        public void UpgradeSkill(string skillName)
        {
            var skill = skills.Find(x => x.Name == skillName);
            SkillPoints-= skill.PointsToLearn;
            skill.level++;
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
                    if(Game1.random.Next(101) > 90)
                    {
                        Game1.actions.Text += "Вам удалось сбежать\n";
                        MusicPlayer.ChangeSong(MusicState.Peaceful);
                        Game1.gameState = GameState.DoorScene;
                        canWalk = true;
                        Scene.DoNewGenerate = true;
                    }
                    else
                        Game1.actions.Text += "Вам не удалось сбежать\n";
                    break;
                default:
                    _currentWeapon?.DamageWeapon();
                    _animation.Play(_attackAnimation);
                    break;
            }
            CooldownTick();
        }
        public void CooldownTick()
        {
            foreach (ActiveSkill skill in skills.FindAll(x => x.GetSkillType() == typeof(ActiveSkill)))
                skill.Cooldown--;
        }
        public override void Draw(SpriteBatch s)
        {
            _animation.Draw(s, _playerSheetTexture, new Vector2(X, Y), _flip);
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
                    RegenerateAttacks();
                    RegenerateSkills();
                    Game1.gameState = GameState.DoorScene;
                    Scene.DoNewGenerate = true;
                    canWalk = true;
                    break;
            }
        }
        #endregion

        #region private methods
        private void Initialize()
        {
            _animation = new AnimationPlayer();
            _healthBar = new HealthBar(5, 10);
            _weaponLabel = new Label(50, 50, "");
            _goldAndPots = new Label(Game1.WindowWidth / 2, 15);
            _expLabel = new Label(Game1.WindowWidth / 2, 15);
            _drawWeaponString = false;
            _idleAnimation = new Animation();
            for (int i = 0; i < 4; i++)
                _idleAnimation.AddFrame(new Rectangle(0 + 200*i, 0, 200, 148), TimeSpan.FromSeconds(0.25));
            _animation.Set(_idleAnimation);
            _walkAnimation = new Animation();
            for (int i = 0; i < 6; i++)
                _walkAnimation.AddFrame(new Rectangle(0 + 200 * i, 148, 200, 148), TimeSpan.FromSeconds(0.15));
            _attackAnimation = new Animation();
            for (int i = 0; i < 7; i++)
                _attackAnimation.AddFrame(new Rectangle(0 + 200 * i, 296, 200, 148), TimeSpan.FromSeconds(0.14));
            Position((Game1.WindowWidth - Width) / 2, 180);
            RegenerateAttacks();
            RegenerateSkills();
        }
        private void WalkingUpdate(GameTime gameTime)
        {
            if (canWalk)
            {
                if (Game1.keyboardState.IsKeyDown(Keys.A))
                {
                    SetAnimation(Animations.Walk);
                    _flip = SpriteEffects.FlipHorizontally;
                    if ((X - velocity * (float)gameTime.ElapsedGameTime.TotalSeconds) <= 0) return;
                    X -= velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (Game1.keyboardState.IsKeyDown(Keys.D))
                {
                    SetAnimation(Animations.Walk);
                    _flip = SpriteEffects.None;
                    if ((X + velocity * (float)gameTime.ElapsedGameTime.TotalSeconds) >= (Game1.WindowWidth - Width)) return;
                    X += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else SetAnimation(Animations.Idle);
            }
            else
            {
                _flip = SpriteEffects.None;
                Position(Game1.WindowWidth / 2 - 100 - Width, (int)Y);
            }
        }        
        private void CheckLevel()
        {
            if(Experience >= _experienceToNextLevel)
            {
                Game1.actions.Text += "Новый Уровень!\n";
                StatPoints += 3;
                SkillPoints += 2;
                _level++;
                Experience -= _experienceToNextLevel;
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
        private void RegenerateSkills()
        {
            foreach (ActiveSkill skill in skills.FindAll(x => x.GetSkillType() == typeof(ActiveSkill)))
            {
                if (skill.level > 0) skill.Regenerate();
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
        #endregion
    }
}
