namespace Utils
{
    public static class ConnectionStrings
    {
        public static string SqlCS(string serverAddress, string database, string username, string password) =>
            $"Server={serverAddress};Database={database};User Id={username};Password={password}";
    }
}
