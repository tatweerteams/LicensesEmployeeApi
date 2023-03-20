using Infra.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infra
{

    public interface IUnitOfWork : IDisposable
    {
        IRepositoryWriteOnly<TEntity> GetRepositoryWriteOnly<TEntity>() where TEntity : class;
        IRepositoryReadOnly<TEntity> GetRepositoryReadOnly<TEntity>() where TEntity : class;
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
        public Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction BignTransation();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext Context { get; }
    }
}
