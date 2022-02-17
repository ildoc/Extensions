namespace Infrastructure.Ldap
{
    public interface ILdapAuthenticationService
    {
        bool Login(string userName, string password);
    }
}
