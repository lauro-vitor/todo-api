using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Models
{
    public class SortingModel
    {
        public string SortExpression { get; set; }
        public string SortDirection { get; set; }

        public SortingModel()
        {
            SortExpression = "";
            SortDirection = "asc";
        }
    }


}
