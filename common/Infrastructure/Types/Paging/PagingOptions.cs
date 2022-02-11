using Utils.Enums;

namespace Boilerplate.Common.Types.Paging
{
    public class PagingOptions
    {
        public int RowsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public string SortingProperty { get; set; }
        public SortingOptions SortingOptions { get; set; }
        public string FilterValue { get; set; }
    }
}
