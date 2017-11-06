using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APMS.DataAccess
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
