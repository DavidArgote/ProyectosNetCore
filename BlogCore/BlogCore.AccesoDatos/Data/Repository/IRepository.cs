using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        IEnumerable<T> GetAll(
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                string includePropierties = null
            );

        T GetFirstDefault(
                Expression<Func<T, bool>> filter = null,
                string includePropierties = null
            );

        void Add(T entity);

        void Remove(int id);

        void Remove(T entity);

    }
}
