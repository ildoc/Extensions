namespace Extensions
{
    public static class BoolExtensions
    {
        public static bool ToBool<T>(this T o) => o switch
        {
            bool b => b,
            int i => i != default,
            string s => bool.TryParse(s, out var res) && res,
            float f => f != default,
            double d => d != default,
            _ => o != null
        };
    }
}
