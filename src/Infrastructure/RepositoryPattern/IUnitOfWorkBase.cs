using Microsoft.AspNetCore.Identity;

namespace Infrastructure.RepositoryPattern
{
    public interface IUnitOfWorkBase
    {
        void Save();
        Task<int> SaveAsync();
    }

    public interface IUnitOfWorkBase<TUser> : IUnitOfWorkBase where TUser : IdentityUser
    {
        void Save(TUser user);
        Task<int> SaveAsync(TUser user);
    }
}
