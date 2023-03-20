using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repository.RepositoryDapper
{
    public interface IGenericRepository<T> where T : class
    {
        T Find(string id);
        bool Exists(T entity);
        int Count();
        T Last();
        IEnumerable<T> Get();
        IEnumerable<T> Page(int pageSize, int pageNumber, int count);

        int Add(T entity);
        int AddRange(List<T> entity);

        void Update(T entity);
        int Delete(T entity);
        int Delete(string id);
        int Delete(string[] id);
        bool Delete(List<T> entities);
        bool Delete(params T[] entities);
    }
}
