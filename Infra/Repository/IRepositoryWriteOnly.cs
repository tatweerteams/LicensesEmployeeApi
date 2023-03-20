using System.Linq.Expressions;

namespace Infra.Repository
{

    public interface IRepositoryWriteOnly<TEntity> : IDisposable where TEntity : class
    {
        Task<bool> Insert(TEntity entity, CancellationToken cancellationToken = default);
        Task<bool> InsertList(List<TEntity> entity, CancellationToken cancellationToken = default);
        Task<bool> Update(TEntity entity);
        Task<bool> Delete(TEntity entity);
        Task<bool> Remove(TEntity entity);
        Task RemoveAll(List<TEntity> entity);
        Task<bool> UpdateAll(List<TEntity> entitys);
        Task SaveChanges(CancellationToken cancellationToken = default);
        Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, decimal>> prop);
    }
}
