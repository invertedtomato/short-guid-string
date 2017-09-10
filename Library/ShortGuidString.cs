using System;
using System.Text.RegularExpressions;

namespace InvertedTomato.Encoding.ShortGuidStrings {
    /// <summary>
    /// Normally a GUID encoded as a string consumes 38 characters. ShortGuidString offers an alternative, encoding the GUID as a 23 character URL-safe string.
    /// </summary>
    public static class ShortGuidString {
        private static readonly Regex ShortPattern = new Regex(@"^[^<>]*\w?\*([a-zA-Z0-9-~]{22})$");
        private static readonly Regex LegacyPattern = new Regex(@"^[^<>]*<([A-Z0-9]{8}-([A-Z0-9]{4}-){3}[A-Z0-9]{12})>$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Encode as a short-GUID string.
        /// </summary>
        public static string Encode(Guid input) {
            var b64 = Convert.ToBase64String(input.ToByteArray());
            return "*" + b64.Replace('+', '-').Replace('/', '~').Substring(0, 22);
        }

        /// <summary>
        /// Encode as a short-GUID string with a label pre-fix.
        /// </summary>
        public static string Encode(Guid input, string label) {
            return label + " " + Encode(input);
        }

        /// <summary>
        /// Decoded a short-GUID string. Any prefix-label is discarded.
        /// </summary>
        public static Guid Decode(string input) {
            if (null == input) {
                throw new ArgumentNullException("input", "Input cannot be null.");
            }
            
            // Attempt to match as a short-GUID
            var matches = ShortPattern.Match(input);
            if (!matches.Success) {
                // Match failed, attempt legacy long-GUID match
                matches = LegacyPattern.Match(input);
                if (!matches.Success) {
                    throw new FormatException("Input doesn't match any known format.");
                }

                // Parse long-GUID
                return new Guid(matches.Groups[1].Value);
            }

            // Parse short-GUID
            var b64 = matches.Groups[1].Value.Replace('-', '+').Replace('~', '/') + "==";
            var bytes = Convert.FromBase64String(b64);
            return new Guid(bytes);

        }
    }
}
