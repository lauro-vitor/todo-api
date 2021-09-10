using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.DAL
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {

        }
        public DbSet<Todo> Todo { get; set; }
        public DbSet<User> User { get; set; }
    }
}
