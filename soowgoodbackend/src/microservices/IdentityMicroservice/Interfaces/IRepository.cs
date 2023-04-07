using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetById(object id);
        Task Add(TEntity entity);
        void AddMany(List<TEntity> entities);
        void Update(TEntity entity);
        void Update(int id, TEntity entity);
        Task Delete(int id);
        Task Save();
    }
}
