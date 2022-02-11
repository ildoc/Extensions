using System;
using System.Threading.Tasks;

namespace Boilerplate.Common.DbPatch
{
    public interface IPatcher
    {
        void QueuePatch(Func<Task> patch);
        Task<Version> ApplyPatches();

        Version Version { get; }
    }
}
