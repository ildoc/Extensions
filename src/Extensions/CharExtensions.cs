namespace Extensions
{
    public static class CharExtensions
    {
        /// <summary>
        /// Transforms the char to lowercase
        /// </summary>
        /// <param name="c"></param>
        public static char ToLower(this char c) =>
            char.ToLower(c);

        /// <summary>
        /// Transforms the char to uppercase
        /// </summary>
        /// <param name="c"></param>
        public static char ToUpper(this char c) =>
            char.ToUpper(c);
    }
}
