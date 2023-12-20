using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Services
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();       
        TEntity Get(int id);
        bool Any(TEntity entity);
        void Add(ref TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<bool> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        bool AddRange(IEnumerable<TEntity> entity);
        Task<bool> DeleteRange(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entity);
        void DeleteById(int id);
        void DeleteByFlag(TEntity entity);
    }
}
