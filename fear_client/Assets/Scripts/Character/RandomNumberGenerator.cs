using System;

namespace Scripts.Character
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        readonly Random random = new Random();
        public int GetRandom(int min, int max) => random.Next(min, max);
    }
}