using System;

namespace Scripts.CharacterClass
{
    /// <inheritdoc cref="IRandomNumberGenerator"/>
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        // System random implementation.
        readonly Random random = new Random();
        
        /// <inheritdoc cref="IRandomNumberGenerator.GetRandom(int, int)"/>
        public int GetRandom(int min, int max) => random.Next(min, max);
    }
}