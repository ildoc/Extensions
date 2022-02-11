namespace Infrastructure.Ldap
{
    public interface ILdapAuthenticationService
    {
        LdapUser Login(string userName, string password);
    }
}