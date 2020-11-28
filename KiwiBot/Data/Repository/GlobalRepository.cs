﻿using KiwiBot.Data.Entities;
using KiwiBot.Data.Enumerations;
using KiwiBot.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KiwiBot.Data.Repository
{
    class GlobalRepository: IGlobalRepository
    {
        private readonly DataContext _dataContext;
        private readonly BotSettings _configuration;

        public GlobalRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        #region Generics
        public async Task<T> FindAsync<T>(object key) where T: class
        {
            return await _dataContext.Set<T>().FindAsync(key);
        } 
        
        public async Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate, 
            params Expression<Func<T, object>>[] includes) where T: class
        {
            IQueryable<T> query = _dataContext.Set<T>();

            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync<T>() where T: class
        {
            return await _dataContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync<T>(T entity) where T : class
        {
            _dataContext.Add(entity);
            await _dataContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync<T>(T entity) where T : class
        {
            _dataContext.Update(entity);
            await _dataContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> RemoveAsync<T>(T entity) where T : class
        {
            _dataContext.Remove(entity);
            await _dataContext.SaveChangesAsync();
            return entity;
        }
        #endregion

        public async Task<Booru> GetSelectedBooruAsync(long chatId)
        {
            return await _dataContext.Boorus.Include(x => x.Chats)
                .FirstOrDefaultAsync(x => x.Chats.Any(x => x.ChatId == chatId));
        }
    }
}
