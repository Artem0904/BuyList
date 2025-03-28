﻿using System.Linq.Expressions;
using Ardalis.Specification;

namespace DataAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAsync(
               Expression<Func<TEntity, bool>> filter = null!,
               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
               params string[] includeProperties);
        Task<TEntity?> GetByIDAsync(object id);
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> exp);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> exp);
        Task InsertAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Delete(object id);
        Task DeleteAsync(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        Task SaveAsync();
        Task<TEntity?> GetItemBySpec(ISpecification<TEntity> specification);
        Task<IEnumerable<TEntity>> GetListBySpec(ISpecification<TEntity> specification);
    }
}
