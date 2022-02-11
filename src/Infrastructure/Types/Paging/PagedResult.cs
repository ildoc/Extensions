namespace Infrastructure.Types.Paging
{
    public class PagedResult<T> //: PagedResultBase
    {
        public IEnumerable<T> Items { get; }
        public int TotalItems { get; }

        public PagedResult(IEnumerable<T> items, int totalItems)
        {
            Items = items;
            TotalItems = totalItems;
        }
    }
}
