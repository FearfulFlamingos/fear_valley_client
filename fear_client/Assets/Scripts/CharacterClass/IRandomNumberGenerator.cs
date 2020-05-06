namespace Scripts.CharacterClass
{
    /// <summary>
    /// Random Number Generator used for attacks and damage rolls.
    /// </summary>
    /// <remarks>The interface is used to substitute during testing.</remarks>
    public interface IRandomNumberGenerator
    {
        /// <summary>
        /// Get a random number between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="min">Smallest number (usually one).</param>
        /// <param name="max">Largest number.</param>
        /// <returns>A random number within the bounds.</returns>
        int GetRandom(int min, int max);
    }
}