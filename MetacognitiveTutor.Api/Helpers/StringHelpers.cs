using System;

namespace MetacognitiveTutor.Api.Helpers
{
    public static class StringHelpers
    {
        public static string GetUntilOrEmpty(this string text, string stopAt) // https://stackoverflow.com/a/1857525/109941, 07/04/2018
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                var charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return string.Empty;
        }
    }
}