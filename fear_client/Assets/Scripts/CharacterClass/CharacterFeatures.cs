namespace Scripts.CharacterClass
{
   /// <inheritdoc cref="ICharacterFeatures"/>
    public class CharacterFeatures : ICharacterFeatures
    {
        /// <inheritdoc cref="ICharacterFeatures.Team"/>
        public int Team { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.Health"/>
        public int Health { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.TroopId"/>
        public int TroopId { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.Charclass"/>
        public string Charclass { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.AttackBonus"/>
        public int AttackBonus { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.DamageBonus"/>
        public int DamageBonus { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.Movement"/>
        public int Movement { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.Perception"/>
        public int Perception { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.ArmorBonus"/>
        public int ArmorBonus { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.Stealth"/>
        public int Stealth { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.AttackRange"/>
        public int AttackRange { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.MaxAttackVal"/>
        public int MaxAttackVal { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.IsLeader"/>
        public int IsLeader { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.IsFocused"/>
        public bool IsFocused { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.IsAttacking"/>
        public bool IsAttacking { get; set; }

        /// <inheritdoc cref="ICharacterFeatures.Rng"/>
        public IRandomNumberGenerator Rng { set; get; }

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

        /// <inheritdoc cref="ICharacterFeatures.DamageCharacter(int)"/>
        public void DamageCharacter(int amount)
        {
            if (amount > Health)
                Health = 0;
            else
                Health -= amount;
        }
        
        /// <inheritdoc cref="ICharacterFeatures.GetAttackRoll"/>
        public int GetAttackRoll() => Rng.GetRandom(1, 20) + AttackBonus;

        /// <inheritdoc cref="ICharacterFeatures.GetDamageRoll"/>
        public int GetDamageRoll() => Rng.GetRandom(1, MaxAttackVal) + DamageBonus;

        /// <inheritdoc cref="ICharacterFeatures.GetMagicAttackRoll"/>
        public int GetMagicAttackRoll() => Rng.GetRandom(1, 20) + 5;

        /// <inheritdoc cref="ICharacterFeatures.GetMagicDamageRoll"/>
        public int GetMagicDamageRoll() => Rng.GetRandom(1, 12) + 1;

    }
}