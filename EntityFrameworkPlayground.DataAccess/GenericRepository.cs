using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.DataAccess
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected BooksContext db;

        public GenericRepository(BooksContext db)
        {
            this.db = db;
        }

        public async Task Create(TEntity entity)
        {
            await db.Set<TEntity>().AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            db.Set<TEntity>().Remove(entity);
            await db.SaveChangesAsync();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return db.Set<TEntity>().AsNoTracking().ToList();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await db.Set<TEntity>()
                .FindAsync(id);
        }

        public async Task Update(int id, TEntity entity)
        {
            db.Set<TEntity>().Update(entity);
            await db.SaveChangesAsync();
        }
    }
}
