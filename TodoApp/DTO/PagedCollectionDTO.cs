using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Models
{
    public class PagedCollectionDTO<T> where T : class
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
    }
}
