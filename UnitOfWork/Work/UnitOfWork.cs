using Infra;
using Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.Repository;

namespace UnitOfWork.Work
{

    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IUnitOfWork
        where TContext : DbContext, IDisposable
    {

        private Dictionary<Type, object> _dictionaryRepositories;
        private Dictionary<Type, object> _dictionaryRepositorieReadOnly;

        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));


        }

        public TContext Context { get; }

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public IRepositoryWriteOnly<TEntity> GetRepositoryWriteOnly<TEntity>() where TEntity : class
        {
            if (_dictionaryRepositories == null) _dictionaryRepositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_dictionaryRepositories.ContainsKey(type)) _dictionaryRepositories[type] =
                    new RepositoryWriteOnly<TEntity>(Context);
            return (IRepositoryWriteOnly<TEntity>)_dictionaryRepositories[type];
        }

        public IRepositoryReadOnly<TEntity> GetRepositoryReadOnly<TEntity>() where TEntity : class
        {
            if (_dictionaryRepositorieReadOnly == null) _dictionaryRepositorieReadOnly = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_dictionaryRepositorieReadOnly.ContainsKey(type)) _dictionaryRepositorieReadOnly[type] =
                    new RepositoryReadOnly<TEntity>(Context);
            return (IRepositoryReadOnly<TEntity>)_dictionaryRepositorieReadOnly[type];
        }

        public IDbContextTransaction BignTransation()
         => Context.Database.CurrentTransaction ?? Context.Database.BeginTransaction();
    }


}
