using System;

namespace Core.Neural
{
    public class RandomWeightProvider : IWeightProvider
    {
        private static readonly Random random = new Random();

        public double GetWeight(int i, int j)
        {
            return random.NextDouble() - 0.5;
        }
    }
}
