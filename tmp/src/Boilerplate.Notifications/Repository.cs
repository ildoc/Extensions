using Boilerplate.Common.Types.Abstracts;

namespace Boilerplate.Notifications
{
    public class Repository<T> : BaseRepository<T> where T : class, new()
    {
        public Repository(NotificationContext context) : base(context)
        {
        }
    }
}
