using System.Linq.Expressions;

namespace Infra.Repository
{
    public interface IRepositoryReadOnly<TEntity> : IDisposable where TEntity : class
    {

        public Task<IQueryable<TEntity>> FindBy(Expression<Func<TEntity, bool>> predicate);
        public Task<List<TResult>> FindBy<TResult>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector, int pageNo, int pageSize);
        Task<List<TResult>> FindBy<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector);
        Task<TEntity> FirstOfDefult(Expression<Func<TEntity, DateTime>> predicate, CancellationToken cancellationToken = default);
        Task<TResult> FirstOfDefult<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector);
        Task<TEntity> FirstOfDefult(Expression<Func<TEntity, DateTime>> predicateOrderBy, Expression<Func<TEntity, bool>> predicateWhere, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken = default);
        Task<int> GetCount(CancellationToken cancellationToken = default);
        Task<int> GetCount(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TEntity> GetByID(string Id);
        Task<TEntity> GetByID(int Id);

        Task<TEntity> SingalOfDefultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TResult> SingalOfDefultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector);

        public Task<TEntity> SingalOfDefultWithIncludAsync<TPropprty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TPropprty>> include);

    }


}
