namespace Scripts.CharacterClass
{
    public interface ICharacterFeatures
    {
        int ArmorBonus { get; set; }
        int AttackBonus { get; set; }
        int AttackRange { get; set; }
        string Charclass { get; set; }
        int DamageBonus { get; set; }
        int Health { get; set; }
        bool IsAttacking { get; set; }
        bool IsFocused { get; set; }
        int IsLeader { get; set; }
        int MaxAttackVal { get; set; }
        int Movement { get; set; }
        int Perception { get; set; }
        IRandomNumberGenerator Rng { get; set; }
        int Stealth { get; set; }
        int Team { get; set; }
        int TroopId { get; set; }

        void DamageCharacter(int amount);
        int GetAttackRoll();
        int GetDamageRoll();
        int GetMagicAttackRoll();
        int GetMagicDamageRoll();
    }
}