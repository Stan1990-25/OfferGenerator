using System;

namespace TMOffersClients
{
    /// <summary>
    /// Helper class for strings
    /// </summary>
    public static class StringHelpers
    {
        /// <summary>
        /// Indicates whether a string contains a certain substring
        /// </summary>
        /// <param name="source">The source string</param>
        /// <param name="value">The string that will be looked for</param>
        /// <param name="comparison">Comparison options</param>
        /// <returns></returns>
        public static bool Contains(this string source, string value, StringComparison comparison)
        {
            return source.IndexOf(value, comparison) > -1;
        }
    }
}
