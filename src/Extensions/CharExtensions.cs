namespace Extensions
{
    public static class CharExtensions
    {
        public static char ToLower(this char c) =>
            char.ToLower(c);

        public static char ToUpper(this char c) =>
            char.ToUpper(c);

        /// <summary>
        /// Checks if a char is [a-zA-Z]
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsAsciiAlphabetLetter(this char ch) =>
            (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
    }
}
