﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    [Serializable]
    public class Player : Character
    {
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

        private int _level;
        private int _experienceToNextLevel;
        private int _potions;
        private int _statPoints;
        private int _strength;
        private int _agility;
        private int _intellect;

        public int Level => _level;
        public int Potions => _potions;
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
        public int Intellect => _intellect;
        public int MaxHealth => _maxHealth;

        public Player()
        {
            _level = 1;
            _experienceToNextLevel = 10;
            _maxHealth = 100;
            _health = _maxHealth;
            _potions = 2;
            _strength = 1;
            _agility = 1;
            _intellect = 1;

            _weakSpots = new List<AttackSpots>();
            _weakness = AttackTypes.None;
            _attacks = new List<Attack>()
            {
                new Attack(30, 45, AttackTypes.Physical, AttackSpots.Head, "удар по голове"),
                new Attack(23, 75, AttackTypes.Physical, AttackSpots.Body, "удар по торсу"),
                new Attack(18, 85, AttackTypes.Physical, AttackSpots.Hands, "удар по рукам"),
                new Attack(15, 95, AttackTypes.Physical, AttackSpots.Legs, "удар по ногам")
            };

            _healthBar = new HealthBar(5, 10);
            _idle = new Animation();
            _idle.AddFrame(new Rectangle(0, 0, 95, 184), TimeSpan.FromSeconds(1));
            _animation = _idle;
            _walk = new Animation();
            for (int i = 1; i < 8; i++)
                _walk.AddFrame(new Rectangle(96*i, 0, 95, 184), TimeSpan.FromSeconds(.25));
            
        }

        public static void Load(ContentManager content)
        {
            _playerSheetTexture = content.Load<Texture2D>("Player/PlayerSheet");
        }

        public override void Update(GameTime gameTime)
        {
            _animation = _idle;
            CheckLevel();
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
                    break;
                case "Body":
                    DoAttack(enemy, _attacks[1]);
                    break;
                case "Hands":
                    DoAttack(enemy, _attacks[2]);
                    break;
                case "Legs":
                    DoAttack(enemy, _attacks[3]);
                    break;
                case "Heal":
                    UsePotion();
                    break;
            }
        }

        public void Initialize()
        {
            _healthBar = new HealthBar(5, 10);
            _idle = new Animation();
            _idle.AddFrame(new Rectangle(0, 0, 95, 184), TimeSpan.FromSeconds(1));
            _animation = _idle;
            _walk = new Animation();
            for (int i = 1; i < 8; i++)
                _walk.AddFrame(new Rectangle(96 * i, 0, 95, 184), TimeSpan.FromSeconds(.25));
        }

        public override void Draw(SpriteBatch s)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            Color color = Color.White;
            var sourceRectangle = _animation.CurrentRectangle;
            s.Draw(_playerSheetTexture, topLeftOfSprite, null, sourceRectangle, null, 0, null, color, _flip);
            _healthBar.Draw(s, _health, _maxHealth);
        }

        public void UsePotion()
        {
            if(_potions > 0)
            {
                _potions--;
                Health += 15; //*Difficulty
            }  
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
                    _intellect++;
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
                attack.Damage = attack.BaseDamage + _strength * 5;
                attack.SuccessChance = attack.BaseChance + _agility * 2;
            }
        }
    }
}
