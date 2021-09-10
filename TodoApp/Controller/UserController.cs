using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.DAL;
using TodoApp.Models;
namespace TodoApp.Controller
{
    [ApiController]
    [Authorize]
    [Route("/api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly TodoContext _context;

        public UserController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            try
            {
                return Ok(_context.User);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
       

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int Id)
        {
            try
            {
                User _user = await _context.User.FindAsync(Id);

                if (_user != null)
                    return Ok(_user);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            try
            {
                User _user = new User();
                _user.Name = user.Name;
                _user.Login = user.Login;
                _user.Password = user.Password;

                await _context.User.AddAsync(_user);
                await _context.SaveChangesAsync();

                string uri = Url.Action("GetById", new
                {
                    Id = _user.UserId,
                });

                return Created(uri, _user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPut("{id}")]
        public IActionResult Edit()
        {
            return null;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete()
        {
            return null;
        }
    }
}
