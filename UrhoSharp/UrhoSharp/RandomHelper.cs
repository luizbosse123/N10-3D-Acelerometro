using System;

namespace UrhoSharp
{
    public static class RandomHelper
    {
        static readonly Random random = new Random();

        public static float NextRandom() { return (float)random.NextDouble(); }

        public static float NextRandom(float range) { return (float)random.NextDouble() * range; }

        public static float NextRandom(float min, float max) { return (float)((random.NextDouble() * (max - min)) + min); }

        public static int NextRandom(int min, int max) { return random.Next(min, max); }
    }
}