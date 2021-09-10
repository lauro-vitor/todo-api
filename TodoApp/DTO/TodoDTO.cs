using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.DTO
{
    public class TodoDTO
    {
        public int TodoId { get; set; }
        public string Name { get; set; }
        public bool? IsDone { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }

        public TodoDTO()
        {
        }

        public TodoDTO(Todo todo)
        {
            TodoId = todo.TodoId;
            Name = todo.Name;
            IsDone = todo.IsDone;
            UserId = todo.UserId;
            UserName = todo.User != null ? todo.User.Name : "";
        }
    }
}
