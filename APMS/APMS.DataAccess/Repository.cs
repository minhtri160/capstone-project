using System;
using System.Linq;
using System.Data.Entity;

namespace APMS.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly APMSEntities entities;

        public Repository()
        {
            entities = new APMSEntities();
        }
        
        public IQueryable<T> GetAll()
        {
            return entities.Set<T>();
        }

        public T Insert(T entity)
        {
            T result = entities.Set<T>().Add(entity);
            entities.SaveChanges();
            return result;
        }

        public void Update(T entity)
        {
            entities.Entry<T>(entity).State = EntityState.Modified;
            entities.SaveChanges();
        }

        public void Delete(T entity)
        {
            entities.Set<T>().Remove(entity);
            entities.SaveChanges();
        }
    }
}
