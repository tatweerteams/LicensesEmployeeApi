using Infra;
using Infra.Repository.RepositoryDapper;
using Repository.Repository;
using System.Data;
using System.Diagnostics;

namespace UnitOfWork.Work
{
    public class UnitOfWorkDapper : IUnitOfWorkDapper
    {

        private string userName = "sa";
        private string Password = "123456";
        private string dataBasename = "CardManagmentDB";
        private string serverName = ".";


        public Action<string> Log { get; set; } = (s) => { Debug.WriteLine(s); };



        private Dictionary<Type, dynamic> _repositories;
        public Dictionary<Type, dynamic> Repositories
        {
            get { return _repositories; }
            set { _repositories = value; }
        }

        private bool _disposed;

        private IDbConnection _connection;
        public IDbConnection Connection
        {
            get { return _connection; }
            private set { _connection = value; }
        }

        private IDbTransaction _transaction;
        public IDbTransaction Transaction
        {
            get { return _transaction; }
            private set { _transaction = value; }
        }

        private IsolationLevel _isolationLevel;
        public IsolationLevel IsolationLevel
        {
            get { return _isolationLevel; }
            set { _isolationLevel = value; }
        }

        public UnitOfWorkDapper(IDbConnection connection, IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            _connection = connection;
            _isolationLevel = isolationLevel;
            _repositories = new Dictionary<Type, dynamic>();

            try
            {
                _connection.Open();
                _transaction = _connection.BeginTransaction(isolationLevel);
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
                throw;
            }
        }
        //public UnitOfWorkDapper(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        //    : this(new SqlConnection("Server=serverName;Database=dataBasename;user id=userName;password=Password;Integrated Security=false;"), isolationLevel) { }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
        {
            //if (ServiceLocator.IsLocationProviderSet)
            //{
            //    return ServiceLocator.Current.GetInstance<IRepository<TEntity>>();
            //}

            if (_repositories == null)
                _repositories = new Dictionary<Type, dynamic>();

            if (_repositories.ContainsKey(typeof(TEntity)))
                return (IGenericRepository<TEntity>)_repositories[typeof(TEntity)];

            var repositoryType = typeof(RepositoryDapper<>);
            _repositories.Add(typeof(TEntity), Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), this));

            return _repositories[typeof(TEntity)];
        }

        public bool Commit()
        {
            try
            {
                _transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                Log(ex.ToString());
                return false;
                throw;
            }
            finally
            {
                Reset();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();

            Reset();
        }

        public void Reset()
        {
            _transaction.Dispose();
            _transaction = _connection.BeginTransaction();
        }

        public void Dispose()
        {
            RealDispose(true);
            GC.SuppressFinalize(true);
        }

        void RealDispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWorkDapper()
        {
            RealDispose(false);
        }
    }


}
