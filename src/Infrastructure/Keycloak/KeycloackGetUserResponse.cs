namespace Infrastructure.Keycloak
{
    //public class KeycloackGetUserResponse
    //{
    //    public string Id { get; set; }
    //    public long CreatedTimestamp { get; set; }
    //    public string Username { get; set; }
    //    public bool Enabled { get; set; }
    //    public bool Totp { get; set; }
    //    public bool EmailVerified { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Email { get; set; }
    //}

    public class KeycloackGetUserResponseResult
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool InternalUser { get; set; }
    }

    public class KeycloackGetUserResponse
    {
        public int ResultType { get; set; }
        public KeycloackGetUserResponseResult Result { get; set; }
    }

}
