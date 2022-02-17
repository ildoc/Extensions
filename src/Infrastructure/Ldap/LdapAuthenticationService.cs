using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;

namespace Infrastructure.Ldap
{
    public class LdapAuthenticationService : ILdapAuthenticationService
    {
        private readonly LdapSettings _settings;
        private readonly ILogger<LdapAuthenticationService> _logger;

        public LdapAuthenticationService(IOptions<LdapSettings> settings, ILogger<LdapAuthenticationService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public bool Login(string username, string password)
        {
            var userDn = $"uid={username},{_settings.UserDomainName}";
            try
            {
                using var connection = new LdapConnection { SecureSocketLayer = false };
                connection.Connect(_settings.Path, LdapConnection.DefaultPort);
                connection.Bind(userDn, password);
                if (connection.Bound)
                    return true;
            }
            catch (LdapException ex)
            {
                _logger.LogError(ex, $"Error Authenticating against LDAP server {_settings.Path}");
            }
            return false;
        }
    }
}
