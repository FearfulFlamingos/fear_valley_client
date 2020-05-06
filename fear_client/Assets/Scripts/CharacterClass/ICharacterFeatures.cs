namespace Scripts.CharacterClass
{
    /// <summary>
    /// This script is largely used to keep track of information on each of the game objects
    /// vital to making the game work.
    /// </summary>
    public interface ICharacterFeatures
    {
        /// <summary>Armor bonus of the character.</summary>
        int ArmorBonus { get; set; }
        
        /// <summary>Attack bonus of the character.</summary>
        int AttackBonus { get; set; }
        
        /// <summary>Attack range of the character.</summary>
        int AttackRange { get; set; }
        
        /// <summary>Class of the character.</summary>
        string Charclass { get; set; }
        
        /// <summary>Damage bonus of the character.</summary>
        int DamageBonus { get; set; }
        
        /// <summary>Health of the character.</summary>
        int Health { get; set; }
        
        /// <summary>Attacking state of the character.</summary>
        bool IsAttacking { get; set; }
        
        /// <summary>Focused state of the character.</summary>
        bool IsFocused { get; set; }
        
        /// <summary>Leadership quality of the character.</summary>
        int IsLeader { get; set; }
        
        /// <summary>Maximum damage value of the character.</summary>
        int MaxAttackVal { get; set; }
        
        /// <summary>Maximum movement distance of the character.</summary>
        int Movement { get; set; }
        
        /// <summary>Perception bonus of the character.</summary>
        /// <remarks>Unused by the game.</remarks>
        int Perception { get; set; }
        
        /// <summary>Stealth bonus of the character.</summary>
        /// <remarks>Unused by the game.</remarks>
        int Stealth { get; set; }
        
        /// <summary>Team number of the character.</summary>
        int Team { get; set; }
        
        /// <summary>ID number of the character.</summary>
        int TroopId { get; set; }
        
        /// <summary>RNG calculator of the character. See <see cref="IRandomNumberGenerator"/>.</summary>
        IRandomNumberGenerator Rng { get; set; }

        /// <summary>
        /// Damages the character.
        /// </summary>
        /// <param name="amount">Amount of damage the character should take.</param>
        void DamageCharacter(int amount);
        
        /// <summary>
        /// Get the attack roll of the character to be compared against an enemy's armor.
        /// See <see cref="Actions.PlayerAttack.Attack"/> for more details.
        /// </summary>
        /// <returns>Integer value of the attack roll.</returns>
        int GetAttackRoll();

        /// <summary>
        /// Get the damage roll of the character.
        /// See <see cref="Actions.PlayerAttack.Attack"/> for more details.
        /// </summary>
        /// <returns>Integer value of the damage roll.</returns>
        int GetDamageRoll();
        
        /// <summary>
        /// Get the magical attack roll of the character.
        /// See <see cref="Actions.PlayerMagic.MagicAttack(UnityEngine.GameObject)"/>.
        /// </summary>
        /// <returns>Integer value of the magical attack roll.</returns>
        int GetMagicAttackRoll();
        
        /// <summary>
        /// Get the magical damage roll of the character.
        /// See <see cref="Actions.PlayerMagic.MagicAttack(UnityEngine.GameObject)"/>.
        /// </summary>
        /// <returns>Integer value of the magical damage roll.</returns>
        int GetMagicDamageRoll();
    }
}