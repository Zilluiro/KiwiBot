using KiwiBot.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KiwiBot.Data.Repository
{
    interface IGlobalRepository
    {
        Task<T> FindAsync<T>(object key) where T: class;
        Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate, 
            params Expression<Func<T, object>>[] includes) where T: class;
        Task<List<T>> GetAllAsync<T>() where T: class;
        Task<T> AddAsync<T>(T entity) where T : class;
        Task<T> UpdateAsync<T>(T entity) where T : class;
        Task<T> RemoveAsync<T>(T entity) where T : class;

        Task<Booru> GetSelectedBooruAsync(long chatId);
    }
}
