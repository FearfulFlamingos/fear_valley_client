using System;

namespace Scripts.Character
{
    /// <summary>
    /// This script is largely used to keep track of information on each of the game objects vital to making the game work
    /// </summary>
    public class CharacterFeatures //: MonoBehaviour
    {

        public int Team { get; set; }
        public int Health { get; set; }
        public int TroopId { get; set; }
        public string Charclass { get; set; }
        public int AttackBonus { get; set; }
        public int DamageBonus { get; set; }
        public int Movement { get; set; }
        public int Perception { get; set; }
        public int ArmorBonus { get; set; }
        public int Stealth { get; set; }
        public int AttackRange { get; set; }
        public int MaxAttackVal { get; set; }
        public int IsLeader { get; set; }
        public bool IsFocused { get; set; }
        public bool IsAttacking { get; set; }

        private readonly RandomNumberGenerator rng = new RandomNumberGenerator();

        /// <summary>
        /// Blank constructor method.
        /// </summary>
        public CharacterFeatures()
        {
            this.Team = 1;
            this.Health = 10;
            this.TroopId = 0;
            this.Charclass = "Peasant";
            this.AttackBonus = 0;
            this.DamageBonus = 0;
            this.Movement = 6;
            this.Perception = 0;
            this.ArmorBonus = 10;
            this.Stealth = 0;
            this.AttackRange = 1;
            this.MaxAttackVal = 1;
            this.IsLeader = 0;
            this.IsFocused = false;
            this.IsAttacking = false;
        }


        public void DamageCharacter(int amount)
        {
            if (amount > Health)
                Health = 0;
            else
                Health -= amount;
        }

        public int GetAttackRoll() => rng.GetRandom(1, 20) + AttackBonus;

        public int GetDamageRoll() => rng.GetRandom(1, MaxAttackVal) + DamageBonus;

        public int GetMagicAttackRoll() => rng.GetRandom(1, 20) + 5;

        public int GetMagicDamageRoll() => rng.GetRandom(1, 12) + 1;

    }

    interface IRandomNumberGenerator
    {
        int GetRandom(int min, int max);
    }

    class RandomNumberGenerator: IRandomNumberGenerator
    {
        readonly Random random = new Random();
        public int GetRandom(int min, int max) => random.Next(min, max);
    }
}