using System;

namespace nfl_outlast.Shared
{
    public static class Extensions
    {
        public static string[] Split(this string originalString, string splitString)
        {
            return originalString.Split(new[] { splitString }, StringSplitOptions.None);
        }

        public static int Index(this string originalString, string value)
        {
            return originalString.IndexOf(value, StringComparison.Ordinal);
        }
    }
}