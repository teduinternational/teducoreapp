using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TeduCoreApp.Infrastructure.Interfaces
{
    public interface IRepository<T, K> where T : class
    {
        T FindById(K id, params Expression<Func<T, object>>[] includeProperties);

        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);

        void Update(T entity);

        void Remove(T entity);

        void Remove(K id);

        void RemoveMultiple(List<T> entities);
    }
}
