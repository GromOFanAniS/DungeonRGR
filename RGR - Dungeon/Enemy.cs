﻿using System;

namespace RGR___Dungeon
{
    abstract class Enemy : Character
    {

        #region Fields
        
        protected int exp;
        protected AttackType defaultAttackType;
        static Random rnd = new Random();

        public int Exp => exp;
        #endregion

        #region Methods
        protected override void TakeDamage(int dmg, Attack attack)
        {
            Health -= dmg;
        }
        public override void InflictAttack(Character attacked)
        {
            Attack UsedAttack = attacks[rnd.Next(0, attacks.Count)];
            if (UsedAttack.Type == AttackType.special) Heal(13 + 5*Player.difficulty);
            UsedAttack.AttackEvent(attacked, UsedAttack, this);
        }
        #endregion
    }

    #region Enemy Types
    class Rat : Enemy
    {
        public Rat()
        {
            name = "Крыса";
            maxhealth = 10 + 10 * Player.difficulty;
            Health = maxhealth;
            exp = 5 + 15 * Player.difficulty;
            defaultAttackType = AttackType.physical;
            weaknessType = AttackType.physical;
            resistance = AttackType.poison;

            attacks.Add(new Attack(3 + 2 * Player.difficulty, 80, "Укус", defaultAttackType));
            attacks.Add(new Attack(8 + 4 * Player.difficulty, 50, "Сильный укус", defaultAttackType));
            weakSpots.Add("удар по голове");
        }
    }
    class DarkKnight : Enemy
    {
        public DarkKnight()
        {
            name = "Падший рыцарь";
            maxhealth = 80 + 20 * Player.difficulty;
            Health = maxhealth;
            exp = 30 + 50 * Player.difficulty;
            defaultAttackType = AttackType.physical;
            weaknessType = AttackType.poison;
            resistance = AttackType.physical;

            attacks.Add(new Attack(17 + 9 * Player.difficulty, 90, "Удар", defaultAttackType));
            attacks.Add(new Attack(0, 100, "Исцеление", AttackType.special));
            attacks.Add(new Attack(20 + 10 * Player.difficulty, 75, "Сильный удар", defaultAttackType));
            weakSpots.Add("удар по голове");
            weakSpots.Add("удар по рукам");
        }
    }
    class SlimeMonster : Enemy
    {
        public SlimeMonster()
        {
            name = "Слизень";
            maxhealth = 50 + 15 * Player.difficulty;
            Health = maxhealth;
            exp = 15 + 20 * Player.difficulty;
            defaultAttackType = AttackType.poison;
            weaknessType = AttackType.physical;
            resistance = AttackType.poison;

            attacks.Add(new Attack(8 + 6 * Player.difficulty, 95, "Плевок слизью", defaultAttackType));
            attacks.Add(new Attack(13 + 8 * Player.difficulty, 80, "Удар", defaultAttackType));
            weakSpots.Add("удар по торсу");
        }
    }
    class VengefulSpirit : Enemy
    {
        public VengefulSpirit()
        {
            name = "Дух мщения";
            maxhealth = 150 + 25 * Player.difficulty;
            Health = maxhealth;
            exp = 50 + 70 * Player.difficulty;
            defaultAttackType = AttackType.magic;
            weaknessType = AttackType.magic;
            resistance = AttackType.ranged;

            attacks.Add(new Attack(17 + 8 * Player.difficulty, 80, "Поглощение жизни", AttackType.special));
            attacks.Add(new Attack(24 + 12 * Player.difficulty , 70, "Крик", defaultAttackType));
            attacks.Add(new Attack(3 * Player.difficulty, 99, "Удар", AttackType.physical));
            weakSpots.Add("удар по торсу");
        }
    }
    class SkeletonArcher : Enemy
    {
        public SkeletonArcher()
        {
            name = "Скелет";
            maxhealth = 75 + 15 * Player.difficulty;
            Health = maxhealth;
            exp = 25 + 30 * Player.difficulty;
            defaultAttackType = AttackType.ranged;
            weaknessType = AttackType.poison;
            resistance = AttackType.ranged;

            attacks.Add(new Attack(10 + 5 * Player.difficulty, 75, "Выстрел", defaultAttackType));
            attacks.Add(new Attack(20 + 10 * Player.difficulty, 60, "Заряженный выстрел", defaultAttackType));
            attacks.Add(new Attack(5 + 4 * Player.difficulty, 90, "Удар луком", AttackType.physical));
            weakSpots.Add("удар по рукам");
            weakSpots.Add("удар по ногам");
        }
    }
    
    class Zombie : Enemy
    {
        public Zombie()
        {
            name = "Зомби";
            maxhealth = 100 + 20 * Player.difficulty;
            Health = maxhealth;
            exp = 25 + 30 * Player.difficulty;
            defaultAttackType = AttackType.physical;
            weaknessType = AttackType.magic;
            resistance = AttackType.poison;

            attacks.Add(new Attack(6 + 4 * Player.difficulty, 90, "Удар", defaultAttackType));
            attacks.Add(new Attack(15 + 8 * Player.difficulty, 50, "Укус", defaultAttackType));
            attacks.Add(new Attack(2 * Player.difficulty, 95, "Слабый удар", defaultAttackType));
            weakSpots.Add("удар по рукам");
            weakSpots.Add("удар по ногам");
        }
    }
    class Mage : Enemy
    { 
        public Mage()
        {
            name = "Маг-отступник";
            maxhealth = 35 + 10 * Player.difficulty;
            Health = maxhealth;
            exp = 30 + 35 * Player.difficulty;
            defaultAttackType = AttackType.magic;
            weaknessType = AttackType.ranged;
            resistance = AttackType.magic;

            attacks.Add(new Attack(30 + 10 * Player.difficulty, 80, "Огненный шар", defaultAttackType));
            attacks.Add(new Attack(20 + 8 * Player.difficulty, 90, "Удар молнии", defaultAttackType));
            attacks.Add(new Attack(0, 99, "Исцеление", AttackType.special));
            weakSpots.Add("удар по голове");
        }
    }
    //TODO: NEW MONSTERS
    #endregion
}
