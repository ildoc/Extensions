namespace Infrastructure.DbPatch
{
    public interface IPatcher
    {
        void QueuePatch(Func<Task> patch);
        Task ApplyPatches();

        Version Version { get; }
    }
}
