namespace Infrastructure.Keycloak
{
    public class KeycloakSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RealmName { get; set; }
        public string KeycloakBaseUrl { get; set; }

        public string TokenUrl { get; set; }
        public string UserByUsernameUrl { get; set; }
        public string UserServiceUrl { get; set; }
        public string UserServiceAppName { get; set; }
        public string UserServiceAppKey { get; set; }
    }
}
