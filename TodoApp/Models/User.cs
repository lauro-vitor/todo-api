using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace TodoApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

    }
}
