using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Utils.Enums;
using static System.IO.Path;

namespace Extensions
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string str) where T : struct
        {
            try
            {
                return str == null ? default : (T)Enum.Parse(typeof(T), str, true);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException($"Cannot parse {str} to {typeof(T).Name}", e);
            }
        }

        public static bool IsEqualTo(this string str, string strToCompare) => IsEqualTo(str, strToCompare, false);

        public static bool IsEqualTo(this string str, string strToCompare, bool caseSensitive)
            => string.Equals(str, strToCompare, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);

        public static string Absolutize(this string path)
            => IsPathRooted(path) ? path : Combine(Directory.GetCurrentDirectory(), path);

        public static string Slice(this string source, int start, int end)
        {
            if (end < 0) // Keep this for negative end support
                end = source.Length + end;
            var len = end - start; // Calculate length
            return source.Substring(start, len); // Return Substring of length
        }

        public static string FromCamelCase(this string source)
        {
            var returnValue = source;

            //Strip leading "_" character
            returnValue = Regex.Replace(returnValue, "^_", "").Trim();
            //Add a space between each lower case character and upper case character
            returnValue = Regex.Replace(returnValue, "([a-z])([A-Z])", "$1 $2").Trim();
            //Add a space between 2 upper case characters when the second one is followed by a lower space character
            returnValue = Regex.Replace(returnValue, "([A-Z])([A-Z][a-z])", "$1 $2").Trim();

            return returnValue;
        }

        public static string ReplaceWith(this string value, string newValue) =>
            newValue.IsNullOrEmpty() ? value : newValue;

        public static bool IsNullOrEmpty(this string value) =>
            value == null || value == string.Empty;

        public static bool IsNullOrWhiteSpace(this string value) =>
            value == null || value.Trim().Length == 0;

        public static bool IsNumeric(this string value) => long.TryParse(value, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out var retNum);

        public static IEnumerable<string> SplitString(this string s) =>
            s.ToCharArray().Select(x => x.ToString());

        public static string CenterString(this string stringToCenter, int totalLength, char paddingChar) => stringToCenter
            .PadLeft(((totalLength - stringToCenter.Length) / 2) + stringToCenter.Length, paddingChar)
            .PadRight(totalLength, paddingChar);

        public static string CenterString(this string stringToCenter, int totalLength) =>
            CenterString(stringToCenter, totalLength, ' ');

        public static byte[] ToBytes(this string input, KeyEncoding keyEncoding = KeyEncoding.UTF8)
        {
            switch (keyEncoding)
            {
                case KeyEncoding.ASCII:
                    return Encoding.ASCII.GetBytes(input);
                case KeyEncoding.Base64:
                    return Convert.FromBase64String(input);
                default:
                    return Encoding.UTF8.GetBytes(input);
            }
        }

        public static string ToString(this byte[] input, KeyEncoding keyEncoding = KeyEncoding.UTF8)
        {
            switch (keyEncoding)
            {
                case KeyEncoding.ASCII:
                    return Encoding.ASCII.GetString(input);
                case KeyEncoding.Base64:
                    return Convert.ToBase64String(input);
                default:
                    return Encoding.UTF8.GetString(input);
            }
        }

        public static string NewLine(this string str) => str + "\n";

        public static IEnumerable<string> SplitLines(this string s)
        {
            string line;
            using (var sr = new StringReader(s))
                while ((line = sr.ReadLine()) != null)
                    yield return line;
        }

        public static string TransformTemplate(this string template, object data)
        {
            var result = template;
            data.GetType().GetProperties().Each(x =>
                result = result.Replace($"{{{{{x.Name}}}}}", x.GetValue(data).ToString())
            );
            return result;
        }

        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }
    }
}
