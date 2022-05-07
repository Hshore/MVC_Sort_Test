﻿namespace MVC_Sort_Test.Extensions
{
    class ThreadSafeRandom
    {

        private static readonly System.Random GlobalRandom = new Random();
        private static readonly ThreadLocal<Random> LocalRandom = new ThreadLocal<Random>(() =>
        {
            lock (GlobalRandom)
            {
                return new Random(GlobalRandom.Next());
            }
        });

        public static int Next(int min = 0, int max = Int32.MaxValue)
        {
            return LocalRandom.Value.Next(min, max);
        }

        public static double NextDouble(double min, double max)
        {
            return LocalRandom.Value.NextDouble() * (max - min) + min;

        }
    }
}
