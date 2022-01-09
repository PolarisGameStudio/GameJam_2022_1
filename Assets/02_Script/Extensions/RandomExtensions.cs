

namespace PCG.Random
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public static class RandomExtensions
    {
        public static T Random<T>(this IEnumerable<T> target)
        {
            int randomIndex = Next(0, target.Count());
            return target.ElementAt(randomIndex);
        }

        public static T Random<T>(this T[] target, System.Func<T, int, int> getPercentage)
        {
            int totalVariable = 0;
            for (int i = 0; i < target.Length; i++)
            {
                totalVariable += getPercentage(target[i], i);
            }

            int random = Next(totalVariable);

            totalVariable = 0;
            for (int i = 0; i < target.Length; i++)
            {
                totalVariable += getPercentage(target[i], i);
                if (random < totalVariable)
                {
                    return target[i];
                }
            }

            return default;
        }

        public static T Random<T>(this IReadOnlyList<T> target, System.Func<T, int, int> getPercentage)
        {
            int totalVariable = 0;
            for (int i = 0; i < target.Count; i++)
            {
                totalVariable += getPercentage(target[i], i);
            }

            int random = Next(totalVariable);

            totalVariable = 0;
            for (int i = 0; i < target.Count; i++)
            {
                totalVariable += getPercentage(target[i], i);
                if (random < totalVariable)
                {
                    return target[i];
                }
            }

            return default;
        }

        public static T Random<T>(this IReadOnlyList<T> target, System.Func<T, int, float> getPercentage)
        {
            float totalVariable = 0;
            for (int i = 0; i < target.Count; i++)
            {
                totalVariable += getPercentage(target[i], i);
            }

            float random = Next(totalVariable);

            totalVariable = 0;
            for (int i = 0; i < target.Count; i++)
            {
                totalVariable += getPercentage(target[i], i);
                if (random < totalVariable)
                {
                    return target[i];
                }
            }

            return default;
        }

        public static T Random<T>(this IReadOnlyList<T> target, System.Func<T, bool> onFilter)
        {
            IEnumerable<T> filteredTarget = target.Where(onFilter);
            int randomIndex = Next(0, filteredTarget.Count());
            return filteredTarget.ElementAt(randomIndex);
        }

        public static List<T> Shuffle<T>(this List<T> target)
        {
            target.Sort((a, b) => Next(-1, 2));
            return target;
        }



        // thread safe System.Random
        // https://stackoverflow.com/questions/3049467/is-c-sharp-random-number-generator-thread-safe
        private static readonly Random s_global = new Random();
        [ThreadStatic] private static Random _local;

        public static int Next()
        {
            InitRandom();
            return _local.Next();
        }

        /// <summary>
        /// int random
        /// </summary>
        /// <param name="max">not included</param>
        /// <returns></returns>
        public static int Next(int max) => Next(0, max);

        /// <summary>
        /// range int random
        /// </summary>
        /// <param name="min">included</param>
        /// <param name="max">not included</param>
        /// <returns></returns>
        public static int Next(int min, int max)
        {
            InitRandom();
            return _local.Next(min, max);
        }

        /// <summary>
        /// float random
        /// </summary>
        /// <param name="max">not included</param>
        /// <returns></returns>
        public static float Next(float max) => Next(0f, max);

        /// <summary>
        /// float random
        /// </summary>
        /// <param name="max">not included</param>
        /// <returns></returns>
        public static float Next(float min, float max)
        {
            InitRandom();
            return (float)_local.NextDouble() * (max - min) + min;
        }

        private static void InitRandom()
        {
            if (_local == null)
            {
                lock (s_global)
                {
                    if (_local == null)
                    {
                        int seed = s_global.Next();
                        _local = new Random(seed);
                    }
                }
            }
        }
    }
}