using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    enum AttackTypes
    {
        None,
        Physical,
        Magic,
        Poison,
        Ranged,
        Special
    }
    abstract class Character
    {
        protected class Attack
        {
            private int _damage;
            private int _baseDamage;
            private int _baseChance;
            private int _successChance;
            private AttackTypes _attackType;

            public int Damage { get => _damage; set => _damage = value; }
            public int BaseDamage { get => _baseDamage; }
            public int BaseChance { get => _baseChance; }
            public int SuccessChance { get => _successChance; }
            public AttackTypes AttackType { get => _attackType; }


        }
    }
}
