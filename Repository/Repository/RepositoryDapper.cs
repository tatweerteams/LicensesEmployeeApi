using Dapper;
using Infra;
using Infra.Repository.RepositoryDapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class RepositoryDapper<T> : IGenericRepository<T> where T : class, IEntity
    {
        protected readonly IUnitOfWorkDapper _dapperUnit;
        public RepositoryDapper(IUnitOfWorkDapper dapperUnit)
        {
            _dapperUnit = dapperUnit;
        }

        public virtual int Add(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual int Delete(T entity)
        {
            return Delete(entity.Id);
        }

        public virtual bool Delete(List<T> entities)
        {
            var ids = entities.Select(e => e.Id).ToArray();
            return Delete(ids) == ids.Count();
        }

        public virtual bool Delete(params T[] entities)
        {
            var ids = entities.Cast<IEntity>().Select(e => e.Id).ToArray();
            return Delete(ids) == ids.Count();
        }

        public virtual int Delete(string[] ids)
        {
            return _dapperUnit.Connection.Execute(
                $"delete from {typeof(T).Name} where Id in @Ids",
                param: new { Ids = ids },
                transaction: _dapperUnit.Transaction);
        }

        public virtual int Delete(string id)
        {
            return _dapperUnit.Connection.Execute(
                $"delete from {typeof(T).Name} where Id = @Id",
                param: new { Id = id },
                transaction: _dapperUnit.Transaction);
        }

        public virtual T Find(string id)
        {
            return _dapperUnit.Connection.Query<T>(
                $"select * from {typeof(T).Name} where Id = @Id",
                param: new { Id = id },
                transaction: _dapperUnit.Transaction)
                .FirstOrDefault();
        }

        public virtual IEnumerable<T> Get()
        {
            return _dapperUnit.Connection.Query<T>(
                $"select * from {typeof(T).Name}")
                .ToList();
        }

        public virtual IEnumerable<T> Page(int pageSize, int pageNumber, int count)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber - 1;
            count = count > pageSize ? pageSize : count;

            var total = Count();
            var mod = count % pageSize;
            var pageCount = (total - mod) / pageSize;
            var start = pageNumber == 1 ? 1 : pageNumber * pageSize;
            var end = start + count;

            var subQuery = $"(select ROW_NUMBER() over (order by Id) as RowNum, * from {typeof(T).Name}) as Result";
            var query = $"select * from {subQuery} where RowNum >= {start} and RowNum < {end} order by RowNum";

            return _dapperUnit.Connection.Query<T>(query, transaction: _dapperUnit.Transaction).ToList();
        }

        public virtual T Last()
        {
            return _dapperUnit.Connection.QueryFirst<T>(
                $"select top 1 * from {typeof(T).Name} order by Id desc",
                transaction: _dapperUnit.Transaction);
        }

        public virtual int Count()
        {
            return _dapperUnit.Connection.ExecuteScalar<int>(
                $"select count(*) from {typeof(T).Name}",
                transaction: _dapperUnit.Transaction);
        }

        public virtual bool Exists(T entity)
        {
            return Find(entity.Id) != null;
        }

        public virtual int AddRange(List<T> entity)
        {
            throw new NotImplementedException();
        }
    }
}
