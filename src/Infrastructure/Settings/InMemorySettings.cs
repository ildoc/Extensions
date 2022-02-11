using Infrastructure.Keycloak;

namespace Infrastructure.Settings
{
    public class InMemorySettings
    {
        private readonly IKeycloakClient _keycloakClient;
        private string _clientToken;

        public InMemorySettings(IKeycloakClient keycloakClient)
        {
            _keycloakClient = keycloakClient;
        }

        public string ClientToken
        {
            get
            {
                if (_clientToken == null || !_keycloakClient.CheckToken(_clientToken).GetAwaiter().GetResult())
                    _clientToken = _keycloakClient.GetTokenUsingClientCredentials().GetAwaiter().GetResult();
                return _clientToken;
            }
        }
    }
}
