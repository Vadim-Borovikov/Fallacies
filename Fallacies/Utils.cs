using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Fallacies
{
    internal static class Utils
    {
        public static string RemovePrefix(this string s, string prefix)
        {
            return s.StartsWith(prefix, StringComparison.Ordinal) ? s.Substring(prefix.Length, s.Length - prefix.Length) : s;
        }

        public static string RemoveSuffix(this string s, string suffix)
        {
            return s.EndsWith(suffix, StringComparison.Ordinal) ? s.Substring(0, s.Length - suffix.Length) : s;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            return pairs.ToDictionary(p => p.Key, p => p.Value);
        }

        public static string StartWithCapital(this string s)
        {
            return s == null ? null : $"{char.ToUpper(s.First())}{s.Substring(1)}";
        }
    }
}
