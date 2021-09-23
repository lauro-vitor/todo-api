using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.DAL
{
    public class TodoDAL : IDAL<Todo>
    {
        private readonly TodoContext _context;

        public TodoDAL(TodoContext context)
        {
            _context = context;
        }

        public async Task<Todo> Create(Todo entity)
        {
            Todo todo = new Todo
            {
                Name = entity.Name,
                IsDone = entity.IsDone,
                UserId = entity.UserId,
            };
            await _context.Todo.AddAsync(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task Delete(int id)
        {
            Todo todo = await _context.Todo.FindAsync(id);
            _context.Remove(todo);
        }

        public IEnumerable<Todo> GetAll(Todo filterEntity, SortingModel sorting)
        {

            Func<Todo, bool> filterTodo = null;
            Func<Todo, object> sortTodo = null;

            if (filterEntity != null)
            {
                filterTodo = t =>
                  (string.IsNullOrEmpty(filterEntity.Name) || t.Name.ToLower().Trim().Contains(filterEntity.Name.ToLower().Trim()))
                  &&
                  (!filterEntity.UserId.HasValue || (t.UserId.HasValue && filterEntity.UserId.Value == t.UserId.Value))
                  &&
                  (!filterEntity.IsDone.HasValue || (t.IsDone.HasValue && filterEntity.IsDone.Value == t.IsDone.Value));
            }

            if (!string.IsNullOrEmpty(sorting.SortExpression.ToLower().Trim()))
            {
                string sortExpression = sorting.SortExpression.ToLower().Trim();
                string sortDirection = sorting.SortDirection.ToLower().Trim();

                switch (sorting.SortExpression)
                {
                    case "todoid":
                        sortTodo = t => t.TodoId;
                        break;
                    case "name":
                        sortTodo = t => t.Name;
                        break;
                    case "isdone":
                        sortTodo = t => t.IsDone;
                        break;
                    case "user":
                        sortTodo = t => t.UserId.HasValue ? t.User.Name : "";
                        break;
                    default:
                        sortTodo = t => t.TodoId;
                        break;
                }

                if (sorting.SortDirection == "asc")
                    return _context.Todo
                           .Where(filterTodo)
                           .OrderBy(sortTodo)
                           .ToList();
                else
                    return _context.Todo
                         .Where(filterTodo)
                         .OrderByDescending(sortTodo)
                         .ToList();
            }
            else
                return _context.Todo.Where(filterTodo)
                    .ToList();
        }

        public Task<Todo> GetById(int Id)
        {
            return _context.Todo.FindAsync(Id);
        }

        public async Task Update(Todo entity)
        {
            Todo todo = await _context.Todo.FindAsync(entity.TodoId);
            todo.Name = entity.Name;
            todo.IsDone = entity.IsDone;
            todo.UserId = entity.UserId;
            await _context.SaveChangesAsync();
        }
    }
}
