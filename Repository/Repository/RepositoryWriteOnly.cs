using Infra.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository.Repository
{



    public class RepositoryWriteOnly<TEntity> : IRepositoryWriteOnly<TEntity> where TEntity : class
    {

        private readonly DbContext _dbContext;

        public RepositoryWriteOnly(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Delete(TEntity entity)
        {
            await Task.FromResult(true);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public async Task RemoveAll(List<TEntity> entity)
        {
            await Task.FromResult(true);
            _dbContext.RemoveRange(entity);
        }
        public async Task<bool> Remove(TEntity entity)
        {
            await Task.FromResult(true);
            _dbContext.Remove(entity);
            return true;
        }


        public async Task<bool> Insert(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(entity, cancellationToken);
            _dbContext.Entry(entity).State = EntityState.Added;
            return true;
        }

        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> Update(TEntity entity)
        {
            await Task.FromResult(true);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public async Task<bool> UpdateAll(List<TEntity> entitys)
        {
            await Task.FromResult(true);
            foreach (var entity in entitys)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
            return true;
        }


        public async Task<bool> InsertList(List<TEntity> entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddRangeAsync(entity, cancellationToken);
            return true;
        }

        public async Task<int> GetCount(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TEntity>().CountAsync(cancellationToken) + 1;
        }

        public async Task<int> GetCount(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TEntity>().CountAsync(predicate, cancellationToken);
        }




        public async Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal>> prop)
            => await _dbContext.Set<TEntity>().Where(predicate).SumAsync(prop);


        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
