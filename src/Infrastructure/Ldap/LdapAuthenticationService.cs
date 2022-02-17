using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;

namespace Infrastructure.Ldap
{
    public class LdapAuthenticationService : ILdapAuthenticationService
    {
        private readonly LdapSettings _settings;

        public LdapAuthenticationService(IOptions<LdapSettings> settings)
        {
            _settings = settings.Value;
        }

        public bool Login(string username, string password)
        {
            string userDn = $"uid={username},{_settings.UserDomainName}";
            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect(_settings.Path, LdapConnection.DefaultPort);
                    connection.Bind(userDn, password);
                    if (connection.Bound)
                        return true;
                }
            }
            catch (LdapException ex)
            {
                // Log exception
            }
            return default;
        }
    }
}
