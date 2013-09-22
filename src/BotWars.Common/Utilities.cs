using System;
using System.Collections.Generic;
using System.Linq;

namespace BotWars.Common
{
    public static class Utilities
    {
        static Random _random = new Random();

        public static T TakeRandom<T>(this IEnumerable<T> source)
        {
            var list = source.ToList();
            var index = _random.Next(list.Count);
            return list[index];
        }
    }
}