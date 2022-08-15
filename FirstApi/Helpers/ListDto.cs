using System.Collections.Generic;

namespace FirstApi.Helpers
{
    public class ListDto<T>
    {
        public int TotalCount { get; set; }
        public List<T> items { get; set; }
    }
}
