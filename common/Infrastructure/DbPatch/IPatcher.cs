using System;
using System.Threading.Tasks;

namespace Infrastructure.DbPatch
{
    public interface IPatcher
    {
        void QueuePatch(Func<Task> patch);
        Task<Version> ApplyPatches();

        Version Version { get; }
    }
}
