using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.DAL
{
    interface IDAL<T> where T : class
    {
        IEnumerable<T> GetAll(T filterEntity, SortingModel sorting);
        Task<T> GetById(int Id);
        Task<T> Create(T entity);
        Task Update(T entity);
        Task Delete(int Id);
    }
}
