using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using TodoApp.DTO;
using TodoApp.DAL;
using TodoApp.Service;

namespace TodoApp.Controller
{
    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<PagedCollectionDTO<TodoDTO>> GetAll([FromQuery] Todo todoFilter,
             [FromQuery] SortingModel sorting,
            [FromQuery] PagingModel paging)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TodoService todoService = new TodoService(_context, HttpContext);
                    PagedCollectionDTO<TodoDTO> result = todoService.GetAll(todoFilter, sorting, paging);
                    return Ok(result);
                }
                else
                    throw new Exception("Model invalid");
            }
            catch (Exception ex)
            {
                //HttpResponseException httpResponseException = new HttpResponseException();
                
                //httpResponseException.Message = ex.Message;
                return BadRequest(ex);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetById(int id)
        {
            try
            {
                Todo todo = await new TodoDAL(_context).GetById(id);

                if (todo == null)
                    return NotFound();
                else
                    return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public async Task<ActionResult<CreatedResult>> Create([FromBody] Todo todo)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                TodoDAL todoDAL = new TodoDAL(_context);

                Todo todoCreated = await todoDAL.Create(todo);

                var uri = Url.Action("GetById", new
                {
                    Id = todoCreated.TodoId
                });

                return Created(uri, todoCreated); //201
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Todo todo)
        {
            try
            {
                if (!ModelState.IsValid || todo.TodoId != id)
                    throw new Exception("You can not update this item");

                TodoDAL todoDAL = new TodoDAL(_context);
                await todoDAL.Update(todo);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                TodoDAL todoDAL = new TodoDAL(_context);
                await todoDAL.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
