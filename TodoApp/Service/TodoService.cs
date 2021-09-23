using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models;
using TodoApp.DTO;
using TodoApp.DAL;
using Microsoft.AspNetCore.Http;

namespace TodoApp.Service
{
    public class TodoService
    {
        private readonly TodoContext _dbContext;
        private readonly HttpContext _httpContext;

        public TodoService(TodoContext dbContext, HttpContext httpContext)
        {
            _dbContext = dbContext;
            _httpContext = httpContext;
        }

        public PagedCollectionDTO<TodoDTO> GetAll(Todo todoFilter, SortingModel sorting, PagingModel paging)
        {
            TodoDAL todoDAL = new TodoDAL(_dbContext);

            IEnumerable<Todo> todos = todoDAL.GetAll(todoFilter, sorting);

            int totalItems = todos.Count();

            int PageQuantity = 0;

            if (totalItems % paging.Limit > 0)
                PageQuantity = (totalItems / paging.Limit) + 1;
            else
                PageQuantity = (totalItems / paging.Limit);


            IEnumerable<Todo> todosPaging = todos
                .Skip((paging.Page - 1) * paging.Limit)
                .Take(paging.Limit);

            string nextPage = "";

            string previousPage = "";

            string uri = _httpContext.Request.Scheme + "://" + _httpContext.Request.Host.ToUriComponent() + "/api/todo/";

            if (paging.Page < PageQuantity)
            {
                nextPage = uri + "?page=" + (paging.Page + 1);
            }

            if (paging.Page > 1 && paging.Page < PageQuantity)
            {
                previousPage = uri + "?page=" + (paging.Page - 1);
            }


            List<TodoDTO> todoListDTO = todosPaging
                .Select(t => new TodoDTO(t))
                .ToList();

            PagedCollectionDTO<TodoDTO> result = new PagedCollectionDTO<TodoDTO>()
            {
                Items = todoListDTO,
                TotalItems = totalItems,
                PageNumber = paging.Page,
                PageSize = paging.Limit,
                PageQuantity = PageQuantity,
                NextPage = nextPage,
                PreviousPage = previousPage,
            };

            return result;
        }

    }
}
