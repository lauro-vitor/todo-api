using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Models
{
    public class PagingModel
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public PagingModel()
        {
            Page = 1;
            Limit = 10;
        }
    }
}
