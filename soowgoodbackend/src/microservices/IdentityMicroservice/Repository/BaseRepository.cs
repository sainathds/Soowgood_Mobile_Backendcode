using IdentityMicroservice.Data;
using IdentityMicroservice.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IdentityMicroserviceContext _context;
        private DbSet<TEntity> dbSet;
        public BaseRepository(IdentityMicroserviceContext context)
        {
            _context = context;
            dbSet = _context.Set<TEntity>();
        }
        public async Task Add(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public void AddMany(List<TEntity> entities)
        {
            dbSet.AddRangeAsync(entities);
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            dbSet.Remove(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            return dbSet.AsQueryable();
        }

        public async Task<TEntity> GetById(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(int id, TEntity entity)
        {
            dbSet.Update(entity);
        }
        public void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }
    }
}
