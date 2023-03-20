using Infra.Repository;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository.Repository
{
    public class RepositoryReadOnly<TEntity> : IRepositoryReadOnly<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;

        public RepositoryReadOnly(DbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<TResult>> FindBy<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, int pageNo = 1, int pageSize = 30)
            => await _dbContext.Set<TEntity>().AsNoTracking().Where(predicate).Select(selector).
                    Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

        public async Task<List<TResult>> FindBy<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector)
            => await _dbContext.Set<TEntity>().AsNoTracking().Where(predicate).Select(selector).ToListAsync();


        public async Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken = default)
            => await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);


        public async Task<int> GetCount(CancellationToken cancellationToken = default)
            => await _dbContext.Set<TEntity>().CountAsync(cancellationToken);


        public async Task<int> GetCount(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await _dbContext.Set<TEntity>().CountAsync(predicate, cancellationToken);


        public async Task<TEntity> FirstOfDefult(Expression<Func<TEntity, DateTime>> predicate, CancellationToken cancellationToken = default)
            => (await _dbContext.Set<TEntity>().OrderByDescending(predicate).FirstOrDefaultAsync(cancellationToken));

        public async Task<TEntity> FirstOfDefult(Expression<Func<TEntity, DateTime>> predicateOrderBy, Expression<Func<TEntity, bool>> predicateWhere, CancellationToken cancellationToken = default)
            => await _dbContext.Set<TEntity>().Where(predicateWhere).OrderByDescending(predicateOrderBy).FirstOrDefaultAsync(cancellationToken);

        public async Task<TEntity> GetByID(string Id)
        => await _dbContext.Set<TEntity>().FindAsync(Id);

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await _dbContext.Set<TEntity>().AnyAsync(predicate, cancellationToken);

        public async Task<TResult> FirstOfDefult<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector)
            => await _dbContext.Set<TEntity>().Where(predicate).Select(selector).FirstOrDefaultAsync();


        public async Task<TEntity> SingalOfDefultAsync(Expression<Func<TEntity, bool>> predicate)
            => await _dbContext.Set<TEntity>().Where(predicate).SingleOrDefaultAsync();


        public async Task<TResult> SingalOfDefultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector)
         => await _dbContext.Set<TEntity>().Where(predicate).Select(selector).SingleOrDefaultAsync();

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public async Task<TEntity> SingalOfDefultWithIncludAsync<TPropprty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TPropprty>> include)
        => await _dbContext.Set<TEntity>().Where(predicate).Include(include).SingleOrDefaultAsync();

        public async Task<TEntity> GetByID(int Id)
         => await _dbContext.Set<TEntity>().FindAsync(Id);

        public async Task<IQueryable<TEntity>> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().AsNoTracking().Where(predicate);
        }
    }
}
