using System.DirectoryServices;
using Microsoft.Extensions.Options;

namespace Infrastructure.Ldap
{
    public class LdapAuthenticationService : ILdapAuthenticationService
    {
        private const string DisplayNameAttribute = "DisplayName";
        private const string SAMAccountNameAttribute = "SAMAccountName";
        private readonly LdapSettings _settings;

        public LdapAuthenticationService(IOptions<LdapSettings> settings)
        {
            _settings = settings.Value;
        }
        public LdapUser Login(string userName, string password)
        {
            try
            {
                using var entry = new DirectoryEntry(_settings.Path, _settings.UserDomainName + "\\" + userName, password);
                using var searcher = new DirectorySearcher(entry);
                searcher.Filter = String.Format("({0}={1})", SAMAccountNameAttribute, userName);
                searcher.PropertiesToLoad.Add(DisplayNameAttribute);
                searcher.PropertiesToLoad.Add(SAMAccountNameAttribute);
                var result = searcher.FindOne();
                if (result != null)
                {
                    var displayName = result.Properties[DisplayNameAttribute];
                    var samAccountName = result.Properties[SAMAccountNameAttribute];

                    return new LdapUser
                    {
                        DisplayName = displayName == null || displayName.Count <= 0 ? null : displayName[0].ToString(),
                        UserName = samAccountName == null || samAccountName.Count <= 0 ? null : samAccountName[0].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                // if we get an error, it means we have a login failure.
                // Log specific exception
            }
            return null;
        }
    }
}
