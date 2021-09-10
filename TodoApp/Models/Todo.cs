using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Models
{
    public class Todo
    {
        public int TodoId { get; set; }
        public string Name { get; set; }
        public bool? IsDone { get; set; }
        public User User { get; set; }
        public int? UserId { get; set; }
    }
}
